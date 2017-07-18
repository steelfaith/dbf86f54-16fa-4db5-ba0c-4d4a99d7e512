using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using Common;
using Common.Messages;
using Common.Messages.Events;
using Common.Messages.Requests;
using Common.Messages.Responses;
using UnityEngine;

namespace Assets.ServerStubHome
{
    public class RemoteActorMovementAgent : MonoBehaviour
    {
        private ClientConnectionManager _connectionManager;
        private static RemoteActorMovementAgent _remoteActorMovementAgent;

        private void Awake()
        {
            _connectionManager = GetComponentInParent<ClientConnectionManager>();
        }

        private void Start()
        {
        }

        private void Update()
        {
            //StartCoroutine(DoNothing());
        }

        private void FixedUpdate()
        {
            
        }

        public void RemoteActorMoved(RouteableMessage routeableMessage)
        {
            PlayerMoveEvent response = routeableMessage.Message as PlayerMoveEvent;

            if (response == null)
                return;
        }

        public static RemoteActorMovementAgent Instance()
        {
            if (!_remoteActorMovementAgent)
            {
                _remoteActorMovementAgent = FindObjectOfType(typeof(RemoteActorMovementAgent)) as RemoteActorMovementAgent;
            }
            return _remoteActorMovementAgent;
        }

        public IEnumerator DoNothing()
        {
            yield return null;
        }
    }
}