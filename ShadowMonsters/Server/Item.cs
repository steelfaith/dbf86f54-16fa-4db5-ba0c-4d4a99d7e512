using Photon.SocketServer.Concurrency;
using ShadowMonsters.Common;
using ShadowMonstersServer.Messages;
using System;
using System.Collections;
using Photon.SocketServer;
using ShadowMonstersServer.Analytics;
using ShadowMonstersServer.Events;
using ShadowMonstersServer.OperationHandlers;

namespace ShadowMonstersServer
{
    public class Item : IDisposable
    {
        public string Id { get; }
        public WorldActorOperationHandler Owner { get; }
        public Vector Rotation { get; set; }
        public Vector Position { get; set; }
        public MessageChannel<ItemEventMessage> EventChannel { get; }
        public MessageChannel<ItemPositionMessage> PositionUpdateChannel { get; }
        public MessageChannel<ItemDisposedMessage> DisposeChannel { get; }
        public Hashtable Properties { get;}
        public int PropertiesRevision { get; set; }
        public byte Type { get; }
        public World World { get; }
        //public IFiber Fiber { get { return Owner.Peer.RequestFiber; } }
        public bool Disposed { get; private set; }

        public Item(Vector position, Vector rotation, Hashtable properties, WorldActorOperationHandler owner, string id, byte type, World world)
        {
            Position = position;
            Rotation = rotation;
            Owner = owner;
            EventChannel = new MessageChannel<ItemEventMessage>(ItemEventMessage.CounterEventSend);
            DisposeChannel = new MessageChannel<ItemDisposedMessage>(MessageCounters.CounterSend);
            PositionUpdateChannel = new MessageChannel<ItemPositionMessage>(MessageCounters.CounterSend);

            Properties = properties ?? new Hashtable();
            if (properties != null)
            {
                PropertiesRevision++;
            }

            Id = id;
            World = world;
            Type = type;
        }

        ~Item()
        {
            Dispose(false);
        }


        public void Destroy()
        {
            OnDestroy();
        }

        public void SetProperties(Hashtable propertiesSet, ArrayList propertiesUnset)
        {
            if (propertiesSet != null)
                foreach (DictionaryEntry entry in propertiesSet)
                    Properties[entry.Key] = entry.Value;

            if (propertiesUnset != null)
                foreach (object key in propertiesUnset)
                    Properties.Remove(key);

            PropertiesRevision++;
        }

        protected internal ItemSnapshot GetItemSnapshot()
        {
            return new ItemSnapshot(this, Position, Rotation, PropertiesRevision);
        }

        protected ItemPositionMessage GetPositionUpdateMessage(Vector position)
        {
            return new ItemPositionMessage(this, position);
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
                DisposeChannel.Publish(new ItemDisposedMessage(this));
                EventChannel.Dispose();
                DisposeChannel.Dispose();
                PositionUpdateChannel.Dispose();
                Disposed = true;
            }
        }

        protected void OnDestroy()
        {
            var eventInstance = new ItemDestroyed { ItemId = Id };
            var eventData = new EventData((byte)EventCode.ItemDestroyed, eventInstance);
            var message = new ItemEventMessage(this, eventData, new SendParameters { ChannelId = Settings.ItemEventChannel });
            EventChannel.Publish(message);
        }

        public void Move(Vector position)
        {
            Position = position;
        }

        public void Spawn(Vector position)
        {
            Position = position;
        }

        public bool GrantWriteAccess(WorldActorOperationHandler actor)
        {
            return Owner == actor;
        }
    }
}
