using System;
using Lifx.Lib.Enums;

namespace Lifx.Lib.Packets
{
    internal class WifiState : AnswerPacketBase
    {
        internal WifiState()
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

        internal override void SetPayload(byte[] payload)
        {
            byte value1 = payload[0];
            if (value1 == 1)
            {
                InterfaceType = Interface.SoftAp;
            }
            else
            {
                InterfaceType = Interface.Station;
            }
            value1 = payload[1];
            switch (value1)
            {
                case 1:
                    WifiStatus = WifiStatus.Connected;
                    break;
                case 2:
                    WifiStatus = WifiStatus.Connecting;
                    break;
                case 3:
                    WifiStatus = WifiStatus.Failed;
                    break;
                default:
                    WifiStatus = WifiStatus.Off;
                    break;

            }
            Array.Copy(payload, 2, Ip4Address, 0, 4);
            Array.Copy(payload, 6, Ip6Address, 0, 16);
        }
    }
}