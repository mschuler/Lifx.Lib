using System.Collections.Generic;
using System.ComponentModel;

namespace Lifx.Lib
{
    public interface IBulbGroup : INotifyPropertyChanged
    {
        ulong Bitmask { get; }

        string Name { get; }

        IEnumerable<IBulb> GetBulbs();
    }
}