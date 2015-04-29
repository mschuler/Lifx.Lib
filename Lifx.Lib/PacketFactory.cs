using System;
using System.Collections.Generic;
using System.Diagnostics;
using Lifx.Lib.Enums;
using Lifx.Lib.Packets;

namespace Lifx.Lib
{
    internal static class PacketFactory
    {
        private static readonly IDictionary<CommandType, Func<CommandPacketBase>> _commandFactories;
        private static readonly IDictionary<AnswerType, Func<AnswerPacketBase>> _answerFactories;

        static PacketFactory()
        {
            _commandFactories = new Dictionary<CommandType, Func<CommandPacketBase>>();
            _answerFactories = new Dictionary<AnswerType, Func<AnswerPacketBase>>();

            // set
            _commandFactories.Add(CommandType.SetAccessPoint, () => new SetAccessPoint());
            _commandFactories.Add(CommandType.SetBulbLabel, () => new SetBulbLabel());
            _commandFactories.Add(CommandType.SetFactoryTestMode, () => new SetFactoryTestMode());
            _commandFactories.Add(CommandType.SetLightColor, () => new SetLightColor());
            _commandFactories.Add(CommandType.SetPowerState, () => new SetPowerState());
            _commandFactories.Add(CommandType.SetTagLabels, () => new SetTagLabels());
            _commandFactories.Add(CommandType.SetTags, () => new SetTags());
            _commandFactories.Add(CommandType.SetTime, () => new SetTime());
            _commandFactories.Add(CommandType.SetWaveform, () => new SetWaveForm());
            _commandFactories.Add(CommandType.SetWifiState, () => new SetWifiState());

            _commandFactories.Add(CommandType.DisableFactoryTestMode, () => new ZeroPayloadCommand());

            // get
            _commandFactories.Add(CommandType.GetPanGateway, () => new GetPanGateway());
            _commandFactories.Add(CommandType.GetTagLabels, () => new GetTagLabels());
            _commandFactories.Add(CommandType.GetWifiState, () => new GetWifiState());

            _commandFactories.Add(CommandType.Reboot, () => new ZeroPayloadCommand());
            _commandFactories.Add(CommandType.GetAccessPoints, () => new ZeroPayloadCommand());
            _commandFactories.Add(CommandType.GetBulbLabel, () => new ZeroPayloadCommand());
            _commandFactories.Add(CommandType.GetInfo, () => new ZeroPayloadCommand());
            _commandFactories.Add(CommandType.GetLightState, () => new ZeroPayloadCommand());
            _commandFactories.Add(CommandType.GetMcuRailVoltage, () => new ZeroPayloadCommand());
            _commandFactories.Add(CommandType.GetMeshFirmware, () => new ZeroPayloadCommand());
            _commandFactories.Add(CommandType.GetMeshInfo, () => new ZeroPayloadCommand());
            _commandFactories.Add(CommandType.GetResetSwitch, () => new ZeroPayloadCommand());
            _commandFactories.Add(CommandType.GetTags, () => new ZeroPayloadCommand());
            _commandFactories.Add(CommandType.GetTime, () => new ZeroPayloadCommand());
            _commandFactories.Add(CommandType.GetVersion, () => new ZeroPayloadCommand());
            _commandFactories.Add(CommandType.GetWifiFirmwareState, () => new ZeroPayloadCommand());
            _commandFactories.Add(CommandType.GetWifiInfo, () => new ZeroPayloadCommand());

            // answer
            _answerFactories.Add(AnswerType.AccessPoint, () => new Packets.AccessPoint());
            _answerFactories.Add(AnswerType.BulbLabel, () => new BulbLabel());
            _answerFactories.Add(AnswerType.Info, () => new Info());
            _answerFactories.Add(AnswerType.LightStatus, () => new LightStatus());
            _answerFactories.Add(AnswerType.McuRailVoltage, () => new McuRailVoltage());
            _answerFactories.Add(AnswerType.MeshFirmwareState, () => new MeshFirmwareState());
            _answerFactories.Add(AnswerType.MeshInfo, () => new Packets.MeshInfo());
            _answerFactories.Add(AnswerType.PanGateway, () => new PanGateWay());
            _answerFactories.Add(AnswerType.PowerState, () => new PowerState());
            _answerFactories.Add(AnswerType.ResetSwitchState, () => new ResetSwitchState());
            _answerFactories.Add(AnswerType.TagLabels, () => new TagLabels());
            _answerFactories.Add(AnswerType.Tags, () => new Tags());
            _answerFactories.Add(AnswerType.TimeState, () => new TimeState());
            _answerFactories.Add(AnswerType.VersionState, () => new VersionState());
            _answerFactories.Add(AnswerType.WifiFirmwareState, () => new WifiFirmwareState());
            _answerFactories.Add(AnswerType.WifiInfo, () => new Packets.WifiInfo());
            _answerFactories.Add(AnswerType.WifiState, () => new WifiState());
        }

        public static CommandPacketBase GetCommand(CommandType commandType)
        {
            Func<CommandPacketBase> factory;
            if (!_commandFactories.TryGetValue(commandType, out factory))
            {
                // TODO: log!!
                return null;
            }

            var command = factory();
            command.Type = commandType;

            if (command.Protocol == 0)
            {
                command.Protocol = PacketBase.CommandProtocol;
            }

            //if (command.Type == CommandType.SetPowerState ||
            //    command.Type == CommandType.SetLightColor)
            //{
            //    command.Protocol = PacketBase.BulbCommand;
            //}

            return command;
        }

        public static AnswerPacketBase GetAnswer(byte[] packetBuffer)
        {
            try
            {
                var packetType = BitConverter.ToUInt16(packetBuffer, 32);
                var type = (AnswerType)packetType;

                Func<AnswerPacketBase> factory;
                if (!_answerFactories.TryGetValue(type, out factory))
                {
                    // TODO: log!!
                    return null;
                }

                var packet = factory();

                packet.Protocol = BitConverter.ToUInt16(packetBuffer, 2);
                Array.Copy(packetBuffer, 8, packet.TargetMacAddress, 0, 6);
                Array.Copy(packetBuffer, 16, packet.GatewayMac, 0, 6);
                packet.Timestamp = BitConverter.ToUInt16(packetBuffer, 24);
                packet.Type = type;

                var payload = new byte[packet.PayloadSize];
                Array.Copy(packetBuffer, 36, payload, 0, packet.PayloadSize);
                packet.SetPayload(payload);

                return packet;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return null;
            }
        }

        internal static void PacketToBuffer(CommandPacketBase packet, byte[] buffer)
        {
            Array.Copy(BitConverter.GetBytes(packet.Size), 0, buffer, 0, 2);
            Array.Copy(BitConverter.GetBytes(packet.Protocol), 0, buffer, 2, 2);
            Array.Copy(packet.TargetMacAddress, 0, buffer, 8, 6);
            Array.Copy(packet.GatewayMac, 0, buffer, 16, 6);
            Array.Copy(BitConverter.GetBytes(packet.Timestamp), 0, buffer, 24, 8);
            Array.Copy(BitConverter.GetBytes((ushort)packet.Type), 0, buffer, 32, 2);

            var payload = new byte[packet.PayloadSize];
            packet.GetPayload(payload);
            Array.Copy(payload, 0, buffer, 36, payload.Length);
        }
    }
}