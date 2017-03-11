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

        CreatureInfo enemyMonster;

        public CreatureInfo GetRandomMonster()
        {
            MonsterList monster = (MonsterList)Enum.Parse(typeof(MonsterList), GetRandomKey());
            enemyMonster = new CreatureInfo(monster) { Level = UnityEngine.Random.Range(0, 101), MaxHealth =500, CurrentHealth = 500 };
            return enemyMonster;
        }
        

        internal static PlayerData GetPlayerData(Guid id)
        {
            var creature1 = new CreatureInfo(MonsterList.PlantBallOfDoom);

            return new PlayerData
            {
                CurrentTeam = new List<CreatureInfo>
                                {
                                    new CreatureInfo(MonsterList.PlantBallOfDoom) {Level = UnityEngine.Random.Range(0,101), MaxHealth = 300 },
                                    new CreatureInfo(MonsterList.SquareOfMountainDeath) {Level = UnityEngine.Random.Range(0,101), MaxHealth = 500 }
                                }
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

        internal static  List<AttackInfo> GetAttackInfo(Guid guid)
        {
            return new List<AttackInfo>
            {
                new AttackInfo
                {
                    Name = "Fire Ball",
                    DamageStyle = DamageStyle.Delayed,
                    MonsterType =MonsterType.Fire,
                    CastTime = 3,
                    Cooldown = 0,
                    BaseDamage = 50
                },
                new AttackInfo
                {
                    Name = "Doom Bolt",
                    DamageStyle = DamageStyle.Delayed,
                    MonsterType =MonsterType.Demon,
                    CastTime = 5,
                    Cooldown = 0,
                    BaseDamage = 70
                },
                 new AttackInfo
                {
                    Name = "Axe Flurry",
                    DamageStyle = DamageStyle.Tick,
                    MonsterType =MonsterType.Mechanical,
                    CastTime = 0,
                    Cooldown = 5,
                    BaseDamage =15
                },
                new AttackInfo
                {
                    Name = "Air Jab",
                    DamageStyle = DamageStyle.Instant,
                    MonsterType = MonsterType.Wind,
                    CastTime = 0,
                    Cooldown = 2,
                    BaseDamage = 35
                },
                new AttackInfo
                {
                    Name = "Wing Smash",
                    DamageStyle = DamageStyle.Instant,
                    MonsterType =MonsterType.Fae,
                    CastTime = 0,
                    Cooldown = 4,
                    BaseDamage = 60
                }
            };
        }

        internal  CreatureInfo SendAttack(AttackInfo data)
        {
            enemyMonster.CurrentHealth = enemyMonster.CurrentHealth - data.BaseDamage;
            return enemyMonster;
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
    }
}
