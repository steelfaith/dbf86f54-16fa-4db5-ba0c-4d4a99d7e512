using System;
using System.Collections.Generic;
using Common;

namespace Client
{
    public class MessageHandlerRegistrar : IMessageHandlerRegistrar
    {
        private readonly Dictionary<OperationCode, Action<RouteableMessage>> _messageHandlers;
        private readonly object _lock = new object();

        public MessageHandlerRegistrar()
        {
            _messageHandlers = new Dictionary<OperationCode, Action<RouteableMessage>>();
        }

        public void Register(OperationCode operationCode, Action<RouteableMessage> handler)
        {
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            lock (_lock)
            {
                _messageHandlers.Add(operationCode, handler);
            }  
        }


        public Action<RouteableMessage> Resolve(OperationCode operationCode)
        {
            lock (_lock)
            {
                Action<RouteableMessage> method;
                if (_messageHandlers.TryGetValue(operationCode, out method))
                    return method;

                return null;
            }
        }
    }
}