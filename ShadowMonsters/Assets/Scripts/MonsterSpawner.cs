
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
        public Transform SphereOfDoom;
        public Transform MountainDeath;
        private Dictionary<string, Transform> _monsterList = new Dictionary<string, Transform>();

        private void Awake()
        {
            _monsterList.Add(SphereOfDoom.gameObject.name,SphereOfDoom);
            _monsterList.Add(MountainDeath.gameObject.name,MountainDeath);
        }

        void Start()
        {            

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

        public GameObject SpawnMonster()
        {
            var creatureInfo = ServerStub.GetRandomMonster();

            Transform monsterToSpawn;            
            _monsterList.TryGetValue(creatureInfo.NameKey, out monsterToSpawn);
            if (monsterToSpawn == null)
            {
                Debug.LogError(string.Format("Could not find Monster Prefab named {0}. Add to Monster List enumeration.", creatureInfo.DisplayName));
                return null;
            }

            Instantiate(monsterToSpawn, new Vector3(-5, 1, -11), Quaternion.identity);

            var y = monsterToSpawn.gameObject.GetComponent<BaseCreature>();
            y.Name = creatureInfo.DisplayName;

            return monsterToSpawn.gameObject;
        }


    }
}
