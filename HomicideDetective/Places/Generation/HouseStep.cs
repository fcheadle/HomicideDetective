using System;
using System.Collections.Generic;
using System.Linq;
using GoRogue;
using GoRogue.MapGeneration;
using SadRogue.Primitives;
using SadRogue.Primitives.GridViews;
using SadRogue.Integration;

namespace HomicideDetective.Places.Generation
{
    public class HouseStep : GenerationStep
    {
        private int _wallGlyph = 176;
        private int _floorPrimaryGlyph = 46;
        private int _floorSecondaryGlyph = 46;
        private int _doorGlyph = 254;
        private Color _wallColor = Color.Red;
        private Color _floorPrimaryColor = Color.Red;
        private Color _floorSecondaryColor = Color.Red;
        private readonly Color _backgroundColor = Color.Black;
        
        private int _horizontalRooms = 3;
        private int _verticalRooms = 3;
        private int _dimension = 7;

        private readonly List<Point> _connections = new List<Point>();
        
        protected override IEnumerator<object?> OnPerform(GenerationContext context)
        {
            var map = context.GetFirstOrNew<ISettableGridView<RogueLikeCell>>
                (() => new ArrayView<RogueLikeCell>(context.Width, context.Height), "house");
            var wallFloor = context.GetFirstOrNew(() => new ArrayView<bool>(context.Width, context.Height), "WallFloor");
            int houseSize = (_horizontalRooms + 1) * _dimension; 
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    SwitchColorTheme(i + j);
                    var bottomLeft = (i * houseSize + j * houseSize + 8, context.Height - _dimension - j * houseSize);
                    foreach (Region plot in CreateParallelogramHouse(context, bottomLeft))
                    {
                        foreach (var region in plot.SubRegions)
                        {
                            DrawRegion(region, map, wallFloor);
                            yield return null;
                        }
                    }
                    foreach (var point in _connections)
                        if(map.Contains(point))
                            DrawDoor(point, map, wallFloor);
                }
            }
        }

        private void DrawDoor(Point point, ISettableGridView<RogueLikeCell> map, ArrayView<bool> wallFloor)
        {
            map[point] = Door(point);
            wallFloor[point] = true;
            
        }

        private IEnumerable<Region> CreateParallelogramHouse(GenerationContext context, Point bottomLeft)
        {
            var house = new Region($"house {bottomLeft}", NorthWest(bottomLeft, _dimension * 3),
                NorthEast(bottomLeft, _dimension * 3), SouthEast(bottomLeft, _dimension * 3),
                bottomLeft);
            for (int i = 0; i < _horizontalRooms; i++)
            {
                for (int j = 0; j < _verticalRooms; j++)
                {
                    int x = bottomLeft.X + j * _dimension + i * _dimension;
                    int y = bottomLeft.Y - j * _dimension;
                    Point southWest = (x, y);
                    var region = new Region($"room of house {southWest}", NorthWest(southWest, _dimension),
                        NorthEast(southWest, _dimension), SouthEast(southWest, _dimension),
                        southWest);
                    house.AddSubRegion(region);

                    if (j == 0)
                        region.AddConnection(MiddlePoint(region.NorthBoundary));
                    else if (j == 2)
                        region.AddConnection(MiddlePoint(region.SouthBoundary));
                    else if (j == 1 && i == 1)
                        ConnectAllSides(region);
                }
            }
            
            CreateCentralHallWay(house);
            CreateEatingSpaces(house);
            TrimUnusedRooms(house);
            yield return house;
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
                !IsCorner(one, p));

            if(overlapping.Any())
            {
                var connectingPoint = MiddlePoint(new Area(overlapping));
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
                difference = (_dimension, 0);
                chance = r.Next(1, 101);
                switch (chance % 3)
                {
                    default:
                    case 0:
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
                difference = (-_dimension, _dimension);
                switch (chance % 3)
                {
                    default:
                    case 0:
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
            
            ConnectAllSides(region);
        }

        private void DrawRegion(Region region, ISettableGridView<RogueLikeCell> map, ArrayView<bool> wallFloor)
        {
            bool isEatingSpace = region.Name == "kitchen" || region.Name == "dining";
            //place floor tiles
            foreach (var point in region.InnerPoints.Where(map.Contains))
            {
                map[point] = isEatingSpace ? KitchenFloor(point) : Floor(point);
                wallFloor[point] = true;
            }

            //place walls on the outer points
            foreach (var point in region.OuterPoints.Where(map.Contains))
            {
                map[point] = Wall(point);
                wallFloor[point] = false;
            }

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

        private void SetDoor(ISettableGridView<RogueLikeCell> cellMap, ArrayView<bool> wallFloor, Point p)
        {
            if(cellMap.Contains(p))
            {
                cellMap[p] = Door(p);
                wallFloor[p] = true;
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
        private RogueLikeCell Floor(Point point) => new RogueLikeCell(point, (point.X + point.Y) % 2 == 1 ? _floorPrimaryColor : _floorSecondaryColor, _backgroundColor, point.Y % 2 == 1 ? _floorPrimaryGlyph : _floorSecondaryGlyph, 0);
        private RogueLikeCell KitchenFloor(Point point) => new RogueLikeCell(point, (point.X + point.Y) % 2 == 1 ? Color.DarkGray : Color.LightGray, _backgroundColor, '+', 0);
        private RogueLikeCell Door(Point point) => new RogueLikeCell(point, _wallColor, _backgroundColor, _doorGlyph, 0);

        private Point NorthEast(Point bottomLeft, int lengthOfSide) => bottomLeft + (lengthOfSide * 2, -lengthOfSide); 
        private Point NorthWest(Point bottomLeft, int lengthOfSide) => bottomLeft + (lengthOfSide, -lengthOfSide); 
        private Point SouthEast(Point bottomLeft, int lengthOfSide) => bottomLeft + (lengthOfSide, 0);
    }
}