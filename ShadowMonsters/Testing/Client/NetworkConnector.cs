using System;
using System.Net;
using System.Net.Sockets;
using Common;
using Common.Interfaces;
using Common.Messages;
using Common.Networking;

namespace Client
{
    public class NetworkConnector
    {
        //private readonly ILog _logger = LogManager.GetLogger(typeof(NetworkConnector));
        private readonly AsyncSocketConnector _asyncSocketConnector;
        private readonly IMessageHandlerRegistrar _messageHandlerRegistrar;
        private readonly IPAddress _localAddress;
        private readonly ushort _port;

        public NetworkConnector(IPAddress localAddress = null, ushort port = 11000, bool useIpV4 = true)
        {
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
            //else
            //    _logger.ErrorFormat("Failed to send message, client is not connected.");


        }
    }
}