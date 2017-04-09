using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Infrastructure;
using UnityEngine;
using Assets.Scripts;
using System.Collections;

namespace Assets.ServerStubHome
{
    public class ServerStub : MonoBehaviour
    {
        Dictionary<MonsterList, ElementalAffinity> monsterAffinityMatchup = new Dictionary<MonsterList, ElementalAffinity>();
        Dictionary<Guid,MonsterDna> spawnedMonsters = new Dictionary<Guid,MonsterDna>();
        Dictionary<Guid, PlayerData> players = new Dictionary<Guid, PlayerData>();
        Dictionary<Guid, AttackInstance> attackInstances = new Dictionary<Guid, AttackInstance>();
        KnownAttacks knownAttacks;

        public MonsterDna GetRandomMonster()
        {
            MonsterList monster = (MonsterList)Enum.Parse(typeof(MonsterList), GetRandomKey<MonsterList>());
            MonsterPresence presence = (MonsterPresence)Enum.Parse(typeof(MonsterPresence), GetRandomKey<MonsterPresence>());

             var enemyMonster = new MonsterDna(monster, UnityEngine.Random.Range(1, 101)) {
                                                                                            MonsterAffinity = monsterAffinityMatchup[monster],
                                                                                            MonsterId = Guid.NewGuid(), MonsterPresence =presence,
                                                                                            AttackIds = GetAttackIdList(KnownAttacks.KnownMonsterAttackList)
                                                                                       };
            spawnedMonsters[enemyMonster.MonsterId] = enemyMonster;
            return enemyMonster;
        }

        public AttackResolution GetNextAttackResult(Guid attackInstanceId)
        {
            AttackInstance instance;
            attackInstances.TryGetValue(attackInstanceId, out instance);
            if (instance == null) return null;
            return instance.attackResultsQueue.Dequeue();
        }

        public ButtonPressResolution GetNextButtonUpdate(Guid attackInstanceId)
        {
            AttackInstance instance;
            attackInstances.TryGetValue(attackInstanceId, out instance);
            if (instance == null) return null;
            return instance.buttonPressQueue.Dequeue();
        }

        public ResourceUpdate GetNextAddResourceUpdate(Guid attackInstanceId)
        {
            AttackInstance instance;
            attackInstances.TryGetValue(attackInstanceId, out instance);
            if (instance == null) return null;
            return instance.playerResourceUpdateQueue.Dequeue();
        }

        public Guid CreateAttackSequence(Guid monsterId, Guid playerId)
        {
            var instanceId = Guid.NewGuid();
            var attackInstance = new AttackInstance(monsterId, playerId, instanceId);
            attackInstances[instanceId] = attackInstance;
            return instanceId;
        }

        internal PlayerData GetPlayerData(Guid id)
        {
            PlayerData outData;
            players.TryGetValue(id, out outData);
            return outData;
        }

        public MonsterDna GetMonsterById(Guid id)
        {
            MonsterDna target;
            spawnedMonsters.TryGetValue(id, out target);
            return target;
        }

        private PlayerData CreatePlayerData(Guid id)
        {
            var team = new List<MonsterDna>
                                {
                                    new MonsterDna(MonsterList.RhinoVirus,  UnityEngine.Random.Range(1,101))
                                    {
                                        MonsterAffinity = monsterAffinityMatchup[MonsterList.RhinoVirus],
                                        NickName = "Rhinasephalasaurus",
                                        MonsterId = Guid.NewGuid(),
                                        TeamOrder = 1,
                                        AttackIds = GetAttackIdList(KnownAttacks.KnownMonsterAttackList),
                                        MonsterPresence = MonsterPresence.Carnal,

                                    },
                                    new MonsterDna(MonsterList.DemonEnforcer,  UnityEngine.Random.Range(1,101))
                                    {
                                        MonsterAffinity = monsterAffinityMatchup[MonsterList.DemonEnforcer],
                                        NickName = "Fluffy",
                                        MonsterId = Guid.NewGuid(),
                                        AttackIds = GetAttackIdList(KnownAttacks.KnownMonsterAttackList),
                                        TeamOrder =2,
                                        MonsterPresence = MonsterPresence.Intangible,
                                    },
                                    new MonsterDna(MonsterList.MiniLandShark,  UnityEngine.Random.Range(1,101))
                                    {
                                        MonsterAffinity = monsterAffinityMatchup[MonsterList.MiniLandShark],
                                        NickName = "Big Eddy",
                                        MonsterId = Guid.NewGuid(),
                                        AttackIds = GetAttackIdList(KnownAttacks.KnownMonsterAttackList),
                                        TeamOrder = 3,
                                        MonsterPresence = MonsterPresence.Corporeal,
                                    },
                                };

            foreach (MonsterDna Monster in team)
            {
                spawnedMonsters[Monster.MonsterId] = Monster;
            }

            var data = new PlayerData
            {
                Id = id,
                
                CurrentTeam = team,
                AttackIds = GetAttackIdList(KnownAttacks.KnownPlayerAttackList),
                PlayerDna = new MonsterDna(MonsterList.unitychan,50)
                {
                    MaxHealth = 150,
                    CurrentHealth = 150,
                    NickName = "OFFHIZMEDZ",
                }
                        
            };
            
            
            return data;
        }

        public void EndAttackInstance(Guid attackInstanceId)
        {
            AttackInstance instance;
            attackInstances.TryGetValue(attackInstanceId, out instance);
            attackInstances.Remove(attackInstanceId);
            instance.Dispose();
        }

        private List<Guid> GetAttackIdList(Dictionary<Guid,AttackInfo> fromDictionary)
        {
            var attackList = new List<Guid>();

            while (attackList.Count < 5)
            {
                var attack = fromDictionary.ElementAt(UnityEngine.Random.Range(0, KnownAttacks.KnownMonsterAttackList.Count));
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

        public void KillMonster(Guid id)
        {
            spawnedMonsters.Remove(id);
        }

        private List<AttackInfo> GetAttackInfoFromIds(List<Guid> attackIds)
        {
            var returnList = new List<AttackInfo>();
            foreach (var attackId in attackIds)
            {
               returnList.Add(KnownAttacks.AllKnownAttackList[attackId]);
            }

            return returnList;
        }

        private static string GetRandomKey<T>()
        {
            
            var list = Enum.GetNames(typeof(T)).ToList();
            list.Remove("unitychan");

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

        public void PlayerAttackAttempt(AttackRequest data)
        {
            AttackInstance ai;
            if(attackInstances.TryGetValue(data.InstanceId, out ai))
                       ai.HandlePlayerAttack(data);            

        }

        public bool CanPerformAttack(Guid AttackId, Guid attacker)
        {
            MonsterDna monster;
            spawnedMonsters.TryGetValue(attacker, out monster);
            if (monster == null) return false;
            return monster.AttackIds.Any(x => x == AttackId);
        }

        public void UpdateAttackInstance(AttackUpdateRequest update)
        {
            AttackInstance ai;
            if (attackInstances.TryGetValue(update.AttackInstanceId, out ai))
                ai.UpdatePlayerChampion(update.CurrentPlayerChampionId);
        }

        public void StartCombat(Guid attackInstanceId)
        {
            AttackInstance ai;
            if (attackInstances.TryGetValue(attackInstanceId, out ai))
                 ai.StartCombat();
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
