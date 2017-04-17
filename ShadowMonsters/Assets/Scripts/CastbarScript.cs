
using UnityEngine;
using UnityEngine.UI;
using Assets.Infrastructure;

namespace Assets.Scripts 
{
    public class CastbarScript : MonoBehaviour
    {
        public Image castBar;
        public Text text;
        private float rechargeEnd;
        private float currentTime;
        private float totalTime;
        private bool casting;

        private void Update()
        {
            if(Time.time >= rechargeEnd)
            {
                if(text != null)
                    text.text = "";
                castBar.fillAmount = 0f;
                return;
            }
            currentTime = rechargeEnd - Time.time;
            UpdateCastbar();
        }

        public void StartCast(AttackInfo attack)
        {
            text.text = attack.Name;
            casting = attack.CastTime > 0;
            totalTime = casting ? attack.CastTime : attack.Cooldown;
            rechargeEnd = Time.time + totalTime;

        }

        private void UpdateCastbar()
        {
            if (totalTime == 0) return;
            var amount = currentTime / totalTime;
            if (casting)
                castBar.fillAmount = 1 - amount;
            else
                castBar.fillAmount = amount;         
        }
    }
}
