using System;
using System.Collections.Generic;
using System.Drawing;
using Common;
using Common.Enums;
using Common.Interface;

namespace Server.Monsters
{
    public class DemonEnforcer : IMonsterDna
    {
        Random _randomNumberGenerator = new Random();
        public DemonEnforcer()
        {
            ColorWheel = MonsterColors.EarthTones;
            ColorWheel.Add(Color.FromArgb(255, 250, 250));
            ColorWheel.Add(Color.FromArgb(0, 0, 0));
            Color = ColorWheel[_randomNumberGenerator.Next(0, ColorWheel.Count)];
            MonsterAffinity = ElementalAffinity.Demon;
            MonsterId = Guid.NewGuid();
            Level = _randomNumberGenerator.Next(1, 101);
            Sizing = (MonsterSize)Enum.Parse(typeof(MonsterSize), Utility.GetRandomEnumMember<MonsterSize>());
            MonsterPresence = (MonsterPresence)Enum.Parse(typeof(MonsterPresence), Utility.GetRandomEnumMember<MonsterPresence>());
            MaxHealth = Level * 5;
            Speed = Level * 3;
            AttackIds = new List<Guid>();
            CurrentHealth = MaxHealth;
        }

        public float Attack { get; set; }

        public List<Guid> AttackIds { get; set; }

        public List<Color> ColorWheel { get; set; }

        public float CurrentHealth { get; set; }

        public float Defense { get; set; }

        public float EssenceCasting { get; set; }

        public float EssenceResistance { get; set; }

        public int Level { get; set; }

        public float MaxHealth { get; set; }

        public ElementalAffinity MonsterAffinity { get; set; }

        public Guid MonsterId { get; set; }

        public MonsterPresence MonsterPresence { get; set; }

        public string NameKey { get { return MonsterList.DemonEnforcer.ToString(); } }

        public string DisplayName
        {
            get { return EnumHelper<MonsterList>.GetEnumDescription(NameKey); }
        }

        public string NickName { get; set; }

        public MonsterSize Sizing { get; set; }

        public float Speed { get; set; }

        public int TeamOrder { get; set; }

        public Color Color { get; set; }
    }
}
