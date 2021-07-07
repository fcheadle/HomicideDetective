using System;
using System.Collections.Generic;
using SadRogue.Integration;
using SadRogue.Primitives;
using SadRogue.Primitives.GridViews;

namespace HomicideDetective.Places.Weather
{
    public class WindyPlain : CellularAutomataArea
    {
        private Direction _direction;
        
        public WindyPlain(Rectangle body, Direction windDirection) : base(body)
        {
            Body = body;
            _direction = windDirection;
            CurrentState = new ArrayView<MovesInWaves.States>(Body.Width, Body.Height);
            NextState = new ArrayView<MovesInWaves.States>(Body.Width, Body.Height);
            Cells = new List<RogueLikeCell>();
        }

        public override void SeedStartingPattern()
        {
            Random r = new Random();
            for (int i = 0; i < CurrentState.Width; i++)
            {
                for (int j = 0; j < CurrentState.Height; j++)
                {
                    if(r.Next(0,101) % 100 == 0)
                        CurrentState[i,j] = MovesInWaves.States.On;
                    else
                        CurrentState[i,j] = MovesInWaves.States.Off;
                }
            }
        }

        public override MovesInWaves.States DetermineNextState(int i, int j)
        {
            var neighborState = CurrentState[WrapPoint(new Point(i,j) - _direction)];
            var currentState = CurrentState[i, j];
            
            if (currentState == MovesInWaves.States.Off)
                if(neighborState == MovesInWaves.States.On)
                    return MovesInWaves.States.On;

            if (currentState == MovesInWaves.States.On)
                return MovesInWaves.States.Dying;

            if(currentState == MovesInWaves.States.Dying)
                return MovesInWaves.States.Off;
            return MovesInWaves.States.Off;
        }
    }
}