using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Infrastructure
{
    public enum MonsterType
    {
        Fae,
        Dragon,
        Light,
        Shadow,
        Demon,
        Mechanical,
        Wood,
        Wind,
        Fire,
        Water,
        Human
    }
    public static class MonsterTypeToColorExtension
    {
        public static Color32 GetColorFromMonsterType(this MonsterType value)
        {
            Color32 colorReturn = Color.white;
            switch (value)
            {
                case MonsterType.Fae:
                    colorReturn = new Color32(102, 0, 102, 255);
                    break;
                case MonsterType.Dragon:
                    colorReturn = new Color32(0, 153, 76, 255);
                    break;
                case MonsterType.Light:
                    colorReturn = new Color32(255, 239, 213, 255);
                    break;
                case MonsterType.Shadow:
                    colorReturn = new Color32(119, 136, 153, 255);
                    break;
                case MonsterType.Demon:
                    colorReturn = new Color32(128, 0, 0, 255);
                    break;
                case MonsterType.Mechanical:
                    colorReturn = new Color32(192, 192, 192, 255);
                    break;
                case MonsterType.Wood:
                    colorReturn = new Color32(160, 82, 45, 255);
                    break;
                case MonsterType.Wind:
                    colorReturn = new Color32(245, 255, 250, 255);
                    break;
                case MonsterType.Fire:
                    colorReturn = new Color32(204, 0, 0, 255);
                    break;
                case MonsterType.Water:
                    colorReturn = new Color32(30, 144, 255, 255);
                    break;
                case MonsterType.Human:
                    colorReturn = new Color32(255, 224, 189, 255); //fleshy!
                    break;
                default:
                    break;
            }

            return colorReturn;
        }
    }
}
