namespace Common
{
    public interface IMessageDispatcher
    {
        void DispatchMessage(RouteableMessage message);
    }
}