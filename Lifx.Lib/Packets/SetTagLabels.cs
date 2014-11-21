using System;

namespace Lifx.Lib.Packets
{
    internal class SetTagLabels : CommandPacketBase
    {
        private readonly byte[] _label;
        private ulong _bitmask;

        internal SetTagLabels()
        {
            _label = new byte[32];
        }

        internal override ushort PayloadSize { get { return 40; } }

        public void Init(ulong bitmask, byte[] label)
        {
            _bitmask = bitmask;
            Array.Copy(label, _label, label.Length);
        }

        internal override void GetPayload(byte[] payload)
        {
            Array.Copy(BitConverter.GetBytes(_bitmask), 0, payload, 0, 8);
            Array.Copy(_label, 0, payload, 8, _label.Length);
        }
    }
}