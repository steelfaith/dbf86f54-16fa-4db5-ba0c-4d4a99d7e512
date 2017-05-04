using UnityEngine;
using Client;
using Common.Messages.Requests;
using Common;
using System;
using Assets.ServerStubHome.MessageHandlers;

namespace Assets.ServerStubHome
{
    public class ClientConnectionManager : MonoBehaviour
    {
        public AuthenticationAgent AuthenticationAgent;
        NetworkConnector _connection;
        private static ClientConnectionManager _clientConnectionManager;

        private void Start()
        {
            _connection = new NetworkConnector();
            _connection.Connect();
            RegisterMessageHandlers();
            SendMessage(new ConnectRequest(1));
            
        }

        private void RegisterMessageHandlers()
        {
            if (_connection == null) return;
            _connection.RegisterHandler(AuthenticationAgent.ConnectionResponse);
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
