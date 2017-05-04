using Common;
using Common.Messages.Responses;

namespace Assets.ServerStubHome.MessageHandlers
{
    public class ConnectionResponseHandler : IMessageHandler
    {
        private AuthenticationAgent _authAgent;
        public ConnectionResponseHandler(AuthenticationAgent agent)
        {
            _authAgent = agent;
        }
        public OperationCode OperationCode
        {
            get
            {
                return OperationCode.ConnectResponse;
            }
        }

        public void HandleMessage(RouteableMessage routeableMessage)
        {
            ConnectResponse response = routeableMessage.Message as ConnectResponse;
            _authAgent.HandleAnnouncement(response.Announcement);
        }
    }
}
