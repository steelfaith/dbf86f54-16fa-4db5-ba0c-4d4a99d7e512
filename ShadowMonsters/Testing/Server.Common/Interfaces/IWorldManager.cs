using Microsoft.Practices.ObjectBuilder2;

namespace Server.Common.Interfaces
{
    public interface IWorldManager : IBuilderAware
    {
        void Start();
        IWorldRegionInstance GetCharacterRegion(int clientId);
    }
}