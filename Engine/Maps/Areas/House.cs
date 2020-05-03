using Engine.Entities;
using Engine.Extensions;
using GoRogue;
using SadConsole;
using System.Collections.Generic;
using Rectangle = GoRogue.Rectangle;

namespace Engine.Maps
{
    internal class House : Area
    {
        internal BasicMap Map;
        new internal Coord Origin;
        internal Dictionary<RoomTypes, Room> Rooms = new Dictionary<RoomTypes, Room>();
        private const int _minRoomSize = 4;
        private const int _maxRoomSize = 9;
        private readonly HouseTypes StructureType;
        private readonly Direction.Types _facing;
        internal Room Parlor { get => Rooms[RoomTypes.Parlor]; }
        internal Room Hallway { get => Rooms[RoomTypes.Hall]; }
        internal Room Kitchen { get => Rooms[RoomTypes.Kitchen]; }
        internal Room GuestBathroom { get => Rooms[RoomTypes.GuestBathroom]; }
        internal Room MasterBedroom { get => Rooms[RoomTypes.MasterBedroom]; }
        internal Room MasterBathroom { get => Rooms[RoomTypes.MasterBathroom]; }
        internal Room MasterBedCloset { get => Rooms[RoomTypes.MasterBedCloset]; }
        internal Room BoysBedroom { get => Rooms[RoomTypes.BoysBedroom]; }
        internal Room BoysCloset { get => Rooms[RoomTypes.BoysCloset]; }
        internal Room GirlsBedroom { get => Rooms[RoomTypes.GirlsBedroom]; }
        internal Room GirlsCloset { get => Rooms[RoomTypes.GirlsCloset]; }
        internal Room DiningRoom { get => Rooms[RoomTypes.DiningRoom]; }
        internal Room HallCloset { get => Rooms[RoomTypes.HallCloset]; }
        internal Room ParlorCloset { get => Rooms[RoomTypes.ParlorCloset]; }
        internal List<Room> Bedrooms { get => new List<Room>() { Rooms[RoomTypes.MasterBedroom], Rooms[RoomTypes.BoysBedroom], Rooms[RoomTypes.GirlsBedroom], Rooms[RoomTypes.GuestBedroom] }; }
        internal List<Room> Closets { get => new List<Room>() { Rooms[RoomTypes.MasterBedCloset], Rooms[RoomTypes.BoysCloset], Rooms[RoomTypes.GirlsCloset], Rooms[RoomTypes.GuestCloset] }; }
        internal List<Room> Bathrooms { get => new List<Room>() { Rooms[RoomTypes.MasterBathroom], Rooms[RoomTypes.GuestBathroom] }; }
        internal string Address { get; set; }

        internal House(Coord origin, HouseTypes type, string name = null, Direction.Types facing = Direction.Types.DOWN): base(
            name ?? origin.X.ToString() + origin.Y.ToString(),
            new Coord(20, 20) + origin,
            new Coord(20, 0) + origin,
            origin,
            new Coord(0,20) + origin
            )
        {
            StructureType = type;
            Map = new BasicMap(21, 21, 1, Distance.MANHATTAN);
            Origin = origin;
            _facing = facing;
            Generate();
        }

        internal void Generate()
        {
            switch (StructureType)
            {
                //case StructureTypes.Testing: CreateTestingStructure(); break;
                case HouseTypes.CentralPassageHouse: CreateCentralPassageHouse(); break;
                default: break;
            }
            int chance = Calculate.Percent();

            switch (_facing)
            {
                default:
                case Direction.Types.DOWN: break; //do nothing

                //facing west
                case Direction.Types.LEFT:
                    if (chance < 50)
                    {
                        Map = Map.Rotate(270);
                    }
                    else
                    {
                        Map = Map.SwapXY();
                        Map = Map.ReverseHorizontal();
                        if (Calculate.Percent() < 50)
                            Map = Map.ReverseVertical();
                    }
                    break;
                case Direction.Types.RIGHT:
                    if (chance < 50)
                    {
                        Map = Map.Rotate(90);
                    }
                    else
                    {
                        Map = Map.SwapXY();
                        if (Calculate.Percent() < 50)
                            Map = Map.ReverseVertical();
                    }
                    break;
                case Direction.Types.UP:
                    if (chance < 50)
                    {
                        Map = Map.Rotate(180);
                    }
                    else
                    {
                        Map = Map.ReverseVertical();
                        if (Calculate.Percent() < 50)
                            Map = Map.ReverseHorizontal();
                    }
                    break;
            }

            foreach (Room room in Rooms.Values)
            {
                DrawRoom(room);
            }

            CreateArch(DiningRoom, Kitchen);
            CreateArch(Parlor, Hallway);
            List<Coord> doors = new List<Coord>();

            doors.Add(new Coord((Map.Width / 2), Hallway.Bottom));
            doors.Add(new Coord((Map.Width / 2), Hallway.Top));
            doors.Add(new Coord(Parlor.Right, Parlor.Bottom - 3));
            doors.Add(new Coord(BoysBedroom.Left, BoysBedroom.Bottom - 1));
            doors.Add(new Coord(MasterBedroom.Right, MasterBedroom.Top + 2));
            doors.Add(new Coord(GuestBathroom.Right, GuestBathroom.Bottom - 2));
            doors.Add(new Coord(DiningRoom.Left, DiningRoom.Bottom - 3));
            doors.Add(new Coord(Kitchen.Left, Kitchen.Bottom - 2));
            doors.Add(new Coord(GirlsBedroom.Right, GirlsBedroom.Bottom - 1));
            foreach (Coord d in doors)
                if (Map.Contains(d))
                    Map.SetTerrain(TerrainFactory.Door(d));
        }

        private void CreateCentralPassageHouse()
        {
            int roomW = 6;
            int roomH = 16;

            Rooms.Add(RoomTypes.Hall, new Room(RoomTypes.Hall.ToString() + ", " + Name, new Rectangle(new Coord((Map.Width / 2) - (roomW / 2), Map.Height - 4 - roomH), new Coord((Map.Width / 2) + (roomW / 2), Map.Height - 4)), RoomTypes.Hall));

            roomW = RandomRoomDimension();
            roomH = RandomRoomDimension() + 2;
            Rooms.Add(RoomTypes.Parlor, new Room(RoomTypes.Parlor.ToString() + ", " + Name, new Rectangle(new Coord(Hallway.Left - roomW, Map.Height - 3 - roomH), new Coord(Hallway.Left, Hallway.Bottom + 2)), RoomTypes.Parlor));
            Rooms.Add(RoomTypes.MasterBedroom, new Room(RoomTypes.MasterBedroom.ToString() + ", " + Name,new Rectangle(new Coord(Parlor.Left, Parlor.Top - roomH),new Coord(Hallway.Left, Parlor.Top)),RoomTypes.MasterBedroom));
            Rooms.Add(RoomTypes.GuestBathroom, new Room(RoomTypes.GuestBathroom.ToString() + ", " + Name, new Rectangle(new Coord(Hallway.Left - 3, Parlor.Top - 3), new Coord(Hallway.Left + 1, Parlor.Top + 2)), RoomTypes.GuestBathroom));

            roomH = (Hallway.Top + Hallway.Bottom) / 2;
            roomW = RandomRoomDimension();
            Rooms.Add(RoomTypes.DiningRoom, new Room(RoomTypes.DiningRoom.ToString() + ", " + Name, new Rectangle(new Coord(Hallway.Right, roomH + 2), new Coord(Hallway.Right + roomW, Hallway.Bottom + 2)), RoomTypes.DiningRoom));
            Rooms.Add(RoomTypes.Kitchen, new Room(RoomTypes.Kitchen.ToString() + ", " + Name, new Rectangle(new Coord(Hallway.Right, DiningRoom.Top - roomH + 1), new Coord(Hallway.Right + roomW, DiningRoom.Top + 1)), RoomTypes.Kitchen));

            roomH = RandomRoomDimension();
            roomW = RandomRoomDimension();

            Rooms.Add(RoomTypes.BoysBedroom, new Room(RoomTypes.BoysBedroom.ToString() + ", " + Name, new Rectangle(new Coord(Hallway.Right, Hallway.Top - roomH), new Coord(Hallway.Right + roomW, Hallway.Top)), RoomTypes.BoysBedroom));
            Rooms.Add(RoomTypes.GirlsBedroom, new Room(RoomTypes.GirlsBedroom.ToString() + ", " + Name, new Rectangle( new Coord(Hallway.Right, Hallway.Top - roomH), new Coord(Hallway.Right + roomW, Hallway.Top)), RoomTypes.GirlsBedroom));
        }

        private void DrawRoom(Room room)
        {
            for (int x = room.Left + 1; x < room.Right; x++)
            {
                for (int y = room.Top + 1; y < room.Bottom; y++)
                {
                    Coord location = new Coord(x, y);
                    if (Map.Contains(location))
                    {
                        BasicTerrain floor;
                        switch (room.Type)
                        {
                            case RoomTypes.BoysBedroom:
                            case RoomTypes.GirlsBedroom:
                            case RoomTypes.HallCloset:
                            case RoomTypes.MasterBedroom:
                            case RoomTypes.GuestBedroom: floor = TerrainFactory.Carpet(location); break;
                            case RoomTypes.Kitchen:
                            case RoomTypes.GuestBathroom: floor = TerrainFactory.Linoleum(location); break;
                            default: floor = TerrainFactory.HardwoodFloor(location); break;
                        }

                        Map.SetTerrain(floor);
                    }
                }
            }

            foreach (Coord location in room.OuterPoints)
                if(Map.Contains(location))
                    Map.SetTerrain(TerrainFactory.Wall(location));
            
        }
        private int RandomRoomDimension()
        {
            return (Settings.Random.Next(_minRoomSize, _maxRoomSize) + Settings.Random.Next(_minRoomSize, _maxRoomSize)) / 2;
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
    }
}
