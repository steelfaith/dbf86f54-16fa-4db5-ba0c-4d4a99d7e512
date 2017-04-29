using ExitGames.Diagnostics.Counter;

namespace ShadowMonstersServer.Analytics
{
    public static class MessageCounters
    {
        public static readonly CountsPerSecondCounter CounterReceive = new CountsPerSecondCounter("ItemMessage.Receive");

        public static readonly CountsPerSecondCounter CounterSend = new CountsPerSecondCounter("ItemMessage.Send");
    }
}