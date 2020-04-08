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

    public enum Directions
    {
        North,
        East,
        South, 
        West,
        None,
        All
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
                return Plan.MinExtentY;
            }
        }
        public int Bottom
        {
            get
            {
                return Plan.MaxExtentY;
            }
        }
        public Room(string name, Rectangle Plan, RoomType type)
        {
            Name = name;
            this.Plan = Plan;
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

        public int Left;
        public int Right;
        public int Top;
        public int Bottom;
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
            _minRoomSize = width / 9;
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
            int parlorLeft;
            int parlorRight;
            int parlorBottom;
            int parlorTop;
            Coord center = new Coord(midX,midY);
            //FenceYard(new Rectangle(new Coord(1, 1), new Coord(Map.Width - 1, Map.Height - 1)));

            //Living Room
            roomW = _maxRoomSize;// Settings.Random.Next(_minRoomSize, _maxRoomSize);
            roomH = _maxRoomSize; // Settings.Random.Next(_minRoomSize, _maxRoomSize);
            offsetX = roomW / 2;
            offsetY = roomH / 2;
            parlorLeft = midX - offsetX;
            parlorRight = midX + offsetX;
            parlorBottom = midY + offsetY;
            parlorTop = midY - offsetY;
            Coord start = new Coord(parlorLeft, parlorTop);
            Coord end = new Coord(parlorRight, parlorBottom);
            CreateRoom(start,end, "parlor", RoomType.Parlor);
            
            //Kids room
            roomW = Settings.Random.Next(_minRoomSize, _maxRoomSize);
            roomH = Settings.Random.Next(_minRoomSize, _maxRoomSize);
            Left = midX - roomW - offsetX;
            parlorLeft = center.X - offsetX;
            start = new Coord(Left, midY - roomH);
            end = new Coord(parlorLeft, midY);
            CreateRoom(start, end, "bedroom", RoomType.Bedroom, Directions.None);

            //other kids room
            start = new Coord(Left, midY + 2);
            end = new Coord(parlorLeft, midY + roomH + 2);
            CreateRoom(start, end, "bedroom", RoomType.Bedroom, Directions.None);

            //central hallway
            start = new Coord(Left, midY);
            end = new Coord(parlorLeft,midY + 2);
            CreateRoom(start, end, "hallway", RoomType.Parlor, Directions.All);

            //hall closet
            start = new Coord(Left - 2, midY);
            end = new Coord(Left, midY + 2);
            CreateRoom(start, end, "closet", RoomType.Closet, Directions.East);

            //kids closet 1
            start = new Coord(Left - 2, midY - 2);
            end = new Coord(Left, midY);
            CreateRoom(start, end, "closet", RoomType.Closet, Directions.East);

            //kids closet 
            start = new Coord(Left - 2, midY + 2);
            end = new Coord(Left, midY + 4);
            CreateRoom(start, end, "closet", RoomType.Closet, Directions.East);

            //bathroom
            start = new Coord(parlorLeft - 1, parlorTop);
            end = new Coord(parlorLeft + _minRoomSize - 1, midY - 1);
            CreateRoom(start, end, "bathroom", RoomType.Bathroom, Directions.South);

            //kitchen
            roomW = Settings.Random.Next(_minRoomSize, _maxRoomSize);
            roomH = Settings.Random.Next(_minRoomSize, _maxRoomSize);
            start = new Coord(midX + 2, midY - roomH);
            end = new Coord(midX + 2 + roomW, midY);
            CreateRoom(start, end, "kitchen", RoomType.Kitchen, Directions.None, true);

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

        private void CreateRoom(Coord start, Coord end, string name, RoomType type, Directions direction = Directions.All, bool replace = true)
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
                            case RoomType.Closet:
                            case RoomType.Bedroom: floor = Terrain.Carpet(location); break;
                            case RoomType.Kitchen:
                            case RoomType.Bathroom: floor = Terrain.Linoleum(location); break;
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
            Rooms.Add(room);
        }
    }
}
