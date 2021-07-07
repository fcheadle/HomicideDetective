using HomicideDetective.Places.Weather;
using SadRogue.Primitives;
using Xunit;

namespace HomicideDetective.Tests.Places.Weather
{
    public abstract class CellularAutomataAreaTests
    {
        protected const int Width = 30;
        protected const int Height = 20;
        protected Rectangle Rect = new Rectangle((0, 0), (Width, Height));

        protected void AssertAreaHasCells(CellularAutomataArea area)
        {
            Assert.NotNull(area.Cells);
            Assert.NotEmpty(area.Cells);
        }
        
        protected Point WrapPoint(Point point)
        {
            if (point.X >= Width)
                point -= (Width, 0);
            if (point.X < 0)
                point += (Width, 0);

            if (point.Y >= Height)
                point -= (0, Height);
            if (point.Y < 0)
                point += (0, Height);

            return point;
        }
    }
}