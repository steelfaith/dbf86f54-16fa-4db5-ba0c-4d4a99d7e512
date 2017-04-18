using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Infrastructure;
using System.Threading;

namespace Assets.ServerStubHome
{
    public class AttackHelper : IDisposable
    {
        public bool IsBusy { get; set; }
        List<DelayedDamageInfo> delayedDamage = new List<DelayedDamageInfo>();
        AttackInstance attackInstance;
        private Timer attackDelayTimer;
        private const int VictoryDance = 5000;
        private MonsterDna target;
        Random random = new Random();
        public AttackInfo currentAttack;
        private ServerStub serverStub;
        bool combatEnded;
        private DateTime attackDelayTimerStartDateTime;
        private float shortCastDamagePercentage;
        private bool isShortCast;


        public AttackHelper(AttackInstance instance, ServerStub stub)
        {
            attackInstance = instance;
            serverStub = stub;
        }

        public event EventHandler AttackComplete;
        public event EventHandler TargetKilled;


        /// <summary>
        /// starts the attack process.  will return int as cooldown (Victory Dance) if first attack is instant and fatal
        /// </summary>
        /// <param name="attack"></param>
        /// <param name="attackTarget"></param>
        /// <returns> timeout in seconds</returns>
        public void StartAttack(AttackInfo attack, MonsterDna attackTarget)
        {
            if (IsBusy) return;
            IsBusy = true;
            shortCastDamagePercentage = 0;
            isShortCast = false;
            target = attackTarget;
            currentAttack = attack;

            switch (attack.DamageStyle)
            {
                case DamageStyle.Instant:
                    var result = CalculateAttack(target, attack);
                    attackInstance.attackResultsQueue.Enqueue(result);
                    var timeOut =GetAttackDelay(result.WasFatal, attack.Cooldown);
                    ResetTimer(timeOut);
                    break;
                case DamageStyle.Delayed:
                    delayedDamage.Add(new DelayedDamageInfo {AttackId = attack.AttackId, Name = attack.Name, Affinity = attack.Affinity, BaseDamage = attack.BaseDamage, Accuracy = attack.Accuracy, NextDueTime = 0, PowerLevel = attack.PowerLevel });
                    attack.PowerLevel = 0;
                    ResetTimer(attack.CastTime * 1000);
                    break;

                case DamageStyle.Tick:
                    var tickResult = CalculateAttack(target, attack);
                    attackInstance.attackResultsQueue.Enqueue(tickResult);
                    //add rest of ticks as delayed unless fatal (then pause to celebrate)
                    if (!tickResult.WasFatal)
                    {
                        for (int i = 0; i < attack.Cooldown -1; i++)
                        {
                            delayedDamage.Add(new DelayedDamageInfo { AttackId = attack.AttackId, Name = attack.Name, Affinity = attack.Affinity, BaseDamage = attack.BaseDamage, Accuracy = attack.Accuracy, NextDueTime = 1000 });
                        }
                    }
                    ResetTimer(GetAttackDelay(tickResult.WasFatal, 1));
                    break;
                default:
                    break;
            }

        }

        public void AttemptShortCast()
        {
            if (currentAttack == null) return;
            if(currentAttack.CastTime > 0 && delayedDamage.Any())
            {
                isShortCast = true;   
                var t = DateTime.Now - attackDelayTimerStartDateTime;
                shortCastDamagePercentage = (float)(t.TotalSeconds / currentAttack.CastTime) * 95;
                if (attackDelayTimer != null)
                    attackDelayTimer.Change(0, Timeout.Infinite);
            }
        }

        public void HandleAttackPowerChange(AttackPowerChangeRequest request)
        {
            if (request.AttackId != currentAttack.AttackId) return;

            if (request.Up)
            {
                if (currentAttack.PowerLevel < 3)
                {
                    if (attackInstance.TryBurnResource())
                    { currentAttack.PowerLevel++; }
                    else
                    {
                        serverStub.ServerMessageQueue.Enqueue(new ServerMessage
                        {
                            AnnoucementType = AnnouncementType.System,
                            Message = string.Format("Not enough {0} resources to power up attack", currentAttack.Affinity.ToString())
                        });
                    }
                }
                else
                {
                    serverStub.ServerMessageQueue.Enqueue(new ServerMessage
                    {
                        AnnoucementType = AnnouncementType.System,
                        Message = "Attack can not be powered up farther!",
                    });
                }
            }
            else
            {
                if (currentAttack.PowerLevel > 0)
                {
                    attackInstance.AddPlayerResource(currentAttack.Affinity);
                    currentAttack.PowerLevel--;
                }

            }
            //update client ui
            attackInstance.attackPowerChangeUpdateQueue.Enqueue(new AttackPowerChangeResolution
            {
                AttackId = currentAttack.AttackId,
                PlayerId = attackInstance.playerId,
                PowerLevel = currentAttack.PowerLevel
            });
        }

        private void PerformAttack(object state)
        {
            if (combatEnded) return;
            int dueTime = 0;
            if (!delayedDamage.Any())
            {
                EndAttack();
                return;
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

        public void EndCombat()
        {
            currentAttack = null;
            combatEnded = true;
            delayedDamage.Clear();
        }

        private void EndAttack()
        {
            IsBusy = false;

            attackInstance.attackPowerChangeUpdateQueue.Enqueue(new AttackPowerChangeResolution
            {
                AttackId = currentAttack.AttackId,
                PlayerId = attackInstance.playerId,
                PowerLevel = 0
            });
            currentAttack = null;
            var handler = AttackComplete;
            if (handler != null)
                handler.Invoke(this, EventArgs.Empty);
        }

        private AttackResolution PerformDelayedAttack(DelayedDamageInfo dAttack)
        {
            var result = CalculateAttack(target, dAttack);
            attackInstance.attackResultsQueue.Enqueue(result);
            return result;
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

        private void ResetTimer(int dueTime)
        {
            attackDelayTimerStartDateTime = DateTime.Now;
            if (attackDelayTimer == null)
            {
                attackDelayTimer = new Timer(PerformAttack, null, dueTime, Timeout.Infinite);
            }
            else if (attackDelayTimer != null)
            {
                attackDelayTimer.Change(dueTime, Timeout.Infinite);
            }
        }

        private AttackResolution CalculateAttack(MonsterDna target, AttackInfo attack)
        {
            //determine hit 
            
            int swing = random.Next(1, 100);
            if (swing > attack.Accuracy)
            {
                attack.PowerLevel = 0; //burned!
                return new AttackResolution
                {
                    MaxHealth = target.MaxHealth,
                    CurrentHealth = target.CurrentHealth,
                    TargetId = target.MonsterId,
                    AttackPerformed = attack,
                    Success = false,
                };
            }

            //attacks base damage to start
            float damage = attack.BaseDamage;
            //adjust shortcast
            if (isShortCast && shortCastDamagePercentage > 0)
            {
                damage = damage * shortCastDamagePercentage / 100;               
            }

            //adjust crit
            var crit = IsCrit();
            damage = damage * (crit ? 2 : 1);

            float powerLevel = 0;
            //adjust for power ups
            if (attack.AttackId == currentAttack.AttackId)
            {
                powerLevel = currentAttack.PowerLevel;
            }
            float powerUpBonusPercentMultiplier = (powerLevel / 10f) + 1;
            damage = damage * powerUpBonusPercentMultiplier;

            //round the numbers 
            damage = (float)Math.Round(damage);

            target.CurrentHealth = target.CurrentHealth - damage;
            shortCastDamagePercentage = 0;
            attack.PowerLevel = 0;

            var fatal = target.CurrentHealth <= 0;
            if (fatal)
            {
                var handler = TargetKilled;
                if (handler != null)
                    handler.Invoke(this, EventArgs.Empty);
            }
            return new AttackResolution
            {
                WasFatal = fatal,
                WasCritical = crit,
                Damage = damage,
                MaxHealth = target.MaxHealth,
                CurrentHealth = target.CurrentHealth,
                TargetId = target.MonsterId,
                AttackPerformed = attack,
                Success = true,
                TimeStamp = DateTime.Now,
                WasShortCast = isShortCast,
            };            
        }


        private bool IsCrit()
        {            
            int hit = random.Next(1, 100);

            if (hit < 20)
                return true;
            return false;
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
                if (attackDelayTimer != null)
                {
                    attackDelayTimer.Dispose();
                    attackDelayTimer = null;
                }
            }

        }
    }
}
