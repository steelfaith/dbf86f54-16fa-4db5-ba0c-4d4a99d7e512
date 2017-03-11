using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Infrastructure;
using UnityEngine;

namespace Assets
{
    public class ServerStub : MonoBehaviour
    {
        Dictionary<Guid,CreatureInfo> spawnedMonsters = new Dictionary<Guid,CreatureInfo>();
        Dictionary<Guid, AttackInfo> knownAttacks = new Dictionary<Guid, AttackInfo>();

        CreatureInfo enemyMonster;

        public CreatureInfo GetRandomMonster()
        {
            MonsterList monster = (MonsterList)Enum.Parse(typeof(MonsterList), GetRandomKey());
            enemyMonster = new CreatureInfo(monster) { Level = UnityEngine.Random.Range(0, 101), MaxHealth =500, CurrentHealth = 500, MonsterId = Guid.NewGuid()};
            spawnedMonsters[enemyMonster.MonsterId] = enemyMonster;
            return enemyMonster;
        }
        

        internal PlayerData GetPlayerData(Guid id)
        {
            var team = new List<CreatureInfo>
                                {
                                    new CreatureInfo(MonsterList.PlantBallOfDoom) {Level = UnityEngine.Random.Range(0,101), MaxHealth = 300, MonsterId = Guid.NewGuid() },
                                    new CreatureInfo(MonsterList.SquareOfMountainDeath) {Level = UnityEngine.Random.Range(0,101), MaxHealth = 500, MonsterId = Guid.NewGuid() }
                                };

            foreach (CreatureInfo creature in team)
            {
                spawnedMonsters[creature.MonsterId] = creature;
            }

            return new PlayerData
            {
                CurrentTeam = team            
            };    
        }

        internal static Guid Authenticate()
        {
            return Guid.NewGuid();
        }

        private static string GetRandomKey()
        {
            var list = Enum.GetNames(typeof(MonsterList)).ToList();

            return list[UnityEngine.Random.Range(0, list.Count)];
        }

        internal List<AttackInfo> GetAttackInfo(Guid guid)
        {
            var attackList = new List<AttackInfo>
            {
                new AttackInfo
                {
                    AttackId = Guid.NewGuid(),
                    Name = "Fire Ball",
                    DamageStyle = DamageStyle.Delayed,
                    MonsterType =MonsterType.Fire,
                    CastTime = 3,
                    Cooldown = 0,
                    BaseDamage = 50
                },
                new AttackInfo
                {
                    AttackId = Guid.NewGuid(),
                    Name = "Doom Bolt",
                    DamageStyle = DamageStyle.Delayed,
                    MonsterType =MonsterType.Demon,
                    CastTime = 5,
                    Cooldown = 0,
                    BaseDamage = 70
                },
                 new AttackInfo
                {
                    AttackId = Guid.NewGuid(),
                    Name = "Axe Flurry",
                    DamageStyle = DamageStyle.Tick,
                    MonsterType =MonsterType.Mechanical,
                    CastTime = 0,
                    Cooldown = 5,
                    BaseDamage =15
                },
                new AttackInfo
                {
                    AttackId = Guid.NewGuid(),
                    Name = "Air Jab",
                    DamageStyle = DamageStyle.Instant,
                    MonsterType = MonsterType.Wind,
                    CastTime = 0,
                    Cooldown = 2,
                    BaseDamage = 35
                },
                new AttackInfo
                {
                    AttackId = Guid.NewGuid(),
                    Name = "Wing Smash",
                    DamageStyle = DamageStyle.Instant,
                    MonsterType =MonsterType.Fae,
                    CastTime = 0,
                    Cooldown = 4,
                    BaseDamage = 60
                }
            };

            foreach (AttackInfo info in attackList)
            {
                knownAttacks[info.AttackId] = info;
            }

            return attackList;
        }

        internal  AttackResolution SendAttack(AttackRequest data)
        {
            CreatureInfo target = null;
            spawnedMonsters.TryGetValue(data.TargetId, out target);
            if(target == null)
            {
                Debug.LogError("Target does not exist. You cannot beat a dead horse.");
                return null;
            }
            AttackInfo attack = null;
            knownAttacks.TryGetValue(data.AttackId, out attack);
            if(attack == null)
            {
                Debug.LogError("Attack does not exist. You cannot attack without knowledge.");
                return null;
            }

            var crit = IsCrit();
            var damage = attack.BaseDamage * (crit ? 2 : 1);
            target.CurrentHealth = target.CurrentHealth - damage;

            bool fatal = false;
            if(target.CurrentHealth <= 0)
            {
                //take a moment to mourn the fallen!
                spawnedMonsters.Remove(target.MonsterId);
                fatal = true;
            }

            return new AttackResolution
            {
                WasFatal = fatal,
                WasCritical = crit,
                Damage = damage,
                MaxHealth = target.MaxHealth,
                CurrentHealth = target.CurrentHealth,
                TargetId = target.MonsterId
            };

        }

        private static ServerStub serverStub;

        public static ServerStub Instance()
        {
            if (!serverStub)
            {
                serverStub = FindObjectOfType(typeof(ServerStub)) as ServerStub;
                if (!serverStub)
                    Debug.LogError("Could not find server!");
            }
            return serverStub;
        }

        private bool IsCrit()
        {
            var hit = UnityEngine.Random.Range(0, 101);
            if (hit < 20)
                return true;
            return false;
        }
    }
}
