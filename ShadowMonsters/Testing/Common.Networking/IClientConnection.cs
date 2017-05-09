using System;

namespace Common.Networking
{
    public interface IClientConnection
    {
        Guid Id { get; }
        void StartReceiveing();
        void Send(Message message);
    }
}