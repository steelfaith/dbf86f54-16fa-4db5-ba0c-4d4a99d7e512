using System;
using System.Collections.Concurrent;
using Common;
using log4net;

namespace Common.Networking.Sockets
{
    public class TcpConnectionManager : ITcpConnectionManager
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(TcpConnectionManager));
        private static readonly AsyncLogger AsyncLogger = new AsyncLogger(Logger);

        private readonly ConcurrentDictionary<Guid, ITcpConnection> _connections = new ConcurrentDictionary<Guid, ITcpConnection>();

        public void AddConnection(ITcpConnection tcpConnection)
        {
            if(tcpConnection == null)
                throw new ArgumentNullException(nameof(tcpConnection));

            try
            {
                if (!_connections.TryAdd(tcpConnection.Id, tcpConnection))
                    AsyncLogger.ErrorFormat($"Failed to add tcpConnection {tcpConnection.Id} to the tcpConnection manager.");

                AsyncLogger.InfoFormat($"Added additional tcpConnection for client {tcpConnection.Id}");

            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        public void Send(RouteableMessage routeableMessage)
        {
            if(routeableMessage == null)
                throw new ArgumentNullException(nameof(routeableMessage));

            ITcpConnection connection = null;
            if (_connections.TryGetValue(routeableMessage.ConnectionId, out connection))
                connection.Send(routeableMessage.Message);//turn message into bytes
            else
                AsyncLogger.InfoFormat($"Unable to find tcpConnection with Id {routeableMessage.ConnectionId}");
        }

    }
}