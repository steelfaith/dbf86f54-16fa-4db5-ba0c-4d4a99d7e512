using System.Threading;
using Common;
using Common.Interfaces;
using Common.Messages;
using Common.Networking;

namespace Client
{
    /// <summary>
    /// simulate a coroutine waiting for a message, unity throttles the speed at which this occurs
    /// our class will instead use a mutex to manage cpu resources effectively
    /// </summary>
    public class UnityCoroutineSimulator : IMessageHandler
    {
        private readonly AutoResetEvent _messageWait = new AutoResetEvent(false);
        private readonly AsyncSocketConnector _connector;
        public OperationCode OperationCode { get; }

        public UnityCoroutineSimulator(AsyncSocketConnector connector, OperationCode operationCode)
        {
            _connector = connector;
            OperationCode = operationCode;
        }

        public void SendSynchronousMessage(Message message)
        {
            _connector.Send(message);
            _messageWait.WaitOne();
        }

        public void SendAsyncMessage(Message message)
        {
            _connector.Send(message);
        }

        
        public void HandleMessage(RouteableMessage routeableMessage)
        {
            _messageWait.Set();
        }
    }
}