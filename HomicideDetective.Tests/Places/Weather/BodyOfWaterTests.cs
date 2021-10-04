using System.Linq;
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
            var area = new BodyOfWater(Rect);
            Assert.Equal(Rect, area.Body);
        }

        [Fact]
        public void SeedStartingPatternTest()
        {
            var area = new BodyOfWater(Rect);
            area.SeedStartingPattern();
            Point? deadPoint = null;
            Point? dyingPoint = null;
            Point? livePoint = null;
            
            for (int i = 0; i < Rect.Width; i++)
            {
                for (int j = 0; j < Rect.Height; j++)
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

        [Fact(Skip = "Bug uncovered. TODO: fix")]
        public void CalculateNextStepTest()
        {
            var area = new BodyOfWater(Rect);
            area.SeedStartingPattern();
            area.DetermineNextStates();
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    var neighborStates = area.GetNeighboringStates((i, j));
                    var currentState = area.CurrentState[i, j];
                    var nextState = area.NextState[i, j];
                    
                    if(currentState == MovesInWaves.States.On)
                        Assert.Equal(MovesInWaves.States.Dying, nextState);
                    
                    if(currentState == MovesInWaves.States.Dying)
                        Assert.Equal(MovesInWaves.States.Off, nextState);
                    
                    if (currentState == MovesInWaves.States.Off)
                    {
                        if (neighborStates.Count(s => s == MovesInWaves.States.On) == 2)
                            Assert.Equal(MovesInWaves.States.On, nextState);
                        
                        else
                            Assert.Equal(MovesInWaves.States.On, nextState);
                    }
                }
            }
        }
    }
}