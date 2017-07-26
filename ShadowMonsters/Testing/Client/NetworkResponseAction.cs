using System;
using Common.Messages;

namespace Client
{
    public class NetworkResponseAction
    {
        public Action<Message> Action { get; private set; }
        public Message Message { get; set; }
        public Type MessageType { get; private set; }

        public NetworkResponseAction(Action<Message> action, Type messageType)
        {
            Action = action;
            MessageType = messageType;
        }

    }
}