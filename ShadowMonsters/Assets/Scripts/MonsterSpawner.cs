
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Assets.Infrastructure;
using System;

namespace Assets.Scripts
{
    //this class would likely need to be partially server side yet the transforms map to actual game objects...??
    public class MonsterSpawner : MonoBehaviour
    {
        private Vector3 _enemyLocation = new Vector3(-5, 300, -11);
        private Vector3 _friendlyLocation = new Vector3(6, 300, 12);
        private Vector3 _enemyRotation = new Vector3(0, 15, 0);
        private Vector3 _friendlyRotation = new Vector3(0, 195, 0);
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

        public GameObject SpawnMonster(CreatureInfo creatureInfo, bool friendly)
        {
            InitializeMonsterCave();
            var monsterToSpawn = _monsterCave.TryGetMonster(creatureInfo);
            if (monsterToSpawn == null)
            {
                Debug.LogError(string.Format("Could not find Monster Prefab named {0}. Add to Monster List enumeration.", creatureInfo.DisplayName));
                return null;
            }

            var spawnLocation = friendly ? _friendlyLocation : _enemyLocation;
            
            var spawnedMonster = Instantiate(monsterToSpawn, spawnLocation, Quaternion.Euler(friendly?_friendlyRotation:_enemyRotation));

            var bc = spawnedMonster.gameObject.GetComponent<BaseCreature>();            
            bc.Name = creatureInfo.DisplayName;
            bc.Level = creatureInfo.Level;
            bc.Health = creatureInfo.MaxHealth;
            bc.MonsterId = creatureInfo.MonsterId;
            bc.NickName = creatureInfo.NickName;
            bc.MonsterRarity = creatureInfo.MonsterRarity;

            //adjust down friendly creatures..they are close to camera
            
            if(friendly)
            {
                var scaleX = spawnedMonster.localScale.x;
                var scaleY = spawnedMonster.localScale.y;
                var scaleZ = spawnedMonster.localScale.z;
                spawnedMonster.localScale = new Vector3(scaleX / 2, scaleY / 2, scaleZ / 2);
            }

            var renderer = spawnedMonster.GetComponent<Renderer>();
            if(renderer != null)
            {
                var newSize = renderer.bounds.size.y /2;
                var localPosition = spawnedMonster.localPosition;
                spawnedMonster.localPosition = new Vector3(localPosition.x, CombatSceneHeight + newSize, localPosition.z);
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
