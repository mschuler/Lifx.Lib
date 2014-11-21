using System;
using Lifx.Lib.Utils;

namespace Lifx.Lib
{
    internal sealed class Gateway : IGateway
    {
        public Gateway(byte[] mac, string ipAddress)
        {
            Mac = mac;
            IpAddress = ipAddress;
        }

        public byte[] Mac { get; private set; }
        public string IpAddress { get; private set; }

        public override string ToString()
        {
            return string.Format("{0} ({1})", IpAddress, Mac.ToHexString());
        }

        private bool Equals(Gateway other)
        {
            return
                ByteArrayComparer.Instance.Equals(Mac, other.Mac) &&
                string.Equals(IpAddress, other.IpAddress, StringComparison.Ordinal);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) { return false; }
            if (ReferenceEquals(this, obj)) { return true; }
            return obj is Gateway && Equals((Gateway)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Mac != null ? Mac.GetHashCode() : 0) * 397) ^ (IpAddress != null ? IpAddress.GetHashCode() : 0);
            }
        }
    }
}