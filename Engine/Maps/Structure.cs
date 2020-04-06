using GoRogue;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Rectangle = GoRogue.Rectangle;
namespace Engine.Maps
{
    public enum RoomType
    {
        Parlor,
        Bedroom,
        MasterBedroom,
        Bathroom,
        Kitchen,
        DiningRoom,
        Yard,
        Closet
    }

    public enum StructureType
    {
        CentralPassageHouse,
        CourtyardHouse,
        Konak,
        LogHouse,
        HouseBarn,
        SplitLevel,
        UpperLusatian,
        Cottage,
        TrailerHome,
        DuplixSemiDetached,
        TriplexTripleDecker,
        Quadplex,
        Townhome,
        DowntownShop,
        DowntownOffice,
        DowntownApartment,
        Motel,
    }

    class Room
    {
        public readonly Rectangle Plan;
        public readonly string Name;
        public readonly RoomType Type;
        public int Left
        {
            get
            {
                return Plan.MinExtentX;
            }
        }
        public int Right
        {
            get
            {
                return Plan.MaxExtentX;
            }
        }
        public int Top
        {
            get
            {
                return Plan.MinExtentX;
            }
        }
        public int Bottom
        {
            get
            {
                return Plan.MaxExtentX;
            }
        }
        public Room(string name, Rectangle Plan, RoomType type)
        {
            Name = name;
            Plan = Plan;
            Type = type;
        }
    }

    class Structure
    {
        public TerrainMap Map;
        Coord _origin;
        public List<Room> Rooms = new List<Room>();
        int _minRoomSize;
        int _maxRoomSize;
        Architecture _architecture;

        public Structure()
        {
            Map = new TerrainMap(32, 32, false);
            
            Rooms = new List<Room>();
            _architecture = new Architecture(StructureType.CentralPassageHouse);
        }

        public Structure(int width, int height, Coord origin)
        {
            Map = new TerrainMap(width, height, false);
            _origin = origin;
            _minRoomSize = width / 8;
            _maxRoomSize = width / 4;
            _architecture = new Architecture(StructureType.CentralPassageHouse);
            Generate();
        }

        public void Generate()
        {
            int midX = Map.Width / 2;
            int midY = Map.Height / 2;
            int roomW;
            int roomH;
            int offsetX;
            int offsetY;
            Coord center = new Coord(midX,midY);
            FenceYard(new Rectangle(new Coord(1, 1), new Coord(Map.Width - 1, Map.Height - 1)));


            //Living Room
            roomW = Settings.Random.Next(_minRoomSize, _maxRoomSize);
            roomH = Settings.Random.Next(_minRoomSize, _maxRoomSize);
            offsetX = roomW / 2;
            offsetY = roomH / 2;
            Coord start = new Coord(center.X - offsetX, center.Y - offsetY);
            Coord end = new Coord(center.X + offsetX, center.Y + offsetY);
            CreateRoom(start,end, "parlor", RoomType.Parlor);

            //Kids room
            roomW = Settings.Random.Next(_minRoomSize, _maxRoomSize);
            roomH = Settings.Random.Next(_minRoomSize, _maxRoomSize);
            start = new Coord(midX - roomW - offsetX, midY - roomH);
            end = new Coord(center.X - offsetX, midY);
            CreateRoom(start, end, "bedroom", RoomType.Bedroom);

            //other kids room
            roomW = Settings.Random.Next(_minRoomSize, _maxRoomSize);
            roomH = Settings.Random.Next(_minRoomSize, _maxRoomSize);
            start = new Coord(midX - roomW - offsetX, midY + 2);
            end = new Coord(center.X - offsetX, midY + roomH + 2);
            CreateRoom(start, end, "bedroom", RoomType.Bedroom);
            //size = new Coord(roomW, roomH);
            //if (Settings.Random.Next(0, 2) % 2 == 0)
            //{
            //    start = new Coord(parlor.MinExtentX - roomW, midY);
            //    //door = new Coord(parlor.MinExtentX, midY + 1);
            //}
            //else
            //{
            //    start = new Coord(parlor.MaxExtentX, midY);
            //    //door = new Coord(parlor.MaxExtentX, midY + 1);
            //}
            //kidsRoom = new Rectangle(start, size);
            //CreateRoom(kidsRoom, "bedroom", RoomType.Bedroom, false);
            ////CreateDoor(door);

            ////bathroom
            //roomW = _minRoomSize;
            //roomH = _minRoomSize;
            //start = new Coord(parlor.MinExtentX, parlor.MinExtentY- roomH);
            //size = new Coord(roomW, roomH);
            //Rectangle bathroom = new Rectangle(start, size);
            //CreateRoom(bathroom, "bathroom", RoomType.Bathroom, false);
            //CreateDoor(new Coord(bathroom.MinExtentX + 1, bathroom.MaxExtentY));

            ////Dining room
            //roomW = Settings.Random.Next(_minRoomSize, _maxRoomSize);
            //roomH = Settings.Random.Next(_minRoomSize, _maxRoomSize);
            ////figure out where to start
            //foreach (Room room in Rooms.Where<Room>(x => x.Type == RoomType.Bedroom))
            //{
            //    if (room.Plan.MinExtentX > midX && room.Plan.MinExtentY < midY)
            //    {
            //        start = new Coord(parlor.MinExtentX - roomW, midY - roomH);
            //        //door = new Coord();
            //    }
            //    else if (room.Plan.MinExtentY == midY && room.Plan.MinExtentX == parlor.MaxExtentX)
            //    {
            //        start = new Coord(parlor.MinExtentX - roomW, midY);
            //    }
            //    else if (room.Plan.MinExtentY == midY && room.Plan.MaxExtentX == parlor.MinExtentX)
            //    {
            //        start = new Coord(parlor.MaxExtentX, midY);
            //    }
            //    else
            //    {
            //        start = new Coord(midX - (roomW / 2), parlor.MaxExtentY);
            //    }
            //}

            //corner = new Coord(roomW, roomH);
            //Rectangle dining = new Rectangle(start, corner);
            //CreateRoom(dining, "dining", RoomType.DiningRoom, false);

            ////kitchen
            //roomW = Settings.Random.Next(_minRoomSize, _maxRoomSize);
            //roomH = Settings.Random.Next(_minRoomSize, _maxRoomSize);
            //if (dining.X < parlor.X)
            //{
            //    start = new Coord(dining.X - roomW, dining.MaxExtentY - roomH);
            //}
            //else
            //{
            //    start = new Coord(dining.MaxExtentX, dining.MinExtentY);
            //}
            //corner = new Coord(roomW, roomH);
            //Rectangle kitchen = new Rectangle(start, corner);
            //CreateRoom(kitchen, "kitchen", RoomType.Kitchen, false);
        }

        private void FenceYard(Rectangle yard)
        {
            int yw = yard.Width;
            int yh = yard.Height;
            Coord offset = new Coord(0, 0);
            yard = new Rectangle(offset, new Coord(yw / 2, yh / 2));
            List<Coord> perimeter = Utils.Calculate.BorderLocations(yard);

            //yard / fence
            foreach (Coord tile in perimeter)
            {
                Map.SetTerrain(Terrain.Fence(tile));
            }

            Coord minExtentX = new Coord(yard.MinExtentX, yard.MinExtentY + (yard.Height / 2));
            Coord minExtentY = new Coord(yard.MinExtentX + (yard.Width / 2), yard.MinExtentY);
            Coord maxExtentX = new Coord(yard.MaxExtentX, yard.MinExtentY + (yard.Height / 2));
            Coord maxExtentY = new Coord(yard.MinExtentX + (yard.Width / 2), yard.MaxExtentY);

            Map.SetTerrain(Terrain.FenceGate(minExtentX));
            Map.SetTerrain(Terrain.FenceGate(maxExtentX));
            Map.SetTerrain(Terrain.FenceGate(minExtentY));
            Map.SetTerrain(Terrain.FenceGate(maxExtentY));
        }

        private void CreateRoom(Coord start, Coord end, string name, RoomType type)
        {
            Rectangle room = new Rectangle(start, end);
            int chance = Utils.Calculate.Chance();

            for (int x = room.MinExtentX + 1; x < room.MaxExtentX; x++)
            {
                for (int y = room.MinExtentY + 1; y < room.MaxExtentY; y++)
                {
                    Coord location = new Coord(x, y);
                    if (Map.Contains(location))
                    {
                        Terrain floor;
                        switch (type)
                        {
                            case RoomType.Closet:
                            case RoomType.Bedroom: floor = Terrain.Carpet(location); break;
                            case RoomType.Kitchen:
                            case RoomType.Bathroom: floor = Terrain.Linoleum(location); break;
                            default: floor = Terrain.HardwoodFloor(location); break;
                        }
                        Map.SetTerrain(Terrain.Copy(floor, location));
                    }
                }
            }

            foreach (Coord location in Utils.Calculate.BorderLocations(room))
            {
                if(Map.Contains(location))
                    Map.SetTerrain(Terrain.Wall(location));
            }

            Coord doorLocation = Utils.Calculate.BorderLocations(room).RandomItem();
            //Map.SetTerrain(Terrain.Door(doorLocation));
            Coord minExtentX = new Coord(room.MinExtentX, room.MinExtentY + (room.Height / 2));
            Coord minExtentY = new Coord(room.MinExtentX + (room.Width / 2), room.MinExtentY);
            Coord maxExtentX = new Coord(room.MaxExtentX, room.MinExtentY + (room.Height / 2));
            Coord maxExtentY = new Coord(room.MinExtentX + (room.Width / 2), room.MaxExtentY);

            Map.SetTerrain(Terrain.Door(minExtentX));
            Map.SetTerrain(Terrain.Door(maxExtentX));
            Map.SetTerrain(Terrain.Door(minExtentY));
            Map.SetTerrain(Terrain.Door(maxExtentY));
            Rooms.Add(new Room(name, room, type));
        }
    }
}
