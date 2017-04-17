using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Infrastructure;
using Assets.ServerStubHome;
using System.Collections;

namespace Assets.Scripts
{
    public class EnemyController : MonoBehaviour
    {
        private GameObject enemy;
        public BaseMonster enemyInfo;
        public GameObject StatusDisplay;
        private MonsterSpawner monsterSpawner;
        private StatusController enemyStatusController;
        private ScrollingCombatTextController scrollingCombatTextController;
        private AnimationController animationController;
        private ServerStub serverStub;
        private TextLogDisplayManager textLogDisplayManager;

        private void Start()
        {
            serverStub = ServerStub.Instance();
            monsterSpawner = MonsterSpawner.Instance();
            enemyStatusController = StatusDisplay.GetComponentInChildren<StatusController>();
            scrollingCombatTextController = ScrollingCombatTextController.Instance();
            animationController = AnimationController.Instance();
            textLogDisplayManager = TextLogDisplayManager.Instance();
        }

        public void Update()
        {
            StartCoroutine(CheckForEnemyAttackUpdates());
            StartCoroutine(CheckForEnemyResourceUdates());
        }

        private IEnumerator CheckForEnemyAttackUpdates()
        {
            var enemyAttackUpdate = serverStub.GetNextEnemyAttackUpdate(AttackInstanceId);
            if (enemyAttackUpdate == null)
            {
                yield return null;
            }
            else
            { HandleEnemyAttackUpdate(enemyAttackUpdate); }
        }

        private IEnumerator CheckForEnemyResourceUdates()
        {
            var enemyResourceUpdate = serverStub.GetNextEnemyResourceUpdate(AttackInstanceId);
            if (enemyResourceUpdate == null)
            {
                yield return null;
            }
            else
            { HandleEnemyResourceUpdate(enemyResourceUpdate); }
        }

        private void HandleEnemyResourceUpdate(EnemyResourceDisplayUpdate enemyResourceUpdate)
        {
            enemyStatusController.UpdateResources(enemyResourceUpdate.Resources);
        }

        private void HandleEnemyAttackUpdate(EnemyAttackUpdate enemyAttackUpdate)
        {
            enemyStatusController.UpdateCastBar(enemyAttackUpdate.Attack);
        }

        private static EnemyController enemyController;

        public Guid AttackInstanceId { get; internal set; }

        public static EnemyController Instance()
        {
            if (!enemyController)
            {
                enemyController = FindObjectOfType(typeof(EnemyController)) as EnemyController;
                if (!enemyController)
                    Debug.LogError("Could not find the enemy controller!");
            }
            return enemyController;
        }

        public void SpawnEnemy()
        {
            enemy = monsterSpawner.SpawnRandomEnemyMonster();
            enemy.SetActive(true);
            enemyInfo = enemy.GetComponent<BaseMonster>();
            enemyStatusController.SetMonster(enemyInfo.DisplayName,enemyInfo.Level.ToString(),enemyInfo.MonsterPresence,enemyInfo.CurrentHealth, enemyInfo.MaxHealth, enemyInfo.MonsterId);
        }

        public void ResolveAttack(AttackResolution results)
        {
            enemyStatusController.UpdateMonster(results);
            animationController.PlayAnimation(enemy, AnimationAction.GetHit);
            scrollingCombatTextController.CreateScrollingCombatTextInstance(results, enemy.transform);

            if (results.WasFatal)
            {
                animationController.PlayAnimation(enemy, AnimationAction.Die);
            }
        }

        public void ResolveMyAttacks(AttackResolution results)
        {
            if (results.WasFatal)
            {
                animationController.PlayAnimation(enemy, AnimationAction.Victory);
                textLogDisplayManager.AddText(string.Format("{0} celebrates his victory!", enemyInfo.DisplayName), AnnouncementType.System);
            }
            else
            {
                animationController.PlayAnimation(enemy, AnimationAction.Attack);
            }
    }

        public void EndCombat()
        {
            Destroy(enemy);
            enemyInfo = null;
        }
    }
}

