using System;
using log4net;

namespace FullTestServer.Sockets
{
    using System.Diagnostics;
    using System.Net.Sockets;
    using System.Runtime;

    class SocketAsyncEventArgsPool : QueuedObjectPool<SocketAsyncEventArgs>
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Program));

        const int SingleBatchSize = 128 * 1024;
        const int MaxBatchCount = 16;
        const int MaxFreeCountFactor = 4;
        readonly int _acceptBufferSize;

        public SocketAsyncEventArgsPool(int acceptBufferSize)
        {
            if (acceptBufferSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(acceptBufferSize));

            _acceptBufferSize = acceptBufferSize;
            int batchCount = (SingleBatchSize + acceptBufferSize - 1) / acceptBufferSize;

            if (batchCount > MaxBatchCount)
                batchCount = MaxBatchCount;

            Initialize(batchCount, batchCount * MaxFreeCountFactor);
        }

        public override bool Return(SocketAsyncEventArgs socketAsyncEventArgs)
        {
            CleanupAcceptSocket(socketAsyncEventArgs);

            if (!base.Return(socketAsyncEventArgs))
            {
                CleanupItem(socketAsyncEventArgs);
                return false;
            }

            return true;
        }

        internal static void CleanupAcceptSocket(SocketAsyncEventArgs socketAsyncEventArgs)
        {
            if(socketAsyncEventArgs == null)
                throw new ArgumentNullException(nameof(socketAsyncEventArgs));

            Socket socket = socketAsyncEventArgs.AcceptSocket;
            if (socket != null)
            {
                socketAsyncEventArgs.AcceptSocket = null;

                try
                {
                    socket.Close(0);
                }
                catch (SocketException ex)
                {
                    Logger.Error(ex);
                }
                catch (ObjectDisposedException ex)
                {
                    Logger.Error(ex);
                }
            }
        }

        protected override void CleanupItem(SocketAsyncEventArgs item)
        {
            item.Dispose();
        }

        protected override SocketAsyncEventArgs Create()
        {
            SocketAsyncEventArgs eventArgs = new SocketAsyncEventArgs();
            byte[] acceptBuffer = new byte[_acceptBufferSize];
            eventArgs.SetBuffer(acceptBuffer, 0, _acceptBufferSize);
            return eventArgs;
        }
    }
}