using System;
using System.Collections.Concurrent;
using Common.Interfaces.Network;
using NLog;
using Server.Common.Interfaces;

namespace Server
{
    public class ConnectionManager : IConnectionManager
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly ConcurrentDictionary<Guid, IClientConnection> _connections = new ConcurrentDictionary<Guid, IClientConnection>();

        public void AddConnection(IClientConnection clientConnection)
        {
            if(clientConnection == null)
                throw new ArgumentNullException(nameof(clientConnection));

            try
            {
                _connections[clientConnection.Id] = clientConnection;
                Logger.Info($"Added additional connection for client {clientConnection.Id}");

            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        public bool TryGetClientConnection(Guid connectionId, out IClientConnection clientConnection)
        {
            if (_connections.TryGetValue(connectionId, out clientConnection))
            {
                if (!clientConnection.IsConnected)
                {
                    if (_connections.TryRemove(connectionId, out clientConnection))
                        Logger.Warn($"Removing client connection {connectionId}");
                    else
                        Logger.Warn($"Failed to remove client connection {connectionId}");

                    return false;
                }
                    
                return true;
            }

            return false;
        }
    }
}