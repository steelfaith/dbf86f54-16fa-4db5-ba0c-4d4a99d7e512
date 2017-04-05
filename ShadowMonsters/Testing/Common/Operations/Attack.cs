using System.Diagnostics;

namespace Common.Networking
{
    public class Attack : RequestHandler
    {
        public override OperationType Type => OperationType.Request;
        public override int OperationCode => 1; //make this valuable later

        private int _attackId;
        public override Message Handle(Message message)
        {
            // find attack id
            //process with server instances
            return null;
        }
    }
}