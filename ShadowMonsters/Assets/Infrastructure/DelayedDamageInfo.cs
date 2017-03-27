using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Infrastructure
{
    public class DelayedDamageInfo : AttackInfo
    {
        public int NextDueTime { get; set; }
    }
}
