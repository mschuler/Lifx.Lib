using Lifx.Lib.Enums;
using Lifx.Lib.Packets;

namespace Lifx.Lib
{
    public static class LifxNetworkBulbExtensions
    {
        public static void ChangeColor(this ILifxNetwork network, IBulb bulb, IColor color, double fadeTimeInSeconds)
        {
            var command = (SetLightColor)PacketFactory.GetCommand(CommandType.SetLightColor);
            var c = color.ToHsv();
            command.Init((ushort)c.Hue, (ushort)(c.Saturation * 100), (ushort)(c.Brightness * 255), (ushort)color.Kelvin, (uint)(fadeTimeInSeconds * 1000));

            ((LifxNetwork)network).SendCommand(bulb, command);

            ((Bulb)bulb).Color = c;
        }

        public static void SwitchOn(this ILifxNetwork network, IBulb bulb)
        {
            SetPowerState(network, bulb, true);
        }

        public static void SwitchOff(this ILifxNetwork network, IBulb bulb)
        {
            SetPowerState(network, bulb, false);
        }

        public static void RenameBulb(this ILifxNetwork network, IBulb bulb, string name)
        {
            var command = (SetBulbLabel)PacketFactory.GetCommand(CommandType.SetBulbLabel);
            command.Init(name);

            ((LifxNetwork)network).SendCommand(bulb, command);

            ((Bulb)bulb).Name = name;
        }

        public static void ReadBulbInfo(this ILifxNetwork network, IBulb bulb)
        {
            ((LifxNetwork)network).SendCommand(bulb, PacketFactory.GetCommand(CommandType.GetInfo));
            ((LifxNetwork)network).SendCommand(bulb, PacketFactory.GetCommand(CommandType.GetMeshFirmware));
            ((LifxNetwork)network).SendCommand(bulb, PacketFactory.GetCommand(CommandType.GetMeshInfo));
            ((LifxNetwork)network).SendCommand(bulb, PacketFactory.GetCommand(CommandType.GetTime));
            ((LifxNetwork)network).SendCommand(bulb, PacketFactory.GetCommand(CommandType.GetVersion));
            ((LifxNetwork)network).SendCommand(bulb, PacketFactory.GetCommand(CommandType.GetWifiFirmwareState));
            ((LifxNetwork)network).SendCommand(bulb, PacketFactory.GetCommand(CommandType.GetWifiInfo));
        }

        private static void SetPowerState(ILifxNetwork network, IBulb bulb, bool isPowerOn)
        {
            var command = (SetPowerState)PacketFactory.GetCommand(CommandType.SetPowerState);
            command.Init(isPowerOn);

            ((LifxNetwork)network).SendCommand(bulb, command);

            ((Bulb)bulb).IsPowerOn = isPowerOn;
        }
    }
}