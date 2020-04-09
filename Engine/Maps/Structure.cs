using Engine.Utils;
using GoRogue;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Rectangle = GoRogue.Rectangle;
namespace Engine.Maps
{
    public enum RoomTypes
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

    public enum StructureTypes
    {
        Testing,
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
        NorthNorthEast,
        EastNorthEast,
        East,
        SouthSouthEast,
        EastSouthEast,
        South, 
        SouthSouthWest,
        WestSouthWest,
        West,
        None,
        All
    }

    class Room
    {
        public readonly Rectangle Area;
        public readonly string Name;
        public readonly RoomTypes RoomType;
        public List<Directions> connections;
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
        public Coord Midpoint
        {
            get
            {
                return new Coord((Left + Right) / 2,(Top + Bottom) / 2);
            }
        }


        public Room(string name, Rectangle Plan, RoomTypes type)
        {
            Name = name;
            this.Area = Plan;
            RoomType = type;
        }
    }

    class Structure
    {
        public TerrainMap Map;
        Coord _origin;
        public Dictionary<string, Room> Rooms = new Dictionary<string, Room>();
        int _minRoomSize;
        int _maxRoomSize;

        public int Left;
        public int Right;
        public int Top;
        public int Bottom;
        public Structure()
        {
            Map = new TerrainMap(32, 32, false);
            
            Rooms = new Dictionary<string, Room>();
        }

        public Structure(int width, int height, Coord origin)
        {
            Map = new TerrainMap(width, height, false);
            _origin = origin;
            _minRoomSize = width / 9;
            _maxRoomSize = width / 4;
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
            CreateRoom(start,end, "parlor", RoomTypes.Parlor);
            
            //Kids room
            roomW = Settings.Random.Next(_minRoomSize, _maxRoomSize);
            roomH = Settings.Random.Next(_minRoomSize, _maxRoomSize);
            Left = midX - roomW - offsetX;
            parlorLeft = center.X - offsetX;
            start = new Coord(Left, midY - roomH);
            end = new Coord(parlorLeft, midY);
            CreateRoom(start, end, "kids_bedroom_1", RoomTypes.Bedroom, Directions.None);

            //other kids room
            start = new Coord(Left, midY + 2);
            end = new Coord(parlorLeft, midY + roomH + 2);
            CreateRoom(start, end, "kids_bedroom_2", RoomTypes.Bedroom, Directions.None);

            //central hallway
            start = new Coord(Left, midY);
            end = new Coord(parlorLeft,midY + 2);
            CreateRoom(start, end, "central_hallway", RoomTypes.Parlor, Directions.All);

            //hall closet
            start = new Coord(Left - 2, midY);
            end = new Coord(Left, midY + 2);
            CreateRoom(start, end, "hall_closet", RoomTypes.Closet, Directions.East);

            //kids closet 1
            start = new Coord(Left - 2, midY - 2);
            end = new Coord(Left, midY);
            CreateRoom(start, end, "kids_closet_1", RoomTypes.Closet, Directions.East);

            //kids closet 2
            start = new Coord(Left - 2, midY + 2);
            end = new Coord(Left, midY + 4);
            CreateRoom(start, end, "kids_closet_2", RoomTypes.Closet, Directions.East);

            //bathroom
            start = new Coord(parlorLeft - 1, parlorTop);
            end = new Coord(parlorLeft + _minRoomSize - 1, midY - 1);
            CreateRoom(start, end, "bathroom", RoomTypes.Bathroom, Directions.South);

            //kitchen
            roomW = Settings.Random.Next(_minRoomSize, _maxRoomSize);
            roomH = Settings.Random.Next(_minRoomSize, _maxRoomSize);
            start = new Coord(midX + 4, midY - roomH);
            end = new Coord(midX + 4 + roomW, midY);
            CreateRoom(start, end, "kitchen", RoomTypes.Kitchen, Directions.South, true);

            //dining room
            roomW = Settings.Random.Next(_minRoomSize, _maxRoomSize);
            roomH = Settings.Random.Next(_minRoomSize, _maxRoomSize);
            start = new Coord(Rooms["kitchen"].Right - roomW, midY);
            end = new Coord(Rooms["kitchen"].Right, midY + roomH);
            CreateRoom(start, end, "dining", RoomTypes.DiningRoom, Directions.West, true);
            CreateArch(Rooms["parlor"], Rooms["kitchen"]);

        }

        private void CreateArch(Room host, Room imposing)
        {
            List<Coord> pointsToReplace = new List<Coord>();
            List<Coord> hostBorder = Calculate.BorderLocations(host.Area);
            List<Coord> imposingBorder = Calculate.BorderLocations(imposing.Area);
            
            foreach (Coord c in imposingBorder)
            {
                if (host.Area.Contains(c) && !hostBorder.Contains(c))
                {
                    pointsToReplace.Add(c);
                }

            }
            switch (imposing.RoomType)
            {

            }
            foreach (Point p in pointsToReplace)
            {
                Map.SetTerrain(GetFloor(imposing.RoomType, p));
            }


            Map.SetTerrain(Terrain.Wall(new Coord(imposing.Area.MinExtentX, imposing.Area.MinExtentY)));
            Map.SetTerrain(Terrain.Wall(new Coord(imposing.Area.MinExtentX, imposing.Area.MaxExtentY)));
            Map.SetTerrain(Terrain.Wall(new Coord(imposing.Area.MaxExtentX, imposing.Area.MinExtentY)));
            Map.SetTerrain(Terrain.Wall(new Coord(imposing.Area.MaxExtentX, imposing.Area.MaxExtentY)));
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

        private void CreateRoom(Coord start, Coord end, string name, RoomTypes type, Directions direction = Directions.All, bool replace = true)
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
                        Terrain floor = GetFloor(type, location);
                        if (replace)
                        {
                            Map.SetTerrain(Terrain.Copy(floor, location));
                        }
                        else if (Map.GetTerrain<Terrain>(location) == null)
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

            foreach (Coord location in Calculate.BorderLocations(area))
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
                    case Directions.WestSouthWest:
                        possible = Calculate.PointsAlongLine(new Coord(room.Left, room.Midpoint.Y), new Coord(room.Left, room.Bottom - 1)).ToList();
                        break;
                    case Directions.SouthSouthWest: 
                        possible = Calculate.PointsAlongLine(new Coord(room.Left + 1, room.Bottom), new Coord(room.Midpoint.X, room.Bottom)).ToList();
                        break;
                    case Directions.EastSouthEast: 
                        possible = Calculate.PointsAlongLine(new Coord(room.Right, room.Top + 1), new Coord(room.Right, room.Bottom - 1)).ToList(); 
                        break;
                    case Directions.North: 
                        possible = Calculate.PointsAlongLine(new Coord(room.Left + 1, room.Top), new Coord(room.Right - 1, room.Top)).ToList(); 
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

        private Terrain GetFloor(RoomTypes type, Coord location)
        {
            Terrain terrain;
            switch (type)
            {
                case RoomTypes.Closet:
                case RoomTypes.Bedroom: terrain = Terrain.Carpet(location); break;
                case RoomTypes.Kitchen:
                case RoomTypes.Bathroom: terrain = Terrain.Linoleum(location); break;
                default: terrain = Terrain.HardwoodFloor(location); break;
            }
            return terrain;
        }
    }
}
