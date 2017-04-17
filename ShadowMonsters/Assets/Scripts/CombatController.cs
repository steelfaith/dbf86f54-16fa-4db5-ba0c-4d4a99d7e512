using System.Collections;
using UnityEngine;
using Assets.Infrastructure;
using Assets.ServerStubHome;
using System;

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
        private FatbicDisplayController fatbicController;
        private ServerStub serverStub;
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
            fatbicController = FatbicDisplayController.Instance();
            enemyController = EnemyController.Instance();
            combatPlayerController = CombatPlayerController.Instance();

            enemyController.SpawnEnemy();
            attackInstanceId = serverStub.CreateAttackSequence(enemyController.enemyInfo.MonsterId, playerController.Id);
            fatbicController.AttackInstanceId = attackInstanceId;
            enemyController.AttackInstanceId = attackInstanceId;

            _textLogDisplayManager.AddText(string.Format("You have been attacked by a {0}!", enemyController.enemyInfo.DisplayName), AnnouncementType.Enemy);
            _beginCombatPopup.PromptUserAction(enemyController.enemyInfo.DisplayName, OnFight, OnRun, OnBond);
            
        }

        public void HandleAttackResolution(AttackResolution attackResult)
        {
            if (attackResult == null) return;
            if(attackResult.TargetId == combatPlayerController.CurrentCombatantId)
            {
                HandleAttackOnPlayerDisplay(attackResult);
            }
            
            if(attackResult.TargetId == enemyController.enemyInfo.MonsterId)
            {
                HandleAttackOnEnemyDisplay(attackResult);
            }

        }

        private void HandleAttackOnPlayerDisplay(AttackResolution attackResult)
        {
            if (attackResult.Success)
            {
                _textLogDisplayManager.AddText(string.Format("{0}: {1}'s {2}{3} hit you for {4} damage!", attackResult.TimeStamp, enemyController.enemyInfo.DisplayName, attackResult.AttackPerformed.Name, attackResult.WasCritical ? " critically" : "", attackResult.Damage.ToString()), AnnouncementType.Enemy);
                combatPlayerController.UpdateTeamHealth(attackResult);
                enemyController.ResolveMyAttacks(attackResult);
            }
            else
            {
                _textLogDisplayManager.AddText(string.Format("{0}: {1}'s {2} missed you.", attackResult.TimeStamp, enemyController.enemyInfo.DisplayName, attackResult.AttackPerformed.Name), AnnouncementType.Enemy);
            }
        }

        private void HandleAttackOnEnemyDisplay(AttackResolution attackResult)
        {
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
                combatPlayerController.EndCombat();
                _textLogDisplayManager.AddText(string.Format("You have defeated a {0}!", enemyController.enemyInfo.DisplayName), AnnouncementType.Friendly);                               
            }
        }
        private IEnumerator EndCombat()
        {
            yield return new WaitForSeconds(2f);

            UnloadCombatScene();
            enemyController.EndCombat();

        }


        // Update is called once per frame
        void Update()
        {
            StartCoroutine(CheckForAttacks());
            StartCoroutine(CheckForEndCombat());
        }

        public IEnumerator CheckForAttacks()
        {
            var attackRes = serverStub.GetNextAttackResult(attackInstanceId);
            if (attackRes == null)
            {
                yield return null;
            }
            HandleAttackResolution(attackRes);
        }

        public IEnumerator CheckForEndCombat()
        {
            var endCombat = serverStub.GetNextAttackInstanceEndedMessage(playerController.Id);
            if (endCombat == null)
            {
                yield return null;
            }
            else
            { HandleCombatEndMessage(endCombat); }
        }

        private void HandleCombatEndMessage(AttackInstanceEnded endCombat)
        {
            StartCoroutine(EndCombat());
        }

        void OnFight()
        {            
            fatbicController.BeginAttack(OnAttackOnePressed, OnAttackTwoPressed, OnAttackThreePressed, OnAttackFourPressed, OnAttackFivePressed, OnStopAttackPressed, OnBond, OnRun);
            combatPlayerController.StartCombat(attackInstanceId);
            serverStub.StartCombat(attackInstanceId);
        }

        private void SendAttack(Guid attackId)
        {
            fatbicController.IsBusy = true;
            //eventually attacks would need to be mapped to animations
            combatPlayerController.DoAnimation(AnimationAction.Attack);
            serverStub.PlayerAttackAttempt(new AttackRequest
            {
                AttackId = attackId,
                InstanceId = attackInstanceId
            });
        }

        private void OnAttackOnePressed()
        {
            SendAttack(fatbicController.attackOneButton.GetComponent<ButtonScript>().attackInfo.AttackId);
        }

        private void OnAttackTwoPressed()
        {
            SendAttack(fatbicController.attackTwoButton.GetComponent<ButtonScript>().attackInfo.AttackId);
        }

        private void OnAttackThreePressed()
        {
            SendAttack(fatbicController.attackThreeButton.GetComponent<ButtonScript>().attackInfo.AttackId);
        }

        private void OnAttackFourPressed()
        {
            SendAttack(fatbicController.attackFourButton.GetComponent<ButtonScript>().attackInfo.AttackId);
        }

        private void OnAttackFivePressed()
        {
            SendAttack(fatbicController.attackFiveButton.GetComponent<ButtonScript>().attackInfo.AttackId);
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
