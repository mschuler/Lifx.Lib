using System;

namespace Lifx.Lib.Packets
{
    internal class TagLabels : AnswerPacketBase
    {
        internal TagLabels()
        {
            Tags = 0;
            Label = new byte[32];
        }

        internal override ushort PayloadSize { get { return 40; } }
        public UInt64 Tags { get; set; }
        public byte[] Label { get; set; }

        internal override void SetPayload(byte[] payload)
        {
            var tagBytes = new byte[8];
            Array.Copy(payload, tagBytes, 8);
            Tags = BitConverter.ToUInt64(tagBytes, 0);

            Array.Copy(payload, 8, Label, 0, 32);
        }
    }
}