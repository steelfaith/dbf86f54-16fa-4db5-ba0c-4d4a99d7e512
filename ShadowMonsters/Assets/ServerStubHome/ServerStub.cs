using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Infrastructure;
using UnityEngine;
using Assets.ServerStubHome.Monsters;
using Utility = Assets.Infrastructure.Utility;
using Common.Messages;


namespace Assets.ServerStubHome
{
    public class ServerStub : MonoBehaviour
    {
        Dictionary<MonsterList, ElementalAffinity> monsterAffinityMatchup = new Dictionary<MonsterList, ElementalAffinity>();
        Dictionary<Guid,IMonsterDna> spawnedMonsters = new Dictionary<Guid,IMonsterDna>();
        Dictionary<Guid, PlayerData> players = new Dictionary<Guid, PlayerData>();
        Dictionary<Guid, AttackInstance> attackInstances = new Dictionary<Guid, AttackInstance>();
        KnownAttacks knownAttacks;

        public ServerStub()
        {
            ServerMessageQueue = new Queue<ServerAnnouncement>();
            AttackInstanceEndedQueue = new Queue<AttackInstanceEnded>();
            PlayerDataUpdateQueue = new Queue<PlayerDataUpdate>();
            Amdm = new AffinityMatchupDamageMultiplier();
            DnaFactory.RegisterMonster(MonsterList.DemonEnforcer);
            DnaFactory.RegisterMonster(MonsterList.Dragonling);
            DnaFactory.RegisterMonster(MonsterList.RobotShockTrooper);
            DnaFactory.RegisterMonster(MonsterList.GreenSpider);
            DnaFactory.RegisterMonster(MonsterList.Humpback);
            DnaFactory.RegisterMonster(MonsterList.Tripod);
            DnaFactory.RegisterMonster(MonsterList.RhinoVirus);
            DnaFactory.RegisterMonster(MonsterList.MiniLandShark);

        }

        public AffinityMatchupDamageMultiplier Amdm { get; set; }
        public Queue<ServerAnnouncement> ServerMessageQueue { get; set; }
        public Queue<AttackInstanceEnded> AttackInstanceEndedQueue { get; set; }
        public Queue<PlayerDataUpdate> PlayerDataUpdateQueue { get; set; }

        public IMonsterDna GetRandomMonster()
        {
            MonsterList monster = (MonsterList)Enum.Parse(typeof(MonsterList), Utility.GetRandomEnumMember<MonsterList>());


            var enemyMonster = DnaFactory.CreateRandomMonsterDna();
            enemyMonster.AttackIds = GetAttackIdList(KnownAttacks.KnownMonsterAttackList);

                                                                             
            spawnedMonsters[enemyMonster.MonsterId] = enemyMonster;
            return enemyMonster;
        }

        public ServerAnnouncement GetNextServerMessage(Guid playerId)
        {
            //TODO: all messages going to one client currently...but prob need to consider this for real server
            if(ServerMessageQueue.Count > 0)
                return ServerMessageQueue.Dequeue();
            return null;
        }

        public AttackInstanceEnded GetNextAttackInstanceEndedMessage(Guid playerId)
        {
            //TODO: all messages going to one client currently...but prob need to consider this for real server
            if (AttackInstanceEndedQueue.Count > 0)
                return AttackInstanceEndedQueue.Dequeue();
            return null;
        }

        public PlayerDataUpdate GetNextPlayerDataUpdate(Guid playerId)
        {
            //TODO: all messages going to one client currently...but prob need to consider this for real server
            if (PlayerDataUpdateQueue.Count > 0)
                return PlayerDataUpdateQueue.Dequeue();
            return null;
        }

        public AttackResolution GetNextAttackResult(Guid attackInstanceId)
        {
            AttackInstance instance;
            attackInstances.TryGetValue(attackInstanceId, out instance);
            if (instance == null) return null;
            if(instance.attackResultsQueue.Count > 0)
                return instance.attackResultsQueue.Dequeue();
            return null;
        }

        public EnemyAttackUpdate GetNextEnemyAttackUpdate(Guid attackInstanceId)
        {
            AttackInstance instance;
            attackInstances.TryGetValue(attackInstanceId, out instance);
            if (instance == null) return null;
            if (instance.enemyAttackUpdateQueue.Count > 0)
                return instance.enemyAttackUpdateQueue.Dequeue();
            return null;
        }

        public EnemyResourceDisplayUpdate GetNextEnemyResourceUpdate(Guid attackInstanceId)
        {
            AttackInstance instance;
            attackInstances.TryGetValue(attackInstanceId, out instance);
            if (instance == null) return null;
            if (instance.enemyResourceDisplayUpdateQueue.Count > 0)
                return instance.enemyResourceDisplayUpdateQueue.Dequeue();
            return null;
        }

        public ButtonPressResolution GetNextButtonUpdate(Guid attackInstanceId)
        {
            AttackInstance instance;
            attackInstances.TryGetValue(attackInstanceId, out instance);
            if (instance == null) return null;
            if(instance.buttonPressQueue.Count > 0)
                return instance.buttonPressQueue.Dequeue();
            return null;
        }

        public ResourceUpdate GetNextAddResourceUpdate(Guid attackInstanceId)
        {
            AttackInstance instance;
            attackInstances.TryGetValue(attackInstanceId, out instance);
            if (instance == null) return null;
            if(instance.playerResourceUpdateQueue.Count >0)
                return instance.playerResourceUpdateQueue.Dequeue();
            return null;
        }

        public AttackPowerChangeResolution GetNextAttackPowerUpdate(Guid attackInstanceId)
        {
            AttackInstance instance;
            attackInstances.TryGetValue(attackInstanceId, out instance);
            if (instance == null) return null;
            if(instance.attackPowerChangeUpdateQueue.Count > 0)
                return instance.attackPowerChangeUpdateQueue.Dequeue();
            return null;
            
        }

        public void ChangeAttackPower(AttackPowerChangeRequest request)
        {
            AttackInstance instance;
            attackInstances.TryGetValue(request.AttackInstanceId, out instance);
            if (instance == null) return;

            instance.HandleAttackPowerChange(request);
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

        public IMonsterDna GetMonsterById(Guid id)
        {
            IMonsterDna target;
            spawnedMonsters.TryGetValue(id, out target);
            return target;
        }

        private PlayerData CreatePlayerData(Guid id)
        {
            var team = new List<IMonsterDna>
                                {
                                    new MonsterDna(MonsterList.RhinoVirus,  UnityEngine.Random.Range(1,101))
                                    {
                                        MonsterAffinity = monsterAffinityMatchup[MonsterList.RhinoVirus],
                                        NickName = "Rhinasephalasaurus",
                                        MonsterId = Guid.NewGuid(),
                                        TeamOrder = 1,
                                        AttackIds = GetAttackIdList(KnownAttacks.KnownMonsterAttackList),
                                        MonsterPresence = MonsterPresence.Carnal,
                                        Sizing =(Size)Enum.Parse(typeof(Size), Utility.GetRandomEnumMember<Size>()),

        },
                                    new MonsterDna(MonsterList.DemonEnforcer,  UnityEngine.Random.Range(1,101))
                                    {
                                        MonsterAffinity = monsterAffinityMatchup[MonsterList.DemonEnforcer],
                                        NickName = "Fluffy",
                                        MonsterId = Guid.NewGuid(),
                                        AttackIds = GetAttackIdList(KnownAttacks.KnownMonsterAttackList),
                                        TeamOrder =2,
                                        MonsterPresence = MonsterPresence.Intangible,
                                        Sizing =(Size)Enum.Parse(typeof(Size), Utility.GetRandomEnumMember<Size>()),
                                    },
                                    new MonsterDna(MonsterList.MiniLandShark,  UnityEngine.Random.Range(1,101))
                                    {
                                        MonsterAffinity = monsterAffinityMatchup[MonsterList.MiniLandShark],
                                        NickName = "Big Eddy",
                                        MonsterId = Guid.NewGuid(),
                                        AttackIds = GetAttackIdList(KnownAttacks.KnownMonsterAttackList),
                                        TeamOrder = 3,
                                        MonsterPresence = MonsterPresence.Corporeal,
                                        Sizing =(Size)Enum.Parse(typeof(Size), Utility.GetRandomEnumMember<Size>()),
                                    },
                                };

            foreach (IMonsterDna Monster in team)
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
                    MonsterId = id,
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
            AttackInstanceEndedQueue.Enqueue(new AttackInstanceEnded
            {
                InstanceId = attackInstanceId
            });

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

        public void RevivePlayer(ReviveRequest request)
        {
            PlayerData player;
            players.TryGetValue(request.PlayerId, out player);
            if (player == null) return;
            HealPlayer(player);
            PlayerDataUpdateQueue.Enqueue(new PlayerDataUpdate
            {
                Update = player,
            });

        }

        private void HealPlayer(PlayerData player)
        {
            player.PlayerDna.CurrentHealth = player.PlayerDna.MaxHealth;
            foreach (IMonsterDna member in player.CurrentTeam)
            {
                member.CurrentHealth = member.MaxHealth;
            }
        }

        internal bool CheckPulse(Guid monsterId)
        {
            IMonsterDna target;
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

        public void ShortCastAttempt(ShortCastRequest request)
        {
            AttackInstance ai;
            if (attackInstances.TryGetValue(request.InstanceId, out ai))
                ai.AttemptPlayerShortCast();

        }

        public bool CanPerformAttack(Guid AttackId, Guid attacker)
        {
            IMonsterDna monster;
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
