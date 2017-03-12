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

        public void UpdateLevelDisplay(string level, MonsterRarity rarity)
        {
            image.color = rarity.GetColorFromRarity();
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
