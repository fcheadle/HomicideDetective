using Engine.Utils;
using GoRogue;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Rectangle = GoRogue.Rectangle;
namespace Engine.Maps
{
    public enum RoomsType
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

    public enum StructuresType
    {
        Testing,
        //CentralPassageHouse,
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
        //DowntownShop,
        //DowntownOffice,
        //DowntownApartment,
        //Motel,
    }

    public enum Directions
    {
        North,
        NorthNorthEast,
        NorthEast,
        EastNorthEast,
        East,
        EastSouthEast,
        SouthEast,
        SouthSouthEast,
        South, 
        SouthSouthWest,
        SouthWest,
        WestSouthWest,
        West,
        WestNorthWest,
        NorthWest,
        NorthNorthWest,
        Up,
        Down,
        None,
        All
    }

    class Room
    {
        public readonly Rectangle Area;
        public readonly string Name;
        public readonly RoomsType Type;
        public int Left
        {
            get
            {
                return Area.MinExtentX;
            }
        }
        public int Right
        {
            get
            {
                return Area.MaxExtentX;
            }
        }
        public int Top
        {
            get
            {
                return Area.MinExtentY;
            }
        }
        public int Bottom
        {
            get
            {
                return Area.MaxExtentY;
            }
        }
        public Room(string name, Rectangle area, RoomsType type)
        {
            Name = name;
            Area = area;
            Type = type;
        }
    }

    class Structure
    {
        public TerrainMap Map;
        Coord _origin;
        public Dictionary<string, Room> Rooms = new Dictionary<string, Room>();
        int _minRoomSize;
        int _maxRoomSize;
        public readonly StructuresType StructureType;
        public int Left;
        public int Right;
        public int Top;
        public int Bottom;
        public Structure()
        {
            //Map = new TerrainMap(32, 32, false);
        }

        public Structure(int width, int height, Coord origin, StructuresType type)
        {
            Map = new TerrainMap(width, height, false);
            _origin = origin;
            _minRoomSize = width / 9;
            _maxRoomSize = width / 4;
            Generate();
        }

        public void Generate()
        {
            switch (StructureType)
            {
                case StructuresType.Testing: CreateTestingStructure(); break;
                default: break;
            }
            
        }

        private void CreateTestingStructure()
        {
            int midX = Map.Width / 2;
            int midY = Map.Height / 2;
            int roomW;
            int roomH;
            int offsetX;
            int offsetY;
            Coord center = new Coord(midX, midY);
            //FenceYard(new Rectangle(new Coord(1, 1), new Coord(Map.Width - 1, Map.Height - 1)));

            //Living Room
            roomW = _maxRoomSize;// Settings.Random.Next(_minRoomSize, _maxRoomSize);
            roomH = _maxRoomSize; // Settings.Random.Next(_minRoomSize, _maxRoomSize);
            offsetX = roomW / 2;
            offsetY = roomH / 2;
            Coord start = new Coord(midX - offsetX, midY - offsetY);
            Coord end = new Coord(midX + offsetX, midY + offsetY);
            CreateRoom(start, end, "parlor", RoomsType.Parlor);

            //Kids room
            roomW = Settings.Random.Next(_minRoomSize, _maxRoomSize);
            roomH = Settings.Random.Next(_minRoomSize, _maxRoomSize);
            Left = midX - roomW - offsetX;
            start = new Coord(Left, midY - roomH);
            end = new Coord(Rooms["parlor"].Left, midY);
            CreateRoom(start, end, "kids_bedroom_1", RoomsType.Bedroom, Directions.None);

            //other kids room
            start = new Coord(Left, midY + 2);
            end = new Coord(Rooms["parlor"].Left, midY + roomH + 2);
            CreateRoom(start, end, "kids_bedroom_2", RoomsType.Bedroom, Directions.None);

            //central hallway
            start = new Coord(Left, midY);
            end = new Coord(Rooms["parlor"].Left, midY + 2);
            CreateRoom(start, end, "hallway", RoomsType.Parlor, Directions.All);

            //hall closet
            start = new Coord(Left - 2, midY);
            end = new Coord(Left, midY + 2);
            CreateRoom(start, end, "closet_1", RoomsType.Closet, Directions.East);

            //kids closet 1
            start = new Coord(Left - 2, midY - 2);
            end = new Coord(Left, midY);
            CreateRoom(start, end, "kids_closet_1", RoomsType.Closet, Directions.East);

            //kids closet 
            start = new Coord(Left - 2, midY + 2);
            end = new Coord(Left, midY + 4);
            CreateRoom(start, end, "kids_closet_2", RoomsType.Closet, Directions.East);

            //bathroom
            start = new Coord(Rooms["parlor"].Left - 1, Rooms["parlor"].Top);
            end = new Coord(Rooms["parlor"].Left + _minRoomSize - 1, midY - 1);
            CreateRoom(start, end, "guest_bathroom", RoomsType.Bathroom, Directions.South);

            //kitchen
            roomW = Settings.Random.Next(_minRoomSize, _maxRoomSize);
            roomH = Settings.Random.Next(_minRoomSize, _maxRoomSize);
            start = new Coord(midX + 2, midY - roomH);
            end = new Coord(midX + 2 + roomW, midY);
            CreateRoom(start, end, "kitchen", RoomsType.Kitchen, Directions.South, true);
            CreateArch(Rooms["parlor"], Rooms["kitchen"]);

            //dining
            roomW = Settings.Random.Next(_minRoomSize, _maxRoomSize);
            roomH = Settings.Random.Next(_minRoomSize, _maxRoomSize);
            start = new Coord(Rooms["kitchen"].Right - roomW, midY);
            end = new Coord(Rooms["kitchen"].Right, midY + roomH);
            CreateRoom(start, end, "dining", RoomsType.DiningRoom, Directions.None, true);
            CreateArch(Rooms["parlor"], Rooms["dining"]);

            //master bed
            roomW = Settings.Random.Next(_minRoomSize, _maxRoomSize);
            roomH = Settings.Random.Next(_minRoomSize, _maxRoomSize);
            start = new Coord(Rooms["kids_bedroom_1"].Right, Rooms["parlor"].Bottom);
            end = new Coord(start.X + roomW, start.Y + roomH);
            CreateRoom(start, end, "master_bed", RoomsType.MasterBedroom, Directions.North, true);

        }

        private void CreateArch(Room host, Room imposing)
        {
            List<Coord> walls = new List<Coord>();

            foreach (Coord c in Calculate.BorderLocations(imposing.Area))
            {
                if(host.Area.Contains(c))
                    walls.Add(c);
            }

            foreach(Coord arch in walls)
            {
                Map.SetTerrain(Terrain.HardwoodFloor(arch));
            }

            foreach(Coord c in Calculate.BorderLocations(host.Area))
            {
                if (walls.Contains(c))
                    Map.SetTerrain(Terrain.Wall(c));
            }
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

        private void CreateRoom(Coord start, Coord end, string name, RoomsType type, Directions direction = Directions.All, bool replace = true)
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
                        Terrain floor;
                        switch (type)
                        {
                            case RoomsType.Closet:
                            case RoomsType.MasterBedroom:
                            case RoomsType.Bedroom: floor = Terrain.Carpet(location); break;
                            case RoomsType.Kitchen:
                            case RoomsType.Bathroom: floor = Terrain.Linoleum(location); break;
                            default: floor = Terrain.HardwoodFloor(location); break;
                        }
                        if (replace || Map.GetTerrain<Terrain>(location) == null)
                        {
                            Map.SetTerrain(Terrain.Copy(floor, location));
                        }
                        else if (Map.GetTerrain<Terrain>(location).IsWalkable == false)
                        {
                            Map.SetTerrain(Terrain.Copy(floor, location));
                        }
                        
                    }
                }
            }

            foreach (Coord location in Utils.Calculate.BorderLocations(area))
            {
                if (Map.Contains(location))
                {
                    if (replace || Map.GetTerrain<Terrain>(location) == null)
                    {
                        Map.SetTerrain(Terrain.Wall(location));
                    }

                }
            }

            if (direction == Directions.All)
            {
                Coord minExtentX = new Coord(area.MinExtentX, area.MinExtentY + (area.Height / 2));
                Coord minExtentY = new Coord(area.MinExtentX + (area.Width / 2), area.MinExtentY);
                Coord maxExtentX = new Coord(area.MaxExtentX, area.MinExtentY + (area.Height / 2));
                Coord maxExtentY = new Coord(area.MinExtentX + (area.Width / 2), area.MaxExtentY);
                Map.SetTerrain(Terrain.Door(minExtentX));
                Map.SetTerrain(Terrain.Door(maxExtentX));
                Map.SetTerrain(Terrain.Door(minExtentY));
                Map.SetTerrain(Terrain.Door(maxExtentY));
            }
            else if (direction == Directions.None)
            {
                //no doors
            }
            else
            {
                List<Coord> possible;
                switch (direction)
                {
                    case Directions.West:
                        possible = Utils.Calculate.PointsAlongLine(new Coord(room.Left, room.Top + 1), new Coord(room.Left, room.Bottom - 1)).ToList();
                        break;
                    case Directions.South: 
                        possible = Utils.Calculate.PointsAlongLine(new Coord(room.Left + 1, room.Bottom), new Coord(room.Right - 1, room.Bottom)).ToList();
                        break;
                    case Directions.East: 
                        possible = Utils.Calculate.PointsAlongLine(new Coord(room.Right, room.Top + 1), new Coord(room.Right, room.Bottom - 1)).ToList(); 
                        break;
                    case Directions.North: 
                        possible = Utils.Calculate.PointsAlongLine(new Coord(room.Left + 1, room.Top), new Coord(room.Right - 1, room.Top)).ToList(); 
                        break;
                    default: possible = new List<Coord>(); break;
                }
                Coord doorLoc;
                if(possible.Count == 0)
                    doorLoc = new Coord(room.Right, room.Bottom);
                else
                    doorLoc = possible.RandomItem();
                Map.SetTerrain(Terrain.Door(doorLoc));
            }
            Rooms.Add(room.Name, room);
        }
    }
}
