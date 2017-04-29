namespace ShadowMonsters.Framework
{
    public interface IMessageHandlerList<T>
    {
        bool RegisterHandler(IMessageHandler<T> handler);
        bool HandleMessage(IMessage message, T peer);
    }
}