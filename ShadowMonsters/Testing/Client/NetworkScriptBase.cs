using System;
using System.Collections.Generic;
using Common.Messages;
using UnityEngine;

namespace Client
{
    public class NetworkScriptBase<T> : MonoBehaviour where T : Message
    {
        private NetworkConnector _networkConnector;
        private readonly Queue<T> _responseQueue = new Queue<T>();
        private readonly object _messageLock = new object();

        public void Start(NetworkConnector connector, OperationCode opCode)
        {
            _networkConnector = connector;

            _networkConnector.RegisterHandler(opCode, OnMessageReceived);
        }

        public void OnDestroy()
        {
            //_networkConnector.Unregister(opCode, OnMessageReceived);
        }

        public void OnMessageReceived(RouteableMessage routeableMessage)
        {
            if (routeableMessage.Message.GetType() != typeof(T))
                return;

            lock (_messageLock)
            {
                _responseQueue.Enqueue(routeableMessage.Message as T);
            }
        }

        public bool TryGetResponse(out T response)
        {
            response = default(T);

            lock (_messageLock)
            {
                if (_responseQueue.Count == 0)
                    return false;

                response = _responseQueue.Dequeue();

                return true;
            }
        }

        public void SendMessage(Message message)
        {
            if (message == null)
                return;

            _networkConnector.SendMessage(message);
        }
    }
}