using System;
using Common;

namespace Server.Common.Interfaces
{
    public interface IWorldRegionInstance : IServerInstance
    {
        void SubscribeToRegion(Character character);
        void Move(RouteableMessage routeableMessage);
    }
}