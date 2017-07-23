using Assets.ServerStubHome;
using Common.Messages.Requests;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

namespace Assets.Scripts
{
    public class ClientPositionController : MonoBehaviour
    {
        private ClientConnectionManager _clientConnectionManager;

        void Start()
        {
            _clientConnectionManager = FindObjectOfType(typeof(ClientConnectionManager)) as ClientConnectionManager;
        }

        /// <summary>
        /// Right now this will throttle the players movement to about 60updates per second its lazy but saves time to 
        /// get us to battle instance testing, in the future this needs to be refined after we calculate a proper update
        /// rate
        /// </summary>
        void FixedUpdate()
        {
            var body = GetComponent<Rigidbody>();

            if (body == null)
                return;

            var convertedPosition = new Common.Vector3
            {
                X = body.transform.position.x,
                Y = body.transform.position.y,
                Z = body.transform.position.z
            };

            var convertedForward = new Common.Vector3
            {
                X = body.transform.forward.x,
                Y = body.transform.forward.y,
                Z = body.transform.forward.z
            };

            _clientConnectionManager.SendMessage(new PlayerMoveRequest(convertedPosition, convertedForward));
        }
    }
}