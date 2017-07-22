using System;
using Common;
using Common.Messages;

namespace Server.Common.Interfaces
{
    public interface IWorldRegionInstance : IServerInstance
    {
        void SubscribeToRegion(Character character);
        void Move(RouteableMessage routeableMessage);
    }
}