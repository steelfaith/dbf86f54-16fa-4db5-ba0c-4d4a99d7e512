using System;
using Common;

namespace Server.Common.Interfaces
{
    public interface ITcpConnection
    {
        Guid Id { get; }
        void StartReceiveing();
        void Send(Message message);
        void AppendData(byte[] data, int bytesRead, Guid connectionId);
    }
}