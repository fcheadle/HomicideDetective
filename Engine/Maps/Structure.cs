using GoRogue;
using SadConsole;
using SadConsole.Maps;
using System.Collections.Generic;
using System.Linq;
using Rectangle = GoRogue.Rectangle;

namespace Engine.Maps
{
    class Room : Area
    {
        internal readonly RoomTypes Type;
        
        internal Room(string name, Rectangle area, RoomTypes type) : base(
            name,
            new Coord(area.MaxExtentX, area.MaxExtentY),
            new Coord(area.MaxExtentX, area.MinExtentY),
            new Coord(area.MinExtentX, area.MinExtentY),
            new Coord(area.MinExtentX, area.MaxExtentY)
            )
        {
            OuterRect = area;
            InnerRect = area;
            Type = type;
        }
    }
    internal class Structure : Area
    {
        internal BasicMap Map;
        internal Coord Origin;
        Dictionary<string, Room> Rooms = new Dictionary<string, Room>();
        int _minRoomSize;
        int _maxRoomSize;
        internal readonly StructureTypes StructureType;
        internal int Left => Rooms.OrderBy(key => key.Value.Left).ToList().First().Value.Left;
        internal int Right => Rooms.OrderBy(key => key.Value.Right).ToList().Last().Value.Right;
        internal int Top => Rooms.OrderBy(key => key.Value.Top).ToList().First().Value.Top;
        internal int Bottom => Rooms.OrderBy(key => key.Value.Bottom).ToList().Last().Value.Bottom;

        internal string Address { get; set; }

        internal Structure(int width, int height, Coord origin, StructureTypes type): base(
            origin.X.ToString() + origin.Y.ToString(),
            new Coord(width, height) + origin,
            new Coord(width, 0) + origin,
            origin,
            new Coord(0,height) + origin
            )
        {
            StructureType = type;
            Map = new BasicMap(width, height, 1, Distance.MANHATTAN);
            Origin = origin;
            _minRoomSize = width / 9;
            _maxRoomSize = width / 4;
            Generate();
        }

        internal void Generate()
        {
            //FenceYard(new Rectangle(new Coord(1, 1), new Coord(Map.Width - 1, Map.Height - 1)));
            switch (StructureType)
            {
                case StructureTypes.Testing: CreateTestingStructure(); break;
                case StructureTypes.CentralPassageHouse: CreateCentralPassageHouse(); break;
                default: break;
            }

            //Map = Map.Subsection(new Coord(Left, Top), new Coord(Right, Bottom));            
        }

        private void CreateCentralPassageHouse()
        {
            int midX = Map.Width / 2;
            int midY = Map.Height / 2;
            int roomW;
            int roomH;
            Coord target;
            //FenceYard(new Rectangle(new Coord(1, 1), new Coord(Map.Width - 1, Map.Height - 1)));

            //central passage
            Coord start;
            Coord end;
            roomW = 6;
            roomH = 12;
            start = new Coord(midX - (roomW / 2), Map.Height - 4 - roomH);
            end = new Coord(midX + 2, Map.Height - 4);
            CreateRoom(start, end, "hallway", RoomTypes.Parlor, Direction.Types.NONE);
            Room hallway = Rooms["hallway"];

            //parlor
            roomW = RandomRoomDimension() * 2;
            roomH = hallway.OuterRect.Height / 2 - 1;
            start = new Coord(Left - roomW, Bottom - roomH);
            end = new Coord(Left, Bottom);
            CreateRoom(start, end, "parlor", RoomTypes.Parlor, Direction.Types.NONE);
            target = new Coord(Rooms["parlor"].Right, Rooms["parlor"].Bottom - 1);
            if (Map.Contains(target))
                Map.SetTerrain(TerrainFactory.Door(target));

            //master bed
            start = new Coord(Left, Rooms["parlor"].Top - roomH);
            end = new Coord(hallway.Left, Rooms["parlor"].Top);
            CreateRoom(start, end, "master_bed", RoomTypes.MasterBedroom, Direction.Types.RIGHT);
            target = new Coord(Rooms["parlor"].Right, Rooms["parlor"].Bottom - 1);
            if (Map.Contains(target))
                Map.SetTerrain(TerrainFactory.Door(new Coord(Rooms["master_bed"].Right, Rooms["master_bed"].Top +1)));

            //main bathroom
            roomW = 5;//RandomRoomDimension();
            roomH = 5;// RandomRoomDimension();
            start = new Coord(hallway.Left - roomW, Rooms["parlor"].Top - (roomH / 2));
            end = new Coord(hallway.Left, Rooms["parlor"].Top + (roomH / 2));
            CreateRoom(start, end, "bathroom", RoomTypes.Bathroom, Direction.Types.RIGHT);

            //dining room
            roomH = (Rooms["hallway"].OuterRect.Height - 3) / 2;
            roomW = roomH;
            start = new Coord(Rooms["hallway"].Right, Rooms["hallway"].Bottom - roomH - 1);
            end = new Coord(Rooms["hallway"].Right + roomW, Rooms["hallway"].Bottom);
            CreateRoom(start, end, "dining", RoomTypes.DiningRoom, Direction.Types.LEFT);

            //kitchen
            start = new Coord(Rooms["hallway"].Right, Rooms["dining"].Top - roomH + 1);
            end = new Coord(Rooms["hallway"].Right + roomW, Rooms["dining"].Top + 1);
            CreateRoom(start, end, "kitchen", RoomTypes.Kitchen, Direction.Types.LEFT);
            CreateArch(Rooms["dining"], Rooms["kitchen"]);

            //kids room 1
            roomH = (Rooms["hallway"].OuterRect.Height - 3) / 2;
            roomW = roomH;
            start = new Coord(Rooms["hallway"].Right, Rooms["kitchen"].Top - roomH + 1);
            end = new Coord(Rooms["hallway"].Right + roomW, Rooms["kitchen"].Top + 1);
            CreateRoom(start, end, "kids_bedroom_1", RoomTypes.Bedroom, Direction.Types.NONE);
            target = new Coord(Rooms["parlor"].Right, Rooms["parlor"].Bottom - 1);
            if (Map.Contains(target))
                Map.SetTerrain(TerrainFactory.Door(new Coord(Rooms["kids_bedroom_1"].Left, Rooms["kids_bedroom_1"].Bottom - 1)));

            //kids room 2
            start = new Coord(Rooms["hallway"].Left - roomW, Rooms["master_bed"].Top - roomH);
            end = new Coord(Rooms["hallway"].Left, Rooms["master_bed"].Top);
            CreateRoom(start, end, "kids_bedroom_2", RoomTypes.Bedroom, Direction.Types.NONE);
            target = new Coord(Rooms["parlor"].Right, Rooms["parlor"].Bottom - 1);
            if (Map.Contains(target))
                Map.SetTerrain(TerrainFactory.Door(new Coord(Rooms["kids_bedroom_2"].Right, Rooms["kids_bedroom_2"].Bottom - 1)));

        }
        private int RandomRoomDimension()
        {
            return (Settings.Random.Next(_minRoomSize, _maxRoomSize) + Settings.Random.Next(_minRoomSize, _maxRoomSize)) / 2;
        }
        private void CreateTestingStructure()
        {
            int midX = Map.Width / 2;
            int midY = Map.Height / 2;
            int roomW;
            int roomH;
            int offsetX;
            int offsetY;

            //Living Room
            roomW = _maxRoomSize;// Settings.Random.Next(_minRoomSize, _maxRoomSize);
            roomH = _maxRoomSize; // Settings.Random.Next(_minRoomSize, _maxRoomSize);
            offsetX = roomW / 2;
            offsetY = roomH / 2;
            Coord start = new Coord(midX - offsetX, midY - offsetY);
            Coord end = new Coord(midX + offsetX, midY + offsetY);
            CreateRoom(start, end, "parlor", RoomTypes.Parlor, Direction.Types.NONE);

            //Kids room
            roomW = RandomRoomDimension();
            roomH = RandomRoomDimension();
            start = new Coord(Rooms["parlor"].Left - roomW, midY - roomH);
            end = new Coord(Rooms["parlor"].Left, midY);
            CreateRoom(start, end, "kids_bedroom_1", RoomTypes.Bedroom, Direction.Types.RIGHT);

            //kids closet 1
            start = new Coord(Rooms["kids_bedroom_1"].Left, midY - 2);
            end = new Coord(Rooms["kids_bedroom_1"].Left + 2, midY);
            CreateRoom(start, end, "kids_closet_1", RoomTypes.Closet, Direction.Types.RIGHT);

            //other kids room
            start = new Coord(Rooms["parlor"].Left - roomW, midY + 2);
            end = new Coord(Rooms["parlor"].Left, midY + roomH + 2);
            CreateRoom(start, end, "kids_bedroom_2", RoomTypes.Bedroom, Direction.Types.RIGHT);

            //kids closet 
            start = new Coord(Rooms["kids_bedroom_2"].Left, midY + 2);
            end = new Coord(Rooms["kids_bedroom_2"].Left + 2, midY + 4);
            CreateRoom(start, end, "kids_closet_2", RoomTypes.Closet, Direction.Types.RIGHT);

            //central hallway
            start = new Coord(Rooms["kids_bedroom_1"].Left, midY);
            end = new Coord(Rooms["parlor"].Left + 3, midY + 2);
            CreateRoom(start, end, "hallway", RoomTypes.Parlor, Direction.Types.NONE);
            CreateArch(Rooms["parlor"], Rooms["hallway"]);

            //hall closet
            start = new Coord(Left, midY);
            end = new Coord(Left + 2, midY + 2);
            CreateRoom(start, end, "closet_1", RoomTypes.Closet, Direction.Types.RIGHT);

            //bathroom
            start = new Coord(Rooms["parlor"].Left, Rooms["parlor"].Top);
            end = new Coord(Rooms["parlor"].Left + _minRoomSize, midY - 1);
            CreateRoom(start, end, "guest_bathroom", RoomTypes.Bathroom, Direction.Types.DOWN);

            //kitchen
            roomW = RandomRoomDimension();
            roomH = RandomRoomDimension();
            start = new Coord(midX + 2, midY - roomH);
            end = new Coord(midX + 2 + roomW, midY);
            CreateRoom(start, end, "kitchen", RoomTypes.Kitchen, Direction.Types.DOWN, true);
            CreateArch(Rooms["parlor"], Rooms["kitchen"]);

            //dining
            roomW = RandomRoomDimension();
            roomH = RandomRoomDimension();
            start = new Coord(Rooms["kitchen"].Right - roomW, midY);
            end = new Coord(Rooms["kitchen"].Right, midY + roomH);
            CreateRoom(start, end, "dining", RoomTypes.DiningRoom, Direction.Types.NONE, true);
            CreateArch(Rooms["parlor"], Rooms["dining"]);

            //master bed
            roomW = RandomRoomDimension();
            roomH = RandomRoomDimension();
            start = new Coord(Rooms["kitchen"].Left - roomW, Rooms["parlor"].Top - roomH);
            end = new Coord(Rooms["kitchen"].Left, Rooms["parlor"].Top);
            CreateRoom(start, end, "master_bed", RoomTypes.MasterBedroom, Direction.Types.DOWN, true);
        }
        private void CreateArch(Room host, Room imposing)
        {
            List<Coord> walls = new List<Coord>();

            foreach (Coord c in Calculate.BorderLocations(imposing.OuterRect))
            {
                if(host.OuterRect.Contains(c))
                    walls.Add(c);
            }

            foreach(Coord arch in walls)
            {
                if (Map.Contains(arch))
                    Map.SetTerrain(TerrainFactory.HardwoodFloor(arch));
            }

            foreach(Coord c in Calculate.BorderLocations(host.OuterRect))
            {
                if (walls.Contains(c) && Map.Contains(c))
                    Map.SetTerrain(TerrainFactory.Wall(c));
            }
        }
        private void FenceYard(Rectangle yard)
        {
            int yw = yard.Width;
            int yh = yard.Height;
            Coord offset = new Coord(0, 0);
            yard = new Rectangle(offset, new Coord(yw / 2, yh / 2));
            List<Coord> perimeter = Calculate.BorderLocations(yard);

            //yard / fence
            foreach (Coord tile in perimeter)
            {
                Map.SetTerrain(TerrainFactory.Fence(tile));
            }

            Coord minExtentX = new Coord(yard.MinExtentX, yard.MinExtentY + (yard.Height / 2));
            Coord minExtentY = new Coord(yard.MinExtentX + (yard.Width / 2), yard.MinExtentY);
            Coord maxExtentX = new Coord(yard.MaxExtentX, yard.MinExtentY + (yard.Height / 2));
            Coord maxExtentY = new Coord(yard.MinExtentX + (yard.Width / 2), yard.MaxExtentY);

            Map.SetTerrain(TerrainFactory.FenceGate(minExtentX));
            Map.SetTerrain(TerrainFactory.FenceGate(maxExtentX));
            Map.SetTerrain(TerrainFactory.FenceGate(minExtentY));
            Map.SetTerrain(TerrainFactory.FenceGate(maxExtentY));
        }
        private void CreateRoom(Coord start, Coord end, string name, RoomTypes type, Direction.Types direction, bool replace = true)
        {
            Rectangle area = new Rectangle(start, end);
            Room room = new Room(name, area, type);
            for (int x = area.MinExtentX + 1; x < area.MaxExtentX; x++)
            {
                for (int y = area.MinExtentY + 1; y < area.MaxExtentY; y++)
                {
                    Coord location = new Coord(x, y);
                    if (Map.Contains(location))
                    {
                        BasicTerrain floor;
                        switch (type)
                        {
                            case RoomTypes.Closet:
                            case RoomTypes.MasterBedroom:
                            case RoomTypes.Bedroom: floor = TerrainFactory.Carpet(location); break;
                            case RoomTypes.Kitchen:
                            case RoomTypes.Bathroom: floor = TerrainFactory.Linoleum(location); break;
                            default: floor = TerrainFactory.HardwoodFloor(location); break;
                        }
                        if (replace || Map.GetTerrain<BasicTerrain>(location) == null)
                        {
                            Map.SetTerrain(TerrainFactory.Copy(floor, location));
                        }
                        else if (Map.GetTerrain<BasicTerrain>(location).IsWalkable == false)
                        {
                            Map.SetTerrain(TerrainFactory.Copy(floor, location));
                        }
                        
                    }
                }
            }

            foreach (Coord location in Calculate.BorderLocations(area))
            {
                if (Map.Contains(location))
                {
                    if (replace || Map.GetTerrain<BasicTerrain>(location) == null)
                    {
                        Map.SetTerrain(TerrainFactory.Wall(location));
                    }

                }
            }
                List<Coord> possible;
                Coord doorLoc = new Coord(-1, -1);
            switch (direction)
            {
                case Direction.Types.LEFT:
                    possible = Calculate.PointsAlongStraightLine(new Coord(room.Left, room.Top + 2), new Coord(room.Left, room.Bottom - 2)).ToList();
                    while (!Map.Contains(doorLoc))
                        doorLoc = possible.RandomItem();
                    Map.SetTerrain(TerrainFactory.Door(doorLoc));
                    break;
                case Direction.Types.DOWN:
                    possible = Calculate.PointsAlongStraightLine(new Coord(room.Left + 2, room.Bottom), new Coord(room.Right - 2, room.Bottom)).ToList();
                    while (!Map.Contains(doorLoc))
                        doorLoc = possible.RandomItem();
                    Map.SetTerrain(TerrainFactory.Door(doorLoc));
                    break;
                case Direction.Types.RIGHT:
                    possible = Calculate.PointsAlongStraightLine(new Coord(room.Right, room.Top + 2), new Coord(room.Right, room.Bottom - 2)).ToList();
                    while (!Map.Contains(doorLoc))
                        doorLoc = possible.RandomItem();
                    Map.SetTerrain(TerrainFactory.Door(doorLoc));
                    break;
                case Direction.Types.UP:
                    possible = Calculate.PointsAlongStraightLine(new Coord(room.Left + 2, room.Top), new Coord(room.Right - 2, room.Top)).ToList();
                    while (!Map.Contains(doorLoc))
                        doorLoc = possible.RandomItem();
                    Map.SetTerrain(TerrainFactory.Door(doorLoc));
                    break;
                default:
                    if(Map.Contains(new Coord(room.Left, (room.Top + room.Bottom) / 2))) Map.SetTerrain(TerrainFactory.Door(new Coord(room.Left, (room.Top + room.Bottom) / 2)));
                    if(Map.Contains(new Coord(room.Right, (room.Top + room.Bottom) / 2))) Map.SetTerrain(TerrainFactory.Door(new Coord(room.Right, (room.Top + room.Bottom) / 2)));
                    if(Map.Contains(new Coord((room.Left + room.Right) / 2, room.Bottom))) Map.SetTerrain(TerrainFactory.Door(new Coord((room.Left + room.Right) / 2, room.Top)));
                    if(Map.Contains(new Coord((room.Left + room.Right) / 2, room.Bottom))) Map.SetTerrain(TerrainFactory.Door(new Coord((room.Left + room.Right) / 2, room.Bottom)));
                    break;
            }

            Rooms.Add(room.Name, room);
        }
    }
}
