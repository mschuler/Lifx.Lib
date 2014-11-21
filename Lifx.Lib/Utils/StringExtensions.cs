using System.Text;

namespace Lifx.Lib.Utils
{
    internal static class StringExtensions
    {
        public static string ToUtf8String(this byte[] bytes)
        {
            return bytes.ToUtf8String(0, bytes.Length);
        }

        public static string ToUtf8String(this byte[] bytes, int index, int count)
        {
            return Encoding.UTF8.GetString(bytes, index, count).Trim('\0');
        }
    }
}