using System;
using System.Text;

namespace Lifx.Lib.Packets
{
    internal class SetBulbLabel : CommandPacketBase
    {
        private readonly byte[] _label;

        internal SetBulbLabel()
        {
            _label = new byte[32];
        }

        internal override ushort PayloadSize { get { return 32; } }

        public void Init(string label)
        {
            var data = Encoding.UTF8.GetBytes(label);
            Array.Copy(data, _label, label.Length);
        }

        internal override void GetPayload(byte[] payload)
        {
            Array.Copy(_label, 0, payload, 0, _label.Length);
        }
    }
}