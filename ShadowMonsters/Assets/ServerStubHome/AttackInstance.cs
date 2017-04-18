using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Infrastructure;

using Assets.ServerStubHome.AiAttackStyles;

namespace Assets.ServerStubHome
{
    public class AttackInstance : IDisposable
    {
        public ServerStub serverStub;
        public Queue<AttackResolution> attackResultsQueue = new Queue<AttackResolution>();
        public Queue<ButtonPressResolution> buttonPressQueue = new Queue<ButtonPressResolution>();
        public Queue<ResourceUpdate> playerResourceUpdateQueue = new Queue<ResourceUpdate>();
        public Queue<AttackPowerChangeResolution> attackPowerChangeUpdateQueue = new Queue<AttackPowerChangeResolution>();
        public Queue<EnemyAttackUpdate> enemyAttackUpdateQueue = new Queue<EnemyAttackUpdate>();
        public Queue<EnemyResourceDisplayUpdate> enemyResourceDisplayUpdateQueue = new Queue<EnemyResourceDisplayUpdate>();

        public Guid playerId;
        private MonsterDna monster;
        private MonsterDna playerChampion;
        private List<AttackInfo> monsterAttacks;
        private IAiAttackStyle aiStyle;        
        private PlayerData playerData;
        private Guid Id;
        private AttackHelper playerAttackHelper;
        private AttackHelper aiAttackHelper;
        List<ElementalAffinity> playerResources = new List<ElementalAffinity>();
        

        public AttackInstance(Guid monsterId, Guid player, Guid instanceId)
        {
            serverStub = ServerStub.Instance();
            playerId = player;
            Id = instanceId;
            playerData = serverStub.GetPlayerData(player);
            monsterAttacks = serverStub.GetAttacksForMonster(monsterId);
            monster = serverStub.GetMonsterById(monsterId);
            aiStyle = new ButtonMasher(this); //someday i will make more
            aiStyle.Attacks = monsterAttacks;
            playerAttackHelper = new AttackHelper(this, serverStub);
            playerAttackHelper.TargetKilled += PlayerAttackHelperTargetKilled;
            playerAttackHelper.AttackComplete += PlayerAttackComplete;
            aiAttackHelper = new AttackHelper(this, serverStub);
            aiAttackHelper.AttackComplete += AiAttackComplete;

        }

        public bool AttackQueuesHaveData()
        {
            if (attackResultsQueue.Count > 0) return true;
            if (buttonPressQueue.Count > 0) return true;
            if (playerResourceUpdateQueue.Count > 0) return true;
            if (attackPowerChangeUpdateQueue.Count > 0) return true;

            return false;
        }

        public MonsterDna PlayerChampion
        {
            get
            {
                if (playerChampion == null || playerChampion.CurrentHealth < 1)
                {
                    return playerData.PlayerDna;
                }
                return playerChampion;
            }

            private set {if (value != null) playerChampion = value; }
        }

        public bool CombatShouldContinue
        {
            get { return playerData.PlayerDna.CurrentHealth > 0 && monster.CurrentHealth > 0; }
        }

        public void StartCombat()
        {
            //if (target == null || target.CurrentHealth < 1) return;
            if (monster == null || monster.CurrentHealth < 1) return;

            AiPerformNewAttack();

        }

        List<DelayedDamageInfo> delayedDamage = new List<DelayedDamageInfo>();

        public void UpdatePlayerChampion(Guid newChampionId)
        {
            PlayerChampion = serverStub.GetMonsterById(newChampionId);
        }

        public void AttemptPlayerShortCast()
        {
            playerAttackHelper.AttemptShortCast();
        }

        public void HandlePlayerAttack(AttackRequest data)
        {
            if (playerAttackHelper.IsBusy) return;
            AttackInfo attack = null;
            KnownAttacks.AllKnownAttackList.TryGetValue(data.AttackId, out attack);
            if (attack == null)
            {               
                return;
            }

            if (!CanPerformAttack(attack)) return;


            AddResourceFromAttack(attack);

            playerAttackHelper.StartAttack(attack, monster);

            var timeOut = attack.Cooldown > 0 ? attack.Cooldown : attack.CastTime;
            
            buttonPressQueue.Enqueue(new ButtonPressResolution
            {
                PlayerId = playerData.Id,
                AttackId = attack.AttackId,
                Clockwise = attack.CastTime > 0,
                TimeOutSeconds = timeOut
            });
            
        }

        private bool CanPerformAttack(AttackInfo attack)
        {
            if (serverStub.CanPerformAttack(attack.AttackId, PlayerChampion.MonsterId))
                return true;
            if(PlayerChampion.MonsterId == playerId )
            {
                return playerData.AttackIds.Contains(attack.AttackId);
            }
            return false;
        }

        public void HandleAttackPowerChange(AttackPowerChangeRequest request)
        {
            playerAttackHelper.HandleAttackPowerChange(request);
        }

        private void PlayerAttackHelperTargetKilled(object sender, EventArgs e)
        {
            //TODO: figure out why this was causing havoc with fatbic
            //buttonPressQueue.Enqueue(
            //new ButtonPressResolution
            //{
            //    PlayerId = playerId,
            //    TimeOutSeconds = 5 //victory timeout for player
            //});
        }

        private void AiAttackComplete(object sender, EventArgs e)
        {
            if (!CombatShouldContinue)
                EndCombat();
            AiPerformNewAttack();
        }


        private void PlayerAttackComplete(object sender, EventArgs e)
        {
            if (!CombatShouldContinue)
                EndCombat();
        }

        private void EndCombat()
        {
            playerAttackHelper.EndCombat();
            aiAttackHelper.EndCombat();
            playerResourceUpdateQueue.Enqueue(new ResourceUpdate
            {
                Resources = new List<ElementalAffinity>(),
                Id = playerId
            });

            serverStub.EndAttackInstance(Id);
        }

        public void AddPlayerResource(ElementalAffinity resource)
        {

            if (playerResources.Count < 6)
            {
                playerResources.Add(resource);
            }

            playerResourceUpdateQueue.Enqueue(new ResourceUpdate
            {
                Resources = playerResources,
                Id = playerId
            });
        } 
        
        private void AddResourceFromAttack(AttackInfo attack)
        {
            if (!attack.IsGenerator) return;
            AddPlayerResource(attack.Affinity);
        }

        public bool TryBurnResource()
        {
            if (playerResources == null || !playerResources.Any()) return false;
            bool success = false;
            if (playerResources.Contains(playerAttackHelper.currentAttack.Affinity))
            {
                playerResources.RemoveAt(playerResources.FindLastIndex(x => x == playerAttackHelper.currentAttack.Affinity));
                success = true;
            }
            playerResourceUpdateQueue.Enqueue(new ResourceUpdate { Id = playerId, Resources = playerResources });
            return success;            
        }

        private float GetModifiedAttackDelayInMilliseconds(float speed, AttackInfo attack)
        {
            //TODO: Use this someday
            var timeout = attack.Cooldown > 0 ? attack.Cooldown : attack.CastTime;

            var speedBonusPercent = speed / 100;
            var speedBonus = (timeout * speedBonusPercent) / 100;
            return (timeout - speedBonus) * 1000;
        }

        private void AiPerformNewAttack()
        {
            if (!CombatShouldContinue) return;
            var attack = aiStyle.ChooseAttack();
            if (attack.IsGenerator)
            {
                aiStyle.AddResource(attack.Affinity);
                enemyResourceDisplayUpdateQueue.Enqueue(new EnemyResourceDisplayUpdate
                {
                    PlayerId = playerId,
                    Resources = aiStyle.GetResources(),
                });
            }
            aiAttackHelper.StartAttack(attack, PlayerChampion);
            enemyAttackUpdateQueue.Enqueue(new EnemyAttackUpdate
            {
                PlayerId = playerId,
                Attack = attack,
            });
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources  
                if (aiAttackHelper != null)
                {
                    aiAttackHelper.Dispose();
                    aiAttackHelper = null;
                }

                if (playerAttackHelper != null)
                {
                    playerAttackHelper.Dispose();
                    playerAttackHelper = null;
                }
            }

        }
    }
}
