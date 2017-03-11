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
        public float maxHealth;
        public float currentHealth;
        public IndicatorBarScript indicatorBar;

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

        internal void UpdateCreature(CreatureInfo creatureUpdate)
        {
            currentHealth = creatureUpdate.CurrentHealth;
            indicatorBar.AdjustHealth(creatureUpdate.CurrentHealth, creatureUpdate.MaxHealth);
        }
    }
}
