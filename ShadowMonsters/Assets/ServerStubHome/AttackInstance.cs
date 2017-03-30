using System;
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
        private MonsterDna monster;
        private MonsterDna target;
        private List<AttackInfo> monsterAttacks;
        private IAiAttackStyle aiStyle;
        private Guid playerId;
        private PlayerData playerData;
        private Timer attackSequenceTimer;
        private const int VictoryDance = 5000;

        public AttackInstance(Guid monsterId, Guid player)
        {
            serverStub = ServerStub.Instance();
            playerId = player;
            playerData = serverStub.GetPlayerData(player);
            monsterAttacks = serverStub.GetAttacksForMonster(monsterId);
            monster = serverStub.GetMonsterById(monsterId);
            aiStyle = new ButtonMasher(); //someday i will make more
            aiStyle.Attacks = monsterAttacks;
        }

        public EventHandler<DataEventArgs<AttackResolution>> EnemyAttack;

        public void StartCombat()
        {
            //if (target == null || target.CurrentHealth < 1) return;
            if (monster == null || monster.CurrentHealth < 1) return;

            PerformAttack(null);

        }

        List<DelayedDamageInfo> delayedDamage = new List<DelayedDamageInfo>();

        private void PerformAttack(object state)
        {
            if(target == null || target.CurrentHealth < 1)
            {
                target = playerData.PlayerDna;
            }
            int dueTime = 0;
            if (!delayedDamage.Any())
            {
                //new attack
                dueTime = PerformNewAttack();
            }
            else
            {
                //delayed attacks
                var dAttack = delayedDamage.FirstOrDefault();
                if (dAttack != null)
                {
                    var result = PerformDelayedAttack(dAttack);
                    if (!result.WasFatal)
                    {
                        delayedDamage.Remove(dAttack);
                        dueTime = dAttack.NextDueTime;
                    }
                    else
                    {
                        delayedDamage.Clear();
                        dueTime = GetAttackDelay(true, dAttack.CastTime);
                    }

                }
            }

            ResetTimer(dueTime);

        }

        private AttackResolution PerformDelayedAttack(DelayedDamageInfo dAttack)
        {
            var result = CalculateAttack(target, dAttack);
            FireEnemyAttack(result);
            return result;
        }

        private void ResetTimer(int dueTime)
        {
            if (attackSequenceTimer == null)
            {
                attackSequenceTimer = new Timer(PerformAttack, null, dueTime, Timeout.Infinite);
            }
            else if (attackSequenceTimer != null)// && playerData.PlayerDna.CurrentHealth > 0 && monster.CurrentHealth > 0)
            {
                attackSequenceTimer.Change(dueTime, Timeout.Infinite);
            }
        }

        private int PerformNewAttack()
        {
            var attack = aiStyle.ChooseAttack();

            if (attack.IsGenerator)
            {
                aiStyle.AddResource(attack.Affinity);
            }

            switch (attack.DamageStyle)
            {
                case DamageStyle.Instant:
                    var results = CalculateAttack(target, attack);
                    FireEnemyAttack(results);                    
                    return GetAttackDelay(results.WasFatal, attack.Cooldown);

                case DamageStyle.Delayed:
                    delayedDamage.Add(new DelayedDamageInfo {Name = attack.Name, Affinity = attack.Affinity, BaseDamage = attack.BaseDamage,Accuracy = attack.Accuracy ,NextDueTime = 0, PowerLevel = attack.PowerLevel });
                    attack.PowerLevel = 0;
                    return attack.CastTime * 1000;

                case DamageStyle.Tick:
                    var tickResult = CalculateAttack(target, attack);
                    FireEnemyAttack(tickResult);
                    //add rest of ticks as delayed unless fatal (then pause to celebrate)
                    if(!tickResult.WasFatal)
                    {
                        for (int i = 0; i < attack.Cooldown; i++)
                        {
                            delayedDamage.Add(new DelayedDamageInfo {Name = attack.Name, Affinity = attack.Affinity, BaseDamage = attack.BaseDamage, Accuracy = attack.Accuracy, NextDueTime = 1000 });
                        }
                    }
                    return GetAttackDelay(tickResult.WasFatal,1);
                default:
                    break;
            }
            return 0;
        }
       
        private int GetAttackDelay(bool fatal, int cooldown)
        {
            int attackDelay = cooldown * 1000;
            if (fatal && attackDelay < VictoryDance)
            {
                return VictoryDance;
            }
            return attackDelay;
        }

        public void UpdatePlayerChampion(Guid newChampionId)
        {
            target = serverStub.GetMonsterById(newChampionId);
        }

        private AttackResolution CalculateAttack(MonsterDna target, AttackInfo attack)
        {
            //determine hit 
            Random random = new Random();
            int swing = random.Next(1, 100);            
            if (swing > attack.Accuracy)
            {
                attack.PowerLevel = 0; //burned!
                return new AttackResolution
                {
                    PlayerId = playerId,
                    MaxHealth = target.MaxHealth,
                    CurrentHealth = target.CurrentHealth,
                    TargetId = target.MonsterId,
                    AttackPerformed = attack,
                    Success = false,
                };
            }

            float powerUpBonusPercentMultiplier = (attack.PowerLevel / 10f) + 1;

            var crit = IsCrit();
            float damage = attack.BaseDamage * (crit ? 2 : 1);

            damage = damage * powerUpBonusPercentMultiplier;

            target.CurrentHealth = target.CurrentHealth - damage;

            attack.PowerLevel = 0;
            return new AttackResolution
            {
                PlayerId = playerId,
                WasFatal = target.CurrentHealth <= 0,
                WasCritical = crit,
                Damage = damage,
                MaxHealth = target.MaxHealth,
                CurrentHealth = target.CurrentHealth,
                TargetId = target.MonsterId,
                AttackPerformed = attack,
                Success = true,
                TimeStamp = DateTime.Now,
            };
        }

        public AttackResolution HandlePlayerAttack(AttackRequest data)
        {

            AttackInfo attack = null;
            KnownAttacks.AllKnownAttackList.TryGetValue(data.AttackId, out attack);
            if (attack == null)
            {
               // Debug.LogError("Attack does not exist. You cannot attack without knowledge.");
                return null;
            }

            var resolution = CalculateAttack(monster, attack);
            if(resolution.WasFatal)
            {
                serverStub.KillMonster(monster.MonsterId);
            }

            return resolution;
        }

        private bool IsCrit()
        {
            Random random = new Random();
            int hit = random.Next(1, 100);
            
            if (hit < 20)
                return true;
            return false;
        }

        private void FireEnemyAttack(AttackResolution resolution)
        {
            var handler = EnemyAttack;
            if (handler != null)
                handler.Invoke(this, new DataEventArgs<AttackResolution> { Data = resolution });
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
                if (attackSequenceTimer != null)
                {
                    attackSequenceTimer.Dispose();
                    attackSequenceTimer = null;
                }
            }

        }
    }
}
