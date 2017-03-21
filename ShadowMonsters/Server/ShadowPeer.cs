using System;
using Photon.SocketServer;
using Photon.SocketServer.Rpc;
using ShadowMonstersServer.OperationHandlers;

namespace ShadowMonstersServer
{
    public class ShadowPeer : Peer
    {
        private readonly IOperationHandler _operationHandler;

        public IDisposable CounterSubscription { get; set; }

        public World World { get; }

        public ShadowPeer(InitRequest initRequest, World world) : base(initRequest)
        {
            World = world;
            _operationHandler = new ConnectionOperationHandler(this);
            SetCurrentOperationHandler(_operationHandler);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (CounterSubscription != null)
                {
                    CounterSubscription.Dispose();
                    CounterSubscription = null;
                }
            }

            base.Dispose(disposing);
        }
    }
}