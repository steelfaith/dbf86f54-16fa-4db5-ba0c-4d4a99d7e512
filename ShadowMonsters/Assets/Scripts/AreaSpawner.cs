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

            var spawnTrigger = spawner.GetComponent<SphereCollider>();

            var spawnPosition = new Vector3(Random.insideUnitSphere.x* spawnTrigger.radius + spawner.localPosition.x,
                transform.position.y, Random.insideUnitSphere.z* spawnTrigger.radius+ spawner.localPosition.z );

            var spawnedMonster = Instantiate(monsterToSpawn, spawnPosition, Quaternion.identity);

            

            spawnedMonsters.Add(spawnedMonster.gameObject);
            _spawnManager.AddSpawn(spawnedMonster.gameObject);
        }

    }
}
