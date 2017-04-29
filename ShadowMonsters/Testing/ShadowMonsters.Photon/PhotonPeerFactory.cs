using ExitGames.Logging;
using Photon.SocketServer;
using ShadowMonsters.Photon.Client;
using ShadowMonsters.Photon.Server;

namespace ShadowMonsters.Photon
{
    public class PhotonPeerFactory
    {
        protected readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        private readonly PhotonServerPeer.Factory _serverPeerFactory;
        private readonly PhotonClientPeer.Factory _clientPeerFactory;
        private readonly PhotonConnectionCollection _subServerCollection;
        private readonly PhotonApplication _application;

        public PhotonPeerFactory(PhotonServerPeer.Factory serverPeerFactory, PhotonClientPeer.Factory clientPeerFactory, PhotonConnectionCollection subServerConnectionCollection, PhotonApplication application)
        {
            _serverPeerFactory = serverPeerFactory;
            _clientPeerFactory = clientPeerFactory;
            _subServerCollection = subServerConnectionCollection;
            _application = application;
        }

        public PeerBase CreatePeer(InitRequest initRequest)
        {
            if (IsServerPeer(initRequest))
            {
                Logger.Info("Received init request from sub server");
                return _serverPeerFactory(initRequest);
            }

            Logger.Info("Received init request from client");
            return _clientPeerFactory(initRequest);
        }

        public PhotonServerPeer CreatePeer(InitResponse initResponse)
        {
            var subServerPeer = _serverPeerFactory(initResponse);
            Logger.Info("Received init request from sub server.");

            //if (initResponse.RemotePort == _application.MasterEndpoint.Port)
            //{
            //    _application.Register(subServerPeer);
            //    Logger.InfoFormat("Sub server has been registered.");
            //}

            return subServerPeer;
        }

        public bool IsServerPeer(InitRequest initRequest)
        {
            return _subServerCollection.IsServerPeer(initRequest);
            //12:03 5
        }
    }
}