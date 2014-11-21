using System;

namespace Lifx.Lib.Packets
{
    internal class GetTagLabels : CommandPacketBase
    {
        private ulong _bitmask;

        internal override ushort PayloadSize { get { return 8; } }

        internal void Init(ulong bitmask)
        {
            _bitmask = bitmask;
        }

        internal override void GetPayload(byte[] payload)
        {
            Array.Copy(BitConverter.GetBytes(_bitmask), 0, payload, 0, 8);
        }
    }
}