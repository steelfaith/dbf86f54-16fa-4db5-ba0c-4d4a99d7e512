using System;
using System.Collections.Generic;

namespace Common.Messages.Responses
{
    [Serializable]
    public class SelectCharacterResponse : Message
    {
        public override OperationType OperationType => OperationType.Response;
        public override OperationCode OperationCode => OperationCode.SelectCharacterResponse;
        public override int ClientId { get; set; }
        public bool Accepted { get; set; }

        public SelectCharacterResponse(int clientId, bool accepted)
        {
            ClientId = clientId;
            Accepted = accepted;
        }
    }
}