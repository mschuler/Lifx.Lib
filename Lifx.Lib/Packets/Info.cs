using System;

namespace Lifx.Lib.Packets
{
    internal class Info : AnswerPacketBase
    {
        private ulong _time;
        private ulong _uptime;
        private ulong _downtime;

        internal override ushort PayloadSize { get { return 24; } }

        internal override void SetPayload(byte[] payload)
        {
            _time = BitConverter.ToUInt64(payload, 0);
            _uptime = BitConverter.ToUInt64(payload, 8);
            _downtime = BitConverter.ToUInt64(payload, 16);
        }

        internal override void Apply(Bulb bulb)
        {
            bulb.TimeInfo = new BulbTimeInfo
            {
                Time = _time,
                Uptime = _uptime,
                Downtime = _downtime,
            };
        }
    }
}