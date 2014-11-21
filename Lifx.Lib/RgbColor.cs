using System;

namespace Lifx.Lib
{
    public class RgbColor : ColorBase
    {
        private readonly double _red;
        private readonly double _green;
        private readonly double _blue;

        public RgbColor(double red, double green, double blue, int kelvin = 3500)
            : base(kelvin)
        {
            _red = Math.Min(255, Math.Max(red, 0));
            _green = Math.Min(255, Math.Max(green, 0));
            _blue = Math.Min(255, Math.Max(blue, 0));
        }

        public double Red { get { return _red; } }

        public double Green { get { return _green; } }

        public double Blue { get { return _blue; } }

        public override string ToString()
        {
            return string.Format("rgb({0:F0}, {1:F0}, {2:F0})", Red, Green, Blue);
        }

        public override HslColor ToHsl()
        {
            var rd = _red / 255d;
            var gd = _green / 255d;
            var bd = _blue / 255d;
            var max = Math.Max(rd, Math.Max(gd, bd));
            var min = Math.Min(rd, Math.Min(gd, bd));
            var h = 0d;
            double s;
            var l = (max + min) / 2d;

            if (Math.Abs(max - min) < 0.001)
            {
                h = s = 0; // achromatic
            }
            else
            {
                var d = max - min;
                s = l > 0.5 ? d / (2 - max - min) : d / (max + min);
                if (Math.Abs(max - rd) < 0.001)
                {
                    h = (gd - bd) / d + (gd < bd ? 6 : 0);
                }
                else if (Math.Abs(max - gd) < 0.001)
                {
                    h = (bd - rd) / d + 2;
                }
                else if (Math.Abs(max - bd) < 0.001)
                {
                    h = (rd - gd) / d + 4;
                }
                h /= 6d;
            }

            h *= 360d;
            return new HslColor(h, s, l, Kelvin);
        }

        public override HsvColor ToHsv()
        {
            var r = _red / 255d;
            var g = _green / 255d;
            var b = _blue / 255d;
            var max = Math.Max(r, Math.Max(g, b));
            var min = Math.Min(r, Math.Min(g, b));

            var h = 0d;
            double s;
            var v = max;
            var d = max - min;
            s = Math.Abs(max) < 0.001 ? 0 : d / max;

            if (Math.Abs(max - min) < 0.001)
            {
                h = 0; // achromatic
            }
            else
            {
                if (Math.Abs(max - r) < 0.001)
                {
                    h = (g - b) / d + (g < b ? 6 : 0);
                }
                else if (Math.Abs(max - g) < 0.001)
                {
                    h = (b - r) / d + 2;
                }
                else if (Math.Abs(max - b) < 0.001)
                {
                    h = (r - g) / d + 4;
                }
                h /= 6;
            }

            h *= 360d;
            return new HsvColor(h, s, v, Kelvin);
        }

        public override RgbColor ToRgb()
        {
            return this;
        }
    }
}