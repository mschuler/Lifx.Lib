namespace Lifx.Lib
{
    public interface IColor
    {
        int Kelvin { get; }

        HsvColor ToHsv();
        RgbColor ToRgb();
        HslColor ToHsl();
    }
}