using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Infrastructure
{
    public class BaseMonster : MonoBehaviour
    {
        private string originalName;
        public string DisplayName
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

        public float MaxHealth { get; set; }

        public float CurrentHealth { get; set; }

        public Guid MonsterId { get; set; }

        public string NickName { get; set; }

        public MonsterPresence MonsterPresence { get; set; }

        public ElementalAffinity MonsterAffinity { get; set; }

        public string NameKey { get; set; }

        public bool IsPlayerTeamLead { get; set; }

    }
}
