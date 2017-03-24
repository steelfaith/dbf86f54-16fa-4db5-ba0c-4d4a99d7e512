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
        public Text displayName;
        public IndicatorBarScript healthBar;
        public LevelDisplayScript levelDisplay;
        public CastbarScript castBar;
        public ResourceCollectorScript resourceCollector;

        public void SetMonster(string name, string level, MonsterPresence presence, float currentHealth, float maxHealth)
        {
            displayName.text = name;
            levelDisplay.UpdateLevelDisplay(level, presence);
            healthBar.AdjustHealth(currentHealth, maxHealth);
        }

        private void Start()
        {
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
            healthBar.AdjustHealth(MonsterUpdate.CurrentHealth, MonsterUpdate.MaxHealth);
        }

        internal void UpdateCastBar()
        {
            //castBar.UpdateCastbar()
        }

        internal void UpdateResources(List<ElementalAffinity> resources)
        {
            resourceCollector.UpdatePlayerResources(resources);
        }
    }
}
