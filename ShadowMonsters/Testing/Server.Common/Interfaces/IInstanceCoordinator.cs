using System;
using System.Collections.Generic;

namespace Server.Common.Interfaces
{
    public interface IInstanceCoordinator
    {
        Guid CreateInstance();
        IBattleInstance GetBattleInstance(Guid instanceId);
    }
}