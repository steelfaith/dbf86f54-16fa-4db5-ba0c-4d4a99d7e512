using System;
using System.Collections.Generic;
using Common;
using log4net;

namespace Client
{
    public class MessageHandlerRegistrar : IMessageHandlerRegistrar
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(MessageHandlerRegistrar));
        private readonly Dictionary<OperationCode, IMessageHandler> _messageHandlers;
        private readonly object _lock = new object();

        public MessageHandlerRegistrar()
        {
            _messageHandlers = new Dictionary<OperationCode, IMessageHandler>();
        }

        public void Register(IMessageHandler handler)
        {
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            lock (_lock)
            {
                _messageHandlers.Add(handler.OperationCode, handler);
            }  
        }

        public IMessageHandler Resolve(OperationCode operationCode)
        {
            lock (_lock)
            {
                IMessageHandler handler;
                if (_messageHandlers.TryGetValue(operationCode, out handler))
                {
                    return handler;
                }

                return null;
            }
        }
    }
}