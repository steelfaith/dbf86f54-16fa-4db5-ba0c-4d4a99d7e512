using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Infrastructure
{
    public class CreatureInfo
    {
        public MonsterType Type { get; set; }

        public MonsterRarity MonsterRarity { get; set; }

        private MonsterList monsterValue;
        public CreatureInfo(MonsterList value, int level)
        {
            Level = level;
            MaxHealth = level * 5;
            monsterValue = value;
            AttackIds = new List<Guid>();            
            CurrentHealth = MaxHealth;
        }

        public string NickName { get; set; }

        public bool IsTeamLead { get; set; }

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

        public List<Guid> AttackIds;

    }
}
