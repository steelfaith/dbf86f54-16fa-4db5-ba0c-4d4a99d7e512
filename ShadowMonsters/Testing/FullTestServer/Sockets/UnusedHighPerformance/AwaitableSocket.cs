using System;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace FullTestServer.Sockets
{
    public class AwaitableSocket : INotifyCompletion
    {
        private static readonly Action Sentinel = () => { };
        internal bool WasCompleted;
        private Action _continuation;
        internal readonly SocketAsyncEventArgs EventArgs;
        public AwaitableSocket(SocketAsyncEventArgs eventArgs)
        {
            if (eventArgs == null) throw new ArgumentNullException(nameof(eventArgs));
            EventArgs = eventArgs;
            eventArgs.Completed += (sender, args) =>
            {
                var prev = _continuation ?? Interlocked.CompareExchange(ref _continuation, Sentinel, null);
                prev?.Invoke();
            };
        }
        internal void Reset()
        {
            WasCompleted = false;
            _continuation = null;
        }
        public AwaitableSocket GetAwaiter() { return this; }
        public bool IsCompleted => WasCompleted;

        public void OnCompleted(Action continuation)
        {
            if (_continuation == Sentinel || Interlocked.CompareExchange(ref _continuation, continuation, null) == Sentinel)
                Task.Run(continuation);

        }
        public void GetResult()
        {
            if (EventArgs.SocketError != SocketError.Success)
                throw new SocketException((int)EventArgs.SocketError);
        }


        //static async Task ReadAsync(Socket s)
        //{
        //    // Reusable SocketAsyncEventArgs and awaitable wrapper 
        //    var args = new SocketAsyncEventArgs();
        //    args.SetBuffer(new byte[0x1000], 0, 0x1000);
        //    var awaitable = new AwaitableSocket(args);
        //    // Do processing, continually receiving from the socket 
        //    while (true)
        //    {
        //        await s.ReceiveAsync(awaitable);
        //        int bytesRead = args.BytesTransferred;
        //        if (bytesRead <= 0) break;
        //        Console.WriteLine(bytesRead);
        //    }
        //}
    }
}