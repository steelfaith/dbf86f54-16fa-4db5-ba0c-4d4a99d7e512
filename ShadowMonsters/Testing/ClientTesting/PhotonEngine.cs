
using ClientTesting.GameStates;
using ExitGames.Client.Photon;

namespace ClientTesting
{
    public class PhotonEngine : IPhotonPeerListener
    {
        public PhotonPeer Peer { get; set; }
        public GameState State { get; set; }

        //probably some type of scene controller

        public string ServerAddress;
        public string ApplicationName;

        public void Initalize()
        {
            Peer = new PhotonPeer(this, ConnectionProtocol.Tcp);
            Peer.Connect(ServerAddress, ApplicationName);
            State = new WaitForConnection(this);

        }

        public void Disconnect()
        {
            if(Peer != null)
                Peer.Disconnect();

            State = new Disconnected(this);
        }

        public void DebugReturn(DebugLevel level, string message)
        {
        }

        public void OnOperationResponse(OperationResponse operationResponse)
        {
        }

        public void OnStatusChanged(StatusCode statusCode)
        {
        }

        public void OnEvent(EventData eventData)
        {
        }
    }
}