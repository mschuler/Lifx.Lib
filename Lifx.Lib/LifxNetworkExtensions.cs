using Lifx.Lib.Enums;
using Lifx.Lib.Packets;

namespace Lifx.Lib
{
    public static class LifxNetworkExtensions
    {
        /// <summary>
        /// Search for gateways. If a new gateway is found, it will be scanned for new bulbs.
        /// </summary>
        public static void ScanNetwork(this ILifxNetwork network, string ipAddress)
        {
            var gateways = GatewayService.Get();
            foreach (var g in gateways)
            {
                ScanGateway(network, g);
            }

            var command = PacketFactory.GetCommand(CommandType.GetPanGateway);
            var gateway = new Gateway(new byte[6], ipAddress);

            ((LifxNetwork)network).SendCommand(gateway, command);
        }

        /// <summary>
        /// Search for bulbs within the mesh network of the given gateway.
        /// </summary>
        public static void ScanGateway(this ILifxNetwork network, IGateway gateway)
        {
            var command = PacketFactory.GetCommand(CommandType.GetLightState);

            ((LifxNetwork)network).SendCommand(gateway, command);
        }

        /// <summary>
        /// Get all access points of a specific gateway bulb.
        /// </summary>
        public static void ScanAccessPoints(this ILifxNetwork network, string ipAddress)
        {
            var command = PacketFactory.GetCommand(CommandType.GetAccessPoints);
            var gateway = new Gateway(new byte[6], ipAddress);

            ((LifxNetwork)network).SendCommand(gateway, command);
        }

        public static void SetAccessPoint(this ILifxNetwork network, IGateway gateway, IAccessPoint accessPoint, string password)
        {
            var n = (LifxNetwork)network;
            var command = (SetAccessPoint)PacketFactory.GetCommand(CommandType.SetAccessPoint);
            var ap = (AccessPoint)accessPoint;
            command.Init(ap.Packet, password);

            n.SendCommand(gateway, command);
            n.SendCommand(gateway, PacketFactory.GetCommand(CommandType.GetWifiState));
        }
    }
}