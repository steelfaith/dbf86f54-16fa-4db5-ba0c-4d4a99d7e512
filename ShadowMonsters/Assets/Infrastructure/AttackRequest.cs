using System;

namespace Assets.Infrastructure
{
    public class AttackRequest
    {
        public Guid InstanceId { get; set; }
        public Guid AttackId { get; set; }
    }
}
