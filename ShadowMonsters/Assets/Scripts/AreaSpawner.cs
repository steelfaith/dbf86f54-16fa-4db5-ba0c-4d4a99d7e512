using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class AreaSpawner : MonoBehaviour
    {

        public int numberTotal;
        public float delayInSeconds;
        public Transform shadePrefab;
        public Transform spawner;
        List<GameObject> spawnedMonsters = new List<GameObject>();
        private AreaSpawnManager _spawnManager;



        // Use this for initialization
        void Start()
        {
            InvokeRepeating("Spawn", 0.0f, delayInSeconds > 4 ? delayInSeconds : 4);
            _spawnManager = AreaSpawnManager.Instance();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Spawn()
        {
            if (spawnedMonsters.Count >= numberTotal) return;

            var monsterToSpawn = shadePrefab;
            if (monsterToSpawn == null)
            {
                Debug.LogError("Could not find Monster Prefab ");
                return;
            }

            var spawnLocation = spawner.localPosition;


            spawnLocation.x = spawnLocation.x + Random.Range(0, 50);
            spawnLocation.z = spawnLocation.z + Random.Range(0, 50);

            var spawnedMonster = Instantiate(monsterToSpawn, spawnLocation, Quaternion.identity);



            spawnedMonsters.Add(spawnedMonster.gameObject);
            _spawnManager.AddSpawn(spawnedMonster.gameObject);
        }

    }
}
