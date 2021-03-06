﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Assets.Infrastructure;

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

        public void CreateScrollingCombatTextInstance(AttackResolution results, Transform location)
        {
            float height = 0;

            var boxCollider = location.gameObject.GetComponentInChildren<BoxCollider>();
            if (boxCollider != null)
            {
                height = boxCollider.bounds.size.y/2;
            }
            
            Vector3 modPosition = new Vector3(location.position.x, location.position.y + height ,location.position.z);
            ScrollingCombatText instance = Instantiate(scrollingText);
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(modPosition);


            instance.transform.SetParent(canvas.transform, false);           

            instance.transform.position = screenPosition;
            
            if(!results.Success)
            { instance.SetText("Miss!", false); }
            else
            {
                instance.SetText(string.Format("-{0}", results.Damage.ToString()), results.WasCritical);
            }
            
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
