using System.Drawing;

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
        public static Color GetColorFromMonsterAffinity(this ElementalAffinity value)
        {
            Color colorReturn = Color.White;
            switch (value)
            {
                case ElementalAffinity.Fae:
                    colorReturn = Color.FromArgb(102, 0, 102);
                    break;
                case ElementalAffinity.Dragon:
                    colorReturn = Color.FromArgb(0, 153, 76);
                    break;
                case ElementalAffinity.Light:
                    colorReturn = Color.FromArgb(255, 239, 213);
                    break;
                case ElementalAffinity.Shadow:
                    colorReturn = Color.FromArgb(119, 136, 153);
                    break;
                case ElementalAffinity.Demon:
                    colorReturn = Color.FromArgb(128, 0, 0);
                    break;
                case ElementalAffinity.Mechanical:
                    colorReturn = Color.FromArgb(192, 192, 192);
                    break;
                case ElementalAffinity.Wood:
                    colorReturn = Color.FromArgb(160, 82, 45);
                    break;
                case ElementalAffinity.Wind:
                    colorReturn = Color.FromArgb(188, 255, 250);
                    break;
                case ElementalAffinity.Fire:
                    colorReturn = Color.FromArgb(204, 0, 0);
                    break;
                case ElementalAffinity.Water:
                    colorReturn = Color.FromArgb(30, 144, 255);
                    break;
                case ElementalAffinity.Human:
                    colorReturn = Color.FromArgb(255, 224, 189); //fleshy!
                    break;
                default:
                    break;
            }

            return colorReturn;
        }
    }
}
