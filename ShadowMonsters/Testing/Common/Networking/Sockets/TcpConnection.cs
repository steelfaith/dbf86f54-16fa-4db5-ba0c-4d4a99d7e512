using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using log4net;

namespace Common.Networking.Sockets
{
    public class TcpConnection
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(TcpConnection));
        private static readonly AsyncLogger AsyncLogger = new AsyncLogger(Logger, true);

        public Guid Id { get; } 
        public Socket Socket { get; }

        public int BufferSize = 1024; //tweak this value based on array expansion?
        public byte[] Buffer;

        private byte[] _receivedData;
        public byte[] ReceivedData => _receivedData;

        private readonly object _dataLock = new object();

        private readonly MessageDispatcher _dispatcher;

        private MessageHeader _currentHeader;

        public TcpConnection(Socket socket, MessageDispatcher dispatcher)
        {
            if (socket == null)
                throw new ArgumentNullException(nameof(socket));

            if (dispatcher == null)
                throw new ArgumentNullException(nameof(dispatcher));

            _dispatcher = dispatcher;

            Socket = socket;

            Id = Guid.NewGuid();

            Buffer = new byte[BufferSize];
        }

        /// <summary>
        /// this needs to stay as fast as possible because
        /// any delay here can cause a slow down in read from the socket server
        /// be super careful with any asycn added here because it might effect the order of messages
        /// from the same client
        /// </summary>
        /// <param name="data"></param>
        /// <param name="bytesRead"></param>
        /// <param name="clientGuid"></param>
        public void AppendData(byte[] data, int bytesRead, Guid clientGuid)
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
                    System.Buffer.BlockCopy(_receivedData, 0, appendedData, 0, _receivedData.Length);//block copy the existing data
                    System.Buffer.BlockCopy(data, 0, appendedData, _receivedData.Length, bytesRead); //block copy the new data
                    _receivedData = appendedData;
                }

                int index;
                while (Utilities.TryGetByteIndex(_receivedData, Constants.Etb, out index))
                {
                    if (_currentHeader == null)
                        _currentHeader = new MessageHeader(data);

                    if (_receivedData.Length - MessageHeader.HeaderLength >= _currentHeader.ContentLength)
                    {
                        var content = new byte[_currentHeader.ContentLength];
                        System.Buffer.BlockCopy(_receivedData,MessageHeader.HeaderLength,content,0, _currentHeader.ContentLength);
                       _dispatcher.DispatchMessasge(new Message(_currentHeader, content, clientGuid));

                        int leftoverOffset = MessageHeader.HeaderLength + _currentHeader.ContentLength; //+ 1?
                        int leftoverSize = _receivedData.Length - MessageHeader.HeaderLength - _currentHeader.ContentLength;
                        if (leftoverSize > 0)
                        {
                            
                            var remainingData = new byte[leftoverSize];
                            System.Buffer.BlockCopy(_receivedData, leftoverOffset, remainingData, 0, leftoverSize);
                            _receivedData = remainingData;
                        }
                        else
                        {
                            _receivedData = null;
                            break;// we dont have any more data in the buffer
                        }
                        _currentHeader = null;
                    }
                }
            }
        }

        public void ClearData()
        {
            lock (_dataLock)
            {
                _receivedData = null;
            }
        }

    }
}