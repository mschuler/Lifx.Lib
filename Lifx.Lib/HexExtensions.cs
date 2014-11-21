using System.Linq;

namespace Lifx.Lib
{
    internal static class HexExtensions
    {
        public static string ToHexString(this byte[] bytes)
        {
            return string.Join(":", bytes.Select(b => b.ToString("x2")));
        }
    }
}