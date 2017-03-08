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
        private TextLogDisplayManager _textLogDisplayManager;
        private BaseCreature _enemyInfo;
        private AreaSpawnManager _areaSpawnManager;
        private FatbicController fatbicController;
        

        // Use this for initialization
        void Start()
        {
            _beginCombatPopup = BeginCombatPopup.Instance();
            _monsterSpawner = MonsterSpawner.Instance();
            _textLogDisplayManager = TextLogDisplayManager.Instance();
            _areaSpawnManager = AreaSpawnManager.Instance();
            fatbicController = FatbicController.Instance();
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

        void OnFight()
        {
            //fatbicController.LoadAttacks()
            fatbicController.BeginAttack(OnAttackOnePressed, OnAttackTwoPressed, OnAttackThreePressed, OnAttackFourPressed, OnAttackFivePressed, OnAttackSixPressed);
        }

        private void OnAttackOnePressed()
        {
            //TODO unhardcode 5
            fatbicController.attackOneButton.GetComponent<ButtonScript>().StartRecharge(5);

        }

        private void OnAttackTwoPressed()
        {
            //TODO unhardcode 5
            fatbicController.attackTwoButton.GetComponent<ButtonScript>().StartRecharge(5);

        }

        private void OnAttackThreePressed()
        {
            //TODO unhardcode 5
            fatbicController.attackThreeButton.GetComponent<ButtonScript>().StartRecharge(5);

        }

        private void OnAttackFourPressed()
        {
            //TODO unhardcode 5
            fatbicController.attackFourButton.GetComponent<ButtonScript>().StartRecharge(5);

        }

        private void OnAttackFivePressed()
        {
            //TODO unhardcode 5
            fatbicController.attackFiveButton.GetComponent<ButtonScript>().StartRecharge(5);

        }
        private void OnAttackSixPressed()
        {
            //TODO unhardcode 5
            fatbicController.attackSixButton.GetComponent<ButtonScript>().StartRecharge(5);

        }
        void OnRun()
        {
            _textLogDisplayManager.AddText("You attempt to run away.", AnnouncementType.Friendly);
            if(_player.ControlledCreatures.Any(x=>x.GetComponent<BaseCreature>().Level > _enemyInfo.Level ))
            {
                UnloadCombatScene();
                _textLogDisplayManager.AddText("You successfully ran away.", AnnouncementType.Friendly);
            }
            else
            {
                //start combat
                _textLogDisplayManager.AddText(string.Format("The {0} blocks your path! You have been forced into combat.", _enemyInfo.Name), AnnouncementType.Enemy);
            }
        }
        void OnBond() { }

       private void UnloadCombatScene()
        {
            _monsterSpawner.DestroyAllSpawns();
            _areaSpawnManager.DestroyAllSpawns();
            AnyManager._anyManager.UnloadCombatScene();
        }
    }
}
