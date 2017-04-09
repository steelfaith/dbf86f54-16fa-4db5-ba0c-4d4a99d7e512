﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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

        public AttackInstance(Guid monsterId, Guid player, Guid attackId)
        {
            serverStub = ServerStub.Instance();
            playerId = player;
            Id = attackId;
            playerData = serverStub.GetPlayerData(player);
            monsterAttacks = serverStub.GetAttacksForMonster(monsterId);
            monster = serverStub.GetMonsterById(monsterId);
            aiStyle = new ButtonMasher(); //someday i will make more
            aiStyle.Attacks = monsterAttacks;
            playerAttackHelper = new AttackHelper(this);
            playerAttackHelper.TargetKilled += PlayerAttackHelperTargetKilled;
            aiAttackHelper = new AttackHelper(this);
            aiAttackHelper.AttackComplete += AiAttackComplete;
            
        }
                
        private void PlayerAttackHelperTargetKilled(object sender, EventArgs e)
        {
            buttonPressQueue.Enqueue(
            new ButtonPressResolution
            {
                PlayerId = playerId,
                TimeOutSeconds = 5 //victory timeout for player
            });
        }

        private void AiAttackComplete(object sender, EventArgs e)
        {
            AiPerformNewAttack();
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


        public void HandlePlayerAttack(AttackRequest data)
        {
            if (playerAttackHelper.IsBusy) return;
            AttackInfo attack = null;
            KnownAttacks.AllKnownAttackList.TryGetValue(data.AttackId, out attack);
            if (attack == null)
            {               
                return;
            }

            if (!serverStub.CanPerformAttack(attack.AttackId, PlayerChampion.MonsterId)) return;

            AddPlayerResource(attack);

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

        private void AddPlayerResource(AttackInfo attack)
        {
            if (!attack.IsGenerator) return;
            if (playerResources.Count < 6)
            {
                playerResources.Add(attack.Affinity);
            }

            playerResourceUpdateQueue.Enqueue(new ResourceUpdate
            {
                Resources = playerResources,
                Id = playerId
            });
        }

        public void BurnResource(BurnResourceRequest request)
        {
            if (playerResources == null || !playerResources.Any()) return;
            if (playerResources.Contains(request.NeededResource))
            {
                playerResources.RemoveAt(playerResources.FindLastIndex(x => x == request.NeededResource));
            }
            playerResourceUpdateQueue.Enqueue( new ResourceUpdate { Id = playerId, Resources = playerResources });
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
            if(attack.IsGenerator)
            {
                aiStyle.AddResource(attack.Affinity);
            }
            aiAttackHelper.StartAttack(attack, PlayerChampion);
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
