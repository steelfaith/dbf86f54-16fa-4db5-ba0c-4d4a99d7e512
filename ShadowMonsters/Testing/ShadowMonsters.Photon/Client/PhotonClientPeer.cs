using System;
using System.Collections.Generic;
using ExitGames.Logging;
using Photon.SocketServer;
using ShadowMonsters.Framework;
using ShadowMonsters.Photon.Server;

namespace ShadowMonsters.Photon.Client
{
    public class PhotonClientPeer : ClientPeer
    {
        protected readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        public Guid Id { get; } = Guid.NewGuid();
        private readonly Dictionary<Type, ClientData> _clientData = new Dictionary<Type, ClientData>();
        private readonly PhotonApplication _server;
        private readonly PhotonClientHandlerList _handlerlist;
        public PhotonServerPeer CurrentServer { get; }

        public delegate PhotonClientPeer Factory(InitRequest initRequest);

        public PhotonClientPeer(InitRequest initRequest, IEnumerable<ClientData> clientData, PhotonClientHandlerList handlerList, PhotonApplication application) : base(initRequest)
        {
            _handlerlist = handlerList;
            _server = application;

            foreach(var item in clientData)
                _clientData.Add(item.GetType(), item);

            _server.ConnectionCollection.Clients.TryAdd(Id, this);
        }

        

        protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters)
        {
            var subcode = operationRequest.Parameters.ContainsKey(_server.SubCodeParameterKey)
                ? (int?) Convert.ToInt32(operationRequest.Parameters[_server.SubCodeParameterKey])
                : null;

            _handlerlist.HandleMessage(new PhotonRequest(operationRequest.OperationCode, subcode, operationRequest.Parameters), this);
        }

        protected override void OnDisconnect(PhotonHostRuntimeInterfaces.DisconnectReason reasonCode, string reasonDetail)
        {
            _server.ConnectionCollection.OnClientDisconnect(this);
            Logger.InfoFormat("Client {0} has disconnected.", Id);
        }

        public T ClientData<T>() where T : ClientData
        {
            ClientData result = null;
            if (_clientData.TryGetValue(typeof(T), out result))
                return result as T;

            return null;
        }

    }
}