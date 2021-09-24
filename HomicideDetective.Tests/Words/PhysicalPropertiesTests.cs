using HomicideDetective.Words;
using Xunit;

namespace HomicideDetective.Tests.Words
{
    public class PhysicalPropertiesTests
    {
        [Fact]
        public void PhysicalPropertiesTest()
        {
            var mass = 101;
            var volume = 420;
            var quality = "shiddy";
            var size = "big";
            var age = "super-old";
            var shape = "L-shaped";
            var color = "chartreuse";
            var proper = "Ivy League";

            var properties = new PhysicalProperties(mass, volume, quality, size, age, shape, color, proper);
            Assert.Equal(mass, properties.Mass);
            Assert.Equal(volume, properties.Volume);
            Assert.Equal(size, properties.SizeAdjective);
            Assert.Equal(age, properties.AgeAdjective);
            Assert.Equal(shape, properties.ShapeAdjective);
            Assert.Equal(color, properties.ColorAdjective);
            Assert.Equal(proper, properties.ProperAdjective);

            var expected = $"{quality} {size} {age} {shape} {color} {proper} ";
            var actual = properties.GetPrintableString();
            Assert.Equal(expected, actual);
        }
    }
}