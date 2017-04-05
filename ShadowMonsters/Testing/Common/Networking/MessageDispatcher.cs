using System;
using System.Collections.Concurrent;
using System.Text;
using System.Threading;
using log4net;

namespace Common.Networking
{
    /// <summary>
    /// design of the dispatcher may change right now its a single thread processing
    /// messsages in a first come first serve order
    /// </summary>
    public class MessageDispatcher
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(MessageDispatcher));
        private static readonly AsyncLogger AsyncLogger = new AsyncLogger(Logger, true);

        private readonly ConcurrentQueue<Message> _messages = new ConcurrentQueue<Message>();

        private readonly AutoResetEvent _messageEvent = new AutoResetEvent(false);

        public MessageDispatcher()
        {
            var processingThread = new Thread(ProcessMessages);
            processingThread.Start();
        }

        private void ProcessMessages()
        {
            while (true)
            {
                try
                {
                    if (_messages.Count == 0)
                        _messageEvent.WaitOne();

                    Message message;
                    if (_messages.TryDequeue(out message))
                    {
                        //somehow we need to route based on opcode here


                        //AsyncLogger.InfoFormat("Attempting to process a message");
                        //AsyncLogger.InfoFormat("Message Type {0} Message Op Code {1} Content Length {2}"
                        //    ,message.Header.OperationType, message.Header.OperationCode, message.Header.ContentLength);
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

        public void DispatchMessasge(Message message)
        {
            try
            {
                _messages.Enqueue(message);
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