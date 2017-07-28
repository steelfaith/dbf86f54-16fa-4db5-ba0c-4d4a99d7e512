using System;
using Common.Messages;

namespace Server.Common
{
    public class InstanceRoute
    {
        public OperationCode OperationCode { get; }
        public Guid InstanceId { get; }

        public InstanceRoute(Guid instanceId, OperationCode operationCode)
        {
            InstanceId = instanceId;
            OperationCode = operationCode;
        }

        protected bool Equals(InstanceRoute other)
        {
            return OperationCode == other.OperationCode && InstanceId.Equals(other.InstanceId);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((InstanceRoute) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((int) OperationCode*397) ^ InstanceId.GetHashCode();
            }
        }
    }
}