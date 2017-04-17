using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Infrastructure
{
    public class EnemyAttackUpdate
    {
        public Guid PlayerId { get; set; }
        public AttackInfo Attack { get; set; }
    }
}
