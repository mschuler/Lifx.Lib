using System.Collections.Generic;

namespace Lifx.Lib.Utils
{
    internal sealed class ByteArrayComparer : IComparer<byte[]>, IEqualityComparer<byte[]>
    {
        public static readonly ByteArrayComparer Instance = new ByteArrayComparer();

        public int Compare(byte[] x, byte[] y)
        {
            if (x.Length != y.Length)
            {
                return x.Length.CompareTo(y.Length);
            }

            for (var i = 0; i < x.Length; i++)
            {
                if (x[i] != y[i])
                {
                    return x[i].CompareTo(y[i]);
                }
            }

            return 0;
        }

        public bool Equals(byte[] x, byte[] y)
        {
            if (x.Length != y.Length)
            {
                return false;
            }

            for (var i = 0; i < x.Length; i++)
            {
                if (x[i] != y[i])
                {
                    return false;
                }
            }

            return true;
        }

        public int GetHashCode(byte[] array)
        {
            if (array == null || array.Length == 0)
            {
                return 0;
            }

            unchecked
            {
                var result = 0;
                foreach (var b in array)
                {
                    result = (result * 31) ^ b;
                }
                return result;
            }
        }
    }
}