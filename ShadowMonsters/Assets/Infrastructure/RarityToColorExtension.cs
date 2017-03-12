using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Infrastructure
{
    public static class RarityToColorExtension
    {
        public static Color32 GetColorFromRarity(this MonsterRarity value)
        {
            Color32 colorReturn = Color.white;
            switch (value)
            {
                case MonsterRarity.Common:
                    colorReturn = new Color32(200, 200, 200, 255);//gray
                    break;
                case MonsterRarity.Uncommon:
                    colorReturn = Color.green;
                    break;
                case MonsterRarity.Rare:
                    colorReturn = Color.blue;
                    break;
                case MonsterRarity.Epic:
                    colorReturn = new Color32(85, 15, 157, 255); //purple
                    break;
                case MonsterRarity.Legendary:
                    colorReturn = new Color32(212,175,55,255);//gold
                    break;
                case MonsterRarity.Prime:
                    colorReturn = new Color32(183, 110, 121, 255); //rose gold
                    break;
                default:
                    colorReturn = new Color32(255, 250, 250, 255); //snow
                    break;
            }
            return colorReturn;
        }
    }
}
