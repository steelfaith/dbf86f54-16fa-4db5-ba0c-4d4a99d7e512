using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class AreaSpawnManager : MonoBehaviour
    {

        public List<GameObject> _spawns = new List<GameObject>();

        private void Awake()
        {
            
        }

        public void DestroyAllSpawns()
        {
            foreach (GameObject spawn in _spawns)
            {
                Destroy(spawn);
            }
            _spawns.Clear();
        }

        internal void AddSpawn(GameObject spawn)
        {
            _spawns.Add(spawn);
        }

        private static AreaSpawnManager _areaSpawnManager;

        public static AreaSpawnManager Instance()
        {
            if (!_areaSpawnManager)
            {
                _areaSpawnManager = FindObjectOfType(typeof(AreaSpawnManager)) as AreaSpawnManager;
                if (!_areaSpawnManager)
                    Debug.LogError("Could not find Area Spawn Manager!");
            }
            return _areaSpawnManager;
        }
    }
}
