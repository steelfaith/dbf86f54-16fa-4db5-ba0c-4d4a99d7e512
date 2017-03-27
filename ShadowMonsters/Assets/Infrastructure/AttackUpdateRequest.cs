using System;

namespace Assets.Infrastructure
{
    public class AttackUpdateRequest
    {
        public Guid CurrentPlayerChampionId { get; set; }
        public Guid AttackInstanceId { get; set; }
    }
}
