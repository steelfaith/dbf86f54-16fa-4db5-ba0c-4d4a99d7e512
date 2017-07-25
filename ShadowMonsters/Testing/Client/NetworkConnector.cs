using System;
using System.Net;
using System.Net.Sockets;
using Common;
using Common.Interfaces;
using Common.Messages;
using Common.Networking;
using NLog;
using NLog.Config;
using NLog.Targets;
using NLog.Targets.Wrappers;

namespace Client
{
    public class NetworkConnector
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly AsyncSocketConnector _asyncSocketConnector;
        private readonly IMessageHandlerRegistrar _messageHandlerRegistrar;
        private readonly IPAddress _localAddress;
        private readonly ushort _port;

        public NetworkConnector(IPAddress localAddress = null, ushort port = 11000, bool useIpV4 = true)
        {


            #region Nlog setup


            try
            {

                string fileName = "ShadowMonsters";
                AsyncTargetWrapper asyncTargetWrapper = new AsyncTargetWrapper();
                asyncTargetWrapper.Name = "asyncTarget";
                FileTarget fileTarget = new FileTarget();
                fileTarget.Name = "fileTarget";
                fileTarget.NetworkWrites = false;
                fileTarget.KeepFileOpen = false;
                var path = System.Environment.CurrentDirectory;
                fileTarget.FileName = string.Concat(System.IO.Path.Combine(path, fileName), ".${date:format=yyyyMMdd}.active");
                fileTarget.ArchiveFileName = string.Concat(System.IO.Path.Combine(path, fileName), ".${date:format=yyyyMMdd}.{#}");
                fileTarget.ArchiveAboveSize = 5242880; // Updated to 5 MB to allow HACC Light to read fewer files.

                fileTarget.ArchiveNumbering = ArchiveNumberingMode.Sequence;
                fileTarget.MaxArchiveFiles = 9999;
                fileTarget.ConcurrentWrites = true;

                fileTarget.Layout = "${longdate}|${level:uppercase=true}|${logger}|${message}";

                asyncTargetWrapper.WrappedTarget = fileTarget;
                NLog.LogManager.Configuration.AddTarget(asyncTargetWrapper);
                NLog.LogManager.Configuration.LoggingRules.Add(new LoggingRule("*", LogLevel.Info, asyncTargetWrapper));
            }
            catch (Exception)
            {
            }


            #endregion

            _messageHandlerRegistrar = new MessageHandlerRegistrar();
            MessageDispatcher messageDispatcher = new MessageDispatcher(_messageHandlerRegistrar);
            _asyncSocketConnector = new AsyncSocketConnector(messageDispatcher);

            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());

            foreach (var ip in ipHostInfo.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetworkV6)
                {
                    _localAddress = ip;
                    break;
                }
            }

            if (_localAddress == null && ipHostInfo.AddressList.Length > 0)
                _localAddress = ipHostInfo.AddressList[0];

            _port = port;
        }

        public void Connect()
        {
            _asyncSocketConnector.Connect(_localAddress, _port);
        }

        public void RegisterHandler(OperationCode operationCode, Action<RouteableMessage> method)
        {
            _messageHandlerRegistrar.Register(operationCode, method);
        }

        public void SendMessage(Message message)
        {
            if (_asyncSocketConnector.IsConnected)
                _asyncSocketConnector.Send(message);
            else
                Logger.Error("Failed to send message, client is not connected.");


        }
    }
}