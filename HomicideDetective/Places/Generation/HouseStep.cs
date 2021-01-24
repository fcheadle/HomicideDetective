using System.Collections.Generic;
using System.Linq;
using GoRogue.MapGeneration;
using SadRogue.Primitives;
using SadRogue.Primitives.GridViews;
using TheSadRogue.Integration;

namespace HomicideDetective.Places.Generation
{
    public class HouseStep : GenerationStep
    {
        private readonly int _houseWidth = 50;
        private readonly int _houseHeight = 20;
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

            var rooms = context.GetFirstOrNew<List<Region>>
                (() => new List<Region>(), "rooms");
            
            for (int i = 0; i < map.Width - _houseWidth; i+= _houseWidth)
            {
                for (int j = 0; j < map.Height - _houseHeight; j+= _houseHeight)
                {
                    var floorSpace = new Rectangle(4, 4, _houseWidth, _houseHeight);

                    foreach (var room in floorSpace.BisectRecursive(_minimumDimension))
                    {
                        var region = Region.FromRectangle("room", room).Rotate(_angle);
                        rooms.Add(region);
                        
                        foreach (var point in region.InnerPoints.Positions.Where(p => map.Contains(p)))
                        {
                            bool alt = (point.X + point.Y) % 2 == 0;
                            map[point] = new RogueLikeCell(point, Color.DarkSlateGray, Color.DarkGray, alt ? 9 : 10, 0);
                        }
                        
                        foreach (var point in region.OuterPoints.Positions.Where(p => map.Contains(p)))
                        {
                            map[point] = new RogueLikeCell(point, Color.DarkGoldenrod, Color.DarkGray, 178, 0, false, false);
                        }

                        var points = region.NorthBoundary.Positions;
                        var door = points[points.Count / 2];

                        map[door] = new RogueLikeCell(door, Color.DarkGoldenrod, Color.Black, 254, 0, true, false);
                        yield return null;
                    }
                }
            }
        }
    }
}