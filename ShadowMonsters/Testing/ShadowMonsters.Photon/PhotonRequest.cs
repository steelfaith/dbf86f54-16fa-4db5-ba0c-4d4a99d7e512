using System.Collections.Generic;
using ShadowMonsters.Framework;

namespace ShadowMonsters.Photon
{
    public class PhotonRequest : IMessage
    {
        public MessageType Type => MessageType.Request;
        public byte OperationCode { get; }
        public int? SubCode { get; }
        public Dictionary<byte, object> Parameters { get; }

        public PhotonRequest(byte operationCode, int? subCode, Dictionary<byte, object> parameters)
        {
            OperationCode = operationCode;
            SubCode = subCode;
            Parameters = parameters;
        }
    }
}