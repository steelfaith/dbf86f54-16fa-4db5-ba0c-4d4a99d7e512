using Common.Messages;

namespace Common.Interfaces
{
    public interface IMessageDispatcher
    {
        void DispatchMessage(RouteableMessage message);
    }
}