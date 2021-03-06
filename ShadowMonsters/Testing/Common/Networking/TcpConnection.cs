﻿using System;
using System.Net.Sockets;
using Common;
using Common.Interfaces;
using Common.Interfaces.Network;
using Common.Messages;
using NLog;

namespace Common.Networking
{
    public class TcpConnection : ITcpConnection
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public Guid Id { get; }
        public Socket Socket { get; }

        public int BufferSize = 1024; //tweak this value based on array expansion?
        public byte[] Buffer;

        private byte[] _receivedData;
        public byte[] ReceivedData => _receivedData;

        private readonly object _dataLock = new object();

        private readonly IMessageDispatcher _dispatcher;

        private ushort? _expectedMessageLength;

        public bool IsConnected { get; set; }

        public TcpConnection(Socket socket, IMessageDispatcher dispatcher)
        {
            if (socket == null)
                throw new ArgumentNullException(nameof(socket));

            if (dispatcher == null)
                throw new ArgumentNullException(nameof(dispatcher));

            _dispatcher = dispatcher;

            Socket = socket;

            Id = Guid.NewGuid();

            Buffer = new byte[BufferSize];
            IsConnected = true;
        }

        public void StartReceiveing()
        {
            Socket.BeginReceive(Buffer, 0, BufferSize, 0, ReadCallback, this);
        }

        private void ReadCallback(IAsyncResult ar)
        {
            try
            {
                TcpConnection tcpConnection = (TcpConnection) ar.AsyncState;
                Socket handler = tcpConnection.Socket;

                int bytesRead = handler.EndReceive(ar);

                if (bytesRead > 0)
                {
                    tcpConnection.AppendData(tcpConnection.Buffer, bytesRead, tcpConnection.Id);
                    handler.BeginReceive(tcpConnection.Buffer, 0, tcpConnection.BufferSize, 0, ReadCallback,
                        tcpConnection);
                }
                else
                {
                    IsConnected = false;
                    Logger.Error("End received returned a 0.");//pretty sure this means the client is dead
                    //look at possible clean up later
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

                Socket.BeginSend(data, 0, data.Length, SocketFlags.None, SendCallback, this);
            }
            catch (Exception ex)
            {
                IsConnected = false;
                Logger.Error(ex);
            }

        }

        private void SendCallback(IAsyncResult ar)
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
                //Logger.Info("Sent {0} bytes to client {1}.", bytesSent, connection.Id);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        /// <summary>
        /// this needs to stay as fast as possible because
        /// any delay here can cause a slow down in read from the socket server
        /// be super careful with any asycn added here because it might effect the order of messages
        /// from the same client
        /// </summary>
        /// <param name="data"></param>
        /// <param name="bytesRead"></param>
        /// <param name="connectionId"></param>
        public void AppendData(byte[] data, int bytesRead, Guid connectionId)
        {
            lock (_dataLock)
            {
                if (_receivedData == null)
                {
                    _receivedData = new byte[bytesRead];
                    System.Buffer.BlockCopy(data, 0, _receivedData, 0, bytesRead);
                }
                else
                {
                    var appendedData = new byte[_receivedData.Length + bytesRead];
                    System.Buffer.BlockCopy(_receivedData, 0, appendedData, 0, _receivedData.Length);
                        //block copy the existing data
                    System.Buffer.BlockCopy(data, 0, appendedData, _receivedData.Length, bytesRead);
                        //block copy the new data
                    _receivedData = appendedData;
                }


                if (_receivedData.Length >= Constants.MessageHeaderLength)
                    _expectedMessageLength = BitConverter.ToUInt16(_receivedData, 0);

                if (_expectedMessageLength.HasValue && _receivedData.Length >= _expectedMessageLength)
                {
                    byte[] fullMessage = new byte[_expectedMessageLength.Value];
                    System.Buffer.BlockCopy(_receivedData, Constants.MessageHeaderLength, fullMessage, 0,
                        _expectedMessageLength.Value);

                    try
                    {
                        var message = Utilities.DeserailizeMessage(fullMessage);

                        if (message != null)
                            _dispatcher.DispatchMessage(new RouteableMessage(connectionId, message));
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ex);
                    }

                    int leftoverData = _receivedData.Length - _expectedMessageLength.Value - Constants.MessageHeaderLength;

                    if (leftoverData > 0)
                    {
                        byte[] remainingData = new byte[leftoverData];
                        System.Buffer.BlockCopy(_receivedData, _expectedMessageLength.Value, remainingData,0, leftoverData);
                        _receivedData = remainingData;
                    }
                    else
                    {
                        _receivedData = null;
                    }

                    _expectedMessageLength = null;
                }
            }
        }
    }
}