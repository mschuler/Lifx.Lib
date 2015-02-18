using System.ComponentModel;

namespace Lifx.Lib
{
    public interface IBulb : INotifyPropertyChanged
    {
        byte[] Mac { get; }
        string MacString { get; }
        IGateway Gateway { get; }

        string Name { get; }
        HsvColor Color { get; }
        bool IsPowerOn { get; }

        BulbVersion Version { get; set; }
        BulbTimeInfo TimeInfo { get; set; }
        MeshInfo MeshInfo { get; set; }
        WifiInfo WifiInfo { get; set; }
    }
}