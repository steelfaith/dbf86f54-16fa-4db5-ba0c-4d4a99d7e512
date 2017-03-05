using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Infrastructure;
using System.Linq;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class CombatController : MonoBehaviour
    {
        private BeginCombatPopup _beginCombatPopup;
        private MonsterSpawner _monsterSpawner;
        private Player _player;
        private GameObject _enemy;
        private BaseCreature _enemyInfo;

        // Use this for initialization
        void Start()
        {
            _beginCombatPopup = BeginCombatPopup.Instance();
            _monsterSpawner = MonsterSpawner.Instance();
            _player = Player.Instance();

            _enemy = _monsterSpawner.SpawnRandomEnemyMonster();
            _enemy.SetActive(true);
            _enemyInfo = _enemy.GetComponent<BaseCreature>();

            _beginCombatPopup.PromptUserAction(_enemyInfo.Name, OnFight, OnRun, OnBond);

        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnFight() { }
        void OnRun()
        {
            if(_player.ControlledCreatures.Any(x=>x.GetComponent<BaseCreature>().Level > _enemyInfo.Level ))
            {
                SceneManager.LoadScene("TestScene");
                //SceneManager.UnloadSceneAsync("CombatScene");
            }
            else
            {
                //start combat
            }
        }
        void OnBond() { }
    }
}
