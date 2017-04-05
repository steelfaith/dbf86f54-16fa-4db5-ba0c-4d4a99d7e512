using System.Collections.Generic;
using ExitGames.Logging;
using ShadowMonsters.Framework;
using ShadowMonsters.Photon.Server.Handlers;

namespace ShadowMonsters.Photon.Server
{
    public class PhotonServerHandlerList
    {
        protected readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        private readonly DefaultRequestHandler _defaultRequestHandler;
        private readonly DefaultResponseHandler _defaultResponseHandler;
        private readonly DefaultEventHandler _defaultEventHandler;

        private readonly Dictionary<int, PhotonServerHandler> _requestHandlers = new Dictionary<int, PhotonServerHandler>();
        private readonly Dictionary<int, PhotonServerHandler> _responseHandlers = new Dictionary<int, PhotonServerHandler>();
        private readonly Dictionary<int, PhotonServerHandler> _eventHandlers = new Dictionary<int, PhotonServerHandler>();

        public PhotonServerHandlerList(IEnumerable<IMessageHandler<PhotonServerHandler>> handlers, DefaultRequestHandler defaultRequestHandler, 
            DefaultResponseHandler defaultResponseHandler, DefaultEventHandler defaultEventHandler)
        {
            _defaultRequestHandler = defaultRequestHandler;
            _defaultResponseHandler = defaultResponseHandler;
            _defaultEventHandler = defaultEventHandler;

            foreach (PhotonServerHandler handler in handlers)
            {
                //add logging for failed handler registration
                if ((handler.Type & MessageType.Request) == MessageType.Request)
                    RegisterHandler(handler, _requestHandlers, MessageType.Request);

                if ((handler.Type & MessageType.Response) == MessageType.Response)
                    RegisterHandler(handler, _responseHandlers, MessageType.Response);

                if ((handler.Type & MessageType.Async) == MessageType.Async)
                    RegisterHandler(handler, _eventHandlers, MessageType.Async);

            }
        }

        private void RegisterHandler(PhotonServerHandler handler, Dictionary<int, PhotonServerHandler> handlers, MessageType type)
        {
            if (handler.SubCode != null && !handlers.ContainsKey(handler.SubCode.Value))
            {
                handlers.Add(handler.SubCode.Value, handler);
                return;
            }

            if (!handlers.ContainsKey(handler.OperationCode))
            {
                handlers.Add(handler.OperationCode, handler);
                return;
            }

            Logger.ErrorFormat("Failed to add {0} handler, Code {1}, Name {2} ", type, handler.OperationCode, handler.GetType().Name);
        }

        public void HandleMessage(IMessage message, PhotonServerPeer peer)
        {
            switch (message.Type)
            {
                case MessageType.Request:
                    HandleMessage(message, peer, _requestHandlers, _defaultRequestHandler);
                    break;
                case MessageType.Response:
                    HandleMessage(message, peer, _responseHandlers, _defaultResponseHandler);
                    break;
                case MessageType.Async:
                    HandleMessage(message, peer, _eventHandlers, _defaultEventHandler);
                    break;
            }
        }

        private void HandleMessage(IMessage message, PhotonServerPeer peer, Dictionary<int, PhotonServerHandler> handlers, PhotonServerHandler defaultHandler)
        {
            if (message.SubCode != null && !handlers.ContainsKey(message.SubCode.Value))
            {
                handlers[message.SubCode.Value].HandleMessage(message, peer);
                return;
            }

            if (!message.SubCode.HasValue && handlers.ContainsKey(message.OperationCode))
            {
                handlers[message.OperationCode].HandleMessage(message, peer);
                return;
            }

            defaultHandler.HandleMessage(message, peer);
        }
    }
}