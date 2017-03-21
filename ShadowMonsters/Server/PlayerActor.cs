using System;
using System.Collections.Generic;
using Photon.SocketServer;

namespace ShadowMonstersServer
{
    public class PlayerActor : IDisposable
    {
        private readonly Dictionary<string, Item> ownedItems = new Dictionary<string, Item>();

        private readonly PeerBase _peer;

        private readonly World _world;

        protected PlayerActor(ShadowPeer peer)
        {
            _peer = peer;
            _world = peer.World;
        }

        ~PlayerActor()
        {
            Dispose(false);
        }

        public Item Avatar { get; set; }

        public PeerBase Peer { get { return _peer; } }

        public World World { get { return _world; } }

        public void AddItem(Item item)
        {
            if (item.Owner != this)
            {
                throw new ArgumentException("foreign owner forbidden");
            }

            ownedItems.Add(item.Id, item);
        }

        public bool RemoveItem(Item item)
        {
            return ownedItems.Remove(item.Id);
        }

        public bool TryGetItem(string itemid, out Item item)
        {
            return ownedItems.TryGetValue(itemid, out item);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {

                foreach (Item item in ownedItems.Values)
                {
                    item.Destroy();
                    item.Dispose();

                    Item removedItem = null;

                    if (_world.ItemCache.TryRemove(item.Id, out removedItem))
                    {
                        removedItem.Dispose();
                    }
                }

                ownedItems.Clear();
            }
        }
    }
}