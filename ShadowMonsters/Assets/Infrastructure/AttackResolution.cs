using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Infrastructure
{
    public class AttackResolution
    {        
        public bool WasCritical { get; set; }
        public float Damage { get; set; }
        public Guid TargetId { get; set; }
        public float MaxHealth { get; set; }
        public float CurrentHealth { get; set; }
        public bool WasFatal { get; set; }
        public AttackInfo AttackPerformed { get; set; }
        public bool Success { get; set; }
        public DateTime TimeStamp { get; set; }
        public bool WasShortCast { get; set; }
    }
}
