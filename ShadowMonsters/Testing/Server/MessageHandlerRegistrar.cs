using System;
using System.Collections.Concurrent;
using Common.Messages;
using Server.Common;
using Server.Common.Interfaces;

namespace Server
{
    public class MessageHandlerRegistrar : IMessageHandlerRegistrar
    {
        private readonly ConcurrentDictionary<OperationCode, Action<RouteableMessage>> _messageHandlers;
        private readonly ConcurrentDictionary<InstanceRoute, Action<InstanceMessage>> _instanceHandlers;


        public MessageHandlerRegistrar()
        {
            _messageHandlers = new ConcurrentDictionary<OperationCode, Action<RouteableMessage>>();
            _instanceHandlers = new ConcurrentDictionary<InstanceRoute, Action<InstanceMessage>>();
        }

        public void Register(OperationCode operationCode, Action<RouteableMessage> handler)
        {
            _messageHandlers[operationCode] = handler;
        }

        public void Register(InstanceRoute instanceRoute, Action<InstanceMessage> handler)
        {
            _instanceHandlers[instanceRoute] = handler;
        }


        public Action<RouteableMessage> Resolve(OperationCode operationCode)
        {
            Action<RouteableMessage> handler;
            if (_messageHandlers.TryGetValue(operationCode, out handler))
                return handler;

            return null; //convert this to return the default message handler later

        }

        public Action<InstanceMessage> Resolve(InstanceRoute instanceRoute)
        {
            Action<InstanceMessage> handler;
            if (_instanceHandlers.TryGetValue(instanceRoute, out handler))
                return handler;

            return null; //convert this to return the default message handler later
        }
    }
}