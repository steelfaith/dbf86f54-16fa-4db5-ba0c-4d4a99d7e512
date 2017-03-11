using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Infrastructure
{
    public class AttackResolution
    {
        public bool WasCritical { get; set; }
        public int Damage { get; set; }
        public Guid TargetId { get; set; }
        public float MaxHealth { get; set; }
        public float CurrentHealth { get; set; }
        public bool WasFatal { get; set; }
    }
}
