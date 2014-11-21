namespace Lifx.Lib.Packets
{
    internal class SetFactoryTestMode : CommandPacketBase
    {
        private byte _value;

        internal SetFactoryTestMode()
        {
            _value = 1;
        }

        internal override ushort PayloadSize { get { return 1; } }

        public void Init(byte value)
        {
            _value = value;
        }

        internal override void GetPayload(byte[] payload)
        {
            payload[0] = _value;
        }
    }
}