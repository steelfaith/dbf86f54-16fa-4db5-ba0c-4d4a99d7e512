using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using log4net;

namespace Common.Networking.Sockets
{
    /// <summary>
    /// Not really sure if this will be used for anything but metrics in the future however I figured it
    /// would be a good idea to put the stub in now.
    /// </summary>
    public class TcpConnectionManager
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(TcpConnectionManager));
        private static readonly AsyncLogger AsyncLogger = new AsyncLogger(Logger, true);

        private readonly ConcurrentDictionary<Guid, TcpConnection> _connections = new ConcurrentDictionary<Guid, TcpConnection>();

        public void AddConnection(TcpConnection tcpConnection)
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
    }
}