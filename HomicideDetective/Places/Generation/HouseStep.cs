using System.Collections.Generic;
using System.Linq;
using GoRogue.MapGeneration;
using SadRogue.Primitives;
using SadRogue.Primitives.GridViews;
using SadRogue.Integration;

namespace HomicideDetective.Places.Generation
{
    public class HouseStep : GenerationStep
    {
        private readonly int _houseWidth = 50;
        private readonly int _houseHeight = 19;
        private readonly int _minimumDimension = 5;
        private readonly double _angle;
        public HouseStep()
        {
            _angle = 0;
        }

        public HouseStep(int angle)
        {
            _angle = angle;
        }
        
        protected override IEnumerator<object?> OnPerform(GenerationContext context)
        {
            //set up generation step
            var map = context.GetFirstOrNew<ISettableGridView<RogueLikeCell>>
                (() => new ArrayView<RogueLikeCell>(context.Width, context.Height), "house");
            var rooms = context.GetFirstOrNew(() => new List<Region>(), "rooms");
            Point midpoint = (map.Width / 2, map.Height / 2);
            
            //Create a cluster of rectangles and iterate over them
            var floorSpace = new Rectangle(16, 16, _houseWidth, _houseHeight);
            foreach (var room in floorSpace.BisectRecursive(_minimumDimension))
            {
                //create a rotated region
                var region = Region.FromRectangle("room", room).Rotate(_angle, midpoint);
                rooms.Add(region);

                //place floorson the inner points
                foreach (var point in region.InnerPoints.Where(p => map.Contains(p)))
                    map[point] = new RogueLikeCell(point, Color.DarkRed, Color.Black, '.', 0);

                //place walls on the outer points
                foreach (var point in region.OuterPoints.Where(p => map.Contains(p)))
                    map[point] = new RogueLikeCell(point, Color.DarkRed, Color.Black, 176, 0, false, false);

                //place doors on the north and south of each room
                var points = region.NorthBoundary;
                var door = points[points.Count / 2];

                if (map.Contains(door))
                    map[door] = new RogueLikeCell(door, Color.DarkGoldenrod, Color.Black, 254, 0, true, false);

                door = (door.X, region.Bottom);
                if (map.Contains(door))
                    map[door] = new RogueLikeCell(door, Color.DarkGoldenrod, Color.Black, 254, 0, true, false);

                yield return null;
            }
        }
    }
}