using System;
using Lifx.Lib.Enums;

namespace Lifx.Lib.Packets
{
    internal class SetWifiState : CommandPacketBase
    {
        internal SetWifiState()
        {
            InterfaceType = 0;
            WifiStatus = 0;
            Ip4Address = new byte[4];
            Ip6Address = new byte[16];
        }

        internal override ushort PayloadSize { get { return 22; } }
        public Interface InterfaceType { get; set; }
        public WifiStatus WifiStatus { get; set; }
        public byte[] Ip4Address { get; set; }
        public byte[] Ip6Address { get; set; }

        internal override void GetPayload(byte[] payload)
        {
            payload[0] = (byte)InterfaceType;
            payload[1] = (byte)WifiStatus;
            Array.Copy(Ip4Address, 0, payload, 2, 4);
            Array.Copy(Ip6Address, 0, payload, 8, 8);
        }
    }
}