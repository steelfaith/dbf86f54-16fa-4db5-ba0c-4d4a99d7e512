using Photon.SocketServer;
using Photon.SocketServer.Rpc;

namespace ShadowMonstersServer.OperationHandlers
{
    public class WorldActorOperationHandler : PlayerActor, IOperationHandler
    {
        private ShadowPeer _peer;

        public WorldActorOperationHandler(ShadowPeer peer) : base(peer)
        {
            _peer = peer;
        }


        public OperationResponse OnOperationRequest(PeerBase peer, OperationRequest operationRequest, SendParameters sendParameters)
        {
            return null;
        }

        public void OnDisconnect(PeerBase peer)
        {
            Dispose();
            ((Peer)peer).SetCurrentOperationHandler(null);
            peer.Dispose();
        }
    }
}