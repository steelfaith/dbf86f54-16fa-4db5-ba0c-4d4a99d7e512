using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Infrastructure;
using UnityEngine;

namespace Assets.ServerStubHome
{
    public class ServerStub : MonoBehaviour
    {
        Dictionary<MonsterList, ElementalAffinity> monsterAffinityMatchup = new Dictionary<MonsterList, ElementalAffinity>();
        Dictionary<Guid,MonsterDna> spawnedMonsters = new Dictionary<Guid,MonsterDna>();
        Dictionary<Guid, PlayerData> players = new Dictionary<Guid, PlayerData>();
        Dictionary<Guid, List<ElementalAffinity>> playerResources = new Dictionary<Guid, List<ElementalAffinity>>();
        KnownAttacks knownAttacks;

        MonsterDna enemyMonster;

        public MonsterDna GetRandomMonster()
        {
            MonsterList monster = (MonsterList)Enum.Parse(typeof(MonsterList), GetRandomKey<MonsterList>());
            MonsterPresence presence = (MonsterPresence)Enum.Parse(typeof(MonsterPresence), GetRandomKey<MonsterPresence>());

            enemyMonster = new MonsterDna(monster, UnityEngine.Random.Range(1, 101)) {
                                                                                            MonsterAffinity = monsterAffinityMatchup[monster],
                                                                                            MonsterId = Guid.NewGuid(), MonsterPresence =presence,
                                                                                            AttackIds = GetAttackIdList(knownAttacks.KnownMonsterAttackList)
                                                                                       };
            spawnedMonsters[enemyMonster.MonsterId] = enemyMonster;
            return enemyMonster;
        }

        public AddResourceResponse AddPlayerResource(AddResourceRequest request)
        {
            List<ElementalAffinity> thisPlayerResources;
            playerResources.TryGetValue(request.PlayerId, out thisPlayerResources);
            if (thisPlayerResources == null) return null;
            if(thisPlayerResources.Count<6)
            {
                thisPlayerResources.Add(request.Affinity);
            }

            return new AddResourceResponse
            {
                Resources = thisPlayerResources,
                Id = request.PlayerId
            };
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
            var team = new List<MonsterDna>
                                {
                                    //new MonsterInfo(MonsterList.RhinoVirus,  UnityEngine.Random.Range(1,101))
                                    //{
                                    //    MonsterAffinity = monsterAffinityMatchup[MonsterList.RhinoVirus],
                                    //    NickName = "Rhinasephalasaurus",
                                    //    MonsterId = Guid.NewGuid(),
                                    //    IsTeamLead = true,
                                    //    AttackIds = GetAttackIdList(knownAttacks.KnownMonsterAttackList),
                                    //},
                                    //new MonsterInfo(MonsterList.DemonEnforcer,  UnityEngine.Random.Range(1,101))
                                    //{
                                    //    MonsterAffinity = monsterAffinityMatchup[MonsterList.DemonEnforcer],
                                    //    NickName = "Fluffy",
                                    //    MonsterId = Guid.NewGuid(),
                                    //    AttackIds = GetAttackIdList(knownAttacks.KnownMonsterAttackList),
                                    //},
                                    new MonsterDna(MonsterList.MiniLandShark,  UnityEngine.Random.Range(1,101))
                                    {
                                        MonsterAffinity = monsterAffinityMatchup[MonsterList.MiniLandShark],
                                        NickName = "Big Eddy",
                                        MonsterId = Guid.NewGuid(),
                                        AttackIds = GetAttackIdList(knownAttacks.KnownMonsterAttackList),
                                        IsTeamLead = true,
                                    },
                                };

            foreach (MonsterDna Monster in team)
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

        public BurnResourceResponse BurnResource(BurnResourceRequest request)
        {
            List<ElementalAffinity> thisPlayerResources;
            playerResources.TryGetValue(request.PlayerId, out thisPlayerResources);
            if (thisPlayerResources == null) return null;
            bool success = false;
            if(thisPlayerResources.Contains(request.NeededResource))
            {
                thisPlayerResources.RemoveAt(thisPlayerResources.FindLastIndex(x => x == request.NeededResource));
                success = true;
            }
            return new BurnResourceResponse { PlayerId = request.PlayerId, CurrentResources = thisPlayerResources, Success = success};
        }

        internal void ClearPlayerResources(Guid id)
        {
            playerResources[id] = new List<ElementalAffinity>();
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
            playerResources.Add(playerId, new List<ElementalAffinity>());
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
            MonsterDna target;
            spawnedMonsters.TryGetValue(monsterId, out target);
            if (target == null) return false;
            if (target.CurrentHealth < 1) return false;
            return true;
        }

        private void Awake()
        {
            GenerateMonsterAffinityListMatchup();
            knownAttacks = new KnownAttacks();
        }

        private void GenerateMonsterAffinityListMatchup()
        {
            monsterAffinityMatchup.Add(MonsterList.DemonEnforcer, ElementalAffinity.Demon);
            monsterAffinityMatchup.Add(MonsterList.RhinoVirus, ElementalAffinity.Shadow);
            monsterAffinityMatchup.Add(MonsterList.RobotShockTrooper, ElementalAffinity.Mechanical);
            monsterAffinityMatchup.Add(MonsterList.GreenSpider, ElementalAffinity.Wood);
            monsterAffinityMatchup.Add(MonsterList.Dragonling, ElementalAffinity.Dragon);
            monsterAffinityMatchup.Add(MonsterList.Humpback, ElementalAffinity.Water);
            monsterAffinityMatchup.Add(MonsterList.Tripod, ElementalAffinity.Mechanical);
            monsterAffinityMatchup.Add(MonsterList.MiniLandShark, ElementalAffinity.Water);
        }

        private void Start()
        {
            
        }


        internal  AttackResolution SendAttack(AttackRequest data)
        {
            MonsterDna target = null;
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

            //determine hit 
            var swing = UnityEngine.Random.Range(1, 101);
            if(swing > attack.Accuracy)
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
                TargetId = target.MonsterId,
                AttackPerformed = attack,
                Success = true,
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
