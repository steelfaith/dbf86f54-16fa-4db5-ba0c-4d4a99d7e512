using System;
using Common;
using Common.Networking;

namespace Server.Common.Interfaces
{
    public interface IConnectionManager
    {
        void AddConnection(IClientConnection clientConnection);
        //void Send(RouteableMessage routeableMessage);
        IClientConnection GetClientConnection(Guid connectionId);
    }
}