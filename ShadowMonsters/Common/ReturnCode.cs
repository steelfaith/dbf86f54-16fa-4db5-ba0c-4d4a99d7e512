namespace ShadowMonsters.Common
{
    public enum ReturnCode : byte
    {
        Ok = 0,
        Fatal,
        InvalidOperation,
        InvalidOperationParameter,
        WorldNotFound,
        WorldAlreadyExists,
    }
}