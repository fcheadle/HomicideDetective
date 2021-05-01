using System.Collections.Generic;
using System.Linq;
using GoRogue.MapGeneration;
using HomicideDetective.Mysteries;
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
            var map = context.GetFirstOrNew<ISettableGridView<RogueLikeCell>>
                (() => new ArrayView<RogueLikeCell>(context.Width, context.Height), "house");

            var rooms = context.GetFirstOrNew<List<Region>>
                (() => new List<Region>(), "rooms");
            Point midpoint = (map.Width / 2, map.Height / 2);

            for (int i = 0; i < map.Width - _houseWidth; i+= _houseWidth)
            {
                for (int j = 0; j < map.Height - _houseHeight; j+= _houseHeight)
                {
                    var floorSpace = new Rectangle(i+4, j+4, _houseWidth, _houseHeight);

                    foreach (var room in floorSpace.BisectRecursive(_minimumDimension))
                    {
                        var region = Region.FromRectangle("room", room).Rotate(_angle, midpoint);
                        // var region = new Place(new Substantive(), room.MinExtent, room.MaxExtent + 1);
                        rooms.Add(region);
                        
                        foreach (var point in region.InnerPoints.Where(p => map.Contains(p)))
                            map[point] = new RogueLikeCell(point, Color.Brown, Color.Black, 240, 0);
                        
                        foreach (var point in region.OuterPoints.Where(p => map.Contains(p)))
                            map[point] = new RogueLikeCell(point, Color.DarkGoldenrod, Color.DarkGray, 178, 0, false, false);

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
    }
}