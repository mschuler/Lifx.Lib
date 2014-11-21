using System;

namespace Lifx.Lib.Packets
{
    internal class SetLightColor : CommandPacketBase
    {
        private byte _stream;
        private ushort _hue;
        private ushort _saturation;
        private ushort _brightness;
        private ushort _kelvin;
        private uint _fadeTime;

        internal SetLightColor()
        {
            _stream = 0;
            _hue = 0;
            _saturation = 0;
            _brightness = 0;
            _kelvin = 4500;
            _fadeTime = 0;
        }

        internal override ushort PayloadSize { get { return 13; } }

        public void Init(ushort hue, ushort saturation, ushort brightness, ushort kelvin = 4500, uint fadeTimeInMilliseconds = 0)
        {
            _hue = hue;
            _saturation = saturation;
            _brightness = brightness;
            _kelvin = kelvin;
            _fadeTime = fadeTimeInMilliseconds;
        }

        internal override void GetPayload(byte[] payload)
        {
            payload[0] = _stream;
            Array.Copy(GetBytes(_hue, 360), 0, payload, 1, 2);
            Array.Copy(GetBytes(_saturation, 100), 0, payload, 3, 2);
            Array.Copy(GetBytes(_brightness), 0, payload, 5, 2);
            Array.Copy(BitConverter.GetBytes(_kelvin), 0, payload, 7, 2);
            Array.Copy(BitConverter.GetBytes(_fadeTime), 0, payload, 9, 4);
        }

        private byte[] GetBytes(ushort value, double factor = 255)
        {
            value = (ushort)(value * 255.0 / factor);
            var bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);
            return bytes;
        }
    }
}