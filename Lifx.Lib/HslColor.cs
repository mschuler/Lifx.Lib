using System;

namespace Lifx.Lib
{
    public class HslColor : ColorBase
    {
        private readonly double _hue;
        private readonly double _saturation;
        private readonly double _lumination;

        public HslColor(double hue, double saturation = 1, double lumination = 1, int kelvin = 3500)
            : base(kelvin)
        {
            _hue = Math.Min(360d, Math.Max(hue, 0));
            _saturation = Math.Min(1d, Math.Max(saturation, 0d));
            _lumination = Math.Min(1d, Math.Max(lumination, 0d));
        }

        public double Hue
        {
            get { return _hue; }
        }

        public double Saturation
        {
            get { return _saturation; }
        }

        public double Lumination
        {
            get { return _lumination; }
        }

        public override RgbColor ToRgb()
        {
            double r, g, b;
            var h = _hue / 360d;

            if (Math.Abs(_saturation) < 0.001)
            {
                r = g = b = _lumination * 255d; // achromatic
            }
            else
            {
                var q = _lumination < 0.5
                    ? _lumination * (1 + _saturation)
                    : _lumination + _saturation - _lumination * _saturation;
                var p = 2 * _lumination - q;
                r = HueToRgb(p, q, h + 1d / 3d) * 255d;
                g = HueToRgb(p, q, h) * 255d;
                b = HueToRgb(p, q, h - 1d / 3d) * 255d;
            }

            return new RgbColor(r, g, b, Kelvin);
        }

        public override HsvColor ToHsv()
        {
            return ToRgb().ToHsv();
        }

        public override HslColor ToHsl()
        {
            return this;
        }

        public override string ToString()
        {
            return string.Format("hsl({0}, {1:F0}%, {2:F0}%)", Hue, Saturation * 100, Lumination * 100);
        }

        private double HueToRgb(double p, double q, double t)
        {
            if (t < 0d) { t += 1d; }
            if (t > 1d) { t -= 1d; }
            if (t < 1d / 6d) { return p + (q - p) * 6d * t; }
            if (t < 1d / 2d) { return q; }
            if (t < 2d / 3d) { return p + (q - p) * (2d / 3d - t) * 6d; }
            return p;
        }
    }
}