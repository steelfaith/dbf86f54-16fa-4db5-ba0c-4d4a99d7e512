using Client;
using Common.Messages;
using UnityEngine;

namespace Assets.Scripts.NetworkAgents
{
    public class ClientConnectionManager : MonoBehaviour
    {
        public AuthenticationAgent AuthenticationAgent;
        public WorldRegionAgent WorldRegionAgent;
        public BattleInstanceAgent BattleInstanceAgent;
        private NetworkConnector _connection;
        private static ClientConnectionManager _clientConnectionManager;

        public int ClientId { get; set; }

        void Awake()
        {
            DontDestroyOnLoad(this);
            InstantiateAgents();
        }

        private void Start()
        {
            Application.runInBackground = true;
            _connection = new NetworkConnector();
            _connection.Connect();
            RegisterMessageHandlers();
        }

        private void InstantiateAgents()
        {
            AuthenticationAgent = Instantiate(AuthenticationAgent, transform);
            AuthenticationAgent.ConnectionManager = this;
            WorldRegionAgent = Instantiate(WorldRegionAgent, transform);
            BattleInstanceAgent = Instantiate(BattleInstanceAgent, transform);
        }

        private void RegisterMessageHandlers()
        {
            _connection.RegisterHandler(OperationCode.ConnectResponse,  AuthenticationAgent.HandleConnectionResponse);
            _connection.RegisterHandler(OperationCode.SelectCharacterResponse, AuthenticationAgent.HandleSelectCharacterResponse);
            _connection.RegisterHandler(OperationCode.PlayerMoveEvent, WorldRegionAgent.RemoteActorMoved);
            _connection.RegisterHandler(OperationCode.CreateBattleInstanceRequest, BattleInstanceAgent.BattleInstanceCreatedResponse);
        }

        public void SendMessage(Message message)
        {
            message.ClientId = ClientId;
            _connection.SendMessage(message);
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
