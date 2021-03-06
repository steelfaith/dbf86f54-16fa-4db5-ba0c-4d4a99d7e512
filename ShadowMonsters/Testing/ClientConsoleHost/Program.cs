﻿using System;
using System.IO;
using System.Net;
using System.Reflection;
using Client;
using Common;
using Common.Interfaces;
using Common.Messages.Requests;
using Common.Networking;
using log4net;
using log4net.Config;

namespace ClientConsoleHost
{
    class Program
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Program));
        private static AsyncSocketConnector _asyncSocketConnector;
        private static IMessageHandlerRegistrar _messageHandlerRegistrar;

        static void Main(string[] args)
        {

            _messageHandlerRegistrar = new MessageHandlerRegistrar();
            MessageDispatcher messageDispatcher = new MessageDispatcher(_messageHandlerRegistrar);
            _asyncSocketConnector = new AsyncSocketConnector(messageDispatcher);


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
                    LoginWithCharacter();

                if (string.CompareOrdinal(consoleResult, "2") == 0)
                    CreateBattleInstance();

                if (string.CompareOrdinal(consoleResult, "3") == 0)
                        SendRequestToGenerateLaterEvent();

                if (string.CompareOrdinal(consoleResult, "4") == 0)
                    CongestServer();

                if (string.CompareOrdinal(consoleResult, "?") == 0)
                    PrintOptions();
            }
        }

        private static void LoginWithCharacter()
        {
            var connectHandler = new UnityCoroutineSimulator(_asyncSocketConnector, OperationCode.ConnectResponse);
            var characterHandler = new UnityCoroutineSimulator(_asyncSocketConnector, OperationCode.SelectCharacterResponse);

            _messageHandlerRegistrar.Register(connectHandler.OperationCode , connectHandler.HandleMessage);
            _messageHandlerRegistrar.Register(characterHandler.OperationCode, characterHandler.HandleMessage);

            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];

            _asyncSocketConnector.Connect(ipAddress, 11000);

            if (_asyncSocketConnector.IsConnected)
            {
                var clientId = 2;
                connectHandler.SendSynchronousMessage(new ConnectRequest());
                characterHandler.SendSynchronousMessage(new SelectCharacterRequest("TestName"));
            }
        }

        private static void CreateBattleInstance()
        {

            var connectHandler = new UnityCoroutineSimulator(_asyncSocketConnector, OperationCode.ConnectResponse);
            var instanceHandler = new UnityCoroutineSimulator(_asyncSocketConnector, OperationCode.CreateBattleInstanceResponse);

            _messageHandlerRegistrar.Register(connectHandler.OperationCode, connectHandler.HandleMessage);
            _messageHandlerRegistrar.Register(instanceHandler.OperationCode, instanceHandler.HandleMessage);

            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];

            _asyncSocketConnector.Connect(ipAddress, 11000);

            if (_asyncSocketConnector.IsConnected)
            {
                connectHandler.SendSynchronousMessage(new ConnectRequest { ClientId = 2 });
                instanceHandler.SendSynchronousMessage(new CreateBattleInstanceRequest());
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
            Console.WriteLine("Press 1 to login with a character.");
            Console.WriteLine("Press 2 to login and create a battle instance.");
            Console.WriteLine("Press 3 to send a request that will generate a later event." );
            Console.WriteLine("Press 4 to congest the server.");
            Console.WriteLine(@"Press ? to repeat this list");
            Console.WriteLine(@"Press x to exit.");
        }
    }
}
