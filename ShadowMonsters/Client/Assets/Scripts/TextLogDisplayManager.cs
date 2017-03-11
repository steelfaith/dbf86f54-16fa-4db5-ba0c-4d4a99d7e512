﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Assets.Infrastructure;

namespace Assets.Scripts
{
    public class TextLogDisplayManager : MonoBehaviour
    {
        public Text _textBlock;
        public GameObject _panel;


        private static TextLogDisplayManager _textLogDisplayManager;


        public static TextLogDisplayManager Instance()
        {
            if (!_textLogDisplayManager)
            {
                _textLogDisplayManager = FindObjectOfType(typeof(TextLogDisplayManager)) as TextLogDisplayManager;
                if (!_textLogDisplayManager)
                    Debug.LogError("Could not find Text Log Display Manager");
            }
            return _textLogDisplayManager;
        }

        public void AddText(string textToAdd, AnnouncementType type)
        {
            string colorHexString = string.Empty;
            switch (type)
            {
                case AnnouncementType.Chat:
                    colorHexString = "#ffffffff";
                    break;
                case AnnouncementType.Combat:
                    colorHexString = "#a52a2aff";
                    break;
                case AnnouncementType.Enemy:
                    colorHexString = "#ff7f50";
                    break;
                case AnnouncementType.Friendly:
                    colorHexString = "#00ff00ff";
                    break;
                case AnnouncementType.System:
                    colorHexString = "#0000ffff";
                    break;
                default:
                    break;
            }

            TruncateTextBasedOnLength(textToAdd, colorHexString);
        }

        private void TruncateTextBasedOnLength(string textToAdd, string colorHex)
        {
            var totalLength = _textBlock.text.Length + textToAdd.Length;

            if(totalLength > 10000)
            {
                var diff = totalLength - 10000;
                var substringLength = _textBlock.text.Length - diff;
                var newText = _textBlock.text.Substring(diff, substringLength);
                _textBlock.text = newText + Environment.NewLine + textToAdd;
            
            }

            _textBlock.text = _textBlock.text + Environment.NewLine + string.Format("<color={0}>{1}</color>",colorHex, textToAdd);
        }
    }
}