using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Infrastructure;

namespace Assets.Scripts
{
    /// <summary>
    /// This script holds ALL the games monsters so we can instantiate them in code
    /// </summary>
    public class MonsterCave : MonoBehaviour
    {
        public Transform SphereOfDoom;
        public Transform MountainDeath;
        public Transform DemonEnforcer;
        public Transform RhinoVirus;
        public Transform RobotShockTrooper;

        private Dictionary<string, Transform> _monsterList = new Dictionary<string, Transform>();

        private static MonsterCave _monsterCave;

        private void Awake()
        {
            _monsterList.Add(SphereOfDoom.gameObject.name, SphereOfDoom);
            _monsterList.Add(MountainDeath.gameObject.name, MountainDeath);
            _monsterList.Add(DemonEnforcer.gameObject.name, DemonEnforcer);
            _monsterList.Add(RhinoVirus.gameObject.name, RhinoVirus);
            _monsterList.Add(RobotShockTrooper.gameObject.name, RobotShockTrooper);
        }
        private void Start()
        {

        }
        public static MonsterCave Instance()
        {
            if (!_monsterCave)
            {
                _monsterCave = FindObjectOfType(typeof(MonsterCave)) as MonsterCave;
                if (!_monsterCave)
                    Debug.LogError("Could not find Monster Cave..where will our monsters live??!");
            }
            return _monsterCave;
        }

        /// <summary>
        /// Gets a monsters transform from the cave.  careful sometimes they bite when disturbed.
        /// </summary>
        /// <param name="creatureInfo"></param>
        /// <returns>null if monster doesn't want to play</returns>
        public Transform TryGetMonster(CreatureInfo creatureInfo)
        {
            Transform monsterTransform;
            _monsterList.TryGetValue(creatureInfo.NameKey, out monsterTransform);
            return monsterTransform;
        }
    }
}
