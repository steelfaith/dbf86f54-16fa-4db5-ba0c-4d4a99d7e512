using ExitGames.Client.Photon;

namespace ClientTesting.GameStates
{
    public class Connected : GameState
    {
        public Connected(PhotonEngine engine) : base(engine)
        {
        }

        public override void OnUpdate()
        {
            _engine.Peer.Service();
        }

        public override void SendOperation(OperationRequest request, bool sendReliable, byte channelId, bool encrypted)
        {
            _engine.Peer.OpCustom(request, sendReliable, channelId, encrypted);
        }
    }
}