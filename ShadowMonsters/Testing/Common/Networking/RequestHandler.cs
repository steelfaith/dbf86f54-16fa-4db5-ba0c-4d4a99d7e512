namespace Common.Networking
{
    public abstract class RequestHandler
    {
        public abstract OperationType Type { get; }
        public abstract int OperationCode { get; }
        public abstract Message Handle(Message message);
    }
}