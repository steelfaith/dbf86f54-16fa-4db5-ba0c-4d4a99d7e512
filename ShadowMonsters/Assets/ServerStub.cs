﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Infrastructure;
using UnityEngine;

namespace Assets
{
    public class ServerStub : MonoBehaviour
    {
        Dictionary<MonsterList, MonsterType> monsterTypeMatchup = new Dictionary<MonsterList, MonsterType>();
        Dictionary<Guid,CreatureInfo> spawnedMonsters = new Dictionary<Guid,CreatureInfo>();
        Dictionary<Guid, PlayerData> players = new Dictionary<Guid, PlayerData>();
        KnownAttacks knownAttacks;

        CreatureInfo enemyMonster;

        public CreatureInfo GetRandomMonster()
        {
            MonsterList monster = (MonsterList)Enum.Parse(typeof(MonsterList), GetRandomKey<MonsterList>());
            MonsterPresence presence = (MonsterPresence)Enum.Parse(typeof(MonsterPresence), GetRandomKey<MonsterPresence>());

            enemyMonster = new CreatureInfo(monster, UnityEngine.Random.Range(1, 101)) {
                                                                                            MonsterType = monsterTypeMatchup[monster],
                                                                                            MonsterId = Guid.NewGuid(), MonsterPresence =presence,
                                                                                            AttackIds = GetAttackIdList(knownAttacks.KnownMonsterAttackList)
                                                                                       };
            spawnedMonsters[enemyMonster.MonsterId] = enemyMonster;
            return enemyMonster;
        }

        public AttackResolution PerformRandomAttackSequence(Guid monsterId, Guid target, Guid callbackId)
        {


            //get both monsters
            //create an attack instance
            //save in collection
            //sends messages to client
            //destroy on combat end
            var attacks = GetAttacksForMonster(monsterId);
            return new AttackResolution();
        }
        

        internal PlayerData GetPlayerData(Guid id)
        {
            var team = new List<CreatureInfo>
                                {
                                    new CreatureInfo(MonsterList.RhinoVirus,  UnityEngine.Random.Range(1,101))
                                    {
                                        MonsterType = monsterTypeMatchup[MonsterList.RhinoVirus],
                                        NickName = "Rhinasephalasaurus",
                                        MonsterId = Guid.NewGuid(),
                                        IsTeamLead = true,
                                        AttackIds = GetAttackIdList(knownAttacks.KnownMonsterAttackList),
                                    },
                                    new CreatureInfo(MonsterList.DemonEnforcer,  UnityEngine.Random.Range(1,101))
                                    {   
                                        MonsterType = monsterTypeMatchup[MonsterList.DemonEnforcer],                                     
                                        NickName = "Fluffy",
                                        MonsterId = Guid.NewGuid(),
                                        AttackIds = GetAttackIdList(knownAttacks.KnownMonsterAttackList),
                                    },
                                };

            foreach (CreatureInfo creature in team)
            {
                spawnedMonsters[creature.MonsterId] = creature;
            }

            var data = new PlayerData
            {
                CurrentTeam = team,
                AttackIds = GetAttackIdList(knownAttacks.KnownPlayerAttackList)            
            };
            players.Add(id, data);
            return data;
        }

        private List<Guid> GetAttackIdList(Dictionary<Guid,AttackInfo> fromDictionary)
        {
            var attackList = new List<Guid>();

            while (attackList.Count < 5)
            {
                var attack = fromDictionary.ElementAt(UnityEngine.Random.Range(0, knownAttacks.KnownMonsterAttackList.Count));
                if (!attackList.Contains(attack.Key))
                { attackList.Add(attack.Key); }
            }
            return attackList;
        }

        internal static Guid Authenticate()
        {
            return Guid.NewGuid();
        }

        public List<AttackInfo> GetAttacksForMonster(Guid monsterId)
        {
            var monster = spawnedMonsters[monsterId];
            return GetAttackInfoFromIds(monster.AttackIds);
        }

        public List<AttackInfo> GetAttacksForPlayer(Guid playerId)
        {
            var player = players[playerId];
            return GetAttackInfoFromIds(player.AttackIds);
        }

        private List<AttackInfo> GetAttackInfoFromIds(List<Guid> attackIds)
        {
            var returnList = new List<AttackInfo>();
            foreach (var attackId in attackIds)
            {
               returnList.Add(knownAttacks.AllKnownAttackList[attackId]);
            }

            return returnList;
        }

        private static string GetRandomKey<T>()
        {
            
            var list = Enum.GetNames(typeof(T)).ToList();

            return list[UnityEngine.Random.Range(0, list.Count)];
        }

        internal bool CheckPulse(Guid monsterId)
        {
            var target = spawnedMonsters[monsterId];
            if (target == null) return false;
            if (target.CurrentHealth < 1) return false;
            return true;
        }

        private void Awake()
        {
            GenerateMonsterTypeListMatchup();
        }

        private void GenerateMonsterTypeListMatchup()
        {
            monsterTypeMatchup.Add(MonsterList.DemonEnforcer, MonsterType.Demon);
            monsterTypeMatchup.Add(MonsterList.RhinoVirus, MonsterType.Shadow);
            monsterTypeMatchup.Add(MonsterList.RobotShockTrooper, MonsterType.Mechanical);
            monsterTypeMatchup.Add(MonsterList.GreenSpider, MonsterType.Wood);
            monsterTypeMatchup.Add(MonsterList.Dragonling, MonsterType.Dragon);
            monsterTypeMatchup.Add(MonsterList.Humpback, MonsterType.Water);
        }

        private void Start()
        {
            knownAttacks = new KnownAttacks();
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
            knownAttacks.AllKnownAttackList.TryGetValue(data.AttackId, out attack);
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