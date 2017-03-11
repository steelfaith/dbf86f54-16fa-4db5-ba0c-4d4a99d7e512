using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Infrastructure;
using System.Linq;
using UnityEngine.SceneManagement;
using System;

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
        private StatusController enemyStatusController;
        private ServerStub serverStub;

        

        // Use this for initialization
        void Start()
        {
            serverStub = ServerStub.Instance();
            _beginCombatPopup = BeginCombatPopup.Instance();
            _monsterSpawner = MonsterSpawner.Instance();
            _textLogDisplayManager = TextLogDisplayManager.Instance();
            _areaSpawnManager = AreaSpawnManager.Instance();
            fatbicController = FatbicController.Instance();
            fatbicController.AttackAttempt += AttackEnemyAttempt;
            enemyStatusController = StatusController.Instance();
            _player = Player.Instance();

            _enemy = _monsterSpawner.SpawnRandomEnemyMonster();
            _enemy.SetActive(true);
            _enemyInfo = _enemy.GetComponent<BaseCreature>();
            enemyStatusController.SetCreature(_enemyInfo);

            _textLogDisplayManager.AddText(string.Format("You have been attack by a {0}!", _enemyInfo.Name), AnnouncementType.Enemy);
            _beginCombatPopup.PromptUserAction(_enemyInfo.Name, OnFight, OnRun, OnBond);

        }

        private void AttackEnemyAttempt(object sender, DataEventArgs<AttackInfo> e)
        {
            //this would probably be an id to an attack instead of the attack
            AttackResolution attackResult = serverStub.SendAttack(
                new AttackRequest {
                                     AttackId = e.Data.AttackId,
                                     TargetId = _enemyInfo.MonsterId
                                  });
            if (attackResult == null) return;
            enemyStatusController.UpdateCreature(attackResult);
            if (attackResult.WasFatal)
            {
                EndCombat();                
            }

               
        }

        private void EndCombat()
        {
            Destroy(_enemy);
            UnloadCombatScene();
            _textLogDisplayManager.AddText(string.Format("You have defeated {0}!",_enemyInfo.Name), AnnouncementType.Friendly);
            _enemyInfo = null;
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
            if(_player.ControlledCreatures.Any(x=>x.GetComponent<BaseCreature>().Level + UnityEngine.Random.Range(1,15) > _enemyInfo.Level ))
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
