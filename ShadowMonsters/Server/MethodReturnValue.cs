
namespace ShadowMonstersServer
{
    public struct MethodReturnValue
    {
        internal const string DebugMessageOk = "Ok";

        internal const short ErrorCodeOk = 0;

        public static MethodReturnValue Ok { get; } = new MethodReturnValue { Error = ErrorCodeOk, Debug = DebugMessageOk };

        public string Debug { get; set; }

        public short Error { get; set; }

        public bool IsOk => Error == ErrorCodeOk;

        public static MethodReturnValue New(short errorCode, string debug)
        {
            return new MethodReturnValue { Error = errorCode, Debug = debug };
        }
    }
}
