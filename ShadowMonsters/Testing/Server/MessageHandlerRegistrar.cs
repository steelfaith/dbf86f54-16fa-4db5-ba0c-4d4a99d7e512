using System;
using System.Collections.Concurrent;
using Common;

namespace Server
{
    public class MessageHandlerRegistrar : IMessageHandlerRegistrar
    {
        private readonly ConcurrentDictionary<OperationCode, Action<RouteableMessage>> _messageHandlers;

        public MessageHandlerRegistrar()
        {
            _messageHandlers = new ConcurrentDictionary<OperationCode, Action<RouteableMessage>>();
        }

        public void Register(OperationCode operationCode, Action<RouteableMessage> handler)
        {
            _messageHandlers[operationCode] = handler;
        }

        public Action<RouteableMessage> Resolve(OperationCode operationCode)
        {
            Action<RouteableMessage> handler;
            if (_messageHandlers.TryGetValue(operationCode, out handler))
                return handler;

            return null; //convert this to return the default message handler later
        }
    }
}