
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class MonsterSpawner : MonoBehaviour
    {
        public Transform SphereOfDoom;
        public Transform MountainDeath;
        private List<Transform> _monsterList = new List<Transform>();
        private Random _randomNumber = new Random();

        void Start()
        {
            _monsterList.Add(SphereOfDoom);
            _monsterList.Add(MountainDeath);
            var spawnIndex = Random.Range(0, _monsterList.Count);

            Instantiate(_monsterList[spawnIndex], new Vector3(-5, 1, -11), Quaternion.identity);
        }
    }
}
