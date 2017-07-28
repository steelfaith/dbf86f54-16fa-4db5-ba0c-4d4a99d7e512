using System;
using Common.Messages;

namespace Client
{
    public interface IMessageHandlerRegistrar
    {
        void Register(OperationCode operationCode, Action<RouteableMessage> handler);
        Action<RouteableMessage> Resolve(OperationCode operationCode);
    }
}