using System;

namespace Lifx.Lib
{
    public struct WifiInfo
    {
        public Single Signal;
        public int Tx;
        public int Rx;
        public short McuTemperature;
        public byte[] WifiFirmwareBuildTime;
        public byte[] WifiFirmwareInstallTime;
    }
}