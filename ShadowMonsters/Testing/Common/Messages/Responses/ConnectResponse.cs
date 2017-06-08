using System;
using System.Collections.Generic;

namespace Common.Messages.Responses
{
    [Serializable]
    public class ConnectResponse : Message
    {
        public override OperationType OperationType => OperationType.Response;
        public override OperationCode OperationCode => OperationCode.ConnectResponse;
        public override int ClientId { get; set; }
        public ServerAnnouncement Announcement { get; set; }
        public List<string> Characters { get; set; }

    }
}