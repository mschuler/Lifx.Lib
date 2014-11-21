namespace Lifx.Lib.Packets
{
    internal class GetPanGateway : CommandPacketBase
    {
        internal GetPanGateway()
        {
            Protocol = DiscoveryProtocol;
        }

        internal override ushort PayloadSize { get { return 0; } }
    }
}