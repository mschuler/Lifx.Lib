namespace Lifx.Lib.Utils
{
    public struct Timestamp
    {
        public byte Second;
        public byte Minute;
        public byte Hour;
        public byte Day;
        public byte[] Month; // JAN, FEB, MAR etc. ASCII encoded
        public byte Year;
    }
}