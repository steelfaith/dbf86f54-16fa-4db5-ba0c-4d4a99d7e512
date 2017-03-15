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
        public Transform DemonEnforcer;
        public Transform RhinoVirus;
        public Transform RobotShockTrooper;
        public Transform GreenSpider;
        public Transform Dragonling;
        public Transform Humpback;

        private Dictionary<string, Transform> _monsterList = new Dictionary<string, Transform>();
        private Dictionary<string, Dictionary<AnimationAction, string>> animationMapping = new Dictionary<string, Dictionary<AnimationAction, string>>();




        private void Awake()
        {
            _monsterList.Add(DemonEnforcer.gameObject.name, DemonEnforcer);
            _monsterList.Add(RhinoVirus.gameObject.name, RhinoVirus);
            _monsterList.Add(RobotShockTrooper.gameObject.name, RobotShockTrooper);
            _monsterList.Add(GreenSpider.gameObject.name, GreenSpider);
            _monsterList.Add(Dragonling.gameObject.name, Dragonling);
            _monsterList.Add(Humpback.gameObject.name, Humpback);

            
            animationMapping[DemonEnforcer.gameObject.name] = new Dictionary<AnimationAction, string>
                                                                            {

                                                                                { AnimationAction.Attack, "creature1Attack1" },
                                                                                { AnimationAction.Die, "creature1Die" },
                                                                                { AnimationAction.GetHit, "creature1GetHit" }
                                                                            };
            animationMapping[RobotShockTrooper.gameObject.name] = new Dictionary<AnimationAction, string>
                                                                            {

                                                                                { AnimationAction.Attack, "hook" },
                                                                                { AnimationAction.Die, "death" },
                                                                                { AnimationAction.GetHit, "bit hit" }
                                                                            };

        }
        private void Start()
        {

        }
        private static MonsterCave _monsterCave;
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

        public string TryGetAnimationName(string nameKey, AnimationAction action)
        {
            Dictionary<AnimationAction, string> animationToStringDict;
            animationMapping.TryGetValue(nameKey, out animationToStringDict);

            if (animationToStringDict == null) return string.Empty;
            return animationToStringDict.FirstOrDefault(x => x.Key == action).Value;
        }
    }
}
