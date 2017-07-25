using System;
using Common;
using Common.Interfaces.Network;

namespace Server.Common.Interfaces
{
    public interface IConnectionManager
    {
        void AddConnection(IClientConnection clientConnection);
        bool TryGetClientConnection(Guid connectionId, out IClientConnection clientConnection);
    }
}