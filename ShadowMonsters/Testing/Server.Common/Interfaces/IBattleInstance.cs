using Common;

namespace Server.Common.Interfaces
{
    public interface IBattleInstance : IServerInstance
    {
        void AttemptRun(RouteableMessage routeableMessages);
    }
}