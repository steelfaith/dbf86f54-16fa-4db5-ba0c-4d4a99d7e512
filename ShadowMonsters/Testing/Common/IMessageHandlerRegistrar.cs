namespace Common
{
    public interface IMessageHandlerRegistrar
    {
        void Register(IMessageHandler handler);
        IMessageHandler Resolve(OperationCode operationCode);
    }
}