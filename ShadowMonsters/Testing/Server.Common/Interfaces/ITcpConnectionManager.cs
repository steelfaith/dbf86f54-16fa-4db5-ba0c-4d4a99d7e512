using Common;

namespace Server.Common.Interfaces
{
    public interface ITcpConnectionManager
    {
        void AddConnection(ITcpConnection tcpConnection);
        void Send(RouteableMessage routeableMessage);
    }
}