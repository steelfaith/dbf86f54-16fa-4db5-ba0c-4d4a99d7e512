using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Config;
using TestClient.Sockets;
using System.Threading;

namespace TestClient
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

            PrintOptions();

            string consoleResult = string.Empty;

            while (string.CompareOrdinal(consoleResult, "x") != 0)
            {
                consoleResult = Console.ReadLine();

                if (string.CompareOrdinal(consoleResult, "1") == 0)
                {
                }
                if (string.CompareOrdinal(consoleResult, "2") == 0)
                {
                }
                if (string.CompareOrdinal(consoleResult, "3") == 0)
                {
                }
                if (string.CompareOrdinal(consoleResult, "?") == 0)
                {
                    PrintOptions();
                }
            }
        }

        private static void CongestServer()
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[1];

            List<byte> testData = new List<byte>();
            testData.AddRange(BitConverter.GetBytes(1));
            testData.AddRange(BitConverter.GetBytes(42));
            var content = Encoding.ASCII.GetBytes("Testing");
            testData.AddRange(BitConverter.GetBytes((ushort)content.Length));
            testData.AddRange(BitConverter.GetBytes('\x17'));
            testData.AddRange(content);

            for (int i = 0; i < 2; i++)
                Task.Run(() =>
                {
                    AsyncClient client = new AsyncClient(new IPEndPoint(ipAddress, 11000));
                    client.Connect();
                    while (true)
                    {
                        if (client.IsConnected)
                        {
                            client.Send(testData.ToArray());
                            Thread.Sleep(100);
                        }
                    }
                });
        }

        private static void PrintOptions()
        {
            Console.WriteLine("Press 1 to send a request/reply.");
            Console.WriteLine("Press 2 to send a request that will generate a later event." );
            Console.WriteLine("Press 3 to congest the server.");
            Console.WriteLine(@"Press ? to repeat this list");
            Console.WriteLine(@"Press x to exit.");
        }
    }
}
