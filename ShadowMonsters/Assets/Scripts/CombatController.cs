using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Infrastructure;
using Assets.ServerStubHome;
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
        private CombatPlayerController combatPlayerController;
        private PlayerController playerController;     
        private TextLogDisplayManager _textLogDisplayManager;        
        private AreaSpawnManager _areaSpawnManager;
        private FatbicController fatbicController;
        private ServerStub serverStub;
        private bool combatEnded;
        private Guid attackInstanceId;

        private EnemyController enemyController;

        // Use this for initialization
        void Start()
        {
            serverStub = ServerStub.Instance();
            playerController = PlayerController.Instance();
            _beginCombatPopup = BeginCombatPopup.Instance();
            _monsterSpawner = MonsterSpawner.Instance();
            _textLogDisplayManager = TextLogDisplayManager.Instance();
            _areaSpawnManager = AreaSpawnManager.Instance();
            fatbicController = FatbicController.Instance();
            fatbicController.AttackAttempt += AttackEnemyAttempt;
            enemyController = EnemyController.Instance();
            combatPlayerController = CombatPlayerController.Instance();

            serverStub.SetCombatInstance(this);
            enemyController.SpawnEnemy();
            attackInstanceId = serverStub.CreateAttackSequence(enemyController.enemyInfo.MonsterId, playerController.Id);

            _textLogDisplayManager.AddText(string.Format("You have been attacked by a {0}!", enemyController.enemyInfo.DisplayName), AnnouncementType.Enemy);
            _beginCombatPopup.PromptUserAction(enemyController.enemyInfo.DisplayName, OnFight, OnRun, OnBond);
            
        }

        private void AttackEnemyAttempt(object sender, DataEventArgs<AttackInfo> e)
        {
            if (combatEnded) return;
            //eventually attacks would need to be mapped to animations
            combatPlayerController.DoAnimation(AnimationAction.Attack);
            //this would probably be an id to an attack instead of the attack
            if(e.Data.IsGenerator)
                playerController.CollectResources(e.Data.Affinity);

            AttackResolution attackResult = serverStub.RouteAttack(
                new AttackRequest {
                                     InstanceId = attackInstanceId,
                                     AttackId = e.Data.AttackId,
                                  });
            if (attackResult == null) return;  //attack instance has closed
            

            enemyController.ResolveAttack(attackResult);
            if (attackResult.Success)
            {
                _textLogDisplayManager.AddText(string.Format("Your {0}{1} hit {2} for {3} damage!", attackResult.AttackPerformed.Name, attackResult.WasCritical ? " critically" : "", enemyController.enemyInfo.DisplayName, attackResult.Damage.ToString()), AnnouncementType.Friendly);
            }
            else
            {
                _textLogDisplayManager.AddText(string.Format("Your {0} missed {1}.", attackResult.AttackPerformed.Name, enemyController.enemyInfo.DisplayName), AnnouncementType.Friendly);
            }
                
            if (attackResult.WasFatal)
            {
                combatEnded = true;
                combatPlayerController.EndCombat();
                combatPlayerController.DoAnimation(AnimationAction.Victory);
                _textLogDisplayManager.AddText(string.Format("You have defeated a {0}!", enemyController.enemyInfo.DisplayName), AnnouncementType.Friendly);
                StartCoroutine(EndCombat());               
            }

               
        }

        public void HandleEnemyAttackOnPlayer(AttackResolution attackResult)
        {
            if (attackResult.Success)
            {
                _textLogDisplayManager.AddText(string.Format("{0}: {1}'s {2}{3} hit you for {4} damage!", attackResult.TimeStamp,enemyController.enemyInfo.DisplayName, attackResult.AttackPerformed.Name, attackResult.WasCritical ? " critically" : "", attackResult.Damage.ToString()), AnnouncementType.Enemy);
            }
            else
            {
                _textLogDisplayManager.AddText(string.Format("{0}: {1}'s {2} missed you.",attackResult.TimeStamp,enemyController.enemyInfo.DisplayName, attackResult.AttackPerformed.Name), AnnouncementType.Enemy);
            }

            combatPlayerController.UpdateTeamHealth(attackResult);

            enemyController.ResolveMyAttacks(attackResult);
        }
        private IEnumerator EndCombat()
        {
            while (true)
            {
                yield return new WaitForSeconds(4f);
         
                UnloadCombatScene();
                enemyController.EndCombat();
                combatEnded = false; //reset for next fight
            }
        }


        // Update is called once per frame
        void Update()
        {

        }

        void OnFight()
        {            
            fatbicController.BeginAttack(OnAttackOnePressed, OnAttackTwoPressed, OnAttackThreePressed, OnAttackFourPressed, OnAttackFivePressed, OnStopAttackPressed, OnBond, OnRun);
            combatPlayerController.StartCombat(attackInstanceId);
            serverStub.StartCombat(attackInstanceId);
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

            if (UnityEngine.Random.Range(0, 101) < 95)
            {
                serverStub.EndAttackInstance(attackInstanceId);
                combatPlayerController.EndCombat();
                UnloadCombatScene();
                _textLogDisplayManager.AddText("You successfully ran away.", AnnouncementType.Friendly);
            }
            else
            {
                //start combat
                _textLogDisplayManager.AddText(string.Format("The {0} blocks your path! You have been forced into combat.", enemyController.enemyInfo.DisplayName), AnnouncementType.Enemy);
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
