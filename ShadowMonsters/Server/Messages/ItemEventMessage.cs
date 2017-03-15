using ExitGames.Diagnostics.Counter;
using Photon.SocketServer;

namespace ShadowMonstersServer.Messages
{
    public class ItemEventMessage
    {
        public static readonly CountsPerSecondCounter CounterEventReceive = new CountsPerSecondCounter("ItemEventMessage.Receive");
        public static readonly CountsPerSecondCounter CounterEventSend = new CountsPerSecondCounter("ItemEventMessage.Send");
        public ItemEventMessage(Item source, IEventData eventData, SendParameters sendParameters)
        {
            Source = source;
            EventData = eventData;
            SendParameters = sendParameters;
        }
        public IEventData EventData { get; private set; }
        public SendParameters SendParameters { get; private set; }
        public Item Source { get; private set; }
    }
}
