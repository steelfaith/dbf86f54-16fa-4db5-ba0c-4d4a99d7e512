using Photon.SocketServer;
using Photon.SocketServer.Rpc;
using ShadowMonsters.Common;

namespace ShadowMonstersServer.OperationHandlers
{
    public class WorldActorOperationHandler : IOperationHandler
    {
        private readonly ShadowPeer _shadowPeer;

        public WorldActorOperationHandler(ShadowPeer peer)
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

                case OperationCode.StartBattle:
                {
                    return null;
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

        //public OperationResponse OperationCreateWorld(PeerBase peer, OperationRequest request)
        //{
        //    var operation = new CreateWorld(peer.Protocol, request);
        //    if (!operation.IsValid)
        //    {
        //        return new OperationResponse(request.OperationCode) { ReturnCode = (int)ReturnCode.InvalidOperationParameter, DebugMessage = operation.GetErrorMessage() };
        //    }

        //    World world;
        //    MethodReturnValue result = WorldCache.Instance.TryCreate(
        //        operation.WorldName, operation.BoundingBox, operation.TileDimensions, out world)
        //                                   ? MethodReturnValue.Ok
        //                                   : MethodReturnValue.New((int)ReturnCode.WorldAlreadyExists, "WorldAlreadyExists");

        //    return operation.GetOperationResponse(result);
        //}

        public OperationResponse OperationEnterWorld(PeerBase peer, OperationRequest request, SendParameters sendParameters)
        {
            return null;
        }
    }
}