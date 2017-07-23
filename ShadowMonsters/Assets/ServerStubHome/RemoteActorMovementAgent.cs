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

        private readonly Dictionary<int, GameObject> _remotePlayers = new Dictionary<int, GameObject>();
        private readonly Queue<Dictionary<int, PositionForwardTuple>> _remoteMovementQueue = new Queue<Dictionary<int, PositionForwardTuple>>();

        private void Awake()
        {
            _connectionManager = GetComponentInParent<ClientConnectionManager>();
        }

        private void Start()
        {
        }

        private void Update()
        {
            if (_remoteMovementQueue.Count > 0)
                StartCoroutine(DequeueMovementUpdates());
        }

        private void FixedUpdate()
        {
            
        }

        public void RemoteActorMoved(RouteableMessage routeableMessage)
        {
            PlayerMoveEvent response = routeableMessage.Message as PlayerMoveEvent;

            if (response == null)
                return;

            if(response.UpdatedPositions != null && response.UpdatedPositions.Count > 0)
                _remoteMovementQueue.Enqueue(response.UpdatedPositions);
        }

        public static RemoteActorMovementAgent Instance()
        {
            if (!_remoteActorMovementAgent)
            {
                _remoteActorMovementAgent = FindObjectOfType(typeof(RemoteActorMovementAgent)) as RemoteActorMovementAgent;
            }
            return _remoteActorMovementAgent;
        }

        public IEnumerator DequeueMovementUpdates()
        {
            if (_remoteMovementQueue.Count > 0)
            {
                var updatedPositions = _remoteMovementQueue.Dequeue();

                if (updatedPositions == null)
                    yield return null;

                foreach (var item in updatedPositions)
                {
                    //if (item.Key != _connectionManager.ClientId)
                    //{
                    //    if (!_remotePlayers.ContainsKey(item.Key))
                    //    {
                    //        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    //        sphere.transform.position = new UnityEngine.Vector3(item.Value.X, item.Value.Y, item.Value.Z);
                    //        _remotePlayers.Add(item.Key, sphere);
                    //    }
                    //    else
                    //    {
                    //        _remotePlayers[item.Key].transform.position = new UnityEngine.Vector3(item.Value.X, item.Value.Y, item.Value.Z);
                    //    }
                    //}
                }
            }

        }
    }
}