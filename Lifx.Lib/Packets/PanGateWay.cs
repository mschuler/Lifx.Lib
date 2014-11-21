using System;
using Lifx.Lib.Enums;

namespace Lifx.Lib.Packets
{
    internal class PanGateWay : AnswerPacketBase
    {
        internal PanGateWay()
        {
            TransportProtocol = TransportProtocol.Tcp;
            Port = 0;
            Protocol = DiscoveryProtocol;
        }

        internal override ushort PayloadSize { get { return 5; } }
        public TransportProtocol TransportProtocol { get; set; }
        public UInt32 Port { get; set; }

        internal override void SetPayload(byte[] payload)
        {
            TransportProtocol = payload[0] == 1 ? TransportProtocol.Udp : TransportProtocol.Tcp;
            Port = BitConverter.ToUInt16(payload, 1);
        }
    }
}