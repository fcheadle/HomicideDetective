using Engine.Entities;
using Engine.Extensions;
using Engine.Maps.Areas;
using GoRogue;
using SadConsole;
using System;
using System.Collections.Generic;
using Rectangle = GoRogue.Rectangle;

namespace Engine.Maps
{
    internal class House : Area
    {
        internal BasicMap Map;
        new internal Coord Origin;
        private const int _minRoomSize = 4;
        private const int _maxRoomSize = 9;
        private readonly HouseTypes StructureType;
        private readonly Direction.Types _facing;
        internal Area Parlor { get => SubAreas[RoomType.Parlor]; }
        internal Area Hallway { get => SubAreas[RoomType.Hall]; }
        internal Area Kitchen { get => SubAreas[RoomType.Kitchen]; }
        internal Area GuestBathroom { get => SubAreas[RoomType.GuestBathroom]; }
        internal Area MasterBedroom { get => SubAreas[RoomType.MasterBedroom]; }
        internal Area MasterBathroom { get => SubAreas[RoomType.MasterBathroom]; }
        internal Area MasterBedCloset { get => SubAreas[RoomType.MasterBedCloset]; }
        internal Area BoysBedroom { get => SubAreas[RoomType.BoysBedroom]; }
        internal Area BoysCloset { get => SubAreas[RoomType.BoysCloset]; }
        internal Area GirlsBedroom { get => SubAreas[RoomType.GirlsBedroom]; }
        internal Area GirlsCloset { get => SubAreas[RoomType.GirlsCloset]; }
        internal Area DiningRoom { get => SubAreas[RoomType.DiningRoom]; }
        internal Area HallCloset { get => SubAreas[RoomType.HallCloset]; }
        internal Area ParlorCloset { get => SubAreas[RoomType.ParlorCloset]; }
        internal List<Area> Bedrooms { get => new List<Area>() { SubAreas[RoomType.MasterBedroom], SubAreas[RoomType.BoysBedroom], SubAreas[RoomType.GirlsBedroom], SubAreas[RoomType.GuestBedroom] }; }
        internal List<Area> Closets { get => new List<Area>() { SubAreas[RoomType.MasterBedCloset], SubAreas[RoomType.BoysCloset], SubAreas[RoomType.GirlsCloset], SubAreas[RoomType.GuestCloset] }; }
        internal List<Area> Bathrooms { get => new List<Area>() { SubAreas[RoomType.MasterBathroom], SubAreas[RoomType.GuestBathroom] }; }
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

            foreach (KeyValuePair<Enum, Area> room in SubAreas)
            {
                DrawRoom(room.Value, (RoomType)room.Key);
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
            int roomW = RandomRoomDimension();
            int roomH = RandomRoomDimension() * 2;
            Rectangle area = new Rectangle(new Coord((Map.Width / 2) - (roomW / 2), Map.Height - 4 - roomH), new Coord((Map.Width / 2) + (roomW / 2), Map.Height - 4));
            Coord se = new Coord(area.MaxExtentX + 1, area.MaxExtentY + 1);
            Coord ne = new Coord(area.MaxExtentX + 1, area.MinExtentY);
            Coord nw = new Coord(area.MinExtentX, area.MinExtentY);
            Coord sw = new Coord(area.MinExtentX, area.MaxExtentY + 1);
            SubAreas.Add(RoomType.Hall, AreaFactory.Room(RoomType.Hall.ToString() + ", " + Name, area));

            roomW = RandomRoomDimension();
            roomH = RandomRoomDimension();
            SubAreas.Add(RoomType.Parlor, AreaFactory.Room(RoomType.Parlor.ToString() + ", " + Name, new Rectangle(new Coord(Hallway.Left - roomW, Map.Height - 3 - roomH), new Coord(Hallway.Left, Hallway.Bottom + 2))));
            SubAreas.Add(RoomType.MasterBedroom, AreaFactory.Room(RoomType.MasterBedroom.ToString() + ", " + Name,new Rectangle(new Coord(Parlor.Left, Parlor.Top - roomH),new Coord(Hallway.Left, Parlor.Top))));
            SubAreas.Add(RoomType.GuestBathroom, AreaFactory.Room(RoomType.GuestBathroom.ToString() + ", " + Name, new Rectangle(new Coord(Hallway.Left - 3, Parlor.Top - 3), new Coord(Hallway.Left + 1, Parlor.Top + 2))));

            roomH = RandomRoomDimension();
            roomW = RandomRoomDimension();
            SubAreas.Add(RoomType.DiningRoom, AreaFactory.Room(RoomType.DiningRoom.ToString() + ", " + Name, new Rectangle(new Coord(Hallway.Right, roomH + 2), new Coord(Hallway.Right + roomW, Hallway.Bottom + 2))));
            SubAreas.Add(RoomType.Kitchen, AreaFactory.Room(RoomType.Kitchen.ToString() + ", " + Name, new Rectangle(new Coord(Hallway.Right, DiningRoom.Top - roomH + 1), new Coord(Hallway.Right + roomW, DiningRoom.Top + 1))));

            roomH = RandomRoomDimension();
            roomW = RandomRoomDimension();

            SubAreas.Add(RoomType.BoysBedroom, AreaFactory.Room(RoomType.BoysBedroom.ToString() + ", " + Name, new Rectangle(new Coord(Hallway.Right, Hallway.Top - roomH), new Coord(Hallway.Right + roomW, Hallway.Top))));
            SubAreas.Add(RoomType.GirlsBedroom, AreaFactory.Room(RoomType.GirlsBedroom.ToString() + ", " + Name, new Rectangle( new Coord(Hallway.Right, Hallway.Top - roomH), new Coord(Hallway.Right + roomW, Hallway.Top))));
        }

        private void DrawRoom(Area room, RoomType type)
        {
            for (int x = room.Left + 1; x < room.Right; x++)
            {
                for (int y = room.Top + 1; y < room.Bottom; y++)
                {
                    Coord location = new Coord(x, y);
                    if (Map.Contains(location))
                    {
                        BasicTerrain floor;
                        switch (type)
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
        private void CreateArch(Area host, Area imposing)
        {
            List<Coord> walls = new List<Coord>();

            foreach (Coord c in imposing.OuterPoints)
            {
                if(host.Contains(c))
                    walls.Add(c);
            }

            foreach(Coord arch in walls)
            {
                if (Map.Contains(arch))
                    Map.SetTerrain(TerrainFactory.HardwoodFloor(arch));
            }

            foreach(Coord c in host.OuterPoints)
            {
                if (walls.Contains(c) && Map.Contains(c))
                    Map.SetTerrain(TerrainFactory.Wall(c));
            }
        }
    }
}
