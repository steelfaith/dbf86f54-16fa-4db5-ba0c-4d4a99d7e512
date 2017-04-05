namespace ShadowMonsters.Framework
{
    public interface IConnectionCollection<Server, Client>
    {
        void OnServerConnect(Server server);
        void OnServerDisconnect(Server server);
        void OnClientConnect(Client client);
        void OnClientDisconnect(Client client);
        Server GetServerByType(int serverType);
    }
}