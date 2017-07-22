using System;
using System.Collections.Concurrent;
using System.Threading;
using Common;
using Common.Interfaces;
using Common.Messages;
using NLog;

namespace Server
{
    /// <summary>
    /// design of the dispatcher may change right now its a single thread processing
    /// messsages in a first come first serve order
    /// </summary>
    public class MessageDispatcher : IMessageDispatcher
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly ConcurrentQueue<RouteableMessage> _incomingMessages = new ConcurrentQueue<RouteableMessage>();
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

                    RouteableMessage routeableMessage;
                    if (_incomingMessages.TryDequeue(out routeableMessage))
                    {
                        var handler = _messageHandlerRegistrar.Resolve(routeableMessage.Message.OperationCode);
                        if(handler == null)
                            Logger.Warn("No handler found for operationcode {0}", routeableMessage.Message.OperationCode);
                        else
                            handler.Invoke(routeableMessage);

                        //Logger.Info("Attempting to process a message");
                        //Logger.Info("Message Type {0} Message Op Code {1} Content Length {2}"
                        //    ,message.Header.OperationType, message.Header.OperationCode, message.Header.MessageLength);
                        //Logger.Info("Message Data {0}", Encoding.ASCII.GetString(message.Content));
                    }
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