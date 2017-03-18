using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Infrastructure
{
    public class BaseMonster : MonoBehaviour
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

        public MonsterPresence MonsterRarity { get; set; }

        public ElementalAffinity MonsterAffinity { get; set; }

        public string NameKey { get; set; }

    }
}
