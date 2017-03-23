using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;
using UnityEngine;

namespace Assets.Infrastructure
{
    public class AttackInfo
    {
        public int Accuracy { get; set; }

        public Guid AttackId { get; set; }

        public string Name { get; set; }

        public ElementalAffinity Affinity { get; set; }

        public int Cooldown { get; set; }

        public DamageStyle DamageStyle { get; set; }

        public int CastTime { get; set; }

        public int BaseDamage { get; set; }

        public Sprite Icon { get; set; }

        public bool IsGenerator { get; set; }

        private float powerLevel;
        public float PowerLevel
        {
            get { return powerLevel; }
            set
            {
                if (!CanPowerUp) return;
                if (value > 3) return;
                powerLevel = value;                
            }
        }

        public bool CanPowerUp { get; set; }

        public bool IsCasting { get; set; }
    }
}
