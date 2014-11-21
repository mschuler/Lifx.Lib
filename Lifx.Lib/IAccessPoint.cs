namespace Lifx.Lib
{
    public interface IAccessPoint
    {
        string Ssid { get; }

        bool IsPasswordRequired { get; }
    }
}