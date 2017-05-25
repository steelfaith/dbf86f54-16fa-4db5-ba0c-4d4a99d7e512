using System;
using System.Collections.Generic;
using Common.Enums;
using UnityEngine;

namespace Common.Interface
{
    public interface IMonsterDna
    {
        float Attack { get; set; }
        List<Guid> AttackIds { get; set; }
        List<Color32> ColorWheel { get; set; }
        float CurrentHealth { get; set; }
        float Defense { get; set; }
        string DisplayName { get; }
        float EssenceCasting { get; set; }
        float EssenceResistance { get; set; }
        int Level { get; set; }
        float MaxHealth { get; set; }
        ElementalAffinity MonsterAffinity { get; set; }
        Guid MonsterId { get; set; }
        MonsterPresence MonsterPresence { get; set; }
        string NameKey { get; }
        string NickName { get; set; }
        MonsterSize Sizing { get; set; }
        float Speed { get; set; }
        int TeamOrder { get; set; }
        Color32 Color { get; set; }
    }
}
