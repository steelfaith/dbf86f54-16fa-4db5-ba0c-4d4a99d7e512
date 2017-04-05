using System.Collections.Generic;
using ExitGames.Logging;
using ShadowMonsters.Framework;
using ShadowMonsters.Photon.Server;

namespace ShadowMonsters.Photon.Client
{
    public class PhotonClientHandlerList
    {
        protected readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        private readonly Dictionary<int, PhotonClientHandler> _requestHandlers = new Dictionary<int, PhotonClientHandler>();

        public PhotonClientHandlerList(IEnumerable<PhotonClientHandler> handlers)
        {
            foreach(var handler in handlers)
                RegisterHandler(handler, _requestHandlers);
        }

        private void RegisterHandler(PhotonClientHandler handler, Dictionary<int, PhotonClientHandler> handlers)
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

            Logger.ErrorFormat("Failed to add handler, Code {1}, Name {2} ", handler.OperationCode, handler.GetType().Name);
        }

        public void HandleMessage(IMessage message, PhotonClientPeer peer)
        {
            if (message.SubCode != null && !_requestHandlers.ContainsKey(message.SubCode.Value))
            {
                _requestHandlers[message.SubCode.Value].HandleMessage(message, peer);
                return;
            }

            if (!message.SubCode.HasValue && _requestHandlers.ContainsKey(message.OperationCode))
            {
                _requestHandlers[message.OperationCode].HandleMessage(message, peer);
                return;
            }
        }
    }
}