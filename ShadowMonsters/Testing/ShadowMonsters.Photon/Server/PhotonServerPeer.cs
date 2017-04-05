using System;
using Photon.SocketServer;
using Photon.SocketServer.ServerToServer;
using PhotonHostRuntimeInterfaces;

namespace ShadowMonsters.Photon.Server
{
    public class PhotonServerPeer : InboundS2SPeer
    {
        private readonly PhotonServerHandlerList _handlerList;
        protected readonly PhotonApplication Server;
        public Guid? Id { get; private set; }

        public delegate PhotonServerPeer Factory(InitResponse response);

        public PhotonServerPeer(InitResponse response, PhotonServerHandlerList handlerList, PhotonApplication application) : base(response)
        {
            _handlerList = handlerList;
            Server = application;
        }

        protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters)
        {
            var subcode = operationRequest.Parameters.ContainsKey(Server.SubCodeParameterKey)
                ? (int?) Convert.ToInt32(operationRequest.Parameters[Server.SubCodeParameterKey])
                : null;

            _handlerList.HandleMessage(new PhotonRequest(operationRequest.OperationCode, subcode, operationRequest.Parameters), this);
        }

        protected override void OnOperationResponse(OperationResponse operationResponse, SendParameters sendParameters)
        {
            var subcode = operationResponse.Parameters.ContainsKey(Server.SubCodeParameterKey)
            ? (int?)Convert.ToInt32(operationResponse.Parameters[Server.SubCodeParameterKey])
            : null;

            _handlerList.HandleMessage(new PhotonResponse(operationResponse.OperationCode, subcode, operationResponse.Parameters, operationResponse.DebugMessage, operationResponse.ReturnCode), this);
        }

        protected override void OnEvent(IEventData eventData, SendParameters sendParameters)
        {
            var subcode = eventData.Parameters.ContainsKey(Server.SubCodeParameterKey)
                ? (int?) Convert.ToInt32(eventData.Parameters[Server.SubCodeParameterKey])
                : null;

            _handlerList.HandleMessage(new PhotonEvent(eventData.Code, subcode, eventData.Parameters), this);
        }

        protected override void OnDisconnect(DisconnectReason reasonCode, string reasonDetail)
        {
            //Server.ConnectionCollection.OnDisconnect(this);
        }

        
    }
}