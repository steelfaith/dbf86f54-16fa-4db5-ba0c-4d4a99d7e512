using Common.Messages;

namespace Common.Interfaces
{
    public interface IMessageHandler
    {
        OperationCode OperationCode { get; }
        void HandleMessage(RouteableMessage routeableMessage);

    }
}