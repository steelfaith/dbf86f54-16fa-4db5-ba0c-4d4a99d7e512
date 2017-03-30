using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Infrastructure;
using Assets.ServerStubHome;

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
            monsterSpawner = MonsterSpawner.Instance();
            enemyStatusController = StatusDisplay.GetComponentInChildren<StatusController>();
            scrollingCombatTextController = ScrollingCombatTextController.Instance();
            animationController = AnimationController.Instance();
            textLogDisplayManager = TextLogDisplayManager.Instance();
        }

        private static EnemyController enemyController;

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

