using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Threading;

namespace Assets.Scripts
{
    public class PlainswalkerTrigger : MonoBehaviour
    {
        private TextLogDisplayManager textLogDisplayManager;
        private void Start()
        {
            textLogDisplayManager = TextLogDisplayManager.Instance();
        }
        private void OnTriggerEnter(Collider other)
        {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            if (player == null) return;
            if(!player.CaughtBetweenPlains)
            {
                textLogDisplayManager.AddText("You are not caught between the plains and do not need a plainswalker.", Infrastructure.AnnouncementType.System);
            }
            
        }


    }
}
