using Engine.Utils;
using GoRogue;
using Microsoft.Xna.Framework;
using SadConsole.Maps;
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

    public class Road : Region
    {
        private bool _horizontal;
        public Coord Start { get; }
        public Coord End { get; }
        public string Name { get; }
        public int Left { get; }
        public int Right { get; }
        public int Top { get; }
        public int Bottom { get; }
        public Dictionary<Coord, Structure> Addresses { get; private set; } = new Dictionary<Coord, Structure>();
        public Road(Coord start, Coord stop, string name, bool horizontal = true)
        {
            Start = start;
            End = stop;
            Name = name;
            InnerPoints = Calculate.PointsAlongLine(start, stop, 8);
            //Populate();
            Left = start.X < stop.X ? start.X : stop.X;
            Right = start.X > stop.X ? start.X : stop.X;
            Bottom = start.Y > stop.Y ? start.Y : stop.Y;
            Top = start.Y < stop.Y ? start.Y : stop.Y;
            _horizontal = horizontal;
        }

        public void AddIntersection(List<Coord> overlap)
        {
            Connections.AddRange(overlap);
        }

        public void Populate()
        {
            Structure house;
            int j;
            Coord c;
            if (_horizontal) //street runs east-west
            {
                for (int i = Start.X; i < End.X - 49; i += 48)
                {
                    j = InnerPoints.Where(l => l.X == i).OrderBy(l => l.Y).First().Y - 49;
                    c = new Coord(i, j);
                    house = new Structure(48, 48, c, StructureTypes.CentralPassageHouse);
                    house.Address = c.ToString() + Name;
                    Addresses.Add(c, house);


                    j = InnerPoints.Where(l => l.X == i).OrderBy(l => l.Y).Last().Y + 49;
                    c = new Coord(i, j);
                    house = new Structure(48, 48, c, StructureTypes.CentralPassageHouse);
                    house.Address = c.ToString() + Name;
                    house.Map.ReverseVertical();
                    Addresses.Add(c, house);
                }
            }
            else //street runs north/south
            {
                for (int i = Start.Y; i < End.Y - 49; i += 48)
                {
                    j = InnerPoints.Where(l => l.Y == i).OrderBy(l => l.X).First().Y - 49;
                    c = new Coord(j, i);
                    house = new Structure(48, 48, c, StructureTypes.CentralPassageHouse);
                    house.Map.SwapXY(); //door now on east side of house
                    Addresses.Add(c, house);

                    j = InnerPoints.Where(l => l.Y == i).OrderBy(l => l.X).Last().Y + 49;
                    c = new Coord(j, i);
                    house = new Structure(48, 48, c, StructureTypes.CentralPassageHouse);
                    house.Map.SwapXY();
                    house.Map.ReverseHorizontal();
                    Addresses.Add(c, house);
                }
            }
        }
    }

    class Room
    {
        public readonly Rectangle Area;
        public readonly string Name;
        public readonly RoomTypes Type;
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
        public Room(string name, Rectangle area, RoomTypes type)
        {
            Name = name;
            Area = area;
            Type = type;
        }
    }

    public class Structure
    {
        public TerrainMap Map;
        public Coord Origin;
        Dictionary<string, Room> Rooms = new Dictionary<string, Room>();
        int _minRoomSize;
        int _maxRoomSize;
        public readonly StructureTypes StructureType;
        public int Left => Rooms.OrderBy(key => key.Value.Left).ToList().First().Value.Left;
        public int Right => Rooms.OrderBy(key => key.Value.Right).ToList().Last().Value.Right;
        public int Top => Rooms.OrderBy(key => key.Value.Top).ToList().First().Value.Top;
        public int Bottom => Rooms.OrderBy(key => key.Value.Bottom).ToList().Last().Value.Bottom;

        public string Address { get; internal set; }

        public Structure()
        {
            //Map = new TerrainMap(32, 32, false);
        }

        public Structure(int width, int height, Coord origin, StructureTypes type)
        {
            Address = origin.X.ToString() + origin.Y.ToString();
            StructureType = type;
            Map = new TerrainMap(width, height, false);
            Origin = origin;
            _minRoomSize = width / 9;
            _maxRoomSize = width / 4;
            Generate();
        }

        public void Generate()
        {
            //FenceYard(new Rectangle(new Coord(1, 1), new Coord(Map.Width - 1, Map.Height - 1)));
            switch (StructureType)
            {
                case StructureTypes.Testing: CreateTestingStructure(); break;
                case StructureTypes.CentralPassageHouse: CreateCentralPassageHouse(); break;
                default: break;
            }

            Map = Map.Subsection(new Coord(Left, Top), new Coord(Right, Bottom));            
        }

        private void CreateCentralPassageHouse()
        {
            int midX = Map.Width / 2;
            int midY = Map.Height / 2;
            int roomW;
            int roomH;
            //FenceYard(new Rectangle(new Coord(1, 1), new Coord(Map.Width - 1, Map.Height - 1)));

            //central passage
            Coord start;
            Coord end;
            roomW = 6;
            roomH = 12;
            start = new Coord(midX - (roomW / 2), Map.Height - 4 - roomH);
            end = new Coord(midX + 2, Map.Height - 4);
            CreateRoom(start, end, "hallway", RoomTypes.Parlor);
            Room hallway = Rooms["hallway"];

            //parlor
            roomW = RandomRoomDimension() * 2;
            roomH = hallway.Area.Height / 2 - 1;
            start = new Coord(Left - roomW, Bottom - roomH);
            end = new Coord(Left, Bottom);
            CreateRoom(start, end, "parlor", RoomTypes.Parlor, Directions.None);
            Map.SetTerrain(Terrain.Door(new Coord(Rooms["parlor"].Right, Rooms["parlor"].Bottom - 1)));

            //master bed
            start = new Coord(Left, Rooms["parlor"].Top - roomH);
            end = new Coord(hallway.Left, Rooms["parlor"].Top);
            CreateRoom(start, end, "master_bed", RoomTypes.MasterBedroom, Directions.East);
            Map.SetTerrain(Terrain.Door(new Coord(Rooms["master_bed"].Right, Rooms["master_bed"].Top +1)));

            //main bathroom
            roomW = 5;//RandomRoomDimension();
            roomH = 5;// RandomRoomDimension();
            start = new Coord(hallway.Left - roomW, Rooms["parlor"].Top - (roomH / 2));
            end = new Coord(hallway.Left, Rooms["parlor"].Top + (roomH / 2));
            CreateRoom(start, end, "bathroom", RoomTypes.Bathroom, Directions.East);

            //dining room
            roomH = (Rooms["hallway"].Area.Height - 3) / 2;
            roomW = roomH;
            start = new Coord(Rooms["hallway"].Right, Rooms["hallway"].Bottom - roomH - 1);
            end = new Coord(Rooms["hallway"].Right + roomW, Rooms["hallway"].Bottom);
            CreateRoom(start, end, "dining", RoomTypes.DiningRoom, Directions.West);

            //kitchen
            start = new Coord(Rooms["hallway"].Right, Rooms["dining"].Top - roomH + 1);
            end = new Coord(Rooms["hallway"].Right + roomW, Rooms["dining"].Top + 1);
            CreateRoom(start, end, "kitchen", RoomTypes.Kitchen, Directions.West);
            CreateArch(Rooms["dining"], Rooms["kitchen"]);

            //kids room 1
            roomH = (Rooms["hallway"].Area.Height - 3) / 2;
            roomW = roomH;
            start = new Coord(Rooms["hallway"].Right, Rooms["kitchen"].Top - roomH + 1);
            end = new Coord(Rooms["hallway"].Right + roomW, Rooms["kitchen"].Top + 1);
            CreateRoom(start, end, "kids_bedroom_1", RoomTypes.Bedroom, Directions.None);
            Map.SetTerrain(Terrain.Door(new Coord(Rooms["kids_bedroom_1"].Left, Rooms["kids_bedroom_1"].Bottom - 1)));

            //kids room 2
            start = new Coord(Rooms["hallway"].Left - roomW, Rooms["master_bed"].Top - roomH);
            end = new Coord(Rooms["hallway"].Left, Rooms["master_bed"].Top);
            CreateRoom(start, end, "kids_bedroom_2", RoomTypes.Bedroom, Directions.None);
            Map.SetTerrain(Terrain.Door(new Coord(Rooms["kids_bedroom_2"].Right, Rooms["kids_bedroom_2"].Bottom - 1)));

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
            CreateRoom(start, end, "parlor", RoomTypes.Parlor);

            //Kids room
            roomW = RandomRoomDimension();
            roomH = RandomRoomDimension();
            start = new Coord(Rooms["parlor"].Left - roomW, midY - roomH);
            end = new Coord(Rooms["parlor"].Left, midY);
            CreateRoom(start, end, "kids_bedroom_1", RoomTypes.Bedroom, Directions.None);

            //kids closet 1
            start = new Coord(Rooms["kids_bedroom_1"].Left, midY - 2);
            end = new Coord(Rooms["kids_bedroom_1"].Left + 2, midY);
            CreateRoom(start, end, "kids_closet_1", RoomTypes.Closet, Directions.East);

            //other kids room
            start = new Coord(Rooms["parlor"].Left - roomW, midY + 2);
            end = new Coord(Rooms["parlor"].Left, midY + roomH + 2);
            CreateRoom(start, end, "kids_bedroom_2", RoomTypes.Bedroom, Directions.None);

            //kids closet 
            start = new Coord(Rooms["kids_bedroom_2"].Left, midY + 2);
            end = new Coord(Rooms["kids_bedroom_2"].Left + 2, midY + 4);
            CreateRoom(start, end, "kids_closet_2", RoomTypes.Closet, Directions.East);

            //central hallway
            start = new Coord(Rooms["kids_bedroom_1"].Left, midY);
            end = new Coord(Rooms["parlor"].Left + 3, midY + 2);
            CreateRoom(start, end, "hallway", RoomTypes.Parlor, Directions.All);
            CreateArch(Rooms["parlor"], Rooms["hallway"]);

            //hall closet
            start = new Coord(Left, midY);
            end = new Coord(Left + 2, midY + 2);
            CreateRoom(start, end, "closet_1", RoomTypes.Closet, Directions.East);

            //bathroom
            start = new Coord(Rooms["parlor"].Left, Rooms["parlor"].Top);
            end = new Coord(Rooms["parlor"].Left + _minRoomSize, midY - 1);
            CreateRoom(start, end, "guest_bathroom", RoomTypes.Bathroom, Directions.South);

            //kitchen
            roomW = RandomRoomDimension();
            roomH = RandomRoomDimension();
            start = new Coord(midX + 2, midY - roomH);
            end = new Coord(midX + 2 + roomW, midY);
            CreateRoom(start, end, "kitchen", RoomTypes.Kitchen, Directions.South, true);
            CreateArch(Rooms["parlor"], Rooms["kitchen"]);

            //dining
            roomW = RandomRoomDimension();
            roomH = RandomRoomDimension();
            start = new Coord(Rooms["kitchen"].Right - roomW, midY);
            end = new Coord(Rooms["kitchen"].Right, midY + roomH);
            CreateRoom(start, end, "dining", RoomTypes.DiningRoom, Directions.None, true);
            CreateArch(Rooms["parlor"], Rooms["dining"]);

            //master bed
            roomW = RandomRoomDimension();
            roomH = RandomRoomDimension();
            start = new Coord(Rooms["kitchen"].Left - roomW, Rooms["parlor"].Top - roomH);
            end = new Coord(Rooms["kitchen"].Left, Rooms["parlor"].Top);
            CreateRoom(start, end, "master_bed", RoomTypes.MasterBedroom, Directions.South, true);
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
            List<Coord> perimeter = Calculate.BorderLocations(yard);

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
                        Terrain floor;
                        switch (type)
                        {
                            case RoomTypes.Closet:
                            case RoomTypes.MasterBedroom:
                            case RoomTypes.Bedroom: floor = Terrain.Carpet(location); break;
                            case RoomTypes.Kitchen:
                            case RoomTypes.Bathroom: floor = Terrain.Linoleum(location); break;
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
                    case Directions.West:
                        possible = Calculate.PointsAlongLine(new Coord(room.Left, room.Top + 2), new Coord(room.Left, room.Bottom - 2)).ToList();
                        break;
                    case Directions.South: 
                        possible = Calculate.PointsAlongLine(new Coord(room.Left + 2, room.Bottom), new Coord(room.Right - 2, room.Bottom)).ToList();
                        break;
                    case Directions.East: 
                        possible = Calculate.PointsAlongLine(new Coord(room.Right, room.Top + 2), new Coord(room.Right, room.Bottom - 2)).ToList(); 
                        break;
                    case Directions.North: 
                        possible = Calculate.PointsAlongLine(new Coord(room.Left + 2, room.Top), new Coord(room.Right - 2, room.Top)).ToList(); 
                        break;
                    default: possible = new List<Coord>(); break;
                }
                Coord doorLoc;
                if (possible.Count == 0)
                    doorLoc = new Coord(room.Right, room.Bottom);
                else
                {
                    doorLoc = possible.RandomItem();
                    while (!Map.Contains(doorLoc))
                        doorLoc = possible.RandomItem();
                }

                Map.SetTerrain(Terrain.Door(doorLoc));
            }

            Rooms.Add(room.Name, room);
        }
    }
}
