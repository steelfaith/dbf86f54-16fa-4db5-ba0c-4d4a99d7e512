﻿using System;
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

        public void UpdateLevelDisplay(string level, MonsterPresence rarity)
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

    }
}
