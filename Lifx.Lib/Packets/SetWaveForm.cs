using System;
using Lifx.Lib.Enums;

namespace Lifx.Lib.Packets
{
    internal class SetWaveForm : CommandPacketBase
    {
        private const byte Stream = 0;
        private const byte Transient = 1;
        private ushort _hue;
        private ushort _saturation;
        private ushort _brightness;
        private ushort _kelvin;
        private uint _period;
        private float _cycles;
        private short _dutyCycles;
        private WaveformType _waveform;

        internal override ushort PayloadSize { get { return 21; } }

        public void Init(ushort hue, ushort saturation, ushort brightness, ushort kelvin, uint period, float cycles, short dutyCycles, WaveformType waveform)
        {
            _hue = hue;
            _saturation = saturation;
            _brightness = brightness;
            _kelvin = kelvin;
            _period = period;
            _cycles = cycles;
            _dutyCycles = dutyCycles;
            _waveform = waveform;
        }

        internal override void GetPayload(byte[] payload)
        {
            payload[0] = Stream;
            payload[1] = Transient;
            Array.Copy(GetBytes(_hue, 360), 0, payload, 2, 2);
            Array.Copy(GetBytes(_saturation, 100), 0, payload, 4, 2);
            Array.Copy(GetBytes(_brightness), 0, payload, 6, 2);
            Array.Copy(BitConverter.GetBytes(_kelvin), 0, payload, 8, 2);
            Array.Copy(BitConverter.GetBytes(_period), 0, payload, 10, 4);
            Array.Copy(BitConverter.GetBytes(_cycles), 0, payload, 14, 4);
            Array.Copy(GetBytes(_dutyCycles), 0, payload, 18, 2);
            payload[20] = (byte)_waveform;
        }

        private byte[] GetBytes(ushort value, double factor)
        {
            value = (ushort)(value * 255.0 / factor);
            return GetBytes(value);
        }

        private byte[] GetBytes(ushort value)
        {
            var bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);
            return bytes;
        }

        private byte[] GetBytes(short value)
        {
            var bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);
            return bytes;
        }
    }
}