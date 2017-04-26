namespace Common
{
    public interface IMessageHandler
    {
        OperationCode OperationCode { get; }
        void HandleMessage(RouteableMessage routeableMessage);

    }
}