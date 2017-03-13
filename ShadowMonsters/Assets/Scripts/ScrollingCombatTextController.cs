using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class ScrollingCombatTextController : MonoBehaviour
    {
        public Canvas canvas;
        public GameObject scrollingCombatTextPrefab;
        private ScrollingCombatText scrollingText;

        private void Start()
        {
            scrollingText = scrollingCombatTextPrefab.GetComponent<ScrollingCombatText>();
        }

        public void CreateScrollingCombatTextInstance(string damage, bool crit, Transform location)
        {
            ScrollingCombatText instance = Instantiate(scrollingText);
            Vector2 screenPosition = Camera.main.WorldToScreenPoint(location.position);
            instance.transform.SetParent(canvas.transform, false);
            instance.transform.position = screenPosition;
            instance.SetText(damage,crit);
        }

        private static ScrollingCombatTextController scrollingCombatTextController;

        public static ScrollingCombatTextController Instance()
        {
            if (!scrollingCombatTextController)
            {
                scrollingCombatTextController = FindObjectOfType(typeof(ScrollingCombatTextController)) as ScrollingCombatTextController;
                if (!scrollingCombatTextController)
                    Debug.LogError("Could not find ScrollingCombatTextController!");
            }
            return scrollingCombatTextController;
        }
    }
}
