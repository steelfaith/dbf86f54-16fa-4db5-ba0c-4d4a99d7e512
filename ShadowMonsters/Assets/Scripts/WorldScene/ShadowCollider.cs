using Assets.Scripts.NetworkAgents;
using Common.Messages.Requests;
using UnityEngine;

namespace Assets.Scripts
{
    public class ShadowCollider : MonoBehaviour
    {
        private ClientConnectionManager _connectionManager;

        private void Awake()
        {
            _connectionManager = GetComponentInParent<ClientConnectionManager>();
        }

        void OnTriggerEnter(Collider collider)
        {
            var shadow = collider.gameObject.GetComponent<Shadow>();

            if (shadow == null)
                return;

            //shadow.enabled = false;
            //shadow.gameObject.SetActive(false);
            //collider.enabled = false;
            //collider.gameObject.SetActive(false);
            var player = gameObject.GetComponent<PlayerController>();
            if (player.CaughtBetweenPlanes)
            {
                //textLogDisplayManager.AddText("You are caught between the planes and cannot fight.  You need to find a planeswaker to return you to your realm first.", AnnouncementType.System);
                //return;
            }
            _connectionManager.SendMessage(new CreateBattleInstanceRequest());

            //we need to call the battle instance here!!

            Destroy(shadow);
        }
    }
}