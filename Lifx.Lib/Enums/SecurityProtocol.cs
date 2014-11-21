namespace Lifx.Lib.Enums
{
    internal enum SecurityProtocol : byte
    {
        Open = 1,
        WepPsk = 2,
        WpaTkipPsk = 3,
        WpaAesPsk = 4,
        Wpa2AesPsk = 5,
        Wpa2TkipPsk = 6,
        Wpa2MixedPsk = 7
    }
}