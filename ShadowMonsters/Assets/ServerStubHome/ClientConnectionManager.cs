using UnityEngine;
using Client;
using Common.Messages.Requests;
using Common;

namespace Assets.ServerStubHome
{
    public class ClientConnectionManager : MonoBehaviour
    {
        NetworkConnector _connection;

        private void Start()
        {
            _connection = new NetworkConnector();
            _connection.Connect();
            SendMessage(new ConnectRequest(1));
            
        }

        public void RegisterMessageHandler(IMessageHandler handler)
        {
            if (_connection == null) return;
            _connection.RegisterHandler(handler);
        }

        public void SendMessage(Message message)
        {
            _connection.SendMessage(message);
        }
    }
}
