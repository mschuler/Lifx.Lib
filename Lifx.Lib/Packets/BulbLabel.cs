using Lifx.Lib.Utils;

namespace Lifx.Lib.Packets
{
    internal class BulbLabel : AnswerPacketBase
    {
        private string _label;

        internal override ushort PayloadSize { get { return 32; } }

        internal override void SetPayload(byte[] payload)
        {
            _label = payload.ToUtf8String();
        }

        internal override void Apply(Bulb bulb)
        {
            bulb.Name = _label;
        }
    }
}