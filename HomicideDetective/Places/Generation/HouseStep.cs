using System.Collections.Generic;
using System.Linq;
using GoRogue.MapGeneration;
using HomicideDetective.Mysteries;
using SadRogue.Primitives;
using SadRogue.Primitives.GridViews;
using TheSadRogue.Integration;

namespace HomicideDetective.Places.Generation
{
    public class HouseStep : GenerationStep
    {
        private readonly int _houseWidth = 50;
        private readonly int _houseHeight = 19;
        private readonly int _minimumDimension = 5;
        private readonly int _angle;
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

            var rooms = context.GetFirstOrNew<List<Place>>
                (() => new List<Place>(), "rooms");
            
            for (int i = 0; i < map.Width - _houseWidth; i+= _houseWidth)
            {
                for (int j = 0; j < map.Height - _houseHeight; j+= _houseHeight)
                {
                    var floorSpace = new Rectangle(4, 4, _houseWidth, _houseHeight);

                    foreach (var room in floorSpace.BisectRecursive(_minimumDimension))
                    {
                        var region = new Place(new Substantive(), room.MinExtent, room.MaxExtent + 1);
                        rooms.Add(region);
                        
                        foreach (var point in region.InnerPoints.Positions.Where(p => map.Contains(p)))
                            map[point] = new RogueLikeCell(point, Color.Brown, Color.Black, 240, 0);
                        
                        
                        foreach (var point in region.OuterPoints.Positions.Where(p => map.Contains(p)))
                        {
                            map[point] = new RogueLikeCell(point, Color.DarkGoldenrod, Color.DarkGray, 178, 0, false, false);
                        }

                        var points = region.NorthBoundary.Positions;
                        var door = points[points.Count / 2];

                        map[door] = new RogueLikeCell(door, Color.DarkGoldenrod, Color.Black, 254, 0, true, false);
                        door = (door.X, region.Bottom);
                        map[door] = new RogueLikeCell(door, Color.DarkGoldenrod, Color.Black, 254, 0, true, false);
                        yield return null;
                    }
                }
            }
        }
    }
}