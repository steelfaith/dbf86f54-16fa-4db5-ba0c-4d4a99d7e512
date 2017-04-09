using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using Assets.ServerStubHome;

namespace Assets.Scripts
{
    public class IncarnationContainer : MonoBehaviour, IDropHandler
    {
        private ServerStub serverStub;
        public Guid MonsterId
        {
            get
            {
                if (item == null) return Guid.Empty;
                var statusController = item.GetComponent<StatusController>();
                if (statusController != null)
                {
                    return statusController.MonsterId;
                }
                return Guid.Empty;
            }
        }
        public GameObject item
        {
            get
            {
                if(transform.childCount > 0)
                {
                    return transform.GetChild(0).gameObject;
                }
                return null;
            }

        }

        private void Start()
        {
            serverStub = ServerStub.Instance();
        }

        public void OnDrop(PointerEventData eventData)
        {
            var fatbic = FatbicDisplayController.Instance();
            if (fatbic != null && fatbic.IsBusy) return;
            var incomingMonsterId = DragHandler.itemBeingDragged.GetComponent<StatusController>().MonsterId;
            if (!serverStub.CheckPulse(incomingMonsterId)) return;

            if (!item)
            {
                DragHandler.itemBeingDragged.transform.SetParent(transform);                
            }
            else
            {
                //item needs to return to its previous parent in position 1
                var leavingItemStatusController = item.GetComponent<StatusController>();
                if (leavingItemStatusController != null)
                {
                    leavingItemStatusController.displayName.color = new Color32(0, 253, 0, 255);
                }
                item.transform.SetParent(DragHandler.itemBeingDragged.transform.parent);
                DragHandler.itemBeingDragged.transform.SetParent(transform);
            }

            var statusController = item.GetComponent<StatusController>();
            if (statusController != null)
            {
                statusController.displayName.color = new Color32(255, 233, 5, 255);
            }

            var combatPlayer = CombatPlayerController.Instance();
            if (combatPlayer != null && combatPlayer.InCombat)
                combatPlayer.IncarnateMonster();
        }

        private static IncarnationContainer incarnationContainer;

        public static IncarnationContainer Instance()
        {
            if (!incarnationContainer)
            {
                incarnationContainer = FindObjectOfType(typeof(IncarnationContainer)) as IncarnationContainer;
                if (!incarnationContainer)
                    Debug.LogError("Could not find Incarnation Container!");
            }
            return incarnationContainer;
        }

        private void Update()
        {
            var image = gameObject.GetComponent<Image>();
            image.enabled = IsItemBeingDragged();
        }

        private bool IsItemBeingDragged()
        {
            return DragHandler.itemBeingDragged != null;
        }
    }
}
