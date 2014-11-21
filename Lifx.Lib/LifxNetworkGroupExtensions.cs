using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lifx.Lib.Enums;
using Lifx.Lib.Packets;

namespace Lifx.Lib
{
    public static class LifxNetworkGroupExtensions
    {
        public static void AddBulbToGroup(this ILifxNetwork network, IBulbGroup bulbGroup, IBulb bulb)
        {
            var n = (LifxNetwork) network;
            var group = (BulbGroup)bulbGroup;

            if (group.Contains(bulb))
            {
                return;
            }

            group.Add(bulb);

            var groups = n.Groups.Where(g => g.Contains(bulb)).ToList();
            SetGroups(n, bulb, groups);

            UpdateGroupName(n, group, bulb);
        }

        public static void RemoveBulbFromGroup(this ILifxNetwork network, IBulbGroup bulbGroup, IBulb bulb)
        {
            var n = (LifxNetwork) network;
            var group = (BulbGroup) bulbGroup;
            group.Remove(bulb);

            var groups = n.Groups.Where(g => g.Contains(bulb)).ToList();
            SetGroups(n, bulb, groups);
        }

        public static void RemoveGroup(this ILifxNetwork network, IBulbGroup bulbGroup)
        {
            var n = (LifxNetwork)network;
            var group = (BulbGroup)bulbGroup;
            n.Remove(group);

            var allGroups = n.Groups.ToList();
            foreach (var bulb in group.GetBulbs())
            {
                var groups = allGroups.Where(g => g.Contains(bulb) && !Equals(g, group)).ToList();
                SetGroups(n, bulb, groups);
            }
        }

        public static IBulbGroup AddGroup(this ILifxNetwork network, string name)
        {
            var n = (LifxNetwork) network;
            var group = n.CreateGroup();
            group.Name = name;
            return group;
        }

        public static void RenameGroup(this ILifxNetwork network, IBulbGroup bulbGroup, string name)
        {
            var n = (LifxNetwork) network;
            var group = (BulbGroup) bulbGroup;
            group.Name = name;

            foreach (var bulb in group.GetBulbs())
            {
                UpdateGroupName(n, group, bulb);
            }
        }

        private static void UpdateGroupName(LifxNetwork network, BulbGroup group, IBulb bulb)
        {
            var command = (SetTagLabels)PacketFactory.GetCommand(CommandType.SetTagLabels);
            command.Init(group.Bitmask, Encoding.UTF8.GetBytes(group.Name));
            network.SendCommand(bulb, command);
        }

        private static void SetGroups(LifxNetwork network, IBulb bulb, IEnumerable<BulbGroup> groups)
        {
            var command = (SetTags) PacketFactory.GetCommand(CommandType.SetTags);
            command.Init(GetBitmask(groups));

            network.SendCommand(bulb, command);
        }

        private static ulong GetBitmask(IEnumerable<BulbGroup> groups)
        {
            ulong bitmask = 0;
            foreach (var group in groups)
            {
                bitmask |= group.Bitmask;
            }
            return bitmask;
        }
    }
}