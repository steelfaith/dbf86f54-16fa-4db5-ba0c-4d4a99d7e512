using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Assets.Infrastructure;

namespace Assets.Scripts
{
    public class LevelDisplayScript:MonoBehaviour
    {
        public Image image;
        public Text levelText;
        public Image blurBorder;

        public void UpdateLevelDisplay(string level, MonsterRarity rarity)
        {
            var rarityColor = rarity.GetColorFromRarity();
            blurBorder.color = rarityColor;
            if ((int)rarity<3)
            {
                blurBorder.enabled = false;
                
            }
            image.color = rarityColor;
            levelText.text = level;
        }

        private static LevelDisplayScript levelDisplayScript;

        public static LevelDisplayScript Instance()
        {
            if (!levelDisplayScript)
            {
                levelDisplayScript = FindObjectOfType(typeof(LevelDisplayScript)) as LevelDisplayScript;
                if (!levelDisplayScript)
                    Debug.LogError("Could not find the level display script!");
            }
            return levelDisplayScript;
        }
    }
}
