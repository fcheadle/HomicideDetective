using System;
using System.Collections.Generic;
using GoRogue.MapGeneration;
using SadRogue.Integration;
using SadRogue.Primitives;
using SadRogue.Primitives.GridViews;

namespace HomicideDetective.Places.Generation
{
    public class PondStep : GenerationStep
    {
        private int _left;
        private int _right;
        private int _top;
        private int _bottom;
        private List<RogueLikeCell> _cells = new();
        protected override IEnumerator<object?> OnPerform(GenerationContext context)
        {
            var pondRadius = 24;
            var map = context.GetFirstOrNew<ISettableGridView<RogueLikeCell>>
                (() => new ArrayView<RogueLikeCell>(context.Width, context.Height), "park");
            var regions = context.GetFirstOrNew(() => new List<BodyOfWater>(), "pond");

            var deepestPoint = map.RandomPosition();
            var nextDeepestPoint = map.RandomPosition();
            while (DistanceBetween(deepestPoint, nextDeepestPoint) > pondRadius || DistanceBetween(deepestPoint, nextDeepestPoint) < pondRadius * 3 / 4)
                nextDeepestPoint = map.RandomPosition();

            var rand = new Random();
            _left = map.Width;
            _right = 0;
            _top = map.Height;
            _bottom = 0;
            for (int i = 0; i < map.Width; i++)
            {
                for (int j = 0; j < map.Height; j++)
                {
                    MovesInWaves.States initialState;
                    var chance = rand.Next();
                    if (chance % 3 == 0)
                        initialState = MovesInWaves.States.Dying;
                    else if (chance % 3 == 1)
                        initialState = MovesInWaves.States.Off;
                    else
                        initialState = MovesInWaves.States.On;
                    //deepest water
                    if (DistanceBetween(deepestPoint, (i,j)) < pondRadius / 2 && DistanceBetween(nextDeepestPoint, (i,j)) < pondRadius / 2)
                        map[i, j] = WaveCell(i, j, Color.DarkBlue, initialState);
                    
                    //middle-depth water
                    else if (DistanceBetween(deepestPoint, (i, j)) < pondRadius && DistanceBetween(nextDeepestPoint, (i, j)) < pondRadius)
                        map[i, j] = WaveCell(i, j, Color.Blue, initialState);
                    
                    //shallow water
                    else if (DistanceBetween(deepestPoint, (i,j)) < pondRadius + 2 && DistanceBetween(nextDeepestPoint, (i,j)) < pondRadius + 2)
                        map[i, j] = WaveCell(i, j, Color.LightBlue, initialState);
                    
                    //else do nothing
                    yield return null;
                }
            }

            var region = new Rectangle((_left, _top), (_right, _bottom));
            var pond = new BodyOfWater(region);
            pond.SeedStartingPattern();
            pond.Cells.AddRange(_cells);
            regions.Add(pond);
        }

        private RogueLikeCell WaveCell(int x, int y, Color color, MovesInWaves.States initialState)
        {
            if (x < _left)
                _left = x;
            if (x > _right)
                _right = x;
            if (y > _bottom)
                _bottom = y;
            if (y < _top)
                _top = y;
            var cell =  new RogueLikeCell((x, y), color, Color.Black, '-', 0, false);
            var waves = new MovesInWaves();
            waves.State = initialState;
            cell.GoRogueComponents.Add(waves);
            _cells.Add(cell);
            return cell;
        }

        private static double DistanceBetween(Point a, Point b)
        {
            int xDiff = b.X - a.X;
            xDiff *= xDiff;

            int yDiff = b.Y - a.Y;
            yDiff *= yDiff;
            return Math.Sqrt(xDiff + yDiff);
        }
    }
}