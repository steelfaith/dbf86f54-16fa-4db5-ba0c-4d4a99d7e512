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

        public AttackHelper(AttackInstance instance)
        {
            attackInstance = instance;
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
            target = attackTarget;

            switch (attack.DamageStyle)
            {
                case DamageStyle.Instant:
                    var result = CalculateAttack(target, attack);
                    attackInstance.attackResultsQueue.Enqueue(result);
                    var timeOut =GetAttackDelay(result.WasFatal, attack.Cooldown);
                    ResetTimer(timeOut);
                    break;
                case DamageStyle.Delayed:
                    delayedDamage.Add(new DelayedDamageInfo { Name = attack.Name, Affinity = attack.Affinity, BaseDamage = attack.BaseDamage, Accuracy = attack.Accuracy, NextDueTime = 0, PowerLevel = attack.PowerLevel });
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
                            delayedDamage.Add(new DelayedDamageInfo { Name = attack.Name, Affinity = attack.Affinity, BaseDamage = attack.BaseDamage, Accuracy = attack.Accuracy, NextDueTime = 1000 });
                        }
                    }
                    ResetTimer(GetAttackDelay(tickResult.WasFatal, 1));
                    break;
                default:
                    break;
            }

        }

        private void PerformAttack(object state)
        {

            int dueTime = 0;
            if (!delayedDamage.Any())
            {
                IsBusy = false;
                var handler = AttackComplete;
                if (handler != null)
                    handler.Invoke(this,EventArgs.Empty);
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

            float powerUpBonusPercentMultiplier = (attack.PowerLevel / 10f) + 1;

            var crit = IsCrit();
            float damage = attack.BaseDamage * (crit ? 2 : 1);

            damage = damage * powerUpBonusPercentMultiplier;
            target.CurrentHealth = target.CurrentHealth - damage;
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
