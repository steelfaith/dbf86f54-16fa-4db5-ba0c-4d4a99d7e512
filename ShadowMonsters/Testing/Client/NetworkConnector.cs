﻿using System;
using System.Net;
using System.Net.Sockets;
using Common;
using Common.Networking;
using Common.Networking.Sockets;
using log4net;

namespace Client
{
    public class NetworkConnector
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(NetworkConnector));
        private readonly AsyncSocketConnector _asyncSocketConnector;
        private readonly IMessageHandlerRegistrar _messageHandlerRegistrar;

        private readonly IPAddress _localAddress;
        private readonly ushort _port;

        public NetworkConnector(IPAddress localAddress = null, ushort port = 11000, bool useIpV4 = true)
        {
            _messageHandlerRegistrar = new MessageHandlerRegistrar();
            var messageDispatcher = new MessageDispatcher(_messageHandlerRegistrar);
            _asyncSocketConnector = new AsyncSocketConnector(messageDispatcher);

            if (localAddress == null)
            {
                IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                if (ipHostInfo == null)
                    throw new InvalidOperationException("No ip addresses detected.");

                foreach (var ip in ipHostInfo.AddressList)
                {
                    if (useIpV4 && ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        _localAddress = ip;
                        break;
                    }
                }
            }
            else
                _localAddress = localAddress;

            _port = port;
        }

        public void Connect()
        {
            _asyncSocketConnector.Connect(new IPEndPoint(_localAddress, _port));
        }

        public void RegisterHandler(IMessageHandler handler)
        {
            _messageHandlerRegistrar.Register(handler);
        }

        public void SendMessage(Message message)
        {
            if (_asyncSocketConnector.IsConnected)
                _asyncSocketConnector.Send(message);
            else
                _logger.ErrorFormat("Failed to send message, client is not connected.");


        }
    }
}