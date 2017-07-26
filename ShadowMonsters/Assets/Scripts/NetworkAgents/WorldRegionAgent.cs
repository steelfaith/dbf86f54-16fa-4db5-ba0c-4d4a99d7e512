using System.Collections;
using System.Collections.Generic;
using Common.Messages;
using Common.Messages.Events;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Assets.Scripts.NetworkAgents
{
    public class WorldRegionAgent : MonoBehaviour
    {
        private ClientConnectionManager _connectionManager;
        private static WorldRegionAgent _remoteActorMovementAgent;

        private readonly Dictionary<int, GameObject> _remotePlayers = new Dictionary<int, GameObject>();
        private readonly Queue<Dictionary<int, PositionForwardTuple>> _remoteMovementQueue = new Queue<Dictionary<int, PositionForwardTuple>>();

        private void Awake()
        {
            _connectionManager = GetComponentInParent<ClientConnectionManager>();
        }

        private void FixedUpdate()
        {
            if (_remoteMovementQueue.Count > 0)
                StartCoroutine(DequeueMovementUpdates());
        }

        public void RemoteActorMoved(RouteableMessage routeableMessage)
        {
            PlayerMoveEvent response = routeableMessage.Message as PlayerMoveEvent;

            if (response == null)
                return;

            if(response.UpdatedPositions != null && response.UpdatedPositions.Count > 0)
                _remoteMovementQueue.Enqueue(response.UpdatedPositions);
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
                    if (item.Key != _connectionManager.ClientId)
                    {
                        if (!_remotePlayers.ContainsKey(item.Key))
                        {
                            var prefab = Resources.Load("UnityChan/Prefabs/unitychan");
                            GameObject clone = Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
                            clone.transform.localScale = new Vector3(5, 5, 5);
                            if (clone == null)
                                yield return null;

                            clone.transform.position = new Vector3(item.Value.Position.X, item.Value.Position.Y, item.Value.Position.Z);
                            clone.transform.forward = new Vector3(item.Value.Forward.X, item.Value.Forward.Y, item.Value.Forward.Z);
                            _remotePlayers.Add(item.Key, clone);
                        }
                        else
                        {
                            var newPosition = new Vector3(item.Value.Position.X, item.Value.Position.Y, item.Value.Position.Z);
                            var newFoward = new Vector3(item.Value.Forward.X, item.Value.Forward.Y, item.Value.Forward.Z);
                            _remotePlayers[item.Key].transform.position = Vector3.Lerp(newPosition, _remotePlayers[item.Key].transform.position, 0.16f);
                        }
                    }
                }
            }

        }
    }
}