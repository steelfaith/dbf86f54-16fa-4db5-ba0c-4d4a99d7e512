using System;
using System.Collections.Concurrent;
using Common;
using Common.Networking;
using log4net;
using Server.Common.Interfaces;

namespace Server
{
    public class ConnectionManager : IConnectionManager
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ConnectionManager));
        private static readonly AsyncLogger AsyncLogger = new AsyncLogger(Logger);
        private readonly ConcurrentDictionary<Guid, IClientConnection> _connections = new ConcurrentDictionary<Guid, IClientConnection>();
        private readonly object _lock = new object();

        public void AddConnection(IClientConnection clientConnection)
        {
            if(clientConnection == null)
                throw new ArgumentNullException(nameof(clientConnection));

            try
            {
                _connections[clientConnection.Id] = clientConnection;
                AsyncLogger.InfoFormat($"Added additional connection for client {clientConnection.Id}");

            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        public IClientConnection GetClientConnection(Guid connectionId)
        {
            IClientConnection clientConnection;
            if(_connections.TryGetValue(connectionId, out clientConnection))
                return clientConnection;

            return null;
        }

        //public void Send(RouteableMessage routeableMessage)
        //{
        //    if(routeableMessage == null)
        //        throw new ArgumentNullException(nameof(routeableMessage));

        //    lock (_lock)
        //    {
        //        IClientConnection connection = null;
        //        if (_connections.TryGetValue(routeableMessage.ConnectionId, out connection))
        //            connection.Send(routeableMessage.Message);//turn message into bytes
        //        else
        //            AsyncLogger.InfoFormat($"Unable to find connection with Id {routeableMessage.ConnectionId}");
        //    }

        //}

    }
}