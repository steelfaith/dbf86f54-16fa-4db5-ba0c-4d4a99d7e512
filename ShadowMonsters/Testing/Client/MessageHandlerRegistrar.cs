﻿using System;
using System.Collections.Generic;
using Common;
using Common.Interfaces;
using Common.Messages;

namespace Client
{
    public class MessageHandlerRegistrar : IMessageHandlerRegistrar
    {
        private readonly Dictionary<OperationCode, Action<RouteableMessage>> _messageHandlers;
        private readonly Dictionary<OperationCode, Action<InstanceMessage>> _instanceMessageHandlers;
        private readonly object _lock = new object();

        public MessageHandlerRegistrar()
        {
            _messageHandlers = new Dictionary<OperationCode, Action<RouteableMessage>>();
            _instanceMessageHandlers = new Dictionary<OperationCode, Action<InstanceMessage>>();
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