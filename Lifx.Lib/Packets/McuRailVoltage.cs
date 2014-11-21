using System;

namespace Lifx.Lib.Packets
{
    internal class McuRailVoltage : AnswerPacketBase
    {
        private uint _voltage;

        internal override ushort PayloadSize { get { return 4; } }

        internal override void SetPayload(byte[] payload)
        {
            _voltage = BitConverter.ToUInt32(payload, 0);
        }

        internal override void Apply(Bulb bulb)
        {
            bulb.Voltage = _voltage;
        }
    }
}