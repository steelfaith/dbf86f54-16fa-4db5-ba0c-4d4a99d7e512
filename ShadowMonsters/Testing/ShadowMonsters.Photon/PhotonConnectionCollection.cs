using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using ExitGames.Logging;
using Photon.SocketServer;
using PhotonHostRuntimeInterfaces;
using ShadowMonsters.Framework;
using ShadowMonsters.Photon.Client;
using ShadowMonsters.Photon.Server;

namespace ShadowMonsters.Photon
{
    public abstract class PhotonConnectionCollection : IConnectionCollection<PhotonServerPeer, PhotonClientPeer>
    {

        protected readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        public ConcurrentDictionary<Guid, PhotonClientPeer> Clients { get; protected set; }
        public ConcurrentDictionary<Guid, PhotonServerPeer> Servers { get; protected set; }

        public PhotonConnectionCollection()
        {
            Clients = new ConcurrentDictionary<Guid, PhotonClientPeer>();
            Servers = new ConcurrentDictionary<Guid, PhotonServerPeer>();
        }

        public void OnServerConnect(PhotonServerPeer server)
        {
            if(!server.Id.HasValue)
                throw new InvalidOperationException("Server id cannot be null.");

            Guid id = server.Id.Value;

            PhotonServerPeer peer;
            if (Servers.TryGetValue(id, out peer))//server already exists in collection disconnect it
            {
                peer.Disconnect();
                Servers.TryRemove(id, out peer);
                Disconnect(peer);
                Logger.WarnFormat("Removing already existing server connection for id {0}", id);
            }

            Servers.TryAdd(id, server);
            Connect(server);

            ResetServers();
        }

        public void OnServerDisconnect(PhotonServerPeer server)
        {
            if (!server.Id.HasValue)
            {
                Disconnect(server);
                throw new InvalidOperationException("Server id cannot be null.");
            }

            Guid id = server.Id.Value;

            PhotonServerPeer peer;
            if (Servers.TryGetValue(id, out peer))//server exists in collection disconnect it
            {
                Servers.TryRemove(id,out peer);
                peer.Disconnect();
                ResetServers();
                Logger.InfoFormat("Disconnecting server {0}", id);
            }
        }

        public void OnClientConnect(PhotonClientPeer client) //these need to be locked as well
        {
            Clients.TryAdd(client.Id, client);
            ClientConnect(client);
        }

        public void OnClientDisconnect(PhotonClientPeer client)//these need to be locked as well
        {
            PhotonClientPeer peer;
            Clients.TryRemove(client.Id, out peer);
            ClientDisconnect(client);
        }

        public PhotonServerPeer GetServerByType(int serverType)
        {
            return OnGetServerByType(serverType);
        }

        public abstract void ServerDisconnect(PhotonServerPeer server);
        public abstract void ServerConnect(PhotonServerPeer server);
        public abstract void ClientDisconnect(PhotonClientPeer client);
        public abstract void ClientConnect(PhotonClientPeer client);
        public abstract void ResetServers();
        public abstract bool IsServerPeer(InitRequest initRequest);
        public abstract PhotonServerPeer OnGetServerByType(int serverType);
    }
}