using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Lifx.Lib
{
    internal sealed class BulbGroup : IBulbGroup
    {
        private readonly object _bulbCollectionLock = new object();
        private readonly IDictionary<byte[], IBulb> _bulbs = new Dictionary<byte[], IBulb>();
        private readonly ulong _bitmask;
        private string _name;

        public BulbGroup(ulong bitmask)
        {
            _bitmask = bitmask;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ulong Bitmask { get { return _bitmask; } }

        public string Name
        {
            get { return _name; }
            set
            {
                if (string.Equals(_name, value, StringComparison.Ordinal))
                {
                    return;
                }
                _name = value ?? string.Empty;
                OnPropertyChanged();
            }
        }

        public IEnumerable<IBulb> GetBulbs()
        {
            lock (_bulbCollectionLock)
            {
                return _bulbs.Values.ToList();
            }
        }

        public bool Add(IBulb bulb)
        {
            lock (_bulbCollectionLock)
            {
                if (_bulbs.ContainsKey(bulb.Mac))
                {
                    return false;
                }

                _bulbs.Add(bulb.Mac, bulb);
                return true;
            }
        }

        public bool Contains(IBulb bulb)
        {
            lock (_bulbCollectionLock)
            {
                return _bulbs.ContainsKey(bulb.Mac);
            }
        }

        public void Remove(IBulb bulb)
        {
            _bulbs.Remove(bulb.Mac);
        }

        private bool Equals(BulbGroup other)
        {
            return _bitmask == other._bitmask;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) { return false; }
            if (ReferenceEquals(this, obj)) { return true; }
            return obj is BulbGroup && Equals((BulbGroup)obj);
        }

        public override int GetHashCode()
        {
            return _bitmask.GetHashCode();
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) { handler(this, new PropertyChangedEventArgs(propertyName)); }
        }
    }
}