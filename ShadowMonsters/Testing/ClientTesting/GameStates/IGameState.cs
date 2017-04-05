using ExitGames.Client.Photon;

namespace ClientTesting.GameStates
{
    public interface IGameState
    {
        void OnUpdate();
        void SendOperation(OperationRequest request, bool sendReliable, byte channelId, bool encrypted);
    }
}