using System;
using Common.Messages;

namespace Common.Interfaces.Network
{
    public interface IClientConnection
    {
        Guid Id { get; }
        void StartReceiveing();
        void Send(Message message);
        bool IsConnected { get; set; }
    }
}