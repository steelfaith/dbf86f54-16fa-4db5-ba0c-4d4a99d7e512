using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Infrastructure
{
    public class AttackPowerChangeResolution
    {
        public Guid PlayerId { get; set; }
        public float PowerLevel { get; set; }
        public Guid AttackId { get; set; }
    }
}
