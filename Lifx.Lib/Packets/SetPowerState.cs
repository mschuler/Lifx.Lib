namespace Lifx.Lib.Packets
{
    internal class SetPowerState : CommandPacketBase
    {
        private bool _isOn = true;

        internal override ushort PayloadSize { get { return 2; } }

        public void Init(bool isOn)
        {
            _isOn = isOn;
        }

        internal override void GetPayload(byte[] payload)
        {
            payload[0] = _isOn ? byte.MaxValue : byte.MinValue;
            payload[1] = _isOn ? byte.MaxValue : byte.MinValue;
        }
    }
}