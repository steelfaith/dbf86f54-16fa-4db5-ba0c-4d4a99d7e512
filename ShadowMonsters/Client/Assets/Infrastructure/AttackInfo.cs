using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Infrastructure
{
    public class AttackInfo
    {
        public string Name { get; set; }

        public MonsterType MonsterType { get; set; }

        public int Cooldown { get; set; }

        public DamageStyle DamageStyle { get; set; }

        public int CastTime { get; set; }

        public int BaseDamage { get; set; }
    }
}
