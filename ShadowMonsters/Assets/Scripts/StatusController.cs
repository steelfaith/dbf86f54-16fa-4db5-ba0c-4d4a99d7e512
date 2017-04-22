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
        public float clickDelta = 0.35f;

        public void SetMonster(string name, string level, MonsterPresence presence, float currentHealth, float maxHealth, Guid id)
        {
            displayName.text = name;
            levelDisplay.UpdateLevelDisplay(level, presence);
            healthBar.AdjustHealth(currentHealth, maxHealth);
            MonsterId = id;
        }
        private bool click = false;
        private float clickTime;

        void Update()
        {
            if (click && Time.time > (clickTime + clickDelta))
            {
                 // Single click
                click = false;
            }
        }

        private void Start()
        {
        }

        void OnMouseDown()
        {
            if (click && Time.time <= (clickTime + clickDelta))
            {
                // Double click
                click = false;
            }
            else
            {
                click = true;
                clickTime = Time.time;
            }
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
            resourceCollector.UpdateResources(resources);
        }
    }
}
