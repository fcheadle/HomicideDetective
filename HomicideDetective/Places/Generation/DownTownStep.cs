using System;
using System.Collections.Generic;
using System.Linq;
using GoRogue;
using GoRogue.MapGeneration;
using SadRogue.Integration;
using SadRogue.Primitives;
using SadRogue.Primitives.GridViews;

namespace HomicideDetective.Places.Generation
{
    public class DownTownStep : GenerationStep
    {
        private int _wallGlyph = 176;
        private int _floorGlyph = 46;
        private int _doorGlyph = 254;
        private Color _wallColor = Color.Red;
        private Color _floorPrimaryColor = Color.Red;
        private Color _floorSecondaryColor = Color.Red;
        private readonly Color _backgroundColor = Color.Black;

        private int _shortLength = 4;
        private int _longLength = 10;
        private int _horizontalRooms = 3;
        private int _verticalRooms = 3;
        private int _dimension = 7;

        private readonly List<Point> _connections = new List<Point>();
        
        protected override IEnumerator<object?> OnPerform(GenerationContext context)
        {
            var map = context.GetFirstOrNew<ISettableGridView<RogueLikeCell>>
                (() => new ArrayView<RogueLikeCell>(context.Width, context.Height), "downtown");
            
            var shops = context.GetFirstOrNew(() => new List<Region>(), "shops");

            var blockWidth = map.Width / 2;
            var blockHeight = blockWidth / 2;
            for (int i = 0; i < blockWidth; i += _longLength)
            {
                for (int j = 0; j < blockHeight - _longLength; j += _longLength)
                {
                    SwitchColorTheme(i + j);
                    var bottomLeft = (i * _longLength + j * _longLength + 8, context.Height - _dimension - j * _longLength);
                    foreach (Region plot in CreateParallelogramDownTownBlock(bottomLeft))
                    {
                        shops.Add(plot);
                        DrawRegion(plot, map);
                        foreach (var region in plot.SubRegions)
                            DrawRegion(region, map);
                    }
                    foreach (var point in _connections)
                        if (map.Contains(point))
                            map[i,j] = Door((i,j));
                    
                    yield return null;
                }
            }
        }
        
        private IEnumerable<Region> CreateParallelogramDownTownBlock(Point bottomLeft)
        {
            yield return TallParallelogram(bottomLeft);
        }

        private void CreateBathrooms(Region house)
        {
            var hall = house.SubRegions.First(r => r.Name == "hall");
            var mensBathroom = house.SubRegions.RandomItem(r => r.Name != "hall" && hall.OuterPoints.Count(r.Contains) > 3);
            mensBathroom.Name = "men's bathroom";
            var womensBathroom = house.SubRegions.RandomItem(r =>
                r.Name != "hall" && hall.OuterPoints.Count(r.Contains) > 3 && mensBathroom.OuterPoints.Count(r.Contains) > 3);
            womensBathroom.Name = "women's bathroom";
        }

        private void TrimUnusedRooms(Region house)
        {
            var hall = house.SubRegions.First(r => r.Name == "hall");
            var regionsToLose = house.SubRegions.Where(reg =>
                hall.OuterPoints.Count(reg.Contains) < 3).ToList();
            foreach(var room in regionsToLose)
                house.RemoveSubRegion(room.Name);
        }
        
        private void ConnectRooms(Region one, Region other)
        {
            if(one.OuterPoints.Any(p =>
                other.OuterPoints.Contains(p) && !IsCorner(other, p) &&
                !IsCorner(one, p)))
            {
                var connectingPoint = MiddlePoint(new Area(one.OuterPoints.Where(p =>
                    other.OuterPoints.Contains(p) && !IsCorner(other, p) &&
                    !IsCorner(one, p))));
                one.AddConnection(connectingPoint);
                other.AddConnection(connectingPoint);
            }
        }
        
        private bool IsCorner(Region region, Point point)
        {
            return point == region.NorthEastCorner || point == region.NorthWestCorner ||
                   point == region.SouthEastCorner || point == region.SouthWestCorner;
        }

        private void CreateTallCentralHallway(Region office)
        {
            
        }

        private void CreateWideCentralHallway(Region office)
        {
            
        }
        
        
        private Region ShortParallelogram(Point bottomLeft)
            => new Region("shop room", bottomLeft + (_shortLength, -_shortLength), bottomLeft + (_longLength + _shortLength, -_shortLength), bottomLeft + (_longLength, 0), bottomLeft);
        
        private Region TallParallelogram(Point bottomLeft)
            => new Region("shop room", bottomLeft + (_longLength, -_longLength), bottomLeft + (_longLength + _shortLength, -_longLength), bottomLeft + (_shortLength, 0), bottomLeft);
        
        private void DrawRegion(Region region, ISettableGridView<RogueLikeCell> map)
        {
            bool isBathroom = region.Name == "bathroom";
            
            //place floor tiles
            foreach (var point in region.InnerPoints.Where(map.Contains))
                map[point] = isBathroom ? BathroomFloor(point) : Floor(point);

            //place walls on the outer points
            foreach (var point in region.OuterPoints.Where(map.Contains))
                map[point] = Wall(point);

            foreach (var point in region.Connections)
                _connections.Add(point);
            
        }
        private void SwitchColorTheme(int index)
        {
            switch (index % 4)
            {
                default:
                case 0: SetWhiteTheme(); break;
                case 1: SetBrownTheme(); break;
                case 2: SetRedTheme(); break;
            }
        }
        private void ConnectAllSides(Region region)
        {
            region.AddConnection(MiddlePoint(region.WestBoundary));
            region.AddConnection(MiddlePoint(region.EastBoundary));
            region.AddConnection(MiddlePoint(region.NorthBoundary));
            region.AddConnection(MiddlePoint(region.SouthBoundary));
        }
        private Point MiddlePoint(IReadOnlyArea area) => area[area.Count() / 2];
        private void SetRedTheme()
        {
            _wallColor = Color.DarkRed;
            _floorPrimaryColor = Color.DarkRed;
            _floorSecondaryColor = Color.Brown;
        }
        private void SetBrownTheme()
        {
            _wallColor = Color.Brown;
            _floorPrimaryColor = Color.Brown;
            _floorSecondaryColor = Color.DarkRed;
        }
        private void SetWhiteTheme()
        {
            _wallColor = Color.White;
            _floorPrimaryColor = Color.DarkGoldenrod;
            _floorSecondaryColor = Color.DarkGray;
        }
        private RogueLikeCell Wall(Point point) => new RogueLikeCell(point, _wallColor, _backgroundColor, _wallGlyph, 0, false, false);
        private RogueLikeCell Floor(Point point) => new RogueLikeCell(point, (point.X + point.Y) % 2 == 1 ? _floorPrimaryColor : _floorSecondaryColor, _backgroundColor, _floorGlyph, 0);
        private RogueLikeCell Door(Point point) => new RogueLikeCell(point, _wallColor, _backgroundColor, _doorGlyph, 0);
        private RogueLikeCell BathroomFloor(Point point) => new RogueLikeCell(point, (point.X + point.Y) % 2 == 1 ? _floorPrimaryColor : _floorSecondaryColor, _backgroundColor, 4, 0);
    }
}