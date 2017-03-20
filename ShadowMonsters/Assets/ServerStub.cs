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
        Dictionary<MonsterList, ElementalAffinity> monsterAffinityMatchup = new Dictionary<MonsterList, ElementalAffinity>();
        Dictionary<Guid,MonsterInfo> spawnedMonsters = new Dictionary<Guid,MonsterInfo>();
        Dictionary<Guid, PlayerData> players = new Dictionary<Guid, PlayerData>();
        Dictionary<Guid, Dictionary<ElementalAffinity, int>> playerResources = new Dictionary<Guid, Dictionary<ElementalAffinity, int>>();
        KnownAttacks knownAttacks;

        MonsterInfo enemyMonster;

        public MonsterInfo GetRandomMonster()
        {
            MonsterList monster = (MonsterList)Enum.Parse(typeof(MonsterList), GetRandomKey<MonsterList>());
            MonsterPresence presence = (MonsterPresence)Enum.Parse(typeof(MonsterPresence), GetRandomKey<MonsterPresence>());

            enemyMonster = new MonsterInfo(monster, UnityEngine.Random.Range(1, 101)) {
                                                                                            MonsterAffinity = monsterAffinityMatchup[monster],
                                                                                            MonsterId = Guid.NewGuid(), MonsterPresence =presence,
                                                                                            AttackIds = GetAttackIdList(knownAttacks.KnownMonsterAttackList)
                                                                                       };
            spawnedMonsters[enemyMonster.MonsterId] = enemyMonster;
            return enemyMonster;
        }

        public void AddPlayerResource(Guid iD,ElementalAffinity resource)
        {
            Dictionary<ElementalAffinity, int> thisPlayerDict;
            playerResources.TryGetValue(iD, out thisPlayerDict);
            if (thisPlayerDict == null) return;
            int currentResource;
            thisPlayerDict.TryGetValue(resource, out currentResource);

            thisPlayerDict[resource] = currentResource++;

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
            PlayerData outData;
            players.TryGetValue(id, out outData);
            return outData;
        }

        private PlayerData CreatePlayerData(Guid id)
        {
            var team = new List<MonsterInfo>
                                {
                                    //new MonsterInfo(MonsterList.RhinoVirus,  UnityEngine.Random.Range(1,101))
                                    //{
                                    //    MonsterAffinity = monsterAffinityMatchup[MonsterList.RhinoVirus],
                                    //    NickName = "Rhinasephalasaurus",
                                    //    MonsterId = Guid.NewGuid(),
                                    //    IsTeamLead = true,
                                    //    AttackIds = GetAttackIdList(knownAttacks.KnownMonsterAttackList),
                                    //},
                                    new MonsterInfo(MonsterList.DemonEnforcer,  UnityEngine.Random.Range(1,101))
                                    {
                                        MonsterAffinity = monsterAffinityMatchup[MonsterList.DemonEnforcer],
                                        NickName = "Fluffy",
                                        MonsterId = Guid.NewGuid(),
                                        AttackIds = GetAttackIdList(knownAttacks.KnownMonsterAttackList),
                                    },
                                };

            foreach (MonsterInfo Monster in team)
            {
                spawnedMonsters[Monster.MonsterId] = Monster;
            }

            var data = new PlayerData
            {
                Id = id,
                DisplayName = "OFFHIZMEDZ",
                CurrentTeam = team,
                AttackIds = GetAttackIdList(knownAttacks.KnownPlayerAttackList),
                MaximumHealth = 50,
                CurrentHealth = 50
                        
            };
            
            
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

        internal Guid Authenticate()
        {
            //creates a player for anyone!  security
            
            var playerId = Guid.NewGuid();
            var data = CreatePlayerData(playerId);
            players.Add(playerId, data);
            playerResources.Add(playerId, new Dictionary<ElementalAffinity, int>());
            return playerId;
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
            MonsterInfo target;
            spawnedMonsters.TryGetValue(monsterId, out target);
            if (target == null) return false;
            if (target.CurrentHealth < 1) return false;
            return true;
        }

        private void Awake()
        {
            GenerateMonsterAffinityListMatchup();
        }

        private void GenerateMonsterAffinityListMatchup()
        {
            monsterAffinityMatchup.Add(MonsterList.DemonEnforcer, ElementalAffinity.Demon);
            monsterAffinityMatchup.Add(MonsterList.RhinoVirus, ElementalAffinity.Shadow);
            monsterAffinityMatchup.Add(MonsterList.RobotShockTrooper, ElementalAffinity.Mechanical);
            monsterAffinityMatchup.Add(MonsterList.GreenSpider, ElementalAffinity.Wood);
            monsterAffinityMatchup.Add(MonsterList.Dragonling, ElementalAffinity.Dragon);
            monsterAffinityMatchup.Add(MonsterList.Humpback, ElementalAffinity.Water);
        }

        private void Start()
        {
            knownAttacks = new KnownAttacks();
        }


        internal  AttackResolution SendAttack(AttackRequest data)
        {
            MonsterInfo target = null;
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

            float powerUpBonusPercentMultiplier = (attack.PowerLevel / 10f) + 1;
            
            var crit = IsCrit();
            float damage = attack.BaseDamage * (crit ? 2 : 1);

            damage = damage * powerUpBonusPercentMultiplier;
          
            target.CurrentHealth = target.CurrentHealth - damage;

            bool fatal = false;
            if(target.CurrentHealth <= 0)
            {
                //take a moment to mourn the fallen!
                spawnedMonsters.Remove(target.MonsterId);
                fatal = true;
            }

            attack.PowerLevel = 0;
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
