﻿namespace Common.Networking.Sockets.UnusedHighPerformance
{
    public class CustomBufferManager //: BufferManager
    {
        private readonly byte[] _buffer;

        public CustomBufferManager(int bufferSize)
        {
            _buffer = new byte[bufferSize];
        }
        //public override byte[] TakeBuffer(int bufferSize)
        //{
        //    return null;
        //}

        //public override void ReturnBuffer(byte[] buffer)
        //{
        //}

        //public override void Clear()
        //{
        //}
    }
}