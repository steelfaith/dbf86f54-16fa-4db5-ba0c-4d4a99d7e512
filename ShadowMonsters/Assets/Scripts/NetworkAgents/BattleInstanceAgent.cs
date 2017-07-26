using Common.Messages;
using Common.Messages.Events;
using Common.Messages.Responses;
using UnityEngine;

namespace Assets.Scripts.NetworkAgents
{
    public class BattleInstanceAgent : MonoBehaviour
    {
        private ClientConnectionManager _connectionManager;

        public delegate void BattleInstanceCreated();
        public static event BattleInstanceCreated OnBattleInstanceCreated;

        private void Awake()
        {
            _connectionManager = GetComponentInParent<ClientConnectionManager>();
        }

        public void BattleInstanceCreatedResponse(RouteableMessage routeableMessage)
        {
            CreateBattleInstanceResponse response = routeableMessage.Message as CreateBattleInstanceResponse;

            if (response == null)
                return;
        }
    }
}