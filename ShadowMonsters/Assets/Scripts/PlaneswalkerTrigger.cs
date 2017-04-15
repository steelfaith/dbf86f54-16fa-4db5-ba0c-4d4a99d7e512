using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Threading;
using Assets.ServerStubHome;
using Assets.Infrastructure;

namespace Assets.Scripts
{
    public class PlaneswalkerTrigger : MonoBehaviour
    {
        private TextLogDisplayManager textLogDisplayManager;
        private ServerStub serverStub;
        private void Start()
        {
            serverStub = ServerStub.Instance();
            textLogDisplayManager = TextLogDisplayManager.Instance();
        }
        private void OnTriggerEnter(Collider other)
        {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            if (player == null) return;
            if(!player.CaughtBetweenPlanes)
            {
                textLogDisplayManager.AddText("You are not caught between the planes and do not need a planeswalker.", AnnouncementType.System);
            }
            else
            {
                textLogDisplayManager.AddText("Your soul has wandered into a place it should not be. Let me bring you back.", AnnouncementType.System);
                serverStub.RevivePlayer(new ReviveRequest
                {
                    PlayerId = player.Id,
                });
            }
            
        }


    }
}
