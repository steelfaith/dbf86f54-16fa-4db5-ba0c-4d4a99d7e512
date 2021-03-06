﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Assets.Infrastructure;

namespace Assets.Scripts
{
    public class ResourceCollectorScript:MonoBehaviour
    {

        public Image globe1Image;
        public Image globe2Image;
        public Image globe3Image;
        public Image globe4Image;
        public Image globe5Image;
        public Image globe6Image;
        private List<Image> images;

        private void Awake()
        {
            images = new List<Image>();
            images.Add(globe1Image);
            images.Add(globe2Image);
            images.Add(globe3Image);
            images.Add(globe4Image);
            images.Add(globe5Image);
            images.Add(globe6Image);

        }

        public void UpdateResources(List<ElementalAffinity> resources)
        {
            Clear();
            int i = 0;
            foreach (ElementalAffinity item in resources)
            {
                images[i].color = item.GetColorFromMonsterAffinity();                
                i++;
            }                  
        }

        private void Clear()
        {
            foreach (Image item in images)
            {
                item.color = new Color32(255, 255, 255, 0);
            }
        }
    }
}
