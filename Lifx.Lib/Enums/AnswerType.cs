namespace Lifx.Lib.Enums
{
    // see https://github.com/magicmonkey/lifxjs/blob/master/Protocol.md
    public enum AnswerType : ushort
    {
        None = 0x0,
        PanGateway = 0x03,
        PowerState = 0x16,
        WifiInfo = 0x11,
        WifiFirmwareState = 0x13,
        WifiState = 0x12f,
        AccessPoint = 0x132,
        BulbLabel = 0x19,
        Tags = 0x1c,
        TagLabels = 0x1f,
        LightStatus = 0x6b,
        TimeState = 0x06,
        ResetSwitchState = 0x08,
        MeshInfo = 0x0d,
        MeshFirmwareState = 0x0e,
        VersionState = 0x21,
        Info = 0x23,
        McuRailVoltage = 0x25
    }
}