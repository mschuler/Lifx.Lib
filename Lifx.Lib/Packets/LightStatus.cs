using System;
using Lifx.Lib.Utils;

namespace Lifx.Lib.Packets
{
    internal class LightStatus : AnswerPacketBase
    {
        private ushort _hue;
        private ushort _saturation;
        private ushort _brightness;
        private ushort _kelvin;
        private ushort _power;
        private string _name;
        private ulong _bitmask;

        internal override ushort PayloadSize { get { return 52; } }
        public ulong Bitmask { get { return _bitmask; } }

        internal override void SetPayload(byte[] payload)
        {
            _hue = GetUInt16(payload, 0, 360);
            _saturation = GetUInt16(payload, 2, 100);
            _brightness = GetUInt16(payload, 4);

            _kelvin = BitConverter.ToUInt16(payload, 6);
            _power = BitConverter.ToUInt16(payload, 10);
            _name = payload.ToUtf8String(12, 32);
            _bitmask = BitConverter.ToUInt64(payload, 44);
        }

        internal override void Apply(Bulb bulb)
        {
            bulb.Name = _name;
            bulb.IsPowerOn = _power != 0;
            bulb.Color = new HsvColor(_hue, _saturation / 100.0, _brightness / 255.0, _kelvin);
        }

        private ushort GetUInt16(byte[] payload, int position, double factor = 255)
        {
            Array.Reverse(payload, position, 2);
            var value = BitConverter.ToUInt16(payload, position);
            return (ushort)(value * factor / 255);
        }
    }
}