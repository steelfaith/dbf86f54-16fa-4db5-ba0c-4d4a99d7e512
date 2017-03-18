using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts 
{
    public class CastbarScript : MonoBehaviour
    {
        public Image castBar;

        public void UpdateCastbar(float currentCooldown, float maxCooldown)
        {
            if (maxCooldown == 0) return;
            castBar.fillAmount = currentCooldown / maxCooldown;           
        }
    }
}
