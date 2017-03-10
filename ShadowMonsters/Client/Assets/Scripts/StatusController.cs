using Assets.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class StatusController : MonoBehaviour
    {
        public int maxHealth;
        public int currentHealth;
        public IndicatorBarScript indicatorBar;

        public void TakeDamage(int damage)
        {
            var tempHealth =currentHealth - damage;

            if(tempHealth < 0)
            {
                currentHealth = 0; }
            else
            {
                currentHealth = tempHealth;
            }
            indicatorBar.AdjustHealth(currentHealth, maxHealth);
        }

        public void HealDamage(int amount)
        {
            var tempHealth = currentHealth + amount;
            if (currentHealth > maxHealth)
            { currentHealth = maxHealth; }
            else
            { currentHealth = tempHealth; }
            indicatorBar.AdjustHealth(currentHealth, maxHealth);
        }

        public void SetCreature(BaseCreature creature)
        {
            maxHealth = creature.Health;
            currentHealth = maxHealth;
            indicatorBar.AdjustHealth(currentHealth, maxHealth);
        }

        private void Start()
        {
            indicatorBar = IndicatorBarScript.Instance();
        }

        private static StatusController statusController;

        public static StatusController Instance()
        {
            if (!statusController)
            {
                statusController = FindObjectOfType(typeof(StatusController)) as StatusController;
                if (!statusController)
                    Debug.LogError("Could not find Health Controller!");
            }
            return statusController;
        }
    }
}
