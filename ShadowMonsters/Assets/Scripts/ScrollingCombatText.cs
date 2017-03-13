using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class ScrollingCombatText: MonoBehaviour
    {
        public Animator anim;
        public Text damageText;

        private void Start()
        {
            Destroy(gameObject, 2);
            //damageText = GetComponent<Text>();
        }

        public void SetText(string text, bool crit)
        {
            damageText.text = string.Format("-{0}", text);
        }


        private static ScrollingCombatText scrollingCombatText;

        public static ScrollingCombatText Instance()
        {
            if (!scrollingCombatText)
            {
                scrollingCombatText = FindObjectOfType(typeof(ScrollingCombatText)) as ScrollingCombatText;
                if (!scrollingCombatText)
                    Debug.LogError("Could not find the scrolling combat text script!");
            }
            return scrollingCombatText;
        }
    }
}
