
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
        private Vector3 _enemyLocation = new Vector3(-5, 1, -11);
        private Vector3 _friendlyLocation = new Vector3(6, 1, 12);
        private Vector3 _enemyRotation = new Vector3(0, 15, 0);
        private Vector3 _friendlyRotation = new Vector3(0, 195, 0);
        private MonsterCave _monsterCave;
        private void Awake()
        {

        }

        void Start()
        {
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
            return SpawnMonster(ServerStub.GetRandomMonster(), false);
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

            monsterToSpawn.localScale = friendly ? new Vector3(5, 5, 5) : new Vector3(10, 10, 10);
            //monsterToSpawn.GetComponent<MeshRenderer>().enabled = friendly ? false : true;
            var spawnedMonster = Instantiate(monsterToSpawn, friendly?_friendlyLocation:_enemyLocation, Quaternion.Euler(friendly?_friendlyRotation:_enemyRotation));

            var bc = spawnedMonster.gameObject.GetComponent<BaseCreature>();            
            bc.Name = creatureInfo.DisplayName;
            bc.Level = creatureInfo.Level;


            return spawnedMonster.gameObject;
        }

        private void InitializeMonsterCave()
        {
            if (!_monsterCave)
                _monsterCave = MonsterCave.Instance();
        }


    }
}
