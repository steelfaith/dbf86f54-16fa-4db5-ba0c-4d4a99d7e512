using Client;
using Common.Messages;
using UnityEngine;

namespace Assets.Scripts.NetworkAgents
{
    public class ClientConnectionManager : MonoBehaviour
    {
        public AuthenticationAgent AuthenticationAgent;
        public WorldRegionAgent WorldRegionAgent;
        public NetworkConnector Connection;
        private static ClientConnectionManager _clientConnectionManager;

        public int ClientId { get; set; }

        void Awake()
        {
            Application.runInBackground = true;
            DontDestroyOnLoad(this);
            InstantiateAgents();
            Connection = new NetworkConnector();
        }

        private void Start()
        {
            Connection.Connect();
            RegisterMessageHandlers();
        }

        private void InstantiateAgents()
        {
            AuthenticationAgent = Instantiate(AuthenticationAgent, transform);
            AuthenticationAgent.ConnectionManager = this;
            WorldRegionAgent = Instantiate(WorldRegionAgent, transform);
        }

        private void RegisterMessageHandlers()
        {
            Connection.RegisterHandler(OperationCode.SelectCharacterResponse, AuthenticationAgent.HandleSelectCharacterResponse);
            Connection.RegisterHandler(OperationCode.PlayerMoveEvent, WorldRegionAgent.RemoteActorMoved);
        }

        public void SendMessage(Message message)
        {
            message.ClientId = ClientId;
            Connection.SendMessage(message);
        }

        public static ClientConnectionManager Instance()
        {
            if (!_clientConnectionManager)
            {
                _clientConnectionManager = FindObjectOfType(typeof(ClientConnectionManager)) as ClientConnectionManager;
            }
            return _clientConnectionManager;
        }
    }
}
