using Lifx.Lib.Enums;

namespace Lifx.Lib.Packets
{
    internal class ResetSwitchState : AnswerPacketBase
    {
        private ResetSwitchPosition _position;

        internal ResetSwitchState()
        {
            _position = ResetSwitchPosition.Up;
        }

        internal override ushort PayloadSize { get { return 2; } }

        internal override void SetPayload(byte[] payload)
        {
            _position = payload[0] == 0 ? ResetSwitchPosition.Up : ResetSwitchPosition.Down;
        }

        internal override void Apply(Bulb bulb)
        {
            bulb.ResetSwitchPosition = _position;
        }
    }
}