using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using FullTestServer.Sockets;
using log4net;
using log4net.Config;

namespace FullTestServer
{
    class Program
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Program));

        static void Main(string[] args)
        {
            var currentPath = Assembly.GetEntryAssembly().Location;
            string directory = Path.GetDirectoryName(currentPath);

            // log4net
            string path = Path.Combine(directory, "log4net.config");
            var file = new FileInfo(path);
            if (file.Exists)
                XmlConfigurator.ConfigureAndWatch(file);

            AsyncSocketListener listner = new AsyncSocketListener();
            Task.Run(() => listner.StartListening());

            Console.WriteLine("Press enter to exit.");
            Console.ReadLine();

        }
    }
}
