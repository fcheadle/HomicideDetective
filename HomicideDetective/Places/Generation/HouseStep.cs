using System;
using System.Collections.Generic;
using System.Linq;
using GoRogue;
using GoRogue.MapGeneration;
using HomicideDetective.Words;
using SadRogue.Primitives;
using SadRogue.Primitives.GridViews;
using SadRogue.Integration.FieldOfView.Memory;

namespace HomicideDetective.Places.Generation
{
    public class HouseStep : GenerationStep
    {
        private int _wallGlyph = 176;
        private int _floorPrimaryGlyph = 46;
        private int _doorGlyph = 254;
        private Color _wallColor = Color.Red;
        private Color _floorPrimaryColor = Color.Red;
        private Color _floorSecondaryColor = Color.Red;
        private readonly Color _backgroundColor = Color.Black;
        private int _themeIndex;
        private int _horizontalRooms = 3;
        private int _verticalRooms = 3;
        private int _sideLength = 7;

        private readonly List<Point> _connections = new List<Point>();
        
        protected override IEnumerator<object?> OnPerform(GenerationContext context)
        {
            var map = context.GetFirstOrNew<ISettableGridView<MemoryAwareRogueLikeCell>>
                (() => new ArrayView<MemoryAwareRogueLikeCell>(context.Width, context.Height), Constants.GridViewTag);
            var block = context.GetFirstOrNew(() => MapGen.BaseRegion("City Block", context.Width, context.Height), Constants.RegionCollectionTag);
            
            int houseSize = (_horizontalRooms + 1) * _sideLength;
            var start = (5, map.Height - 5);
            var side = map.Width / 2;
            var overallPlot = new Region("total house area", NorthWest(start, side), NorthEast(start, side), SouthEast(start, side), start);
            for (int j = map.Height - 5; j > houseSize / 2; j -= houseSize) 
            {
                for (int i = map.Height - j; i < map.Width - houseSize * 1.25; i += houseSize)
                {
                    if(overallPlot.Contains((i,j)))
                    {
                        SwitchColorTheme();
                        var bottomLeft = (i, j);
                        foreach (Region plot in CreateParallelogramHouse(bottomLeft))
                        {
                            block.AddSubRegion(plot);
                            foreach (var region in plot.SubRegions)
                            {
                                DrawRegion(region, map);
                                yield return null;
                            }
                        }

                        foreach (var point in _connections)
                            if (map.Contains(point))
                                map[point] = Door(point);

                        _connections.Clear();
                    }
                }
            }
            
            MapGen.Finalize(map);
        }
        private IEnumerable<Region> CreateParallelogramHouse(Point bottomLeft)
        {
            var house = new Region($"house {bottomLeft}", NorthWest(bottomLeft, _sideLength * 3),
                NorthEast(bottomLeft, _sideLength * 3), SouthEast(bottomLeft, _sideLength * 3),
                bottomLeft);
            for (int i = 0; i < _horizontalRooms; i++)
            {
                for (int j = 0; j < _verticalRooms; j++)
                {
                    int x = bottomLeft.X + j * _sideLength + i * _sideLength;
                    int y = bottomLeft.Y - j * _sideLength;
                    Point southWest = (x, y);
                    var region = new Region($"room of house {southWest}", NorthWest(southWest, _sideLength),
                        NorthEast(southWest, _sideLength), SouthEast(southWest, _sideLength),
                        southWest);
                    house.AddSubRegion(region);

                    if (j == 0)
                        region.AddConnection(MapGen.MiddlePoint(region.NorthBoundary));
                    else if (j == 2)
                        region.AddConnection(MapGen.MiddlePoint(region.SouthBoundary));
                    else if (j == 1 && i == 1)
                        MapGen.ConnectAllSides(region);
                }
            }
            
            CreateCentralHallWay(house);
            CreateEatingSpaces(house);
            CreateBathroom(house);
            TrimUnusedRooms(house);
            yield return house;
        }

        private void CreateBathroom(Region house)
        {
            var hall = house.SubRegions.First(r => r.Name == "hall");
            var bathroom = house.SubRegions.RandomItem(r => r.Name != "hall" && r.Name != "dining" && r.Name != "kitchen" && hall.OuterPoints.Count(r.Contains) > 3);
            bathroom.Name = "bathroom";
        }

        private void TrimUnusedRooms(Region house)
        {
            var hall = house.SubRegions.First(r => r.Name == "hall");
            var kitchen = house.SubRegions.First(r => r.Name == "kitchen");
            var dining  = house.SubRegions.First(r => r.Name == "dining");
            var regionsToLose = house.SubRegions.Where(reg =>
                hall.OuterPoints.Count(reg.Contains) + kitchen.OuterPoints.Count(reg.Contains) +
                dining.OuterPoints.Count(reg.Contains) < 3).ToList();
            foreach(var room in regionsToLose)
                house.RemoveSubRegion(room.Name);
        }

        private void CreateEatingSpaces(Region house)
        {
            var hall = house.SubRegions.First(r => r.Name == "hall");
            var kitchen = house.SubRegions.RandomItem(r => r.Name != "hall"  && hall.OuterPoints.Count(r.Contains) > 3);
            kitchen.Name = "kitchen";
            
            var dining = house.SubRegions.RandomItem(r => r.Name != "hall" && r.Name != "kitchen" && r.OuterPoints.Where(kitchen.Contains).Count() > 2);
            dining.Name = "dining";
            ConnectRooms(kitchen, dining);
        }
        private void ConnectRooms(Region one, Region other)
        {
            var overlapping = one.OuterPoints.Where(p =>
                other.OuterPoints.Contains(p) && !IsCorner(other, p) &&
                !IsCorner(one, p)).ToList();

            if(overlapping.Any())
            {
                var connectingPoint = MapGen.MiddlePoint(new Area(overlapping));
                one.AddConnection(connectingPoint);
                other.AddConnection(connectingPoint);
            }
        }
        private bool IsCorner(Region region, Point point)
        {
            return point == region.NorthEastCorner || point == region.NorthWestCorner ||
                   point == region.SouthEastCorner || point == region.SouthWestCorner;
        }
        private void CreateCentralHallWay(Region house)
        {
            var r = new Random();
            var chance = r.Next(1, 101);
            Point southEast;
            Point southwest;
            Point northEast;
            Point northwest;
            Point difference;
            
            if (chance % 2 == 0)
            {
                //vertically-oriented central hall
                difference = (_sideLength, 0);
                chance = r.Next(1, 101);
                switch (chance % 3)
                {
                    default:
                        southEast = house.SouthEastCorner;
                        southwest = house.SouthEastCorner - difference;
                        northEast = house.NorthEastCorner;
                        northwest = house.NorthEastCorner - difference;
                        break;
                    case 1:
                        southEast = house.SouthWestCorner + difference;
                        southwest = house.SouthWestCorner;
                        northEast = house.NorthWestCorner + difference;
                        northwest = house.NorthWestCorner;
                        break;
                    case 2:
                        southEast = house.SouthEastCorner - difference;
                        southwest = house.SouthWestCorner + difference;
                        northEast = house.NorthEastCorner - difference;
                        northwest = house.NorthWestCorner + difference;
                        break;
                }
            }
            else
            {
                //horizontally-aligned central hall
                chance = r.Next(1, 101);
                difference = (-_sideLength, _sideLength);
                switch (chance % 3)
                {
                    default:
                        southEast = house.SouthEastCorner;
                        southwest = house.SouthWestCorner;
                        northEast = house.SouthEastCorner - difference;
                        northwest = house.SouthWestCorner - difference;
                        break;
                    case 1:
                        southEast = house.SouthEastCorner - difference;
                        southwest = house.SouthWestCorner - difference;
                        northEast = house.NorthEastCorner + difference;
                        northwest = house.NorthWestCorner + difference;
                        break;
                    case 2:
                        southEast = house.NorthEastCorner + difference;
                        southwest = house.NorthWestCorner + difference;
                        northEast = house.SouthEastCorner - difference;
                        northwest = house.SouthWestCorner - difference;
                        break;
                }
            }
            
            var region = new Region("hall", northwest, northEast, southEast, southwest);
            house.AddSubRegion(region);

            var regionsToLose = house.SubRegions.Where(reg => region.InnerPoints.Contains(reg.InnerPoints) && region != reg).ToList();
            foreach(var reg in regionsToLose)
                house.RemoveSubRegion(reg.Name);
            
            MapGen.ConnectAllSides(region);
        }
        private void DrawRegion(Region region, ISettableGridView<MemoryAwareRogueLikeCell> map)
        {
            bool isEatingSpace = region.Name == "kitchen" || region.Name == "dining";
            bool isBathroom = region.Name == "bathroom";
            
            foreach (var point in region.InnerPoints.Where(map.Contains))
                map[point] = isEatingSpace ? KitchenFloor(point) : isBathroom ? BathroomFloor(point) : Floor(point);
            
            foreach (var point in region.OuterPoints.Where(map.Contains))
                map[point] = Wall(point);

            foreach (var point in region.Connections)
                _connections.Add(point);
        }
        
        private void SwitchColorTheme()
        {
            _themeIndex++;
            switch (_themeIndex % 4)
            {
                default: SetWhiteTheme(); break;
                case 1: SetBrownTheme(); break;
                case 2: SetRedTheme(); break;
                case 3: SetYellowTheme(); break;
            }
        }
        private void SetYellowTheme()
        {
            _wallColor = Color.Goldenrod;
            _floorPrimaryColor = Color.DarkGoldenrod;
            _floorSecondaryColor = Color.DarkKhaki;
        }
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
        private MemoryAwareRogueLikeCell Wall(Point point) => new MemoryAwareRogueLikeCell(point, _wallColor, _backgroundColor, _wallGlyph, 0, false, false);
        private MemoryAwareRogueLikeCell Floor(Point point) => new MemoryAwareRogueLikeCell(point, (point.X + point.Y) % 2 == 1 ? _floorPrimaryColor : _floorSecondaryColor, _backgroundColor, _floorPrimaryGlyph, 0);
        private MemoryAwareRogueLikeCell KitchenFloor(Point point) => new MemoryAwareRogueLikeCell(point, (point.X + point.Y) % 2 == 1 ? _floorPrimaryColor : _floorSecondaryColor, _backgroundColor, '+', 0);
        private MemoryAwareRogueLikeCell Door(Point point) => new MemoryAwareRogueLikeCell(point, _wallColor, _backgroundColor, _doorGlyph, 0, true, false);
        private MemoryAwareRogueLikeCell BathroomFloor(Point point) => new MemoryAwareRogueLikeCell(point, (point.X + point.Y) % 2 == 1 ? _floorPrimaryColor : _floorSecondaryColor, _backgroundColor, 4, 0);
        private Point NorthEast(Point bottomLeft, int lengthOfSide) => bottomLeft + (lengthOfSide * 2, -lengthOfSide); 
        private Point NorthWest(Point bottomLeft, int lengthOfSide) => bottomLeft + (lengthOfSide, -lengthOfSide); 
        private Point SouthEast(Point bottomLeft, int lengthOfSide) => bottomLeft + (lengthOfSide, 0);
    }
}