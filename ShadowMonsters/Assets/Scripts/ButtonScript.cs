using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class ButtonScript : MonoBehaviour
    {

        public Button button;
        public Image cooldownImage;
        private float rechargeEnd;
        private float rechargeTime;
        public Color startColor;
        public Color endColor;

        private void Update()
        {
            if(rechargeEnd <= Time.time)
            {
                EndRecharge();
                return;
            }
            SetOverlay();
        }

        public void StartRecharge(float recharge)
        {
            if (button.enabled == false) return;
            button.enabled = false;
            rechargeTime = recharge;
            rechargeEnd = Time.time + rechargeTime;
            cooldownImage.fillAmount = 1.0f;
        }

        public void SetOverlay()
        {
            cooldownImage.fillAmount = (rechargeEnd - Time.time) / rechargeTime;
            cooldownImage.color = Color.Lerp(startColor, endColor, 1 - cooldownImage.fillAmount);
        }

        public void EndRecharge()
        {
            cooldownImage.fillAmount = 0.0f;
            button.enabled = true;
        }
    }
}
