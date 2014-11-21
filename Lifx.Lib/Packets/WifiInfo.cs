using System;

namespace Lifx.Lib.Packets
{
    internal class WifiInfo : AnswerPacketBase
    {
        private float _signal;
        private int _tx;
        private int _rx;
        private short _mcuTemperature;

        internal override ushort PayloadSize { get { return 14; } }

        internal override void SetPayload(byte[] payload)
        {
            _signal = BitConverter.ToSingle(payload, 0);
            _tx = BitConverter.ToUInt16(payload, 4);
            _rx = BitConverter.ToUInt16(payload, 8);
            _mcuTemperature = BitConverter.ToInt16(payload, 12);
        }

        internal override void Apply(Bulb bulb)
        {
            bulb.WifiInfo = new Lib.WifiInfo
            {
                Signal = _signal,
                Tx = _tx,
                Rx = _rx,
                McuTemperature = _mcuTemperature,
                WifiFirmwareBuildTime = bulb.WifiInfo.WifiFirmwareBuildTime,
                WifiFirmwareInstallTime = bulb.WifiInfo.WifiFirmwareInstallTime,
            };
        }
    }
}