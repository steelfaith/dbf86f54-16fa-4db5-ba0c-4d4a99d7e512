using System;
using Server.Common.Interfaces;

namespace Server
{
    public class BattleInstance : IBattleInstance
    {
        public readonly Guid InstanceRoutingId;
        public BattleInstance()
        {
            InstanceRoutingId = Guid.NewGuid();
        }

        public bool AttemptRun()
        {
            return true;
        }
    }
}