using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class LightController : MonoBehaviour
    {
        public Light light;


        private static LightController lightController;

        public static LightController Instance()
        {
            if (!lightController)
            {
                lightController = FindObjectOfType(typeof(LightController)) as LightController;
                if (!lightController)
                    Debug.LogError("Could not find the light! Darkness reigns.");
            }
            return lightController;
        }

        internal void ChangeColor(Color32 newColor)
        {
            light.color = newColor;
        }
    }
}
