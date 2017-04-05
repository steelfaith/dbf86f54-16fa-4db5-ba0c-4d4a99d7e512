using ExitGames.Client.Photon;
using Photon.SocketServer;

namespace ClientTesting.GameStates
{
    public class GameState : IGameState
    {
        protected PhotonEngine _engine;

        protected GameState(PhotonEngine engine)
        {
            _engine = engine;
        }

        public virtual void OnUpdate()//do nothing
        {
        }

        public virtual void SendOperation(OperationRequest request, bool sendReliable, byte channelId, bool encrypted)//do nothing
        {
        }
    }
}