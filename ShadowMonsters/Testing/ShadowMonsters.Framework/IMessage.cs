using System.Collections.Generic;

namespace ShadowMonsters.Framework
{
    public interface IMessage
    {
        MessageType Type { get; }
        byte OperationCode { get;  }
        int? SubCode { get;  }
        Dictionary<byte, object> Parameters { get; }
    }
}