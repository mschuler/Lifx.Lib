using System;

namespace Lifx.Lib.Packets
{
    internal class TimeState : AnswerPacketBase
    {
        private ulong _time;

        internal override ushort PayloadSize { get { return 8; } }

        internal override void SetPayload(byte[] payload)
        {
            _time = BitConverter.ToUInt64(payload, 0);
        }

        internal override void Apply(Bulb bulb)
        {
            bulb.TimeInfo = new BulbTimeInfo
            {
                Time = _time,
                Uptime = bulb.TimeInfo.Uptime,
                Downtime = bulb.TimeInfo.Downtime,
            };
        }
    }
}