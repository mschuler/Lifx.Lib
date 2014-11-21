namespace Lifx.Lib.Packets
{
    internal class ZeroPayloadCommand : CommandPacketBase
    {
        internal override ushort PayloadSize { get { return 0; } }
    }
}