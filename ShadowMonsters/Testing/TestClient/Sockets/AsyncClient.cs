using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Common;
using Common.Networking;
using Common.Networking.Sockets;
using log4net;

namespace TestClient.Sockets
{
    public class AsyncClient
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(AsyncClient));
        private static ManualResetEvent connectDone = new ManualResetEvent(false);
        private static readonly MessageDispatcher MessageDispatcher = new MessageDispatcher();

        private static String response = String.Empty;

        private readonly IPEndPoint _remoteEp;
        private Socket _client;

        public bool IsConnected { get; private set; }

        public AsyncClient(IPEndPoint remotEndPoint)
        {
            _remoteEp = remotEndPoint;
        }

        public void Connect()
        {
            try
            {
                _client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                _client.BeginConnect(_remoteEp, ConnectCallback, _client);
                connectDone.WaitOne();
                IsConnected = true;

                Receive(_client);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        private static void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                Socket client = (Socket) ar.AsyncState;

                client.EndConnect(ar);

                Logger.InfoFormat("Socket connected to {0}", client.RemoteEndPoint);

                connectDone.Set();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        private static void Receive(Socket client)
        {
            try
            {
                TcpConnection tcpConnection = new TcpConnection(client, MessageDispatcher);

                client.BeginReceive(tcpConnection.Buffer, 0, tcpConnection.BufferSize, 0, ReceiveCallback, tcpConnection);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        private static void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                TcpConnection tcpConnection = (TcpConnection) ar.AsyncState;
                Socket client = tcpConnection.Socket;

                int bytesRead = client.EndReceive(ar);

                if (bytesRead > 0)
                {
                    tcpConnection.AppendData(tcpConnection.Buffer, bytesRead, tcpConnection.Id);
                    Logger.InfoFormat("Read {0} bytes from socket. \n Data : {1}", bytesRead,
                        tcpConnection.ReceivedData?.Length ?? 0);

                    int index;
                    if (Utilities.TryGetByteIndex(tcpConnection.Buffer, Constants.Etx , out index))
                    {
                        Logger.InfoFormat("Read ETX at position {0}", index);
                        //TODO: send this data someplace
                        tcpConnection.ClearData();
                    }
                    else
                    {
                        client.BeginReceive(tcpConnection.Buffer, 0, tcpConnection.BufferSize, 0, ReceiveCallback, tcpConnection);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        public void Send(byte[] data)
        {
            try
            {
                _client.BeginSend(data, 0, data.Length, 0, SendCallback, _client);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

        }

        private static void SendCallback(IAsyncResult ar)
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