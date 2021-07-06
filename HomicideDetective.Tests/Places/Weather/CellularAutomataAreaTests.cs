using HomicideDetective.Places.Weather;
using SadRogue.Primitives;
using Xunit;

namespace HomicideDetective.Tests.Places.Weather
{
    public abstract class CellularAutomataAreaTests
    {
        protected const int _width = 30;
        protected const int _height = 20;
        protected Rectangle _rect = new Rectangle((0, 0), (_width, _height));

        protected void AssertAreaHasCells(CellularAutomataArea area)
        {
            Assert.NotNull(area.Cells);
            Assert.NotEmpty(area.Cells);
        }
    }
}