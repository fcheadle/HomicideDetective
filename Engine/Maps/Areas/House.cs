using Engine.Entities;
using Engine.Extensions;
using GoRogue;
using SadConsole;
using System.Collections.Generic;
using Rectangle = GoRogue.Rectangle;

namespace Engine.Maps
{
    public enum HouseTypes
    {
        //Testing,
        CentralPassageHouse,
        //CourtyardHouse,
        //Konak,
        //LogHouse,
        //HouseBarn,
        //SplitLevel,
        //UpperLusatian,
        //Cottage,
        //TrailerHome,
        //DuplixSemiDetached,
        //TriplexTripleDecker,
        //Quadplex,
        //Townhome,
    }
    internal class House : Area
    {
        internal BasicMap Map;
        new internal Coord Origin;
        internal Dictionary<RoomType, Room> Rooms = new Dictionary<RoomType, Room>();
        private const int _minRoomSize = 4;
        private const int _maxRoomSize = 9;
        private readonly HouseTypes StructureType;
        private readonly Direction.Types _facing;
        internal Room Parlor { get => Rooms[RoomType.Parlor]; }
        internal Room Hallway { get => Rooms[RoomType.Hall]; }
        internal Room Kitchen { get => Rooms[RoomType.Kitchen]; }
        internal Room GuestBathroom { get => Rooms[RoomType.GuestBathroom]; }
        internal Room MasterBedroom { get => Rooms[RoomType.MasterBedroom]; }
        internal Room MasterBathroom { get => Rooms[RoomType.MasterBathroom]; }
        internal Room MasterBedCloset { get => Rooms[RoomType.MasterBedCloset]; }
        internal Room BoysBedroom { get => Rooms[RoomType.BoysBedroom]; }
        internal Room BoysCloset { get => Rooms[RoomType.BoysCloset]; }
        internal Room GirlsBedroom { get => Rooms[RoomType.GirlsBedroom]; }
        internal Room GirlsCloset { get => Rooms[RoomType.GirlsCloset]; }
        internal Room DiningRoom { get => Rooms[RoomType.DiningRoom]; }
        internal Room HallCloset { get => Rooms[RoomType.HallCloset]; }
        internal Room ParlorCloset { get => Rooms[RoomType.ParlorCloset]; }
        internal List<Room> Bedrooms { get => new List<Room>() { Rooms[RoomType.MasterBedroom], Rooms[RoomType.BoysBedroom], Rooms[RoomType.GirlsBedroom], Rooms[RoomType.GuestBedroom] }; }
        internal List<Room> Closets { get => new List<Room>() { Rooms[RoomType.MasterBedCloset], Rooms[RoomType.BoysCloset], Rooms[RoomType.GirlsCloset], Rooms[RoomType.GuestCloset] }; }
        internal List<Room> Bathrooms { get => new List<Room>() { Rooms[RoomType.MasterBathroom], Rooms[RoomType.GuestBathroom] }; }
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

            Rooms.Add(RoomType.Hall, new Room(RoomType.Hall.ToString() + ", " + Name, new Rectangle(new Coord((Map.Width / 2) - (roomW / 2), Map.Height - 4 - roomH), new Coord((Map.Width / 2) + (roomW / 2), Map.Height - 4)), RoomType.Hall));

            roomW = RandomRoomDimension();
            roomH = RandomRoomDimension() + 2;
            Rooms.Add(RoomType.Parlor, new Room(RoomType.Parlor.ToString() + ", " + Name, new Rectangle(new Coord(Hallway.Left - roomW, Map.Height - 3 - roomH), new Coord(Hallway.Left, Hallway.Bottom + 2)), RoomType.Parlor));
            Rooms.Add(RoomType.MasterBedroom, new Room(RoomType.MasterBedroom.ToString() + ", " + Name,new Rectangle(new Coord(Parlor.Left, Parlor.Top - roomH),new Coord(Hallway.Left, Parlor.Top)), RoomType.MasterBedroom));
            Rooms.Add(RoomType.GuestBathroom, new Room(RoomType.GuestBathroom.ToString() + ", " + Name, new Rectangle(new Coord(Hallway.Left - 3, Parlor.Top - 3), new Coord(Hallway.Left + 1, Parlor.Top + 2)), RoomType.GuestBathroom));

            roomH = (Hallway.Top + Hallway.Bottom) / 2;
            roomW = RandomRoomDimension();
            Rooms.Add(RoomType.DiningRoom, new Room(RoomType.DiningRoom.ToString() + ", " + Name, new Rectangle(new Coord(Hallway.Right, roomH + 2), new Coord(Hallway.Right + roomW, Hallway.Bottom + 2)), RoomType.DiningRoom));
            Rooms.Add(RoomType.Kitchen, new Room(RoomType.Kitchen.ToString() + ", " + Name, new Rectangle(new Coord(Hallway.Right, DiningRoom.Top - roomH + 1), new Coord(Hallway.Right + roomW, DiningRoom.Top + 1)), RoomType.Kitchen));

            roomH = RandomRoomDimension();
            roomW = RandomRoomDimension();

            Rooms.Add(RoomType.BoysBedroom, new Room(RoomType.BoysBedroom.ToString() + ", " + Name, new Rectangle(new Coord(Hallway.Right, Hallway.Top - roomH), new Coord(Hallway.Right + roomW, Hallway.Top)), RoomType.BoysBedroom));
            Rooms.Add(RoomType.GirlsBedroom, new Room(RoomType.GirlsBedroom.ToString() + ", " + Name, new Rectangle( new Coord(Hallway.Right, Hallway.Top - roomH), new Coord(Hallway.Right + roomW, Hallway.Top)), RoomType.GirlsBedroom));
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
                            case RoomType.BoysBedroom:
                            case RoomType.GirlsBedroom:
                            case RoomType.HallCloset:
                            case RoomType.MasterBedroom:
                            case RoomType.GuestBedroom: floor = TerrainFactory.Carpet(location); break;
                            case RoomType.Kitchen:
                            case RoomType.GuestBathroom: floor = TerrainFactory.Linoleum(location); break;
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
