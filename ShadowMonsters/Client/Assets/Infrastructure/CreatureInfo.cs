using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Infrastructure
{
    public class CreatureInfo
    {
        public MonsterType Type { get; set; }

        private MonsterList monsterValue;
        public CreatureInfo(MonsterList value)
        {
            monsterValue = value;
        }

        public float MaxHealth { get; set; }

        public float CurrentHealth { get; set; }

        public Guid MonsterId { get; set; }

        public string NameKey
        {
            get { return monsterValue.ToString(); }
        }
        public string DisplayName
        {
            get { return EnumHelper<MonsterList>.GetEnumDescription(NameKey); }
        }
        public int Level { get; set; }

        public AttackInfo Attack1 { get; set; }

        public AttackInfo Attack2 { get; set; }

        public AttackInfo Attack3 { get; set; }

        public AttackInfo Attack4 { get; set; }

        public AttackInfo Attack5 { get; set; }

        public AttackInfo Attack6 { get; set; }

    }
}
