namespace ShadowMonsters.Framework
{
    public interface IMessageHandler<T>
    {
        MessageType Type { get; }
        byte OperationCode { get; }
        int? SubCode { get; }
        bool HandleMessage(IMessage message, T peer);
    }
}