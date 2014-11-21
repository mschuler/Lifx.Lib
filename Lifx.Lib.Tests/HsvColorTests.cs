using NUnit.Framework;

namespace Lifx.Lib.Tests
{
    [TestFixture]
    public class HsvColorTests
    {
        [Test]
        public void AverageTest()
        {
            var c1 = new HsvColor(10, 0.4, 0.6);
            var c2 = new HsvColor(60, 0.2, 0.4);
            var avg = HsvColor.Average(new[] { c1, c2 });

            Assert.AreEqual(0.3, avg.Saturation, 0.0001);
            Assert.AreEqual(0.5, avg.Brightness, 0.0001);
            Assert.AreEqual(35, avg.Hue, 0.0001);
        }
    }
}
