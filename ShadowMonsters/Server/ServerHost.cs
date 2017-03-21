using System;
using System.IO;
using ExitGames.Logging;
using ExitGames.Logging.Log4Net;
using log4net.Config;
using Photon.SocketServer;

namespace ShadowMonstersServer
{
    public class ServerHost : ApplicationBase
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        protected override PeerBase CreatePeer(InitRequest initRequest)
        {
            return null;
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