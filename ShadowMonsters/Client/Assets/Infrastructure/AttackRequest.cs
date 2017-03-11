using System;

namespace Assets.Infrastructure
{
    public class AttackRequest
    {
        public Guid AttackId { get; set; }
        public Guid TargetId { get; set; }
    }
}
