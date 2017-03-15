using Photon.SocketServer;
using Photon.SocketServer.Rpc;
using ShadowMonstersServer.OperationHandlers;

namespace ShadowMonstersServer
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