using System;

namespace Server.Common
{
    public class StorageProviderException : Exception
    {
        public StorageProviderException() { }

        public StorageProviderException(string message, int? errorNumber = null)
                : base(message)
        {
            ErrorNumber = errorNumber;
        }

        public StorageProviderException(string message, Exception innerException, int? errorNumber = null)
                : base(message, innerException)
        {
            ErrorNumber = errorNumber;
        }

        public StorageProviderException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
                : base(info, context) { }

        public int? ErrorNumber { get; }
    }
}