using System.Collections.Generic;
using GoRogue.GameFramework;
using GoRogue.GameFramework.Components;
using SadRogue.Integration;
using SadRogue.Primitives;
using SadRogue.Primitives.GridViews;

namespace HomicideDetective.Places.Weather
{
    public abstract class CellularAutomataArea : IGameObjectComponent
    {
        public IGameObject? Parent { get; set; }
        public Rectangle Body;
        public ArrayView<MovesInWaves.States> CurrentState;
        public ArrayView<MovesInWaves.States> NextState;
        public List<RogueLikeCell> Cells;
        
        public CellularAutomataArea(Rectangle body)
        {
            Body = body;
            CurrentState = new ArrayView<MovesInWaves.States>(Body.Width, Body.Height);
            NextState = new ArrayView<MovesInWaves.States>(Body.Width, Body.Height);
            Cells = new List<RogueLikeCell>();
        }

        public abstract void SeedStartingPattern();
        public abstract void DetermineNextStates();
        public abstract MovesInWaves.States DetermineNextState(int i, int j);
        public abstract IEnumerable<MovesInWaves.States> GetNeighboringStates(Point point);
    }
}