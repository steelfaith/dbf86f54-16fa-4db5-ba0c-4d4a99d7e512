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
        public Guid MonsterId;

        public void SetMonster(string name, string level, MonsterPresence presence, float currentHealth, float maxHealth, Guid id)
        {
            displayName.text = name;
            levelDisplay.UpdateLevelDisplay(level, presence);
            healthBar.AdjustHealth(currentHealth, maxHealth);
            MonsterId = id;
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

        internal void UpdateMonster(AttackResolution attack)
        {            
            healthBar.AdjustHealth(attack.CurrentHealth, attack.MaxHealth);
        }

        internal void UpdateCastBar(AttackInfo attack)
        {
            castBar.StartCast(attack);
        }

        internal void UpdateResources(List<ElementalAffinity> resources)
        {
            resourceCollector.UpdatePlayerResources(resources);
        }
    }
}
