using System;
using System.Collections.Generic;
using System.Linq;

namespace Common
{
    public static class Utilities
    {
        public static bool TryGetByteIndex(byte[] data, byte indentifier, out int index)
        {
            index = 0;
            for (int i = 0; i < data.Length; i++)
                if (data[i] == indentifier)
                {
                    index = i;
                    return true;
                }

            return false;
        }

        public static bool TryGetByteIndicies(byte[] data, byte indentifier, out List<int> indicies)
        {
            bool hasAtLeastOneEtb = false;

            indicies = null;

            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] == indentifier)
                {
                    if (indicies == null)
                        indicies = new List<int>();

                    indicies.Add(i);
                    hasAtLeastOneEtb = true;
                }
            }

            return hasAtLeastOneEtb;
        }

        public static byte[] RemoveTo(byte[] source, int index)
        {
            try
            {
                var newLength = source.Length - index;
                byte[] resizedArray = new byte[newLength];

                System.Buffer.BlockCopy(source, index, resizedArray, 0, newLength);

                return resizedArray;
            }
            catch (Exception ex)
            {
                return new byte[0];
            }

        }
    }
}