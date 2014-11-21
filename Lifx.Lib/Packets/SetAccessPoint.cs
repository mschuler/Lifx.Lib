using System;
using System.Text;
using Lifx.Lib.Enums;

namespace Lifx.Lib.Packets
{
    internal class SetAccessPoint : CommandPacketBase
    {
        private readonly byte[] _ssid = new byte[32];
        private readonly byte[] _password = new byte[64];
        private byte _securityProtocol;

        internal override ushort PayloadSize { get { return 98; } }

        internal void Init(AccessPoint accessPoint, string password)
        {
            var pw = Encoding.UTF8.GetBytes(password);
            Array.Copy(accessPoint.Ssid, _ssid, accessPoint.Ssid.Length);
            Array.Copy(pw, _password, pw.Length);
            _securityProtocol = (byte)accessPoint.SecurityProtocol;
        }

        internal override void GetPayload(byte[] payload)
        {
            payload[0] = (byte)Interface.Station;
            Array.Copy(_ssid, 0, payload, 1, _ssid.Length);
            Array.Copy(_password, 0, payload, 33, _password.Length);
            payload[97] = _securityProtocol;
        }
    }
}