using System.Collections.Generic;

namespace Lifx.Lib.Utils
{
    internal static class GroupExtensions
    {
        public static bool IsSingleGroup(this ulong bitmask)
        {
            for (var i = 0; i < 64; i++)
            {
                var groupTag = (ulong)1 << i;

                if (bitmask == groupTag)
                {
                    return true;
                }
            }

            return false;
        }

        public static IEnumerable<ulong> GetGroupTags(this ulong tag)
        {
            // 0000'0000'0000'0000'0000'0000'0000'0000'0000'0000'0000'0000'0000'0000'0000'0001
            // 0000'0000'0000'0000'0000'0000'0000'0000'0000'0000'0000'0000'0000'0000'0000'0010
            // 0000'0000'0000'0000'0000'0000'0000'0000'0000'0000'0000'0000'0000'0000'0000'0100
            // 0000'0000'0000'0000'0000'0000'0000'0000'0000'0000'0000'0000'0000'0000'0000'1000
            // ...

            for (int i = 0; i < 64; i++)
            {
                var groupTag = (ulong)1 << i;

                if ((tag & groupTag) == groupTag)
                {
                    yield return groupTag;
                }
            }
        }
    }
}