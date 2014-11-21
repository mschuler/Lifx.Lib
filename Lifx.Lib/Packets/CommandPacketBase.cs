using Lifx.Lib.Enums;

namespace Lifx.Lib.Packets
{
    internal abstract class CommandPacketBase : PacketBase
    {
        public CommandType Type { get; set; }

        internal virtual void GetPayload(byte[] payload) { }
    }
}