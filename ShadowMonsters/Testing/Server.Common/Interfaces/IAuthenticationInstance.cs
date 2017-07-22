using Common;
using Common.Messages;

namespace Server.Common.Interfaces
{
    public interface IAuthenticationInstance : IServerInstance
    {
        void Login(RouteableMessage routeableMessage);
        void CharacterSelected(RouteableMessage routeableMessage);
    }
}