using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Infrastructure;
using System.Linq;
using UnityEngine.SceneManagement;
using System;
using System.Threading;

namespace Assets.Scripts
{
    public class CombatController : MonoBehaviour
    {
        private BeginCombatPopup _beginCombatPopup;
        private MonsterSpawner _monsterSpawner;
        private Player _player;        
        private TextLogDisplayManager _textLogDisplayManager;        
        private AreaSpawnManager _areaSpawnManager;
        private FatbicController fatbicController;
        private ServerStub serverStub;

        private EnemyController enemyController;

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
            enemyController = EnemyController.Instance();

            _player = Player.Instance();
            enemyController.SpawnEnemy();
            


            _textLogDisplayManager.AddText(string.Format("You have been attacked by a {0}!", enemyController.enemyInfo.Name), AnnouncementType.Enemy);
            _beginCombatPopup.PromptUserAction(enemyController.enemyInfo.Name, OnFight, OnRun, OnBond);

        }

        private void AttackEnemyAttempt(object sender, DataEventArgs<AttackInfo> e)
        {
            //this would probably be an id to an attack instead of the attack
            AttackResolution attackResult = serverStub.SendAttack(
                new AttackRequest {
                                     AttackId = e.Data.AttackId,
                                     TargetId = enemyController.enemyInfo.MonsterId
                                  });
            enemyController.ResolveAttack(attackResult);
            if (attackResult == null) return;


            if (attackResult.WasFatal)
            {                
                _player.RevertIncarnation();
                _textLogDisplayManager.AddText(string.Format("You have defeated a {0}!", enemyController.enemyInfo.Name), AnnouncementType.Friendly);
                StartCoroutine(EndCombat());               
            }

               
        }
        private IEnumerator EndCombat()
        {
            while (true)
            {
                yield return new WaitForSeconds(4f);
         
                UnloadCombatScene();
                enemyController.EndCombat();
            }
        }


        // Update is called once per frame
        void Update()
        {

        }

        void OnFight()
        {
            var leadMonster =_player.IncarnateMonster();
            enemyController.StartEnemyAttack(leadMonster);

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
            if(_player.ControlledMonsters.Any(x=>x.GetComponent<BaseMonster>().Level + UnityEngine.Random.Range(100,150) > enemyController.enemyInfo.Level ))
            {
                UnloadCombatScene();
                _textLogDisplayManager.AddText("You successfully ran away.", AnnouncementType.Friendly);
            }
            else
            {
                //start combat
                _textLogDisplayManager.AddText(string.Format("The {0} blocks your path! You have been forced into combat.", enemyController.enemyInfo.Name), AnnouncementType.Enemy);
                OnFight();
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
