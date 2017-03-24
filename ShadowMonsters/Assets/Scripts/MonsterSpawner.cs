
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Assets.Infrastructure;
using Assets.ServerStubHome;
using System;

namespace Assets.Scripts
{
    //this class would likely need to be partially server side yet the transforms map to actual game objects...??
    public class MonsterSpawner : MonoBehaviour
    {
        private Vector3 _enemyLocation = new Vector3(-5, 300, -11);
        private Vector3 _friendlyLocation = new Vector3(6, 300, 12);
        private const int EnemyRotation = 15;
        private const int FriendlyRotation = 195;
        private MonsterCave _monsterCave;
        private List<GameObject> _spawns = new List<GameObject>();
        private ServerStub serverStub;
        private const int CombatSceneHeight = 300;
        private void Awake()
        {

        }

        void Start()
        {
            serverStub = ServerStub.Instance();
            InitializeMonsterCave();
        }

        private static MonsterSpawner _monsterSpawner;

        public static MonsterSpawner Instance()
        {
            if (!_monsterSpawner)
            {                
                _monsterSpawner = FindObjectOfType(typeof(MonsterSpawner)) as MonsterSpawner;
                if (!_monsterSpawner)
                    Debug.LogError("Could not find Monster Spawner!");
            }
            return _monsterSpawner;
        }
        public GameObject SpawnRandomEnemyMonster()
        {
            return SpawnMonster(serverStub.GetRandomMonster(), false);
        }

        public GameObject SpawnMonster(MonsterDna MonsterInfo, bool friendly)
        {
            InitializeMonsterCave();
            var monsterToSpawn = _monsterCave.TryGetMonster(MonsterInfo);
            if (monsterToSpawn == null)
            {
                Debug.LogError(string.Format("Could not find Monster Prefab named {0}. Add to Monster List enumeration.", MonsterInfo.DisplayName));
                return null;
            }

            var spawnLocation = friendly ? _friendlyLocation : _enemyLocation;

            //rotation
            var currentRot = monsterToSpawn.rotation;

            var friendlyYRot = ((int)currentRot.y + FriendlyRotation) % 360;
            var friendlyRot = new Vector3(currentRot.x, friendlyYRot, currentRot.z);
            var enemyYRot = ((int)currentRot.y + EnemyRotation) % 360;
            var enemyRot = new Vector3(currentRot.x, enemyYRot, currentRot.z);

            var spawnedMonster = Instantiate(monsterToSpawn, spawnLocation, Quaternion.Euler(friendly? friendlyRot:enemyRot));

            var bc = spawnedMonster.gameObject.GetComponent<BaseMonster>();            
            bc.DisplayName = MonsterInfo.DisplayName;
            bc.Level = MonsterInfo.Level;
            bc.MaxHealth = MonsterInfo.MaxHealth;
            bc.CurrentHealth = MonsterInfo.CurrentHealth;
            bc.MonsterId = MonsterInfo.MonsterId;
            bc.NickName = MonsterInfo.NickName;
            bc.MonsterPresence = MonsterInfo.MonsterPresence;
            bc.MonsterAffinity = MonsterInfo.MonsterAffinity;
            bc.NameKey = MonsterInfo.NameKey;
            bc.IsPlayerTeamLead = MonsterInfo.IsTeamLead;

            if(bc.DisplayName == "Humpback Whale")
            {
                //reset rotation for stupid whale
                bc.transform.rotation = Quaternion.Euler(new Vector3(0, 195, 0));
            }

            //adjust down friendly Monsters..they are close to camera
            
            if(friendly)
            {
                var scaleX = spawnedMonster.localScale.x;
                var scaleY = spawnedMonster.localScale.y;
                var scaleZ = spawnedMonster.localScale.z;
                spawnedMonster.localScale = new Vector3(scaleX / 2, scaleY / 2, scaleZ / 2);
            }

            var renderer = spawnedMonster.GetComponentInChildren<Renderer>();
            if (renderer != null)
            {
                //this is how you can change the prefab's color
                //renderer.material.color = new Color32(255, 223, 0, 50); //gold
                //renderer.material.color = new Color32(0, 67, 200, 50);
            }

            _spawns.Add(spawnedMonster.gameObject);
            return spawnedMonster.gameObject;
        }

        private void InitializeMonsterCave()
        {
            if (!_monsterCave)
                _monsterCave = MonsterCave.Instance();
        }

        public void DestroyAllSpawns()
        {
            foreach (GameObject spawn in _spawns)
            {
                Destroy(spawn);
            }
            _spawns.Clear();
        }
    }
}
