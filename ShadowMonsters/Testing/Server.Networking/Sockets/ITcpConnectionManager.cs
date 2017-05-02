using Common;
using Common.Networking.Sockets;

namespace Server.Networking.Sockets
{
    public interface ITcpConnectionManager
    {
        void AddConnection(ITcpConnection tcpConnection);
        void Send(RouteableMessage routeableMessage);
    }
}