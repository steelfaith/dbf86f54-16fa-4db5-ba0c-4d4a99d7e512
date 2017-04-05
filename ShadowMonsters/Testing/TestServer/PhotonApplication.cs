using System.IO;
using ExitGames.Logging;
using ExitGames.Logging.Log4Net;
using log4net.Config;
using Photon.SocketServer;

namespace TestServer
{
    public class PhotonApplication : ApplicationBase
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        protected override PeerBase CreatePeer(InitRequest initRequest)
        {
            

            var testPeer = new TestPeer(initRequest);




            return testPeer;
        }

        protected override void Setup()
        {
            Logger.InfoFormat("Attempting to start test server {0}");

        }

        protected override void TearDown()
        {
        }
    }
}