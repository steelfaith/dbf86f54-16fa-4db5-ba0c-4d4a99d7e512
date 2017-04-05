using ExitGames.Logging;
using ShadowMonsters.Framework;

namespace ShadowMonsters.Photon.Client
{
    public abstract class PhotonClientHandler : IMessageHandler<PhotonClientPeer>
    {
        protected ILogger Logger = LogManager.GetCurrentClassLogger();

        protected PhotonApplication Server;
        public abstract MessageType Type { get; }
        public abstract byte OperationCode { get; }
        public abstract int? SubCode { get; }

        public PhotonClientHandler(PhotonApplication application)
        {
            Server = application;
        }

        public bool HandleMessage(IMessage message, PhotonClientPeer peer)
        {
            OnHandleMessage(message, peer);
            return true;
        }

        protected abstract bool OnHandleMessage(IMessage message, PhotonClientPeer peer);
    }
}