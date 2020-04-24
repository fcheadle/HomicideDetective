using GoRogue;
using SadConsole;
using SadConsole.Maps;
using System.Collections.Generic;
using System.Linq;
using Rectangle = GoRogue.Rectangle;

namespace Engine.Maps
{
    internal class Structure : Area
    {
        internal BasicMap Map;
        new internal Coord Origin;
        internal Dictionary<RoomTypes, Room> Rooms = new Dictionary<RoomTypes, Room>();
        int _minRoomSize;
        int _maxRoomSize;
        internal readonly StructureTypes StructureType;

        internal string Address { get; set; }

        internal Structure(int width, int height, Coord origin, StructureTypes type, string name = null): base(
            name ?? origin.X.ToString() + origin.Y.ToString(),
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
            switch (StructureType)
            {
                case StructureTypes.Testing: CreateTestingStructure(); break;
                case StructureTypes.CentralPassageHouse: CreateCentralPassageHouse(); break;
                default: break;
            }        
        }

        private void CreateCentralPassageHouse()
        {
            int midX = Map.Width / 2;
            int midY = Map.Height / 2;
            int roomW;
            int roomH;
            Coord target;

            //central passage
            Coord start;
            Coord end;
            roomW = 6;
            roomH = 12;
            start = new Coord(midX - (roomW / 2), Map.Height - 4 - roomH);
            end = new Coord(midX + (roomW / 2), Map.Height - 4);
            CreateRoom(start, end, "Hallway", RoomTypes.Hall, Direction.Types.NONE);
            Room hallway = Rooms[RoomTypes.Hall];

            //parlor
            roomW = RandomRoomDimension() * 2;
            roomH = hallway.OuterRect.Height / 2 - 1;
            start = new Coord(hallway.Left - roomW, hallway.Bottom - roomH);
            end = new Coord(hallway.Left, hallway.Bottom);
            CreateRoom(start, end, "Parlor", RoomTypes.Parlor, Direction.Types.NONE);
            Room parlor = Rooms[RoomTypes.Parlor];
            target = new Coord(parlor.Right, parlor.Bottom - 1);
            Map.SetTerrain(TerrainFactory.Door(target));

            //master bed
            start = new Coord(parlor.Left, parlor.Top - roomH);
            end = new Coord(hallway.Left, parlor.Top);
            CreateRoom(start, end, "Master Bedroom", RoomTypes.MasterBedroom, Direction.Types.RIGHT);
            Room masterBed = Rooms[RoomTypes.MasterBedroom];
            target = new Coord(parlor.Right, parlor.Bottom - 1);
            Map.SetTerrain(TerrainFactory.Door(new Coord(masterBed.Right, masterBed.Top + 1)));

            //main bathroom
            roomW = 5;
            roomH = 5;
            start = new Coord(hallway.Left - roomW, parlor.Top - (roomH / 2));
            end = new Coord(hallway.Left, parlor.Top + (roomH / 2));
            CreateRoom(start, end, "Bathroom", RoomTypes.Bathroom, Direction.Types.RIGHT);

            //dining room
            roomH = (parlor.OuterRect.Height - 3) / 2;
            roomW = roomH;
            start = new Coord(hallway.Right, hallway.Bottom - roomH - 1);
            end = new Coord(hallway.Right + roomW, hallway.Bottom);
            CreateRoom(start, end, "Dining", RoomTypes.DiningRoom, Direction.Types.LEFT);

            //kitchen
            start = new Coord(hallway.Right, hallway.Top - roomH + 1);
            end = new Coord(hallway.Right + roomW, hallway.Top + 1);
            CreateRoom(start, end, "Kitchen", RoomTypes.Kitchen, Direction.Types.LEFT);
            CreateArch(Rooms[RoomTypes.DiningRoom], Rooms[RoomTypes.Kitchen]);

            //kids room 1
            roomH = (hallway.OuterRect.Height - 3) / 2;
            roomW = roomH;
            start = new Coord(hallway.Right, hallway.Top - roomH + 1);
            end = new Coord(hallway.Right + roomW, hallway.Top + 1);
            CreateRoom(start, end, "kids_bedroom_1", RoomTypes.BoysBedroom, Direction.Types.NONE);
            target = new Coord(parlor.Right, parlor.Bottom - 1);
            Map.SetTerrain(TerrainFactory.Door(new Coord(Rooms[RoomTypes.BoysBedroom].Left, Rooms[RoomTypes.BoysBedroom].Bottom - 1)));

            //kids room 2
            start = new Coord(hallway.Left - roomW, masterBed.Top - roomH);
            end = new Coord(hallway.Left, hallway.Top);
            CreateRoom(start, end, "kids_bedroom_2", RoomTypes.GirlsBedroom, Direction.Types.NONE);
            target = new Coord(Rooms[RoomTypes.Parlor].Right, Rooms[RoomTypes.Parlor].Bottom - 1);
            Map.SetTerrain(TerrainFactory.Door(new Coord(Rooms[RoomTypes.GirlsBedroom].Right, Rooms[RoomTypes.GirlsBedroom].Bottom - 1)));
        }
        private int RandomRoomDimension()
        {
            return (Settings.Random.Next(_minRoomSize, _maxRoomSize) + Settings.Random.Next(_minRoomSize, _maxRoomSize)) / 2;
        }
        private void CreateTestingStructure()
        {
            //int midX = Map.Width / 2;
            //int midY = Map.Height / 2;
            //int roomW;
            //int roomH;
            //int offsetX;
            //int offsetY;

            ////Living Room
            //roomW = _maxRoomSize;// Settings.Random.Next(_minRoomSize, _maxRoomSize);
            //roomH = _maxRoomSize; // Settings.Random.Next(_minRoomSize, _maxRoomSize);
            //offsetX = roomW / 2;
            //offsetY = roomH / 2;
            //Coord start = new Coord(midX - offsetX, midY - offsetY);
            //Coord end = new Coord(midX + offsetX, midY + offsetY);
            //CreateRoom(start, end, "parlor", RoomTypes.Parlor, Direction.Types.NONE);

            ////Kids room
            //roomW = RandomRoomDimension();
            //roomH = RandomRoomDimension();
            //start = new Coord(Rooms[RoomTypes.Parlor].Left - roomW, midY - roomH);
            //end = new Coord(Rooms["parlor"].Left, midY);
            //CreateRoom(start, end, "kids_bedroom_1", RoomTypes.BoysBedroom, Direction.Types.RIGHT);

            ////kids closet 1
            //start = new Coord(Rooms["kids_bedroom_1"].Left, midY - 2);
            //end = new Coord(Rooms["kids_bedroom_1"].Left + 2, midY);
            //CreateRoom(start, end, "kids_closet_1", RoomTypes.Closet, Direction.Types.RIGHT);

            ////other kids room
            //start = new Coord(Rooms["parlor"].Left - roomW, midY + 2);
            //end = new Coord(Rooms["parlor"].Left, midY + roomH + 2);
            //CreateRoom(start, end, "kids_bedroom_2", RoomTypes.BoysBedroom, Direction.Types.RIGHT);

            ////kids closet 
            //start = new Coord(Rooms["kids_bedroom_2"].Left, midY + 2);
            //end = new Coord(Rooms["kids_bedroom_2"].Left + 2, midY + 4);
            //CreateRoom(start, end, "kids_closet_2", RoomTypes.Closet, Direction.Types.RIGHT);

            ////central hallway
            //start = new Coord(Rooms["kids_bedroom_1"].Left, midY);
            //end = new Coord(Rooms["parlor"].Left + 3, midY + 2);
            //CreateRoom(start, end, "hallway", RoomTypes.Parlor, Direction.Types.NONE);
            //CreateArch(Rooms["parlor"], Rooms["hallway"]);

            ////hall closet
            //start = new Coord(Left, midY);
            //end = new Coord(Left + 2, midY + 2);
            //CreateRoom(start, end, "closet_1", RoomTypes.Closet, Direction.Types.RIGHT);

            ////bathroom
            //start = new Coord(Rooms["parlor"].Left, Rooms["parlor"].Top);
            //end = new Coord(Rooms["parlor"].Left + _minRoomSize, midY - 1);
            //CreateRoom(start, end, "guest_bathroom", RoomTypes.Bathroom, Direction.Types.DOWN);

            ////kitchen
            //roomW = RandomRoomDimension();
            //roomH = RandomRoomDimension();
            //start = new Coord(midX + 2, midY - roomH);
            //end = new Coord(midX + 2 + roomW, midY);
            //CreateRoom(start, end, "kitchen", RoomTypes.Kitchen, Direction.Types.DOWN, true);
            //CreateArch(Rooms["parlor"], Rooms["kitchen"]);

            ////dining
            //roomW = RandomRoomDimension();
            //roomH = RandomRoomDimension();
            //start = new Coord(Rooms["kitchen"].Right - roomW, midY);
            //end = new Coord(Rooms["kitchen"].Right, midY + roomH);
            //CreateRoom(start, end, "dining", RoomTypes.DiningRoom, Direction.Types.NONE, true);
            //CreateArch(Rooms["parlor"], Rooms["dining"]);

            ////master bed
            //roomW = RandomRoomDimension();
            //roomH = RandomRoomDimension();
            //start = new Coord(Rooms["kitchen"].Left - roomW, Rooms["parlor"].Top - roomH);
            //end = new Coord(Rooms["kitchen"].Left, Rooms["parlor"].Top);
            //CreateRoom(start, end, "master_bed", RoomTypes.MasterBedroom, Direction.Types.DOWN, true);
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
                            case RoomTypes.BoysBedroom:
                            case RoomTypes.GirlsBedroom:
                            case RoomTypes.Closet:
                            case RoomTypes.MasterBedroom:
                            case RoomTypes.GuestBedroom: floor = TerrainFactory.Carpet(location); break;
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

            Rooms.Add(room.Type, room);
        }
    }
}
