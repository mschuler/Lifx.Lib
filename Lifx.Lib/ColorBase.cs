using System;

namespace Lifx.Lib
{
    public abstract class ColorBase : IColor
    {
        private readonly int _kelvin;

        protected ColorBase(int kelvin)
        {
            _kelvin = Math.Min(10000, Math.Max(kelvin, 2500));
        }

        public int Kelvin { get { return _kelvin; } }

        public abstract HsvColor ToHsv();
        public abstract RgbColor ToRgb();
        public abstract HslColor ToHsl();
    }
}