using Lifx.Lib.Utils;

namespace Lifx.Lib
{
    public struct MeshInfo
    {
        public float Signal;
        public int Tx;
        public int Rx;
        public short McuTemperature;
        public Timestamp FirmwareBuild;
        public Timestamp FirmwareInstall;
        public uint FirmwareVersion;
    }
}