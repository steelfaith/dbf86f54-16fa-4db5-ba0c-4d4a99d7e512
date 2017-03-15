using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Infrastructure;

namespace Assets.Scripts
{
    public class EnemyController : MonoBehaviour
    {
        private GameObject enemy;
        public BaseCreature enemyInfo;
        private MonsterSpawner monsterSpawner;
        private StatusController enemyStatusController;
        private ScrollingCombatTextController scrollingCombatTextController;
        private AnimationController animationController;

        private void Start()
        {
            monsterSpawner = MonsterSpawner.Instance();
            enemyStatusController = StatusController.Instance();
            enemyStatusController = StatusController.Instance();
            scrollingCombatTextController = ScrollingCombatTextController.Instance();
            animationController = AnimationController.Instance();
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
            enemyInfo = enemy.GetComponent<BaseCreature>();
            enemyStatusController.SetCreature(enemyInfo);
        }

        public void ResolveAttack(AttackResolution results)
        {
            enemyStatusController.UpdateCreature(results);
            animationController.PlayAnimation(enemy, AnimationAction.GetHit);
            scrollingCombatTextController.CreateScrollingCombatTextInstance(results.Damage.ToString(), results.WasCritical, enemy.transform);

            if (results.WasFatal)
            {
                animationController.PlayAnimation(enemy, AnimationAction.Die);
            }
        }

        public void EndCombat()
        {
            Destroy(enemy);
            enemyInfo = null;
        }
    }
}

