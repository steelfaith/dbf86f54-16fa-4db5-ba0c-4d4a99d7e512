using System;
using System.IO;
using ExitGames.Diagnostics.Monitoring;
using ExitGames.Logging;
using ExitGames.Logging.Log4Net;
using log4net.Config;
using Photon.SocketServer;
using Photon.SocketServer.Diagnostics;
using ShadowMonsters.Common;
using Protocol = Photon.SocketServer.Protocol;

namespace ShadowMonstersServer
{
    public class ServerHost : ApplicationBase
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        private readonly World _world;

        public static CounterSamplePublisher CounterPublisher;

        public ServerHost()
        {
            _world = new World("Everywhere", 
                new BoundingBox(
                    new Vector(0,0,0), 
                    new Vector(1000, 1000, 1000)));

            CounterPublisher = new CounterSamplePublisher(1);
        }

        protected override PeerBase CreatePeer(InitRequest initRequest)
        {
            return new ShadowPeer(initRequest, _world);
        }

        protected override void Setup()
        {
            log4net.GlobalContext.Properties["Photon:ApplicationLogPath"] = Path.Combine(ApplicationRootPath, "_logger");
            var configFileInfo = new FileInfo(Path.Combine(BinaryPath, "log4net.config"));
            if (configFileInfo.Exists)
            {
                LogManager.SetLoggerFactory(Log4NetLoggerFactory.Instance);
                XmlConfigurator.ConfigureAndWatch(configFileInfo);
            }

            AppDomain.CurrentDomain.UnhandledException += AppDomain_OnUnhandledException;

            Protocol.TryRegisterCustomType(typeof(Vector), (byte)ShadowMonsters.Common.Protocol.CustomTypeCodes.Vector, ShadowMonsters.Common.Protocol.SerializeVector, ShadowMonsters.Common.Protocol.DeserializeVector);
            Protocol.TryRegisterCustomType(typeof(BoundingBox), (byte)ShadowMonsters.Common.Protocol.CustomTypeCodes.BoundingBox, ShadowMonsters.Common.Protocol.SerializeBoundingBox, ShadowMonsters.Common.Protocol.DeserializeBoundingBox);

            CounterPublisher.AddCounter(new CpuUsageCounterReader(), "Cpu");

            CounterPublisher.Start();

        }

        protected override void TearDown()
        {
        }

        private static void AppDomain_OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Logger.Error(e.ExceptionObject);
        }
    }
}