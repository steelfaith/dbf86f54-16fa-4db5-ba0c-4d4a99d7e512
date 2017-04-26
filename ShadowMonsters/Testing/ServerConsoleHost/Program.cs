using System;
using System.IO;
using System.Reflection;
using log4net;
using log4net.Config;
using Server;

namespace ServerConsoleHost
{
    class Program
    {
        static void Main(string[] args)
        {
            var currentPath = Assembly.GetEntryAssembly().Location;
            string directory = Path.GetDirectoryName(currentPath);

            // log4net
            string path = Path.Combine(directory, "log4net.config");
            var file = new FileInfo(path);
            if (file.Exists)
                XmlConfigurator.ConfigureAndWatch(file);

            InstanceController coordinator = new InstanceController();
            coordinator.Start();

            Console.WriteLine("Press enter to exit.");
            Console.ReadLine();

        }
    }
}
