using System;
using System.Collections.Generic;
using System.Linq;
using Lifx.Lib.Utils;

namespace Lifx.Lib
{
    internal static class GatewayService
    {
        private static readonly object _gatewayCollectionLock = new object();
        private static readonly List<Gateway> _gateways = new List<Gateway>();

        public static IGateway Get(byte[] mac, string ipAddress)
        {
            return _gateways.SingleOrDefault(g =>
                ByteArrayComparer.Instance.Equals(g.Mac, mac) &&
                string.Equals(g.IpAddress, ipAddress, StringComparison.Ordinal));
        }

        public static IEnumerable<IGateway> Get()
        {
            return _gateways.ToList();
        }

        public static bool AddOrUpdate(byte[] mac, string ipAddress)
        {
            var gateway = new Gateway(mac, ipAddress);

            if (_gateways.Contains(gateway))
            {
                return false;
            }

            lock (_gatewayCollectionLock)
            {
                if (_gateways.Contains(gateway))
                {
                    return false;
                }

                _gateways.Add(gateway);
            }

            return true;
        }
    }
}