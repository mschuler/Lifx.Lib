using System;
using Lifx.Lib.Utils;

namespace Lifx.Lib.Packets
{
    internal class MeshFirmwareState : AnswerPacketBase
    {
        private Timestamp _fwBuild;
        private Timestamp _fwInstall;
        private uint _fwVersion;

        internal MeshFirmwareState()
        {
            _fwBuild = new Timestamp();
            _fwInstall = new Timestamp();
        }

        internal override ushort PayloadSize { get { return 20; } }

        internal override void SetPayload(byte[] payload)
        {
            _fwBuild.Second = payload[0];
            _fwBuild.Minute = payload[1];
            _fwBuild.Hour = payload[2];
            _fwBuild.Day = payload[3];
            _fwBuild.Month = new byte[3];
            _fwBuild.Month[0] = payload[4];
            _fwBuild.Month[1] = payload[5];
            _fwBuild.Month[2] = payload[6];
            _fwBuild.Year = payload[7];
            _fwInstall.Second = payload[8];
            _fwInstall.Minute = payload[9];
            _fwInstall.Hour = payload[10];
            _fwInstall.Day = payload[11];
            _fwInstall.Month = new byte[3];
            _fwInstall.Month[0] = payload[12];
            _fwInstall.Month[1] = payload[13];
            _fwInstall.Month[2] = payload[14];
            _fwInstall.Year = payload[15];
            _fwVersion = BitConverter.ToUInt32(payload, 16);
        }

        internal override void Apply(Bulb bulb)
        {
            bulb.MeshInfo = new Lib.MeshInfo
            {
                FirmwareBuild = _fwBuild,
                FirmwareInstall = _fwInstall,
                FirmwareVersion = _fwVersion,
                Signal = bulb.MeshInfo.Signal,
                Tx = bulb.MeshInfo.Tx,
                Rx = bulb.MeshInfo.Rx,
                McuTemperature = bulb.MeshInfo.McuTemperature,
            };
        }
    }
}