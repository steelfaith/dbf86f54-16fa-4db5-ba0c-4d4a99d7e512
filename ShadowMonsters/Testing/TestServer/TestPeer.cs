using ExitGames.Logging;
using log4net.Repository.Hierarchy;
using Photon.SocketServer;
using PhotonHostRuntimeInterfaces;

namespace TestServer
{
    public class TestPeer : ClientPeer
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        public TestPeer(InitRequest initRequest) : base(initRequest)
        {
        }

        protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters)
        {
           
        }

        protected override void OnDisconnect(DisconnectReason reasonCode, string reasonDetail)
        {
        }
    }
}