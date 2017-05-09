using System;
using System.Collections.Generic;

namespace Server.Common.Interfaces
{
    public interface IInstanceCoordinator
    {
        Guid CreateBattleInstance();
        IBattleInstance GetBattleInstance(Guid instanceId);
        IWorldRegionInstance CreateWorldRegion();
        IAuthenticationInstance CreateAuthenticationInstance();
    }
}