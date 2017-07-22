using System;
using System.IO;
using System.Net;
using System.Reflection;
using Microsoft.Practices.Unity;
using NLog;
using NLog.Config;
using Server;
using Server.Common.Interfaces;

namespace ServerConsoleHost
{
    class Program
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();
        static void Main(string[] args)
        {
            var container = UnityRegistrar.GetUnityContainer();
            var manager = container.Resolve<IWorldManager>();
            manager.OnBuiltUp(null);
            manager.Start();

            Console.ReadLine();
        }
    }
}
