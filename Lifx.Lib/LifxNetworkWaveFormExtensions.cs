using Lifx.Lib.Enums;
using Lifx.Lib.Packets;

namespace Lifx.Lib
{
    public static class LifxNetworkWaveFormExtensions
    {
        public static void Pulse(this ILifxNetwork network, IBulb bulb, IColor color, double cycleDurationInSeconds, float cycles)
        {
            network.Waveform(bulb, color, cycleDurationInSeconds, cycles, WaveformType.Pulse);
        }

        public static void Sine(this ILifxNetwork network, IBulb bulb, IColor color, double cycleDurationInSeconds, float cycles)
        {
            network.Waveform(bulb, color, cycleDurationInSeconds, cycles, WaveformType.Sine);
        }

        private static void Waveform(this ILifxNetwork network, IBulb bulb, IColor color, double cycleDurationInSeconds, float cycles, WaveformType waveform)
        {
            var command = (SetWaveForm)PacketFactory.GetCommand(CommandType.SetWaveform);
            var c = color.ToHsv();
            var period = (uint)cycleDurationInSeconds * 1000;
            command.Init((ushort)c.Hue, (ushort)(c.Saturation * 100), (ushort)(c.Brightness * 255), (ushort)c.Kelvin, period, cycles, 0, waveform);

            ((LifxNetwork)network).SendCommand(bulb, command);
        }
    }
}