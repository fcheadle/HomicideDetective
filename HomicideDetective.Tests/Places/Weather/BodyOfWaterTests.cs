using System;
using HomicideDetective.Places;
using HomicideDetective.Places.Weather;
using SadRogue.Primitives;
using Xunit;

namespace HomicideDetective.Tests.Places.Weather
{
    public class BodyOfWaterTests : CellularAutomataAreaTests
    {
        [Fact]
        public void NewBodyOfWaterTest()
        {
            var area = new BodyOfWater(_rect);
        }

        [Fact]
        public void SeedStartingPatternTest()
        {
            var area = new BodyOfWater(_rect);
            area.SeedStartingPattern();
            Point? deadPoint = null;
            Point? dyingPoint = null;
            Point? livePoint = null;
            
            for (int i = 0; i < _rect.Width; i++)
            {
                for (int j = 0; j < _rect.Height; j++)
                {
                    if (area.CurrentState[i, j] == MovesInWaves.States.Dying)
                        dyingPoint = (i, j);

                    if (area.CurrentState[i, j] == MovesInWaves.States.Off)
                        deadPoint = (i, j);

                    if (area.CurrentState[i, j] == MovesInWaves.States.On)
                        livePoint = (i, j);
                }
            }
            
            Assert.NotNull(deadPoint);
            Assert.NotNull(dyingPoint);
            Assert.NotNull(livePoint);
        }

        [Fact(Skip = "Not Implemented")]
        public void CalculateNextStepTest()
        {
            var area = new BodyOfWater(_rect);
            throw new NotImplementedException();
        }
    }
}