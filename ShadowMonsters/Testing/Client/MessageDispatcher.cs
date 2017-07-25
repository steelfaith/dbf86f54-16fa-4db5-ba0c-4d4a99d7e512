using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Common.Interfaces;
using Common.Messages;
using NLog;

namespace Common.Networking
{
    /// <summary>
    /// design of the dispatcher may change right now its a single thread processing
    /// messsages in a first come first serve order
    /// </summary>
    public class MessageDispatcher : IMessageDispatcher
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly Queue<RouteableMessage> _incomingMessages = new Queue<RouteableMessage>();
        private readonly AutoResetEvent _messageEvent = new AutoResetEvent(false);

        private readonly IMessageHandlerRegistrar _messageHandlerRegistrar;

        public MessageDispatcher(IMessageHandlerRegistrar messageHandlerRegistrar)
        {
            _messageHandlerRegistrar = messageHandlerRegistrar;
            var processingThread = new Thread(ProcessMessages);
            processingThread.Start();
        }

        private void ProcessMessages()
        {
            while (true)
            {
                try
                {
                    if (_incomingMessages.Count == 0)
                        _messageEvent.WaitOne();

                    var routeableMessage = _incomingMessages.Dequeue();

                    var handler = _messageHandlerRegistrar.Resolve(routeableMessage.Message.OperationCode);
                    handler?.Invoke(routeableMessage);

                    //Logger.Info("Attempting to process a message");
                    //Logger.Info("Message Type {0} Message Op Code {1} ", routeableMessage.Message.OperationType, routeableMessage.Message.OperationCode,);
                    //Logger.Info("Message Data {0}", Encoding.ASCII.GetString(routeableMessage.Message.));

                }
                catch (Exception ex)
                {
                    Logger.Error(ex.Message);
                    throw;
                }
            }
        }

        public void DispatchMessage(RouteableMessage message)
        {
            try
            {
                _incomingMessages.Enqueue(message);
                _messageEvent.Set();
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                throw;
            }
        }
    }
}