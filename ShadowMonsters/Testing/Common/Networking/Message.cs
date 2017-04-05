using System;

namespace Common.Networking
{
    public class Message
    {
        public Guid ClientId { get; }
        public MessageHeader Header { get; }
        public byte[] Content { get; }

        public Message(MessageHeader header, byte[] content, Guid clientId)
        {
            if(header == null)
                throw new ArgumentNullException(nameof(header));

            if(content == null)
                throw new ArgumentNullException(nameof(content));

            ClientId = clientId;

            Header = header;
            Content = content;
        }
    }
}