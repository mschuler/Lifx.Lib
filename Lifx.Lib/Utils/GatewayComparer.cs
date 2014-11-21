using System.Collections.Generic;

namespace Lifx.Lib.Utils
{
    internal sealed class GatewayComparer : IComparer<Gateway>, IEqualityComparer<Gateway>
    {
        public static readonly GatewayComparer Instance = new GatewayComparer();

        public int Compare(Gateway x, Gateway y)
        {
            return string.Compare(x.IpAddress, y.IpAddress, System.StringComparison.Ordinal);
        }

        public bool Equals(Gateway x, Gateway y)
        {
            return x.Equals(y);
        }

        public int GetHashCode(Gateway obj)
        {
            return obj.GetHashCode();
        }
    }
}