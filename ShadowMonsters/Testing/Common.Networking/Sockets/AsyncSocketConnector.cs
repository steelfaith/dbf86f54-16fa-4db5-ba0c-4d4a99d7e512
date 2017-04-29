using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using log4net;

namespace Common.Networking.Sockets
{
    public class AsyncSocketConnector
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(AsyncSocketConnector));
        private static ManualResetEvent _connectDone = new ManualResetEvent(false);

        private readonly IMessageDispatcher _messageDispatcher;

        private IPEndPoint _remoteEp;
        private Socket _client;

        public bool IsConnected { get; private set; }

        public AsyncSocketConnector(IMessageDispatcher messageDispatcher)
        {
            _messageDispatcher = messageDispatcher;
        }

        public void Connect(IPEndPoint remoteEndPoint)
        {
            try
            {
                _remoteEp = remoteEndPoint;

                _client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                _client.BeginConnect(_remoteEp, ConnectCallback, _client);
                _connectDone.WaitOne();
                IsConnected = true;

                Receive(_client);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                Socket client = (Socket) ar.AsyncState;

                client.EndConnect(ar);

                Logger.InfoFormat("Socket connected to {0}", client.RemoteEndPoint);

                _connectDone.Set();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        private void Receive(Socket client)
        {
            try
            {
                TcpConnection tcpConnection = new TcpConnection(client, _messageDispatcher);

                client.BeginReceive(tcpConnection.Buffer, 0, tcpConnection.BufferSize, 0, ReceiveCallback, tcpConnection);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                TcpConnection tcpConnection = (TcpConnection) ar.AsyncState;
                Socket client = tcpConnection.Socket;

                int bytesRead = client.EndReceive(ar);

                if (bytesRead > 0)
                {
                    tcpConnection.AppendData(tcpConnection.Buffer, bytesRead, tcpConnection.Id);
                    Logger.InfoFormat("Read {0} bytes from socket. \n Data : {1}", bytesRead, tcpConnection.ReceivedData?.Length ?? 0);
                    client.BeginReceive(tcpConnection.Buffer, 0, tcpConnection.BufferSize, 0, ReceiveCallback, tcpConnection);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        public void Send(Message message)
        {
            try
            {
                var data = Utilities.SerailizeMessage(message);

                _client.BeginSend(data, 0, data.Length, SocketFlags.None, SendCallback, _client);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                Socket client = (Socket)ar.AsyncState;

                int bytesSent = client.EndSend(ar);
                Logger.InfoFormat("Sent {0} bytes to server.", bytesSent);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }
    }
}