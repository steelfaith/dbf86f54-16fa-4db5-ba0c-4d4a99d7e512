using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class IndicatorBarScript : MonoBehaviour
    {

        public Image healthBarImage;
        public Color startColor;
        public Color endColor;

        public void AdjustHealth(float currentHealth, float maxHealth)
        {
            if (maxHealth == 0) return;
            healthBarImage.GetComponentInChildren<Text>().text = currentHealth.ToString();
            healthBarImage.fillAmount = currentHealth / maxHealth;
            healthBarImage.color = Color.Lerp(startColor, endColor, 1 - healthBarImage.fillAmount);
        }

        private static IndicatorBarScript indicatorBarScript;

        public static IndicatorBarScript Instance()
        {
            if (!indicatorBarScript)
            {
                indicatorBarScript = FindObjectOfType(typeof(IndicatorBarScript)) as IndicatorBarScript;
                if (!indicatorBarScript)
                    Debug.LogError("Could not find Indicator Bar Script!");
            }
            return indicatorBarScript;
        }

    }
}
