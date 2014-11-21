using System;

namespace Lifx.Lib.Packets
{
    internal class SetTime : CommandPacketBase
    {
        private ulong _time;

        internal override ushort PayloadSize { get { return 8; } }

        internal void Init(ulong time)
        {
            _time = time;
        }

        internal override void GetPayload(byte[] payload)
        {
            Array.Copy(BitConverter.GetBytes(_time), 0, payload, 0, 8);
        }
    }
}