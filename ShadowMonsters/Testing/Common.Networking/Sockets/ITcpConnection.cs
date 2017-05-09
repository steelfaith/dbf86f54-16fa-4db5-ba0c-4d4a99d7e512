using System;

namespace Common.Networking.Sockets
{
    public interface ITcpConnection : IClientConnection
    {
        void AppendData(byte[] data, int bytesRead, Guid connectionId);
    }
}