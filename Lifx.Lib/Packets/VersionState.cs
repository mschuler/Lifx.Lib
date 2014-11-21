using System;

namespace Lifx.Lib.Packets
{
    internal class VersionState : AnswerPacketBase
    {
        private uint _bulbVendor;
        private uint _bulbProduct;
        private uint _bulbVersion;

        internal override ushort PayloadSize { get { return 12; } }

        internal override void SetPayload(byte[] payload)
        {
            _bulbVendor = BitConverter.ToUInt32(payload, 0);
            _bulbProduct = BitConverter.ToUInt32(payload, 4);
            _bulbVersion = BitConverter.ToUInt32(payload, 8);
        }

        internal override void Apply(Bulb bulb)
        {
            bulb.Version = new BulbVersion
            {
                Product = _bulbProduct,
                Vendor = _bulbVendor,
                Version = _bulbVersion,
            };
        }
    }
}