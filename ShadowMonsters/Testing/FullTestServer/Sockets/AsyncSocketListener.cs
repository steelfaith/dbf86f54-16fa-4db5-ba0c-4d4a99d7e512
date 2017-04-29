using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Common;
using Common.Networking;
using Common.Networking.Sockets;
using log4net;

namespace FullTestServer.Sockets
{
    public class AsyncSocketListener
    {
        
        private static readonly ILog Logger = LogManager.GetLogger(typeof(AsyncSocketListener));
        private static readonly AsyncLogger AsyncLogger = new AsyncLogger(Logger, true);

        private static readonly TcpConnectionManager TcpConnectionManager = new TcpConnectionManager();
        private static readonly MessageDispatcher MessageDispatcher = new MessageDispatcher();

        public static ManualResetEvent AllDone = new ManualResetEvent(false);

        public void StartListening()
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[1]; //this is hack for the ipv4 address
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);

            Socket listener = new Socket(AddressFamily.InterNetwork,SocketType.Stream, ProtocolType.Tcp);

            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(100);

                while (true)
                {
                    AllDone.Reset();

                    AsyncLogger.InfoFormat("Accepting incoming connections on address {0}.", ipAddress);
                    listener.BeginAccept(AcceptCallback, listener);

                    AllDone.WaitOne();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        public static void AcceptCallback(IAsyncResult ar)
        {
            try
            {
                AllDone.Set();

                Socket listener = (Socket)ar.AsyncState;
                Socket handler = listener.EndAccept(ar);

                TcpConnection tcpConnection = new TcpConnection(handler, MessageDispatcher);

                TcpConnectionManager.AddConnection(tcpConnection);

                handler.BeginReceive(tcpConnection.Buffer, 0, tcpConnection.BufferSize, 0, ReadCallback, tcpConnection);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

        }

        public static void ReadCallback(IAsyncResult ar)
        {
            try
            {
                TcpConnection tcpConnection = (TcpConnection) ar.AsyncState;
                Socket handler = tcpConnection.Socket;

                int bytesRead = handler.EndReceive(ar);

                if (bytesRead > 0)
                {
                    tcpConnection.AppendData(tcpConnection.Buffer, bytesRead, tcpConnection.Id);
                    //AsyncLogger.InfoFormat("Read {0} bytes for client {1}", bytesRead, tcpConnection.Id);
                    handler.BeginReceive(tcpConnection.Buffer, 0, tcpConnection.BufferSize, 0, ReadCallback, tcpConnection);
                }
                else
                {
                    Logger.ErrorFormat("End received returned a 0.");//pretty sure this means the client is dead
                    //look at possible clean up later
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

        }

        private static void Send(Socket handler, byte[] data)
        {
            try
            {
                handler.BeginSend(data, 0, data.Length, 0, SendCallback, handler);
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
                var connection = ar.AsyncState as TcpConnection;
                if (connection == null)
                {
                    Logger.Error("Null connection state.");
                    return;
                }

                int bytesSent = connection.Socket.EndSend(ar);
                AsyncLogger.InfoFormat("Sent {0} bytes to client {1}.", bytesSent, connection.Id);

                //connection.Socket.Shutdown(SocketShutdown.Both);
                //connection.Socket.Close();

            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }
    }
}