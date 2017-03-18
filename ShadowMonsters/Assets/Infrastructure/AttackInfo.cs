using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Infrastructure
{
    public class AttackInfo
    {
        public Guid AttackId { get; set; }

        public string Name { get; set; }

        public ElementalAffinity MonsterAffinity { get; set; }

        public int Cooldown { get; set; }

        public DamageStyle DamageStyle { get; set; }

        public int CastTime { get; set; }

        public int BaseDamage { get; set; }
    }
}
