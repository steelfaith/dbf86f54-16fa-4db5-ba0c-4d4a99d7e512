using ExitGames.Client.Photon;
using UnityEditor;
using UnityEngine.EventSystems;

namespace Assets.Infrastructure.UI.Controllers
{
    public interface IViewController
    {
        bool IsConnected { get; }
        void ApplicationQuit();
        void Connect();
        void SendOperation(OperationRequest request, bool isReliable, byte channeId, bool encrypt);

        #region Photon

        void DebugReturn(DebugLevel level, string message);
        void OnOperationResponse(OperationResponse response);
        void OnEvent(EventData data);
        void OnUnexpectedEvent(EventData data);
        void OnUnexpectedOperationResponse(OperationResponse response);
        void OnUnexpectedStatusCode(StatusCode statusCode);
        void OnDisconnected(string message);

        #endregion

    }
}