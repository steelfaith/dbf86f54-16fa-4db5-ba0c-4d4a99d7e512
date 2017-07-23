using UnityEngine;

namespace Assets.Scripts
{
    public class ShadowCollider : MonoBehaviour
    {
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

            //we will eventually need to pass data through this more research on that later i suppose
            //this is also currently nasty it just flash switches the entire view lol 
            if (AnyManager._anyManager.LoadCombatScene())
                Destroy(shadow);
        }
    }
}