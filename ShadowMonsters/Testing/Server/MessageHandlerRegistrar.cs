using System.Collections.Concurrent;
using Common;

namespace Server
{
    public class MessageHandlerRegistrar : IMessageHandlerRegistrar
    {
        private readonly ConcurrentDictionary<OperationCode, IMessageHandler> _messageHandlers;

        public MessageHandlerRegistrar()
        {
            _messageHandlers = new ConcurrentDictionary<OperationCode, IMessageHandler>();
        }

        public void Register(IMessageHandler handler)
        {
            _messageHandlers.TryAdd(handler.OperationCode, handler);
        }

        public IMessageHandler Resolve(OperationCode operationCode)
        {
            IMessageHandler handler;
            if (_messageHandlers.TryGetValue(operationCode, out handler))
            {
                return handler;
            }

            return null; //convert this to return the default message handler later
        }
    }
}