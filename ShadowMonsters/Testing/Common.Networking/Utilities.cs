using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Common;

namespace Common.Networking
{
    public static class Utilities
    {
        public static Message DeserailizeMessage(byte[] data)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream(data);
            return formatter.Deserialize(stream, null) as Message;
        }

        public static byte[] SerailizeMessage(Message message)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            formatter.Serialize(stream, message);

            byte[] data = new byte[stream.Length + Constants.MessageHeaderLength];
            var length = BitConverter.GetBytes((ushort)stream.Length);

            Buffer.BlockCopy(length, 0, data, 0, Constants.MessageHeaderLength);
            Buffer.BlockCopy(stream.ToArray(), 0, data, Constants.MessageHeaderLength, (int)stream.Length);

            return data;
        }
    }
}