using System;
using System.Collections.Concurrent;
using Common;
using log4net;

namespace Client
{
    public class MessageHandlerRegistrar : IMessageHandlerRegistrar
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(MessageHandlerRegistrar));
        private readonly ConcurrentDictionary<OperationCode, IMessageHandler> _messageHandlers;

        public MessageHandlerRegistrar()
        {
            _messageHandlers = new ConcurrentDictionary<OperationCode, IMessageHandler>();
        }

        public void Register(IMessageHandler handler)
        {
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            try
            {
                _messageHandlers.TryAdd(handler.OperationCode, handler);
            }
            catch (OverflowException ex)
            {
                Logger.Error(ex);
            }
            
        }

        public IMessageHandler Resolve(OperationCode operationCode)
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