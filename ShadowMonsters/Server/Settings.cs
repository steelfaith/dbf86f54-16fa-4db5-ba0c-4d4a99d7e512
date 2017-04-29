using Photon.SocketServer;

namespace ShadowMonstersServer
{
    public static class Settings
    {
        static Settings()
        {
            ItemAutoUnsubcribeDelay = 5000;
            ItemEventChannel = 0;
            DiagnosticsEventChannel = 0;
            DiagnosticsEventReliability = Reliability.Reliable;
        }

        /// <summary>
        /// This property determines which enet channel to use when sending event CounterDataEvent to the client.
        /// Default value is #2.
        /// </summary>
        public static byte DiagnosticsEventChannel { get; set; }

        /// <summary>
        /// Determines if event CounterDataEvent is sent reliable or unreliable to the client.
        /// Defaut value is Reliability.Reliable.
        /// </summary>
        public static Reliability DiagnosticsEventReliability { get; set; }

        /// <summary>
        /// Maximum unsubscribe delay of items that leave the outer view threshold.
        /// Default value is 5 seconds.
        /// </summary>
        public static int ItemAutoUnsubcribeDelay { get; set; }

        /// <summary>
        /// The enet channel used for events that are published with the Item.EventChannel.
        /// Default value is 0.
        /// </summary>
        public static byte ItemEventChannel { get; set; }
    }
}