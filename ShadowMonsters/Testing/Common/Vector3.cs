using System;
using System.Runtime.Serialization;

namespace Common
{
    [Serializable]
    public struct Vector3 : IEquatable<Vector3>
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public bool Equals(Vector3 other)
        {
            return X == other.X && Y == other.Y && Z == other.Z;
        }
    }
}