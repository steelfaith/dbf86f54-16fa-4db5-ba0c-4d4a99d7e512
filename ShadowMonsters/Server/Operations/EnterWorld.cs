using System.Collections;
using Photon.SocketServer;
using Photon.SocketServer.Rpc;
using ShadowMonsters.Common;

namespace ShadowMonstersServer.Operations
{
    public class EnterWorld : Operation
    {
        public EnterWorld(IRpcProtocol protocol, OperationRequest request): base(protocol, request){}

        [DataMember(Code = (byte)ParameterCode.Position)]
        public Vector Position { get; set; }

        [DataMember(Code = (byte)ParameterCode.Properties, IsOptional = true)]
        public Hashtable Properties { get; set; }

        [DataMember(Code = (byte)ParameterCode.Rotation, IsOptional = true)]
        public Vector Rotation { get; set; }

        [DataMember(Code = (byte)ParameterCode.Username)]
        public string Username { get; set; }

        public OperationResponse GetOperationResponse(short errorCode, string debugMessage)
        {
            var responseObject = new EnterWorldResponse { BoundingBox = new BoundingBox()};//figure out how to return the world bounding box
            return new OperationResponse(OperationRequest.OperationCode, responseObject) { ReturnCode = errorCode, DebugMessage = debugMessage };
        }

        public OperationResponse GetOperationResponse(MethodReturnValue returnValue)
        {
            return GetOperationResponse(returnValue.Error, returnValue.Debug);
        }
    }

    public class EnterWorldResponse
    {
        [DataMember(Code = (byte)ParameterCode.BoundingBox, IsOptional = true)]
        public BoundingBox BoundingBox { get; set; }

        [DataMember(Code = (byte)ParameterCode.BoundingBox, IsOptional = true)]
        public string WorldName { get; set; }
    }
}