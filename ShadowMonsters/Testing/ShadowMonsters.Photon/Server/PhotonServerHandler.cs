using System.Web;
using ShadowMonsters.Framework;

namespace ShadowMonsters.Photon.Server
{
    public abstract class PhotonServerHandler : IMessageHandler<PhotonServerPeer>
    {
        public MessageType Type { get; }
        public byte OperationCode { get; }
        public int? SubCode { get; }
        protected PhotonApplication Server;
        

        public PhotonServerHandler(PhotonApplication application)
        {
            Server = application;
        }

        public bool HandleMessage(IMessage message, PhotonServerPeer peer)
        {
            OnHandleMessage(message, peer);
            return true;
        }

        protected abstract bool OnHandleMessage(IMessage message, PhotonServerPeer serverPeer);
    }
}