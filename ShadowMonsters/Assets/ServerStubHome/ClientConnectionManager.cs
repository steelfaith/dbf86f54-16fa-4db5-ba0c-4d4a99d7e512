﻿using UnityEngine;
using Client;
using Common.Messages.Requests;
using Common;


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
            InstantiateAgents();
            RegisterMessageHandlers();      
        }

        private void InstantiateAgents()
        {
            //this is just so we don't have a million agents showing in the editor
            AuthenticationAgent = Instantiate(AuthenticationAgent, transform);
        }

        private void RegisterMessageHandlers()
        {
            if (_connection == null) return;
            _connection.RegisterHandler(OperationCode.ConnectResponse,  AuthenticationAgent.HandleAnnouncement);
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
