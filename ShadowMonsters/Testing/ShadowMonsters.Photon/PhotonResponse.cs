

using System.Collections.Generic;
using ShadowMonsters.Framework;

namespace ShadowMonsters.Photon
{
    public class PhotonResponse : IMessage
    {
        public MessageType Type => MessageType.Response;
        public byte OperationCode { get; }
        public int? SubCode { get; }
        public Dictionary<byte, object> Parameters { get;}
        public short ReturnCode { get; }
        public string DebugMesage { get; }

        public PhotonResponse(byte operationCode, int? subCode, Dictionary<byte, object> parameters, string debugMessage, short returnCode)
        {
            OperationCode = operationCode;
            SubCode = subCode;
            Parameters = parameters;
            DebugMesage = debugMessage;
            ReturnCode = returnCode;
        }
    }
}