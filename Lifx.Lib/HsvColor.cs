using System;
using System.Linq;

namespace Lifx.Lib
{
    public class HsvColor : ColorBase
    {
        public static HsvColor Average(HsvColor[] colors)
        {
            colors = colors.Where(c => c != null).ToArray();
            if (colors.Length == 0)
            {
                return new HsvColor(0, 0, 0);
            }

            var hueXTotal = 0.0;
            var hueYTotal = 0.0;
            var saturationTotal = 0.0;
            var brightnessTotal = 0.0;
            long kelvinTotal = 0;

            foreach (var color in colors)
            {
                hueXTotal += Math.Sin(color.Hue * Math.PI / 180.0);
                hueYTotal += Math.Cos(color.Hue * Math.PI / 180.0);
                saturationTotal += color.Saturation;
                brightnessTotal += color.Brightness;

                if (color.Kelvin == 0)
                {
                    kelvinTotal += 3500;
                }
                else
                {
                    kelvinTotal += color.Kelvin;
                }
            }

            var hue = Math.Atan(hueXTotal / hueYTotal) * 180.0 / Math.PI;
            if (hue < 0.0) { hue += 1.0; }
            var saturation = saturationTotal / colors.Length;
            var brightness = brightnessTotal / colors.Length;
            var kelvin = (int)(kelvinTotal / colors.Length);

            return new HsvColor(hue, saturation, brightness, kelvin);
        }

        private readonly double _hue;
        private readonly double _saturation;
        private readonly double _brightness;

        public HsvColor(double hue, double saturation = 1, double brightness = 1, int kelvin = 3500)
            : base(kelvin)
        {
            _hue = Math.Min(360d, Math.Max(hue, 0));
            _saturation = Math.Min(1d, Math.Max(saturation, 0d));
            _brightness = Math.Min(1d, Math.Max(brightness, 0d));
        }

        public double Hue { get { return _hue; } }

        public double Saturation { get { return _saturation; } }

        public double Brightness { get { return _brightness; } }

        public override RgbColor ToRgb()
        {
            return ToRgb(false);
        }

        public RgbColor ToRgb(bool ignoreBrightnessAndSaturation)
        {
            var r = 0d;
            var g = 0d;
            var b = 0d;
            var h = _hue / 360d;
            var i = (int)(h * 6d);
            var f = h * 6d - i;
            var s = ignoreBrightnessAndSaturation ? 1.0 : _saturation;
            var v = ignoreBrightnessAndSaturation ? 1.0 : _brightness;
            var p = v * (1 - s);
            var q = v * (1 - f * s);
            var t = v * (1 - (1 - f) * s);

            switch (i % 6)
            {
                case 0:
                    r = v;
                    g = t;
                    b = p;
                    break;
                case 1:
                    r = q;
                    g = v;
                    b = p;
                    break;
                case 2:
                    r = p;
                    g = v;
                    b = t;
                    break;
                case 3:
                    r = p;
                    g = q;
                    b = v;
                    break;
                case 4:
                    r = t;
                    g = p;
                    b = v;
                    break;
                case 5:
                    r = v;
                    g = p;
                    b = q;
                    break;
            }

            return new RgbColor((byte)(r * 255), (byte)(g * 255), (byte)(b * 255), Kelvin);
        }

        public override HslColor ToHsl()
        {
            return ToRgb().ToHsl();
        }

        public override HsvColor ToHsv()
        {
            return this;
        }

        protected bool Equals(HsvColor other)
        {
            return _hue.Equals(other._hue) &&
                _saturation.Equals(other._saturation) &&
                _brightness.Equals(other._brightness);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) { return false; }
            if (ReferenceEquals(this, obj)) { return true; }
            if (obj.GetType() != GetType()) { return false; }
            return Equals((HsvColor)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = _hue.GetHashCode();
                hashCode = (hashCode * 397) ^ _saturation.GetHashCode();
                hashCode = (hashCode * 397) ^ _brightness.GetHashCode();
                return hashCode;
            }
        }

        public override string ToString()
        {
            return string.Format("hsv({0:F0}, {1:F0}%, {2:F0}%)", Hue, Saturation * 100, Brightness * 100);
        }
    }
}