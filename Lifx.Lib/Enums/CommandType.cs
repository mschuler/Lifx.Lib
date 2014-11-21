using System;

namespace Lifx.Lib.Enums
{
    // see https://github.com/magicmonkey/lifxjs/blob/master/Protocol.md
    internal enum CommandType
    {
        GetPanGateway = 0x02,
        [Obsolete("Use GetLightState instead.")]
        GetPowerState = 0x14,
        SetPowerState = 0x15,
        GetWifiInfo = 0x10,
        GetWifiFirmwareState = 0x12,
        GetWifiState = 0x12d,
        SetWifiState = 0x12e,
        GetAccessPoints = 0x130,
        SetAccessPoint = 0x131,
        GetBulbLabel = 0x17,
        SetBulbLabel = 0x18,
        GetTags = 0x1a,
        SetTags = 0x1b,
        GetTagLabels = 0x1d,
        SetTagLabels = 0x1e,
        GetLightState = 0x65,
        SetLightColor = 0x66,
        SetWaveform = 0x67,
        [Obsolete("Use SetLightState instead.")]
        SetDimAbs = 0x68,
        [Obsolete("Use SetLightState instead.")]
        SetDimRel = 0x69,
        GetTime = 0x04,
        SetTime = 0x05,
        GetResetSwitch = 0x07,
        GetMeshInfo = 0x0c,
        GetMeshFirmware = 0x0e,
        GetVersion = 0x20,
        GetInfo = 0x22,
        GetMcuRailVoltage = 0x24,
        Reboot = 0x26,
        SetFactoryTestMode = 0x27,
        DisableFactoryTestMode = 0x28,
    }
}