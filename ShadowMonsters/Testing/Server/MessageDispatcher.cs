﻿using System;
using System.Collections.Concurrent;
using System.Threading;
using Common;
using log4net;
using Server.Common.Logging;

namespace Server
{
    /// <summary>
    /// design of the dispatcher may change right now its a single thread processing
    /// messsages in a first come first serve order
    /// </summary>
    public class MessageDispatcher : IMessageDispatcher
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(MessageDispatcher));
        private static readonly AsyncLogger AsyncLogger = new AsyncLogger(Logger);
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
                            AsyncLogger.WarnFormat("No handler found for operationcode {0}", routeableMessage.Message.OperationCode);
                        else
                            handler.Invoke(routeableMessage);

                        //AsyncLogger.InfoFormat("Attempting to process a message");
                        //AsyncLogger.InfoFormat("Message Type {0} Message Op Code {1} Content Length {2}"
                        //    ,message.Header.OperationType, message.Header.OperationCode, message.Header.MessageLength);
                        //AsyncLogger.InfoFormat("Message Data {0}", Encoding.ASCII.GetString(message.Content));
                    }
                }
                catch (Exception ex)
                {
                    AsyncLogger.Error(ex.Message);
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
                AsyncLogger.Error(ex.Message);
                throw;
            }
        }
    }
}