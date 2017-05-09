using System;
using Common;

namespace Server.Common.Interfaces
{
    public interface IWorldRegionInstance : IServerInstance
    {
        void SubscribeToRegion(User user);
        void Move(RouteableMessage routeableMessage);
    }
}