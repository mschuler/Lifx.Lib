using System;
using Lifx.Lib.Enums;
using Lifx.Lib.Utils;

namespace Lifx.Lib.Packets
{
    internal class AccessPoint : AnswerPacketBase
    {
        internal AccessPoint()
        {
            InterfaceType = Interface.Station;
            Ssid = new byte[32];
            SsidName = string.Empty;
            SecurityProtocol = SecurityProtocol.Open;
            Strength = 0;
            Channel = 0;
        }

        internal override ushort PayloadSize { get { return 38; } }
        public Interface InterfaceType { get; set; }
        public byte[] Ssid { get; set; }
        public string SsidName { get; set; }
        public SecurityProtocol SecurityProtocol { get; set; }
        public UInt16 Strength { get; set; }
        public UInt16 Channel { get; set; }

        internal override void SetPayload(byte[] payload)
        {
            if (payload[0] > 0 && payload[0] < 3)
            {
                InterfaceType = (Interface)payload[0];
            }
            else
            {
                InterfaceType = Interface.SoftAp;
            }

            Array.Copy(payload, 1, Ssid, 0, 32);
            SsidName = Ssid.ToUtf8String();
            SecurityProtocol = (SecurityProtocol)payload[33];
            Strength = BitConverter.ToUInt16(payload, 34);
            Channel = BitConverter.ToUInt16(payload, 36);
        }
    }
}