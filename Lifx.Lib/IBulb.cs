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
    }
}