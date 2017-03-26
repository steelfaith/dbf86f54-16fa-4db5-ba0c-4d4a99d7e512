using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Infrastructure
{
    /// <summary>
    /// the instructions to spawn a monster
    /// </summary>
    public class MonsterDna
    {
        public ElementalAffinity MonsterAffinity { get; set; }

        public MonsterPresence MonsterPresence { get; set; }

        private MonsterList monsterValue;
        public MonsterDna(MonsterList value, int level)
        {
            Level = level;
            MaxHealth = level * 5;
            monsterValue = value;
            AttackIds = new List<Guid>();            
            CurrentHealth = MaxHealth;
        }

        public string NickName { get; set; }

        public int TeamOrder { get; set; }

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
        
        public List<Color32> ColorWheel { get; set; }     

    }
}
