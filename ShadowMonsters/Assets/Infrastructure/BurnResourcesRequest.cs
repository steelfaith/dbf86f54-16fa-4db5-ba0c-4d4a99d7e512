using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Infrastructure
{
    public class BurnResourceRequest
    {
        public Guid AttackInstanceId;
        public ElementalAffinity NeededResource { get; set; }
    }
}
