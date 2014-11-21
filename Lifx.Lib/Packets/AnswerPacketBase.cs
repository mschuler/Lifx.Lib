using Lifx.Lib.Enums;

namespace Lifx.Lib.Packets
{
    internal abstract class AnswerPacketBase : PacketBase
    {
        public AnswerType Type { get; set; }

        internal virtual void SetPayload(byte[] payload) { }

        internal virtual void Apply(Bulb bulb) { }
    }
}