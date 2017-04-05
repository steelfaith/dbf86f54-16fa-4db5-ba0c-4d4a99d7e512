using System.Collections.Generic;
using ShadowMonsters.Framework;

namespace ShadowMonsters.Photon
{
    public class PhotonEvent : IMessage
    {
        public MessageType Type => MessageType.Async;
        public byte OperationCode { get; }
        public int? SubCode { get; }
        public Dictionary<byte, object> Parameters { get; }

        public PhotonEvent(byte operationCode, int? subCode, Dictionary<byte, object> parameters)
        {
            OperationCode = operationCode;
            SubCode = subCode;
            Parameters = parameters;
        }
    }
}