namespace Lifx.Lib
{
    public interface IGateway
    {
        byte[] Mac { get; }
        string IpAddress { get; }
    }
}