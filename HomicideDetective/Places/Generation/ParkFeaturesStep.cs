using System;
using System.Collections.Generic;
using System.Linq;
using GoRogue.MapGeneration;
using HomicideDetective.Places.Weather;
using HomicideDetective.Words;
using SadRogue.Integration;
using SadRogue.Integration.FieldOfView.Memory;
using SadRogue.Primitives;
using SadRogue.Primitives.GridViews;
// ReSharper disable PossibleLossOfFraction

namespace HomicideDetective.Places.Generation
{
    public class ParkFeaturesStep : GenerationStep
    {
        private int _left;
        private int _right;
        private int _top;
        private int _bottom;
        private List<RogueLikeCell> _cells = new();

        private Point _deepest;
        private Point _secondDeepest;
        private Point _thirdDeepest;
        private int _pondRadius = 24;

        protected override IEnumerator<object?> OnPerform(GenerationContext context)
        {
            var map = PlantFlowers(context);
            var regions = context.GetFirstOrNew(() => new Region("park", (0, 0), (context.Width, 0),
                (context.Width, context.Height), (0, context.Height)), Constants.RegionCollectionTag);

            _deepest = map.RandomPosition();
            _secondDeepest = map.RandomPosition();
            while (DistanceBetween(_deepest, _secondDeepest) > _pondRadius * 3 / 4 || DistanceBetween(_deepest, _secondDeepest) < _pondRadius / 4)
                _secondDeepest = map.RandomPosition();
            _thirdDeepest = map.RandomPosition();
            while (DistanceBetween(_deepest, _thirdDeepest) > _pondRadius * 3 / 4 || DistanceBetween(_secondDeepest, _thirdDeepest) > _pondRadius * 3 / 4)
                _thirdDeepest = map.RandomPosition();

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

                    // var fx = Fx(i);
                    var distance = DistanceFromCenter((i, j)); 
                    if(distance < _pondRadius)
                        map[i, j] = WaveCell(i, j, initialState);
                    else if(distance < _pondRadius + 9)
                    {
                        if (distance > _pondRadius + 4)
                            map[i, j] = PathCell(i, j);
                    }
                    
                    yield return null;
                }
            }

            var bodyOfWater = context.GetFirstOrNew(() => new BodyOfWater(new Rectangle((_left, _top), (_right, _bottom))), Constants.BodyOfWaterTag);
            bodyOfWater.SeedStartingPattern();
            bodyOfWater.Cells.AddRange(_cells);

            var region = MapGen.BaseRegion("Butterfly Park", context.Width, context.Height);
            regions.AddSubRegion(region);
            region = new Region("Clark's Pond", (_left, _top), (_right, _top), (_right, _bottom), (_left, _bottom));
            regions.AddSubRegion(region);
        }

        private ArrayView<MemoryAwareRogueLikeCell> PlantFlowers(GenerationContext context)
        {
            var flowerMap = context.GetFirstOrNew(() => new ArrayView<MemoryAwareRogueLikeCell>(context.Width, context.Height), Constants.GridViewTag);
            FlowerPatch(flowerMap, 8, Color.Goldenrod, 15);
            FlowerPatch(flowerMap, 6, Color.DarkGoldenrod, 15);
            FlowerPatch(flowerMap, 6, Color.LightGoldenrodYellow, 15);
            
            FlowerPatch(flowerMap, 8, Color.MistyRose, 248);
            FlowerPatch(flowerMap, 6, Color.DarkRed, 248);
            FlowerPatch(flowerMap, 6, Color.IndianRed, 248);

            FlowerPatch(flowerMap, 8, Color.WhiteSmoke, 167);
            FlowerPatch(flowerMap, 6, Color.LightGray, 167);
            FlowerPatch(flowerMap, 6, Color.AntiqueWhite, 167);

            FlowerPatch(flowerMap, 8, Color.SkyBlue, 5);
            FlowerPatch(flowerMap, 6, Color.DarkCyan, 5);
            FlowerPatch(flowerMap, 6, Color.CornflowerBlue, 5);
            return flowerMap;
        }

        private static void FlowerPatch(ArrayView<MemoryAwareRogueLikeCell> map, int flowersCount, Color color, int glyph)
        {
            var center = map.RandomPosition();
            for (int i = 0; i < flowersCount; i++)
            {
                var pos = map.RandomPosition();
                while (DistanceBetween(center, pos) > flowersCount)
                    pos = map.RandomPosition();
                
                map[pos] = new MemoryAwareRogueLikeCell(pos, color, Color.Black, glyph, 0);
            }
        }

        private MemoryAwareRogueLikeCell PathCell(int x, int y) =>
            new MemoryAwareRogueLikeCell((x, y), Color.SandyBrown, Color.Black, '.', 0);

        private Color FigureColorFromDepth(int i, int j)
        {
            int blue = 0;
            var here = (i, j);
            var distance = DistanceBetween(here, _deepest);
            distance += DistanceBetween(here, _secondDeepest);
            distance += DistanceBetween(here, _thirdDeepest);
            
            blue = (int) (128.0 * (distance / _pondRadius));
            Color color = new Color(0, 0, blue);
            return color;
        }

        private MemoryAwareRogueLikeCell WaveCell(int x, int y, MovesInWaves.States initialState)
        {
            var color = FigureColorFromDepth(x, y);
            if (x < _left)
                _left = x;
            if (x > _right)
                _right = x;
            if (y > _bottom)
                _bottom = y;
            if (y < _top)
                _top = y;
            var cell =  new MemoryAwareRogueLikeCell((x, y), color, Color.Black, '-', 0, false);
            var waves = new MovesInWaves();
            waves.State = initialState;
            cell.GoRogueComponents.Add(waves);
            _cells.Add(cell);
            return cell;
        }

        private double DistanceFromCenter(Point here)
        {
            var distances = new List<double>();
            distances.Add(DistanceBetween(here, _deepest));
            distances.Add(DistanceBetween(here, _secondDeepest));
            distances.Add(DistanceBetween(here, _thirdDeepest));
            return distances.OrderBy(d => d).First();
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