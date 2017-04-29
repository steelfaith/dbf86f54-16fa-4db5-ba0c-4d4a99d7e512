using System;
using System.Collections.Generic;
using Common;
using log4net;

namespace Common.Networking.Sockets
{
    public class TcpConnectionManager : ITcpConnectionManager
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(TcpConnectionManager));
        private static readonly AsyncLogger AsyncLogger = new AsyncLogger(Logger);
        private readonly Dictionary<Guid, ITcpConnection> _connections = new Dictionary<Guid, ITcpConnection>();
        private readonly object _lock = new object();

        public void AddConnection(ITcpConnection tcpConnection)
        {
            if(tcpConnection == null)
                throw new ArgumentNullException(nameof(tcpConnection));

            try
            {
                lock (_lock)
                {
                    _connections.Add(tcpConnection.Id, tcpConnection);
                }
                
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

            lock (_lock)
            {
                ITcpConnection connection = null;
                if (_connections.TryGetValue(routeableMessage.ConnectionId, out connection))
                    connection.Send(routeableMessage.Message);//turn message into bytes
                else
                    AsyncLogger.InfoFormat($"Unable to find tcpConnection with Id {routeableMessage.ConnectionId}");
            }

        }

    }
}