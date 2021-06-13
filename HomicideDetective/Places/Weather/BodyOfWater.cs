using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using GoRogue.GameFramework;
using GoRogue.GameFramework.Components;
using SadRogue.Integration;
using SadRogue.Primitives;
using SadRogue.Primitives.GridViews;

namespace HomicideDetective.Places.Weather
{
    public class BodyOfWater : CellularAutomataArea
    {
        public IGameObject? Parent { get; set; }
        public Rectangle Body;
        public ArrayView<MovesInWaves.States> CurrentState;
        public ArrayView<MovesInWaves.States> NextState;
        public List<RogueLikeCell> Cells;
        
        public BodyOfWater(Rectangle body) : base(body)
        {
            Body = body;
            CurrentState = new ArrayView<MovesInWaves.States>(Body.Width, Body.Height);
            NextState = new ArrayView<MovesInWaves.States>(Body.Width, Body.Height);
            Cells = new List<RogueLikeCell>();
        }

        public override void SeedStartingPattern()
        {
            for (int i = 0; i < Body.Area / 10; i++)
                CurrentState[CurrentState.RandomPosition()] = MovesInWaves.States.On;
        }

        public override void DetermineNextStates()
        {
            for (int i = 0; i < CurrentState.Width; i++)
            {
                for (int j = 0; j < CurrentState.Height; j++)
                {
                    NextState[i, j] = DetermineNextState(i, j);
                }
            }
        }

        public override MovesInWaves.States DetermineNextState(int i, int j)
        {
            if (CurrentState[i, j] == MovesInWaves.States.Off)
            {
                if (GetNeighboringStates((i, j)).Count(s => s == MovesInWaves.States.On) == 2)
                    return MovesInWaves.States.On;
            }
            else if (CurrentState[i, j] == MovesInWaves.States.On)
                return MovesInWaves.States.Dying;

            // if (CurrentState[i, j] == MovesInWaves.States.Dying)
                return MovesInWaves.States.Off;
        }

        public override IEnumerable<MovesInWaves.States> GetNeighboringStates(Point point)
        {
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    int x = i + point.X;
                    if (x >= CurrentState.Width)
                        x -= CurrentState.Width;
                    if (x < 0)
                        x += CurrentState.Width;

                    int y = j + point.Y;
                    if (y >= CurrentState.Height)
                        y -= CurrentState.Height;
                    if (y < 0)
                        y += CurrentState.Height;
                    
                    if(CurrentState.Contains(x,y))
                        yield return CurrentState[x, y];
                    
                    else
                    {
                        throw new InvalidEnumArgumentException();
                    }
                }
            }
        }
    }
}