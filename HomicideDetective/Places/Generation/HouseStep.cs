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
        private readonly int _horizontalRooms = 3;
        private readonly int _verticalRooms = 3;
        private readonly int _sideLength = 7;
        private readonly int _horizontalStreetIndex;
        private readonly int _verticalStreetIndex;
        private readonly List<Point> _connections = new ();

        public HouseStep()
        {
            _horizontalStreetIndex = 0;
            _verticalStreetIndex = 0;
        }
        public HouseStep(int horizontalStreetIndex, int verticalStreetIndex)
        {
            _horizontalStreetIndex = horizontalStreetIndex;
            _verticalStreetIndex = verticalStreetIndex;
        }
        
        protected override IEnumerator<object?> OnPerform(GenerationContext context)
        {
            var map = context.GetFirstOrNew<ISettableGridView<MemoryAwareRogueLikeCell>>
                (() => new ArrayView<MemoryAwareRogueLikeCell>(context.Width, context.Height), Constants.GridViewTag);
            var blockArea = PolygonArea.Rectangle(new Rectangle((0, 0), (context.Width, context.Height)));
            var blockName = $"{_horizontalStreetIndex}00 block {(RoadNames)_verticalStreetIndex} street";
            var block = context.GetFirstOrNew(
                () => new Place(blockArea, blockName, Constants.BlockDescription, Constants.BlockNouns,
                    Constants.ItemPronouns, new PhysicalProperties(0, 0)), Constants.RegionCollectionTag);
            
            int houseSize = (_horizontalRooms + 1) * _sideLength;
            bool top = false;
            for (int j = map.Height - 5; j > houseSize / 2; j -= houseSize)
            {
                int addressOffset = 5;
                for (int i = map.Height - j; i < map.Width - houseSize /* * 1.25*/; i += houseSize)
                {
                    var address = ChooseAddress(addressOffset, top);
                    var house = CreateAndDrawHouse(i, j, address, map);
                    block.AddSubRegion(house);
                    addressOffset += 2;
                    yield return null;
                }
                top = !top;
            }
            
            MapGen.Finalize(map);
        }

        private string ChooseAddress(int addressOffset, bool top)
        {
            var offset = addressOffset < 10 ? $"{_horizontalStreetIndex}0{addressOffset}" : $"{_horizontalStreetIndex}{addressOffset}";
            string street;
            if (top)
                street = $"{(RoadNames)_verticalStreetIndex} street";
            else
                street = $"{(RoadNames)(_verticalStreetIndex + 1)} street";
            return $"{offset} {street}";
        }

        private Place CreateAndDrawHouse(int x, int y, string address, ISettableGridView<MemoryAwareRogueLikeCell> map)
        {                    
            SwitchColorTheme();
            var house = CreateParallelogramHouse((x,y), address);
            foreach(var room in house.SubAreas)
                DrawRegion((Place)room, map);//safe for me

            foreach (var point in _connections)
                if (map.Contains(point))
                    map[point] = Door(point);

            _connections.Clear();
            return house;
        }

        private Place CreateParallelogramHouse(Point bottomLeft, string name)
        {
            var house = new Place(PolygonArea.Parallelogram(bottomLeft, _sideLength * 3, _sideLength * 3), name,
                Constants.HouseDescription, MapGen.HouseNouns(bottomLeft.X + bottomLeft.Y), Constants.ItemPronouns, new PhysicalProperties(0, 0));

            for (int i = 0; i < _horizontalRooms; i++)
            {
                for (int j = 0; j < _verticalRooms; j++)
                {
                    int x = bottomLeft.X + j * _sideLength + i * _sideLength;
                    int y = bottomLeft.Y - j * _sideLength;
                    Point southWest = (x, y);
                    var region = PolygonArea.Parallelogram(southWest, _sideLength, _sideLength);
                    var room = new Place(region, Constants.RoomSubstantive);
                    house.AddSubRegion(room);
                    ConnectOnAllSides(room);
                }
            }
            
            CreateCentralHallWay(house);
            CreateEatingSpaces(house);
            CreateBathroom(house);
            TrimUnusedRooms(house);
            return house;
        }

        private void ConnectOnAllSides(Place room)
        {
            foreach(var boundary in room.Area.OuterPoints.SubAreas)
                room.AddConnection(boundary[boundary.Count / 2]);
        }

        private void CreateBathroom(Place house)
        {
            var bathroom = house.SubAreas.RandomItem(r => r.Name != "hall" && r.Name != "dining" && r.Name != "kitchen");
            bathroom.Name = "bathroom";
        }

        private void TrimUnusedRooms(Place house)
        {
            var hall = house.SubAreas.First(r => r.Name == "hall");
            var kitchen = house.SubAreas.First(r => r.Name == "kitchen");
            var dining  = house.SubAreas.First(r => r.Name == "dining");
            var regionsToLose = house.SubAreas.Where(reg =>
                hall.Area.OuterPoints.Count(reg.Area.Contains) + kitchen.Area.OuterPoints.Count(reg.Area.Contains) +
                dining.Area.OuterPoints.Count(reg.Area.Contains) < 3).ToList();
            foreach(var room in regionsToLose)
                house.RemoveSubRegion(room);
        }

        private void CreateEatingSpaces(Place house)
        {
            var hall = house.SubAreas.First(r => r.Name == "hall");
            var kitchen = house.SubAreas.RandomItem(r => r.Name != "hall"  && hall.Area.OuterPoints.Count(r.Area.Contains) > 3);
            kitchen.Name = "kitchen";
            kitchen.Description = "A room used for preparing food";
            
            var dining = house.SubAreas.RandomItem(r => r.Name != "hall" && r.Name != "kitchen" && kitchen.Area.OuterPoints.Count(r.Area.Contains) > 3);
            dining.Name = "dining";
            ConnectRooms(kitchen, dining);
        }
        private void ConnectRooms(Place one, Place other)
        {
            var overlapping = one.Area.OuterPoints.Where(p =>
                other.Area.OuterPoints.Contains(p) && !other.Area.Corners.Contains(p) &&
                !one.Area.Corners.Contains(p)).ToList();

            if(overlapping.Any())
            {
                var connectingPoint = MapGen.MiddlePoint(new Area(overlapping));
                one.AddConnection(connectingPoint);
                other.AddConnection(connectingPoint);
            }
        }
        
        private void CreateCentralHallWay(Place house)
        {
            var r = new Random();
            Point southwest = (house.Area.Left, house.Area.Bottom);
            int width = _sideLength;
            int height = _sideLength;
            Point offset;

            var chance = r.Next(1, 101);
            if (chance % 2 == 0) //vertically-oriented central hall
            {
                width *= 3;
                offset = (_sideLength, 0);
            }

            else //horizontally-aligned central hall
            {
                height *= 3;
                offset = (_sideLength, -_sideLength);
            }
            
            chance = r.Next(3);
            southwest += offset * chance;

            var region = PolygonArea.Parallelogram(southwest, width, height);
            var hallway = Constants.HallSubstantive;
            var place = new Place(region, hallway);
            house.AddSubRegion(place);

            var regionsToLose = house.SubAreas.Where(reg => region.InnerPoints.Contains(reg.Area.InnerPoints) && place != reg).ToList();
            foreach(var reg in regionsToLose)
                house.RemoveSubRegion(reg);
            
            ConnectOnAllSides(place);
        }
        private void DrawRegion(Place region, ISettableGridView<MemoryAwareRogueLikeCell> map)
        {
            bool isEatingSpace = region.Name == "kitchen" || region.Name == "dining";
            bool isBathroom = region.Name == "bathroom";
            
            foreach (var point in region.Area.InnerPoints.Where(map.Contains))
                map[point] = isEatingSpace ? KitchenFloor(point) : isBathroom ? BathroomFloor(point) : Floor(point);
            
            foreach (var point in region.Area.OuterPoints.Where(map.Contains))
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
        private MemoryAwareRogueLikeCell Wall(Point point) 
            => new (point, _wallColor, _backgroundColor, _wallGlyph, 0, false, false);
        private MemoryAwareRogueLikeCell Floor(Point point) 
            => new (point, (point.X + point.Y) % 2 == 1 ? _floorPrimaryColor : _floorSecondaryColor, _backgroundColor, _floorPrimaryGlyph, 0);
        private MemoryAwareRogueLikeCell KitchenFloor(Point point) 
            => new (point, (point.X + point.Y) % 2 == 1 ? _floorPrimaryColor : _floorSecondaryColor, _backgroundColor, '+', 0);
        private MemoryAwareRogueLikeCell Door(Point point) 
            => new (point, _wallColor, _backgroundColor, _doorGlyph, 0, true, false);
        private MemoryAwareRogueLikeCell BathroomFloor(Point point) 
            => new (point, (point.X + point.Y) % 2 == 1 ? _floorPrimaryColor : _floorSecondaryColor, _backgroundColor, 4, 0);
    }
}