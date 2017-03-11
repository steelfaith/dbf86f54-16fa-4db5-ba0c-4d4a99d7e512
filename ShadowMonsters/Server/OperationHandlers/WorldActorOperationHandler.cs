using Photon.SocketServer;
using Photon.SocketServer.Rpc;
using ShadowMonsters.Common;

namespace ShadowMonsters.Server.OperationHandlers
{
    public class WorldActorOperationHandler : IOperationHandler
    {
        private readonly ShadowPeer _shadowPeer;

        public WorldActorOperationHandler(ShadowPeer peer)
        {
            _shadowPeer = peer;
        }

        public void OnDisconnect(PeerBase peer)
        {
        }

        public OperationResponse OnOperationRequest(PeerBase peer, OperationRequest operationRequest,SendParameters sendParameters)
        {
            switch ((OperationCode) operationRequest.OperationCode)
            {
                case OperationCode.EnterWorld:
                {
                    return OperationEnterWorld(peer, operationRequest,sendParameters);
                }

                case OperationCode.StartBattle:
                {
                    return null;
                }
            }

            return null;
        }


        public
            OperationResponse OperationEnterWorld(PeerBase peer, OperationRequest request, SendParameters sendParameters)
        {
            return null;
        }
    }
}