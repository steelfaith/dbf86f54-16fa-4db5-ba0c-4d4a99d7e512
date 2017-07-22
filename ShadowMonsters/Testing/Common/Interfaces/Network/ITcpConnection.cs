using System;

namespace Common.Interfaces.Network
{
    public interface ITcpConnection : IClientConnection
    {
        void AppendData(byte[] data, int bytesRead, Guid connectionId);
    }
}