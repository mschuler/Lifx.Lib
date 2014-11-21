namespace Lifx.Lib.Packets
{
    internal class GetWifiState : CommandPacketBase
    {
        internal override ushort PayloadSize { get { return 1; } }

        internal override void GetPayload(byte[] payload)
        {
            payload[0] = 2;
        }
    }
}