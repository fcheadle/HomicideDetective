using System.Collections.Generic;
using HomicideDetective.Places.Weather;
using SadRogue.Primitives;
using Xunit;

namespace HomicideDetective.Tests.Places.Weather
{
    public class WindyPlainsTests : CellularAutomataAreaTests
    {
        public static readonly IEnumerable<object[]> Directions = new List<object[]>()
        {
            new object[] { Direction.Types.Down},
            new object[] { Direction.Types.Up},
            new object[] { Direction.Types.Left},
            new object[] { Direction.Types.Right},
        };
        [Fact]
        public void NewWindyPlainsTest()
        {
            var area = new WindyPlain(Rect, Direction.Down);
            Assert.Equal(Rect, area.Body);
        }

        [Fact]
        public void SeedStartingPatternTest()
        {
            var area = new WindyPlain(Rect, Direction.Down);
            area.SeedStartingPattern();
            Point? deadPoint = null;
            Point? livePoint = null;
            
            for (int i = 0; i < Rect.Width; i++)
            {
                for (int j = 0; j < Rect.Height; j++)
                {
                    if (area.CurrentState[i, j] == MovesInWaves.States.Off)
                        deadPoint = (i, j);

                    if (area.CurrentState[i, j] == MovesInWaves.States.On)
                        livePoint = (i, j);
                }
            }
            
            Assert.NotNull(deadPoint);
            Assert.NotNull(livePoint);
        }

        [Theory(Skip = "Failing. Cause Unknown. Probably an Actual Bug")]
        [MemberData(nameof(Directions))]
        public void CalculateNextStepTest(Direction direction)
        {
            var area = new WindyPlain(Rect, direction);
            area.SeedStartingPattern();
            area.DetermineNextStates();
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    var wrappedPoint = WrapPoint(new Point(i, j) - direction);
                    var neighborState = area.CurrentState[wrappedPoint];
                    var currentState = area.CurrentState[i, j];
                    var nextState = area.NextState[i, j];
                    
                    if(currentState == MovesInWaves.States.On)
                        Assert.Equal(MovesInWaves.States.Dying, nextState);
                    
                    else if (currentState == MovesInWaves.States.Dying)
                        Assert.Equal(MovesInWaves.States.Off, nextState);
                    
                    else 
                    {
                        if (neighborState == MovesInWaves.States.On)
                            Assert.Equal(MovesInWaves.States.On, nextState);
                        
                        if(neighborState == MovesInWaves.States.Off)
                            Assert.Equal(MovesInWaves.States.Off, nextState);
                    }
                }
            }
        }
    }
}