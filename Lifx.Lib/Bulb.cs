using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Lifx.Lib.Enums;

namespace Lifx.Lib
{
    internal class Bulb : IBulb
    {
        private readonly byte[] _mac;
        private readonly IGateway _gateway;
        private string _name;
        private bool _isPowerOn;
        private HsvColor _color;

        public Bulb(byte[] macAddress, IGateway gateway)
        {
            _mac = macAddress;
            _gateway = gateway;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public byte[] Mac { get { return _mac; } }
        public string MacString { get { return _mac.ToHexString(); } }
        public IGateway Gateway { get { return _gateway; } }

        public HsvColor Color
        {
            get { return _color; }
            set
            {
                if (_color != null && _color.Equals(value))
                {
                    return;
                }
                _color = value;
                OnPropertyChanged();
            }
        }

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

        public bool IsPowerOn
        {
            get { return _isPowerOn; }
            set
            {
                if (_isPowerOn == value)
                {
                    return;
                }
                _isPowerOn = value;
                OnPropertyChanged();
            }
        }

        public BulbVersion Version { get; set; }
        public BulbTimeInfo TimeInfo { get; set; }
        public MeshInfo MeshInfo { get; set; }
        public WifiInfo WifiInfo { get; set; }

        public ResetSwitchPosition ResetSwitchPosition { get; set; }
        public UInt32 Voltage { get; set; }

        #region Equality

        protected bool Equals(Bulb other)
        {
            return _mac.SequenceEqual(other._mac);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != GetType())
            {
                return false;
            }
            return Equals((Bulb)obj);
        }

        public override int GetHashCode()
        {
            if (_mac == null || _mac.Length == 0)
            {
                return 0;
            }

            unchecked
            {
                var result = 0;
                foreach (var b in _mac)
                {
                    result = (result * 31) ^ b;
                }
                return result;
            }
        }

        #endregion

        public override string ToString()
        {
            return string.Format("{0} ({1})", Name, Mac.ToHexString());
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) { handler(this, new PropertyChangedEventArgs(propertyName)); }
        }
    }
}