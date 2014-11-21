using System;

namespace Lifx.Lib.Packets
{
    internal class Tags : AnswerPacketBase
    {
        internal Tags()
        {
            Bitmask = 0;
        }

        internal override ushort PayloadSize { get { return 8; } }
        public UInt64 Bitmask { get; set; }

        internal override void SetPayload(byte[] payload)
        {
            Bitmask = BitConverter.ToUInt64(payload, 0);
        }
    }
}