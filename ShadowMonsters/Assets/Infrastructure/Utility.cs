using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Infrastructure
{    
    public static class Utility
    {
        private static System.Random _randomNumberGen = new System.Random();
        public static Color32 ContrastColor(Color32 color)
        {
            byte d = 0;

            // Counting the perceptive luminance - human eye favors green color... 
            double a = 1 - (0.299 * color.r + 0.587 * color.g + 0.114 * color.b) / 255;

            if (a < 0.5)
                d = 0; // bright colors - black font
            else
                d = 255; // dark colors - white font

            return new Color32(d, d, d, color.a);
        }

        public static string GetRandomEnumMember<T>()
        {

            var list = Enum.GetNames(typeof(T)).ToList();
            list.Remove("unitychan");

            return list[_randomNumberGen.Next(0, list.Count)];
        }
    }
}
