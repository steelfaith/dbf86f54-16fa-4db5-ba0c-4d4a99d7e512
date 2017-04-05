using System;

namespace Common.Networking
{
    /// <summary>
    /// Operation Type (int 4 bytes) Request,Response,Event
    /// Operation Code (int 4 bytes) used for routing the final message
    /// Content Length (ushort 2 bytes) 
    /// ETB (char 2 bytes)
    /// Body 
    /// </summary>
    public class MessageHeader
    {
        public const int HeaderLength = 12;
        public int OperationType { get; }
        public int OperationCode { get; }
        public ushort ContentLength { get; }
        public MessageHeader(byte[] data)
        {
            if(data == null)
                throw new ArgumentNullException(nameof(data));

            OperationType = BitConverter.ToInt32(data, 0);
            OperationCode = BitConverter.ToInt32(data, 4);
            ContentLength = BitConverter.ToUInt16(data, 8);
        }
    }
}