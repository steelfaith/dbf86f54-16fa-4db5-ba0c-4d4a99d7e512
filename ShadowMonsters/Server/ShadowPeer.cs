using Photon.SocketServer;
using Photon.SocketServer.Rpc;
using ShadowMonsters.Server.OperationHandlers;

namespace ShadowMonsters.Server
{
    public class ShadowPeer : Peer
    {
        private readonly IOperationHandler _operationHandler;

        public ShadowPeer(InitRequest initRequest) : base(initRequest)
        {
            _operationHandler = new WorldActorOperationHandler(this);
            SetCurrentOperationHandler(_operationHandler);
        }
    }
}