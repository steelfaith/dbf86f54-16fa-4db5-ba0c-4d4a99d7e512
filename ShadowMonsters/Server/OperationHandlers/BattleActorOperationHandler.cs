using Photon.SocketServer;
using Photon.SocketServer.Rpc;

namespace ShadowMonsters.Server.OperationHandlers
{
    public class BattleActorOperationHandler : IOperationHandler
    {
        private readonly ShadowPeer _shadowPeer;
        public BattleActorOperationHandler(ShadowPeer peer)
        {
            _shadowPeer = peer;
        }

        public void OnDisconnect(PeerBase peer)
        {
        }

        public OperationResponse OnOperationRequest(PeerBase peer, OperationRequest operationRequest, SendParameters sendParameters)
        {
            return null;
        }
    }
}