using System;
using Server.Common.Interfaces;

namespace Server.Instances
{
    public class ChatInstance :  IServerInstance
    {
        public Guid InstanceId { get; }
    }
}