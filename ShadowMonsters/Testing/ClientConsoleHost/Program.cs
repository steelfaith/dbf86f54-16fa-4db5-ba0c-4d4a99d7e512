using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Client;
using Common;
using Common.Messages.Requests;
using Common.Networking;
using Common.Networking.Sockets;
using log4net;
using log4net.Config;
using Microsoft.Practices.Unity;

namespace ClientConsoleHost
{
    class Program
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Program));
        private static readonly IUnityContainer _container = new UnityContainer();
        private static AsyncSocketConnector _asyncSocketConnector;
        private static IMessageHandlerRegistrar _messageHandlerRegistrar;

        static void Main(string[] args)
        {
            _container.RegisterType<MessageDispatcher>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IMessageHandlerRegistrar, MessageHandlerRegistrar>(new ContainerControlledLifetimeManager());
            _container.RegisterType<AsyncSocketConnector>(new ContainerControlledLifetimeManager());
            _messageHandlerRegistrar = _container.Resolve<IMessageHandlerRegistrar>();


            _asyncSocketConnector = _container.Resolve<AsyncSocketConnector>();

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
                    CreateBattleInstance();

                if (string.CompareOrdinal(consoleResult, "2") == 0)
                    SendRequestToGenerateLaterEvent();

                if (string.CompareOrdinal(consoleResult, "3") == 0)
                    CongestServer();

                if (string.CompareOrdinal(consoleResult, "?") == 0)
                    PrintOptions();
            }
        }

        private static void CreateBattleInstance()
        {
            var connectHandler = new UnityCoroutineSimulator(_asyncSocketConnector, OperationCode.ConnectResponse);
            var instanceHandler = new UnityCoroutineSimulator(_asyncSocketConnector, OperationCode.CreateBattleInstanceResponse);

            _messageHandlerRegistrar.Register(connectHandler);
            _messageHandlerRegistrar.Register(instanceHandler);

            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[1];

            _asyncSocketConnector.Connect(new IPEndPoint(ipAddress, 11000));

            if (_asyncSocketConnector.IsConnected)
            {
                var clientId = 2;
                connectHandler.SendSynchronousMessage(new ConnectRequest(clientId));
                instanceHandler.SendSynchronousMessage(new CreateBattleInstanceRequest { ClientId = clientId });
            }
        }


        private static void SendRequestToGenerateLaterEvent()
        {
            
        }

        private static void CongestServer()//this needs to be converted after DI switchover
        {
            //IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            //IPAddress ipAddress = ipHostInfo.AddressList[1];

            //ConnectRequest request = new ConnectRequest(1);

            //for (int i = 0; i < 2; i++)
            //    Task.Run(() =>
            //    {
            //        AsyncSocketConnector client = new AsyncSocketConnector();
            //        client.Connect(new IPEndPoint(ipAddress, 11000));
            //        while (true)
            //        {
            //            if (client.IsConnected)
            //            {
            //                client.Send(request);
            //                Thread.Sleep(20);
            //            }
            //        }
            //    });
        }

        private static void PrintOptions()
        {
            Console.WriteLine("Press 1 to create a battle instance.");
            Console.WriteLine("Press 2 to send a request that will generate a later event." );
            Console.WriteLine("Press 3 to congest the server.");
            Console.WriteLine(@"Press ? to repeat this list");
            Console.WriteLine(@"Press x to exit.");
        }
    }
}
