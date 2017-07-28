using System;
using Common.Messages;

namespace Server.Common.Interfaces
{
    public interface IMessageHandlerRegistrar
    {
        void Register(OperationCode operationCode, Action<RouteableMessage> handler);
        Action<RouteableMessage> Resolve(OperationCode operationCode);
        void Register(InstanceRoute instanceRoute, Action<InstanceMessage> handler);
        Action<InstanceMessage> Resolve(InstanceRoute instanceRoute);

    }
}