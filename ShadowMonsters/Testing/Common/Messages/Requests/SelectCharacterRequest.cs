using System;

namespace Common.Messages.Requests
{
    [Serializable]
    public class SelectCharacterRequest : Message
    {
        public override OperationType OperationType => OperationType.Request;
        public override OperationCode OperationCode => OperationCode.SelectCharacterRequest;
        public override int ClientId { get; set; }
        public string CharacterName { get; set; }

        public SelectCharacterRequest(int clientId, string characterName)
        {
            ClientId = clientId;
            CharacterName = characterName;
        }
    }
}