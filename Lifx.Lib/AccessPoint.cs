using Lifx.Lib.Enums;

namespace Lifx.Lib
{
    internal sealed class AccessPoint : IAccessPoint
    {
        public AccessPoint(Packets.AccessPoint packet)
        {
            Packet = packet;
            Ssid = packet.SsidName;
            IsPasswordRequired = packet.SecurityProtocol != SecurityProtocol.Open;
        }

        public string Ssid { get; private set; }
        public bool IsPasswordRequired { get; private set; }
        public Packets.AccessPoint Packet { get; private set; }
    }
}