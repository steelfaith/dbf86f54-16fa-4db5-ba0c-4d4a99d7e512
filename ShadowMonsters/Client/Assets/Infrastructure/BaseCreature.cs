using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Infrastructure
{
    public class BaseCreature : MonoBehaviour
    {
        private string originalName;
        public string Name
        {
            get
            {
                if (string.IsNullOrEmpty(NickName))
                    return originalName;
                return NickName;
                    
            }

            set { originalName = value; }
        }

        public int Level { get; set; }

        public float Health { get; set; }

        public Guid MonsterId { get; set; }

        public string NickName { get; set; }

    }
}
