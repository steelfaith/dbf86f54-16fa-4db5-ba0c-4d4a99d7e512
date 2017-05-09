namespace Server.Common.Interfaces
{
    public interface IWorldManager
    {
        void Start();
        IWorldRegionInstance GetCharacterRegion(int clientId);
    }
}