using System;

namespace Lifx.Lib.Packets
{
    internal class MeshInfo : AnswerPacketBase
    {
        private float _signal;
        private int _tx;
        private int _rx;
        private short _mcuTemperature;

        internal override ushort PayloadSize { get { return 14; } }

        internal override void SetPayload(byte[] payload)
        {
            _signal = BitConverter.ToSingle(payload, 0);
            _tx = BitConverter.ToInt32(payload, 4);
            _rx = BitConverter.ToInt32(payload, 8);
            _mcuTemperature = BitConverter.ToInt16(payload, 12);
        }

        internal override void Apply(Bulb bulb)
        {
            bulb.MeshInfo = new Lib.MeshInfo
            {
                Signal = _signal,
                Rx = _rx,
                Tx = _tx,
                McuTemperature = _mcuTemperature,
                FirmwareBuild = bulb.MeshInfo.FirmwareBuild,
                FirmwareInstall = bulb.MeshInfo.FirmwareInstall,
                FirmwareVersion = bulb.MeshInfo.FirmwareVersion,
            };
        }
    }
}