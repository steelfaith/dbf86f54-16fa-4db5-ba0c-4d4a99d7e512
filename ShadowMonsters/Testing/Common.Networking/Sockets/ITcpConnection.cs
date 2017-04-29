using System;

namespace Common.Networking.Sockets
{
    public interface ITcpConnection
    {
        Guid Id { get; }
        void StartReceiveing();
        void Send(Message message);
        void AppendData(byte[] data, int bytesRead, Guid connectionId);
    }
}