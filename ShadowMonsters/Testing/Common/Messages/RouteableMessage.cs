using System;

namespace Common.Messages
{
    /// <summary>
    /// Probably come up with a new name for this later, routeable isnt really clear whats being routed
    /// really the TcpConnectionId, is the client connection Id and routeable means client directable
    /// </summary>
    public class RouteableMessage
    {
        public Message Message { get; }
        public Guid TcpConnectionId { get; }

        public RouteableMessage(Guid tcpConnectionId, Message message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            if (tcpConnectionId == Guid.Empty)
                throw new ArgumentException(nameof(tcpConnectionId));

            TcpConnectionId = tcpConnectionId;
            Message = message;
        }

    }
}