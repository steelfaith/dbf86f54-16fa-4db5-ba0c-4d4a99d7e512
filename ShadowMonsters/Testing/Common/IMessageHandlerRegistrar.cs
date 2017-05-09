using System;

namespace Common
{
    public interface IMessageHandlerRegistrar
    {
        void Register(OperationCode operationCode, Action<RouteableMessage> handler);
        Action<RouteableMessage> Resolve(OperationCode operationCode);
    }
}