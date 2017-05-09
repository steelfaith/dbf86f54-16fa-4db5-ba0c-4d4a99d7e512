using System;
using System.Linq;

namespace Common
{
    public static class Utilities
    {
        private static Random _randomNumberGen = new Random();

        public static string GetRandomEnumMember<T>()
        {

            var list = Enum.GetNames(typeof(T)).ToList();
            list.Remove("unitychan");  //dumb but effective!
            return list[_randomNumberGen.Next(0, list.Count)];
        }
    }
}
