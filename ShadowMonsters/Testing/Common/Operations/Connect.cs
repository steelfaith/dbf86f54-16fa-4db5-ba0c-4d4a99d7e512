using Common.Networking;

namespace Common.Operations
{
    public class Connect : RequestHandler
    {
        public override OperationType Type { get; }
        public override int OperationCode { get; }
        public override Message Handle(Message message)
        {
            //eventually we will need to do something with the character/db lookup here
            //for now lets just skip this and return the client character data as vaild.

            return null;
        }
    }
}