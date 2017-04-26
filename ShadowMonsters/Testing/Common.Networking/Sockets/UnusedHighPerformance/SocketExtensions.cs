using System.Net.Sockets;

namespace Common.Networking.Sockets.UnusedHighPerformance
{
    public static class SocketExtensions
    {
        public static AwaitableSocket ReceiveAsync(this Socket socket, AwaitableSocket awaitable)
        {
            awaitable.Reset();
            if (!socket.ReceiveAsync(awaitable.EventArgs))
                awaitable.WasCompleted = true;

            return awaitable;
        }
        public static AwaitableSocket SendAsync(this Socket socket,AwaitableSocket awaitable)
        {
            awaitable.Reset();
            if (!socket.SendAsync(awaitable.EventArgs))
                awaitable.WasCompleted = true;

            return awaitable;
        }
    }
}