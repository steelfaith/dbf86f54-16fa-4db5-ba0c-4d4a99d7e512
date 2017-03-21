using Photon.SocketServer;
using Photon.SocketServer.Rpc;
using ShadowMonsters.Common;
using ShadowMonstersServer.Operations;

namespace ShadowMonstersServer.OperationHandlers
{
    public class ConnectionOperationHandler : IOperationHandler
    {
        private readonly ShadowPeer _shadowPeer;

        public ConnectionOperationHandler(ShadowPeer peer)
        {
            _shadowPeer = peer;
        }

        public void OnDisconnect(PeerBase peer)
        {
        }

        public OperationResponse OnOperationRequest(PeerBase peer, OperationRequest operationRequest,SendParameters sendParameters)
        {
            switch ((OperationCode) operationRequest.OperationCode)
            {
                case OperationCode.EnterWorld:
                {
                    return OperationEnterWorld(peer, operationRequest,sendParameters);
                }
            }

            return null;
        }
        public static OperationResponse InvalidOperation(OperationRequest request)
        {
            return new OperationResponse(request.OperationCode)
            {
                ReturnCode = (int)ReturnCode.InvalidOperation,
                DebugMessage = "InvalidOperation: " + (OperationCode)request.OperationCode
            };
        }

        public OperationResponse OperationEnterWorld(PeerBase peer, OperationRequest request, SendParameters sendParameters)
        {
            var operation = new EnterWorld(peer.Protocol, request);
            if (!operation.IsValid)
            {
                return new OperationResponse(request.OperationCode) { ReturnCode = (int)ReturnCode.InvalidOperationParameter, DebugMessage = operation.GetErrorMessage() };
            }

            var actor = new WorldActorOperationHandler(_shadowPeer);
            var avatar = new Item(operation.Position, operation.Rotation, operation.Properties, actor, operation.Username, (byte)ItemType.Avatar, _shadowPeer.World);

            Item existingAvatar = null;
            if (_shadowPeer.World.ItemCache.TryGetValue(avatar.Id, out existingAvatar))//remove an already existing avatar
            {
                existingAvatar.Dispose();
            }

            _shadowPeer.World.ItemCache.TryAdd(avatar.Id, avatar);

            actor.AddItem(avatar);
            actor.Avatar = avatar;

            ((Peer)peer).SetCurrentOperationHandler(actor);

            // set return values
            var responseObject = new EnterWorldResponse
            {
                BoundingBox = _shadowPeer.World.BoundingBox,
                WorldName = _shadowPeer.World.Name
            };

            // send response; use item channel to ensure that this event arrives before any move or subscribe events
            var response = new OperationResponse(request.OperationCode, responseObject);
            sendParameters.ChannelId = Settings.ItemEventChannel;
            peer.SendOperationResponse(response, sendParameters);

            avatar.Spawn(operation.Position);

            // response already sent
            return null;
        }

    }
}