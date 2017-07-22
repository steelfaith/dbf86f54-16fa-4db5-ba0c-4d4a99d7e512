using System;
using Common.Messages;

namespace Common.Interfaces
{
    public interface IMessageHandlerRegistrar
    {
        void Register(OperationCode operationCode, Action<RouteableMessage> handler);
        Action<RouteableMessage> Resolve(OperationCode operationCode);
    }
}