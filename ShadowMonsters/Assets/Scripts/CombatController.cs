using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Infrastructure;

namespace Assets.Scripts
{
    public class CombatController : MonoBehaviour
    {
        private BeginCombatPopup _beginCombatPopup;
        private MonsterSpawner _monsterSpawner;

        // Use this for initialization
        void Start()
        {
            _beginCombatPopup = BeginCombatPopup.Instance();
            _monsterSpawner = MonsterSpawner.Instance();

            var mob = _monsterSpawner.SpawnMonster();
            var mobInfo = mob.GetComponent<BaseCreature>();

            _beginCombatPopup.PromptUserAction(mobInfo.Name, OnFight, OnRun, OnBond);

        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnFight() { }
        void OnRun() { }
        void OnBond() { }
    }
}
