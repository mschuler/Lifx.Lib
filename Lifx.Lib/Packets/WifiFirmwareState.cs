using System;

namespace Lifx.Lib.Packets
{
    internal class WifiFirmwareState : AnswerPacketBase
    {
        private readonly byte[] _buildTimestamp;
        private readonly byte[] _installTimestamp;
        private uint _version;

        internal WifiFirmwareState()
        {
            _buildTimestamp = new byte[8];
            _installTimestamp = new byte[8];
        }

        internal override ushort PayloadSize { get { return 20; } }

        internal override void SetPayload(byte[] payload)
        {
            Array.Copy(payload, 0, _buildTimestamp, 0, 8);
            Array.Copy(payload, 8, _installTimestamp, 0, 8);
            _version = BitConverter.ToUInt16(payload, 16);
        }

        internal override void Apply(Bulb bulb)
        {
            bulb.WifiInfo = new Lib.WifiInfo
            {
                WifiFirmwareBuildTime = _buildTimestamp,
                WifiFirmwareInstallTime = _installTimestamp,
                Signal = bulb.WifiInfo.Signal,
                Tx = bulb.WifiInfo.Tx,
                Rx = bulb.WifiInfo.Rx,
                McuTemperature = bulb.WifiInfo.McuTemperature,
            };
        }
    }
}