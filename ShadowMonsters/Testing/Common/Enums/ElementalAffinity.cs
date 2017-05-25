using UnityEngine;

namespace Common.Enums
{
    public enum ElementalAffinity
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
    public static class MonsterAffinityToColorExtension
    {
        public static Color32 GetColorFromMonsterAffinity(this ElementalAffinity value)
        {
            Color32 colorReturn = new Color32(255,255,255,255);
            switch (value)
            {
                case ElementalAffinity.Fae:
                    colorReturn = new Color32(102, 0, 102,255);
                    break;
                case ElementalAffinity.Dragon:
                    colorReturn = new Color32(0, 153, 76,255);
                    break;
                case ElementalAffinity.Light:
                    colorReturn = new Color32(255, 239, 213,255);
                    break;
                case ElementalAffinity.Shadow:
                    colorReturn = new Color32(119, 136, 153,255);
                    break;
                case ElementalAffinity.Demon:
                    colorReturn = new Color32(128, 0, 0,255);
                    break;
                case ElementalAffinity.Mechanical:
                    colorReturn = new Color32(192, 192, 192,255);
                    break;
                case ElementalAffinity.Wood:
                    colorReturn = new Color32(160, 82, 45,255);
                    break;
                case ElementalAffinity.Wind:
                    colorReturn = new Color32(188, 255, 250,255);
                    break;
                case ElementalAffinity.Fire:
                    colorReturn = new Color32(204, 0, 0,255);
                    break;
                case ElementalAffinity.Water:
                    colorReturn = new Color32(30, 144, 255,255);
                    break;
                case ElementalAffinity.Human:
                    colorReturn = new Color32(255, 224, 189,255); //fleshy!
                    break;
                default:
                    break;
            }

            return colorReturn;
        }
    }
}
