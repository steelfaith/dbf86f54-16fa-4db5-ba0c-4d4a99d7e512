using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Infrastructure
{
    public class AttackInfo
    {
        public string Name { get; set; }

        public MonsterType Type { get; set; }

        public int Cooldown { get; set; }
    }
}
