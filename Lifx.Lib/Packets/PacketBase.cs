
namespace Lifx.Lib.Packets
{
    internal abstract class PacketBase
    {
        //// https://github.com/kayno/arduinolifx/blob/master/lifx.h
        //// const unsigned int LifxProtocol_AllBulbsResponse = 21504; // 0x5400
        //// const unsigned int LifxProtocol_AllBulbsRequest  = 13312; // 0x3400
        //// const unsigned int LifxProtocol_BulbCommand      = 5120;  // 0x1400

        //// https://github.com/mpapi/lazylights/blob/master/lazylights.py
        public const ushort DiscoveryProtocol = 0x5400;
        public const ushort CommandProtocol = 0x3400;
        public const ushort BulbCommand = 0x1400;

        protected PacketBase()
        {
            TargetMacAddress = new byte[6];
            GatewayMac = new byte[6];
        }

        public ushort Size { get { return (ushort)(PayloadSize + 36); } }
        internal ushort Protocol { get; set; }
        public byte[] TargetMacAddress { get; set; }

        public byte[] GatewayMac { get; set; }
        public ulong Timestamp { get; set; }

        internal abstract ushort PayloadSize { get; }
    }
}
