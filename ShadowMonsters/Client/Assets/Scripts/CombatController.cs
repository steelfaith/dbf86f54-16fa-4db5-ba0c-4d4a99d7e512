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
            fatbicController.BeginAttack(OnAttackOnePressed, OnAttackTwoPressed, OnAttackThreePressed, OnAttackFourPressed, OnAttackFivePressed, OnStopAttackPressed, OnBond,OnRun);            
        }

        private void OnAttackOnePressed()
        {
            fatbicController.attackOneButton.GetComponent<ButtonScript>().StartButtonAction();
        }

        private void OnAttackTwoPressed()
        {
            fatbicController.attackTwoButton.GetComponent<ButtonScript>().StartButtonAction();
        }

        private void OnAttackThreePressed()
        {
            fatbicController.attackThreeButton.GetComponent<ButtonScript>().StartButtonAction();
        }

        private void OnAttackFourPressed()
        {
            fatbicController.attackFourButton.GetComponent<ButtonScript>().StartButtonAction();
        }

        private void OnAttackFivePressed()
        {
            fatbicController.attackFiveButton.GetComponent<ButtonScript>().StartButtonAction();
        }
        private void OnStopAttackPressed()
        {
            //TODO unhardcode 5
            //fatbicController.stopAttackButton.GetComponent<ButtonScript>().StartRecharge(5);
            //fatbicController.StartGlobalRecharge();
        }
        void OnRun()
        {
            var runButtonScript = fatbicController.runButton.GetComponent<ButtonScript>();
            runButtonScript.StartCooldown(2);
            fatbicController.StartGlobalRecharge(2, runButtonScript.attackIndex);
            _textLogDisplayManager.AddText("You attempt to run away.", AnnouncementType.Friendly);
            if(_player.ControlledCreatures.Any(x=>x.GetComponent<BaseCreature>().Level + Random.Range(1,15) > _enemyInfo.Level ))
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
        void OnBond()
        {
            var bondButtonScript = fatbicController.bondButton.GetComponent<ButtonScript>();
            bondButtonScript.StartCooldown(10);
            fatbicController.StartGlobalRecharge(10, bondButtonScript.attackIndex);
        }

       private void UnloadCombatScene()
        {
            _monsterSpawner.DestroyAllSpawns();
            _areaSpawnManager.DestroyAllSpawns();
            AnyManager._anyManager.UnloadCombatScene();
        }
    }
}
