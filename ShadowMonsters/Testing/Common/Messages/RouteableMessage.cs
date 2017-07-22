using System;

namespace Common.Messages
{
    public class RouteableMessage
    {
        public Message Message { get; }
        public Guid ConnectionId { get; }

        public RouteableMessage(Guid connectionId, Message message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            if (connectionId == Guid.Empty)
                throw new ArgumentException(nameof(connectionId));

            ConnectionId = connectionId;
            Message = message;
        }

    }
}