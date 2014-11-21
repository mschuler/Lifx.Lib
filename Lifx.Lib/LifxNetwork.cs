using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Lifx.Lib.Enums;
using Lifx.Lib.Packets;
using Lifx.Lib.Utils;

namespace Lifx.Lib
{
    internal class LifxNetwork : ILifxNetwork
    {
        private readonly object _bulbCollectionLock = new object();
        private readonly IDictionary<byte[], Bulb> _bulbs = new Dictionary<byte[], Bulb>(ByteArrayComparer.Instance);
        private readonly object _groupCollectionLock = new object();
        private readonly IDictionary<ulong, BulbGroup> _groups = new Dictionary<ulong, BulbGroup>();
        private readonly object _accessPointCollectionLock = new object();
        private readonly IDictionary<string, AccessPoint> _accessPoints = new Dictionary<string, AccessPoint>(StringComparer.Ordinal);
        private Action<IGateway, byte[]> _sender;

        public event EventHandler BulbCollectionChanged;
        public event EventHandler BulbGroupCollectionChanged;

        public IEnumerable<Bulb> Bulbs { get { return _bulbs.Values; } }

        public IEnumerable<BulbGroup> Groups { get { return _groups.Values; } }

        public bool IsBulbSuccessfullyConnectedToWifi { get; internal set; }

        public IEnumerable<IBulb> GetBulbs()
        {
            return new ReadOnlyCollection<IBulb>(_bulbs.Values.Cast<IBulb>().OrderBy(b => b.Name).ToList());
        }

        public IEnumerable<IBulbGroup> GetBulbGroups()
        {
            return new ReadOnlyCollection<IBulbGroup>(_groups.Values.Cast<IBulbGroup>().OrderBy(g => g.Name).ToList());
        }

        public IEnumerable<IAccessPoint> GetAccessPoints()
        {
            return new ReadOnlyCollection<IAccessPoint>(_accessPoints.Values.Cast<IAccessPoint>().OrderBy(a => a.Ssid).ToList());
        }

        public IEnumerable<IGateway> GetGateways()
        {
            return new ReadOnlyCollection<IGateway>(GatewayService.Get().ToList());
        }

        public void Remove(BulbGroup group)
        {
            lock (_groupCollectionLock)
            {
                _groups.Remove(group.Bitmask);
            }
        }

        public BulbGroup CreateGroup()
        {
            for (var i = 0; i < 64; i++)
            {
                var groupBitmask = (ulong)1 << i;

                if (!_groups.ContainsKey(groupBitmask))
                {
                    lock (_groupCollectionLock)
                    {
                        if (!_groups.ContainsKey(groupBitmask))
                        {
                            return CreateGroup(groupBitmask);
                        }
                    }
                }
            }

            return null;
        }

        private BulbGroup CreateGroup(ulong bitmask)
        {
            var group = new BulbGroup(bitmask);
            _groups.Add(group.Bitmask, group);
            OnBulbGroupCollectionChanged();
            return group;
        }

        public void RegisterSender(Action<IGateway, byte[]> sender)
        {
            _sender = sender;
        }

        public Tuple<IGateway, IBulb, AnswerType> ReceivedPacket(string ipAddress, byte[] data)
        {
            var answerType = AnswerType.None;
            IGateway gateway = null;
            IBulb bulb = null;

            try
            {
                var packet = PacketFactory.GetAnswer(data);

                if (packet == null)
                {
                    return Tuple.Create((IGateway)null, (IBulb)null, AnswerType.None);
                }

                answerType = packet.Type;
                gateway = HandleGatewayPacket(packet.GatewayMac, ipAddress);
                bulb = HandleBulbPacket(gateway, packet);
                HandleWifiStatePacket(bulb, packet);
                HandleBulbGroupsPacket(bulb, packet);
                HandleGroupNamePacket(packet);
                HandleAccessPoint(packet);
            }
            catch (Exception e)
            {
                Debug.WriteLine("ReceivedPacket: {0}", e.Message);
                Debug.WriteLine(e.StackTrace);
            }

            return Tuple.Create(gateway, bulb, answerType);
        }

        public void SendCommand(IBulb bulb, CommandPacketBase command)
        {
            command.TargetMacAddress = bulb.Mac;
            SendCommand(bulb.Gateway, command);
        }

        public void SendCommand(IGateway gateway, CommandPacketBase command)
        {
            var sender = _sender;
            if (sender == null)
            {
                return;
            }

            var payload = new byte[command.Size];
            command.GatewayMac = gateway.Mac;
            PacketFactory.PacketToBuffer(command, payload);
            sender(gateway, payload);
        }

        private IGateway HandleGatewayPacket(byte[] mac, string ipAddress)
        {
            var isNewGateway = GatewayService.AddOrUpdate(mac, ipAddress);
            var gateway = GatewayService.Get(mac, ipAddress);
            if (isNewGateway)
            {
                this.ScanGateway(gateway);
            }
            return gateway;
        }

        private Bulb HandleBulbPacket(IGateway gateway, AnswerPacketBase packet)
        {
            Bulb bulb;
            if (!_bulbs.TryGetValue(packet.TargetMacAddress, out bulb))
            {
                lock (_bulbCollectionLock)
                {
                    if (!_bulbs.TryGetValue(packet.TargetMacAddress, out bulb))
                    {
                        bulb = new Bulb(packet.TargetMacAddress, gateway);
                        _bulbs.Add(bulb.Mac, bulb);
                        OnBulbCollectionChanged();
                    }
                }
            }

            packet.Apply(bulb);
            return bulb;
        }

        private void HandleWifiStatePacket(IBulb bulb, AnswerPacketBase packet)
        {
            var wifiState = packet as WifiState;
            if (wifiState == null)
            {
                return;
            }

            if (wifiState.InterfaceType == Interface.Station && wifiState.WifiStatus == WifiStatus.Connected)
            {
                IsBulbSuccessfullyConnectedToWifi = true;
            }
        }

        private void HandleBulbGroupsPacket(IBulb bulb, AnswerPacketBase packet)
        {
            var lightStatus = packet as LightStatus;
            var tags = packet as Tags;

            if (tags != null || lightStatus != null)
            {
                var bitmask = lightStatus != null ? lightStatus.Bitmask : tags.Bitmask;

                var groupBitmasks = bitmask.GetGroupTags().ToList();

                foreach (var groupBitmask in groupBitmasks)
                {
                    BulbGroup group;
                    if (!_groups.TryGetValue(groupBitmask, out group))
                    {
                        lock (_groupCollectionLock)
                        {
                            if (!_groups.TryGetValue(groupBitmask, out group))
                            {
                                group = CreateGroup(groupBitmask);
                            }
                        }
                    }

                    group.Add(bulb);

                    if (string.IsNullOrEmpty(group.Name))
                    {
                        var getTagLabels = (GetTagLabels)PacketFactory.GetCommand(CommandType.GetTagLabels);
                        getTagLabels.Init(groupBitmask);
                        SendCommand(bulb, getTagLabels);
                    }
                }
            }
        }

        private void HandleGroupNamePacket(AnswerPacketBase packet)
        {
            var tagLabels = packet as TagLabels;
            if (tagLabels != null)
            {
                var name = tagLabels.Label.ToUtf8String();
                if (tagLabels.Tags == 0 || string.IsNullOrEmpty(name) || !tagLabels.Tags.IsSingleGroup())
                {
                    return;
                }

                BulbGroup g;
                if (!_groups.TryGetValue(tagLabels.Tags, out g) || string.Equals(g.Name, name, StringComparison.Ordinal))
                {
                    return;
                }

                g.Name = name;
            }
        }

        private void HandleAccessPoint(AnswerPacketBase packet)
        {
            var accessPoint = packet as Packets.AccessPoint;

            if (accessPoint == null)
            {
                return;
            }

            if (!_accessPoints.ContainsKey(accessPoint.SsidName))
            {
                lock (_accessPointCollectionLock)
                {
                    if (!_accessPoints.ContainsKey(accessPoint.SsidName))
                    {
                        _accessPoints.Add(accessPoint.SsidName, new AccessPoint(accessPoint));
                    }
                }
            }
        }

        protected virtual void OnBulbCollectionChanged()
        {
            var handler = BulbCollectionChanged;
            if (handler != null) { handler(this, EventArgs.Empty); }
        }

        protected virtual void OnBulbGroupCollectionChanged()
        {
            var handler = BulbGroupCollectionChanged;
            if (handler != null) { handler(this, EventArgs.Empty); }
        }
    }
}