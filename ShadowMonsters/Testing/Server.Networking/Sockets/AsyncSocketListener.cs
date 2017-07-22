using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Common;
using Common.Interfaces;
using Common.Networking;
using Microsoft.Practices.Unity;
using NLog;
using Server.Common.Interfaces;

namespace Server.Networking.Sockets
{
    public class AsyncSocketListener
    {

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly IConnectionManager _connectionManager;
        private readonly IMessageDispatcher _messageDispatcher;
        private readonly IMessageHandlerRegistrar _messageHandlerRegistrar;

        public static ManualResetEvent AcceptResetEvent = new ManualResetEvent(false);

        [InjectionConstructor]
        public AsyncSocketListener(IConnectionManager connectionManager, IMessageDispatcher messageDispatcher, IMessageHandlerRegistrar messageHandlerRegistrar)
        {
            _connectionManager = connectionManager;
            _messageDispatcher = messageDispatcher;
            _messageHandlerRegistrar = messageHandlerRegistrar;
        }

        public void StartListening()
        {
            Socket listener = null;
            IPAddress ipAddress = null;
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());

            foreach (var ip in ipHostInfo.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetworkV6)
                {
                    ipAddress = ip;
                    listener = new Socket(SocketType.Stream, ProtocolType.Tcp) { DualMode = true };
                    break;
                }
            }

            if (ipAddress == null && ipHostInfo.AddressList.Length > 0)
            {
                listener = new Socket(AddressFamily.InterNetwork,SocketType.Stream, ProtocolType.Tcp);
                ipAddress = ipHostInfo.AddressList[0];
            }
                

            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);

            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(100);

                while (true)
                {
                    AcceptResetEvent.Reset();

                    Logger.Info("Accepting incoming connections on address {0}.", ipAddress);
                    listener.BeginAccept(AcceptCallback, listener);

                    AcceptResetEvent.WaitOne();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        public void AcceptCallback(IAsyncResult ar)
        {
            try
            {
                AcceptResetEvent.Set();

                Socket listener = (Socket)ar.AsyncState;
                Socket handler = listener.EndAccept(ar);

                TcpConnection tcpConnection = new TcpConnection(handler, _messageDispatcher);

                _connectionManager.AddConnection(tcpConnection);

                tcpConnection.StartReceiveing();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

        }
    }
}