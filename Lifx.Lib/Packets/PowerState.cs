using System;

namespace Lifx.Lib.Packets
{
    internal class PowerState : AnswerPacketBase
    {
        private bool _isOn;

        internal PowerState()
        {
            _isOn = true;
        }

        internal override ushort PayloadSize { get { return 2; } }

        internal override void SetPayload(byte[] payload)
        {
            var value = BitConverter.ToUInt16(payload, 0);
            _isOn = value != 0;
        }

        internal override void Apply(Bulb bulb)
        {
            bulb.IsPowerOn = _isOn;
        }
    }
}