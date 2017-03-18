using Assets.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class StatusController : MonoBehaviour
    {
        public float maxHealth;
        public float currentHealth;
        public Text displayName;
        public IndicatorBarScript indicatorBar;
        public LevelDisplayScript levelDisplay;

        public void SetMonster(BaseMonster Monster)
        {
            displayName.text = Monster.Name;
            maxHealth = Monster.Health;
            currentHealth = maxHealth;
            levelDisplay.UpdateLevelDisplay(Monster.Level.ToString(), Monster.MonsterRarity);
            indicatorBar.AdjustHealth(currentHealth, maxHealth);
        }

        private void Start()
        {
            indicatorBar = IndicatorBarScript.Instance();
            levelDisplay = LevelDisplayScript.Instance();
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

        internal void UpdateMonster(AttackResolution MonsterUpdate)
        {
            currentHealth = MonsterUpdate.CurrentHealth;
            indicatorBar.AdjustHealth(MonsterUpdate.CurrentHealth, MonsterUpdate.MaxHealth);
        }
    }
}
