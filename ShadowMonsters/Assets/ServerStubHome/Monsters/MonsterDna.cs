using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Infrastructure;

namespace Assets.ServerStubHome.Monsters
{
    /// <summary>
    /// the instructions to spawn a monster
    /// </summary>
    public class MonsterDna : IMonsterDna
    {
        public ElementalAffinity MonsterAffinity { get; set; }

        public MonsterPresence MonsterPresence { get; set; }

        private MonsterList monsterValue;
        public MonsterDna(MonsterList value, int level)
        {
            Level = level;
            MaxHealth = level * 5;
            Speed = level * 3;
            monsterValue = value;
            AttackIds = new List<Guid>();            
            CurrentHealth = MaxHealth;
        }

        public float MaxHealth { get; set; }

        public float Speed { get; set; }

        public float EssenceResistance { get; set; }

        public float EssenceCasting { get; set; }

        public float Defense { get; set; }

        public float Attack { get; set; }

        public string NickName { get; set; }

        public int TeamOrder { get; set; }        

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

        public List<Guid> AttackIds { get; set; }   
        
        public List<Color32> ColorWheel { get; set; }     

        public Size Sizing { get; set; }

        public Color32 Color { get; set; }

    }
}
