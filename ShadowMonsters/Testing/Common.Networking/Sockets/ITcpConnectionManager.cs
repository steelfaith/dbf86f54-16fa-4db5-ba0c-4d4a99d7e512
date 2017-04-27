namespace Common.Networking.Sockets
{
    public interface ITcpConnectionManager
    {
        void AddConnection(ITcpConnection tcpConnection);
        void Send(RouteableMessage routeableMessage);
    }
}