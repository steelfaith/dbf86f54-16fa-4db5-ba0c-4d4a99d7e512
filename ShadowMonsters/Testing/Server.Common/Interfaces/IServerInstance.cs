using System;

namespace Server.Common.Interfaces
{
    public interface IServerInstance 
    {
        Guid InstanceId { get; }
    }
}