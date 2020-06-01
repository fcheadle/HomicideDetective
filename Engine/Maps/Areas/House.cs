using Engine.Entities.Terrain;
using Engine.Extensions;
using Engine.Maps.Areas;
using GoRogue;
using GoRogue.Pathing;
using SadConsole;
using System;
using System.Collections.Generic;
using System.Linq;
using Rectangle = GoRogue.Rectangle;

namespace Engine.Maps
{
    public class House : Area
    {
        TerrainFactory factory = new TerrainFactory();
        public BasicMap Map;
        private const int _minRoomSize = 4;
        private const int _maxRoomSize = 7;
        private const int _width = 24;
        private const int _height = 24;
        private readonly HouseType StructureType;
        private readonly Direction.Types _facing;
        internal string Address { get => Name; }
        public Coord Origin { get; set; }
        public IEnumerable<Coord> Walls
        {
            get
            {
                foreach (Area area in SubAreas.Values)
                    foreach (Coord point in area.OuterPoints)
                        yield return point;
            }
        }

        public IEnumerable<Coord> Doors
        {
            get
            {
                foreach (Area area in SubAreas.Values)
                    foreach (Coord point in area.Connections)
                        yield return point;

            }
        }

        public House(string name, Coord origin, HouseType type, Direction.Types facing) :
            base(
                name ?? origin.X.ToString() + origin.Y.ToString(),
                se: origin + new Coord(_width, _height),
                ne: origin + new Coord(_width, 0),
                nw: origin,
                sw: origin + new Coord(0, _height)
            )
        {
            StructureType = type;
            Origin = origin;
            Map = new BasicMap(25, 25, 1, Distance.MANHATTAN);
            _facing = facing;
            //Generate();
        }

        public void Generate()
        {
            switch (StructureType)
            {
                default:
                case HouseType.PrairieHome: CreatePrairieHome(); break;
                case HouseType.Backrooms: CreateBackrooms(); break;
                    //case HouseType.CentralPassageHouse: CreateCentralPassageHouse(); break;
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
                        Map = Map.RotateDiscreet(270);
                    }
                    if (chance < 50)
                        Map = Map.ReverseVertical();

                    break;
                case Direction.Types.RIGHT:
                    Map = Map.RotateDiscreet(90);

                    if (chance < 50)
                        Map = Map.ReverseVertical();
                    break;
                case Direction.Types.UP:
                    if (chance < 50)
                    {
                        Map = Map.RotateDiscreet(180);
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
                ConnectRoomToNeighbors(room.Value);
                DrawRoom(room.Value, (RoomType)room.Key);
            }

            for (int i = 0; i < Map.Width; i++)
            {
                for (int j = 0; j < Map.Height; j++)
                {
                    Coord here = new Coord(i, j);
                    BasicTerrain terrain = Map.GetTerrain<BasicTerrain>(here);
                    if (terrain != null)
                    {
                        if (terrain.IsWalkable)
                        {
                            Path path = Map.AStar.ShortestPath(new Coord(Map.Width / 2, Map.Height / 2), here);
                            if (path == null)
                            {
                                foreach (Coord neighbor in here.Neighbors())
                                {
                                    if (Map.Contains(neighbor))
                                        if (!Map.GetTerrain<BasicTerrain>(neighbor).IsWalkable)
                                            Map.SetTerrain(factory.Door(neighbor));
                                }
                            }
                        }
                    }
                }
            }
        }

        private void CreateBackrooms()
        {
            Map = new BasicMap(Map.Width, Map.Height, 1, Distance.EUCLIDEAN);
            Rectangle wholeHouse = new Rectangle(0, 0, Map.Width, Map.Height); //six is the magic number for some reason
            List<Rectangle> rooms = wholeHouse.RecursiveBisect(_minRoomSize).ToList();

            int index = 0;
            foreach (Rectangle plan in rooms.OrderBy(r => r.Area).ToArray())
            {
                CreateRoom((RoadNumbers)index, plan);
                index++;
            }
        }

        public void ConnectRoomToNeighbors(Area subArea)
        {
            List<Area> neighbors = new List<Area>();
            foreach (Area area in SubAreas.Values)
            {
                if (subArea != area)
                {
                    foreach (Coord point in area.OuterPoints)
                    {
                        if (subArea.OuterPoints.Contains(point))
                        {
                            if (!neighbors.Contains(area))
                                neighbors.Add(area);
                        }
                    }
                }
            }
            foreach (Area area in neighbors)
            {
                AddConnectionBetween(area, subArea);
            }

            Coord[] doors =
            {
                EastBoundary.RandomItem(),
                SouthBoundary.RandomItem(),
                NorthBoundary.RandomItem(),
                WestBoundary.RandomItem(),
            };

            foreach (Coord door in doors)
            {
                Map.SetTerrain(factory.Door(door - Origin));
            }
        }

        private void CreatePrairieHome()
        {
            string suffix = ", " + Name;
            int mid = Map.Width / 2;
            int roomW = _maxRoomSize;
            int roomH = _maxRoomSize - 2;
            int tempH = roomW;
            roomW = roomW > roomH ? roomW : roomH;
            roomH = roomH < tempH ? roomH : tempH;
            tempH = Program.Settings.Random.Next(-1, 2);
            Rectangle wholeHouse = new Rectangle(0, 0, Map.Width - 6, Map.Height / 2); //six is the magic number for some reason
            List<Rectangle> rooms = wholeHouse.RecursiveBisect(_minRoomSize).ToList();

            int index = 0;
            foreach (Rectangle plan in rooms.OrderBy(r => r.Area).ToArray())
            {
                CreateRoom((RoomType)index, plan);
                index++;
            }
        }

        public void CreateRoom(Enum type, Rectangle plan)
        {
            plan = new Rectangle(Origin + plan.MinExtent, Origin + plan.MaxExtent);
            Area room = AreaFactory.FromRectangle(type.ToString() + ", " + Name, plan);
            SubAreas.Add(type, room);
        }

        /*
        private void CreateCentralPassageHouse()
        {
            throw new NotImplementedException("Central Passage Houses are just fucked up right now. Do not use.");
            string suffix = ", " + Name;
            int mid = Map.Width / 2;
            int roomW = _maxRoomSize;
            int roomH = _maxRoomSize - 2;
            int tempH = roomW;
            Rectangle area;
            roomW = roomW > roomH ? roomW : roomH;
            roomH = roomH < tempH ? roomH : tempH;
            tempH = Settings.Random.Next(-1, 2);

            //parlor
            area = new Rectangle(new Coord(mid - roomW - (roomW / 2), mid - tempH), new Coord(mid - roomW / 2, mid - tempH + roomH));
            SetCorners(area);
            SubAreas.Add(RoomType.Parlor, new Area(RoomType.Parlor.ToString() + suffix, _se, _ne, _nw, _sw));
            SubAreas.Add(RoomType.ParlorCloset, AreaFactory.Closet(RoomType.ParlorCloset.ToString() + suffix, _nw));


            //MasterBedroom
            area = new Rectangle(new Coord(Parlor.Left, Parlor.Top - roomH), new Coord(Parlor.Right, Parlor.Top));
            SetCorners(area);
            SubAreas.Add(RoomType.MasterBedroom, new Area(RoomType.MasterBedroom.ToString() + suffix, _se, _ne, _nw, _sw));
            SubAreas.Add(RoomType.MasterBedCloset, AreaFactory.Closet(RoomType.MasterBedCloset.ToString() + suffix, _sw - new Coord(0, 2)));

            //Dining & Kitchen
            tempH = roomW;
            roomW = roomH;
            roomH = tempH;
            if (Calculate.Percent() % 2 == 1)
                Map = Map.ReverseVertical();

            area = new Rectangle(new Coord(mid + 2, mid), new Coord(mid + roomW + 2, mid + roomH));
            SetCorners(area);
            SubAreas.Add(RoomType.DiningRoom, new Area(RoomType.DiningRoom.ToString() + suffix, _se, _ne, _nw, _sw));

            area = new Rectangle(new Coord(mid + 2, mid - roomH), new Coord(mid + roomW + 2, mid));
            SetCorners(area);
            SubAreas.Add(RoomType.Kitchen, new Area(RoomType.Kitchen.ToString() + suffix, _se, _ne, _nw, _sw));

            DiningRoom.RemoveOverlappingOuterpoints(Kitchen);
            Kitchen.RemoveOverlappingOuterpoints(DiningRoom);

            if (Calculate.Percent() % 2 == 1)
                Map = Map.ReverseHorizontal();

            //first bathroom then hall
            //area = new Rectangle(new Coord(mid - roomW - 5, mid - 2), new Coord(mid - 3, mid + 2));
            //SetCorners(area);
            //SubAreas.Add(RoomType.GuestBathroom, new Area(RoomType.GuestBathroom.ToString() + suffix, _se, _ne, _nw, _sw));

            area = new Rectangle(new Coord(mid - roomH / 2, mid - roomW), new Coord(mid + roomH / 2, mid + roomH));
            SetCorners(area);
            SubAreas.Add(RoomType.Hall, new Area(RoomType.Hall.ToString() + suffix, _se, _ne, _nw, _sw));

            Parlor.RemoveOverlappingOuterpoints(Hallway);
            Hallway.RemoveOverlappingOuterpoints(Parlor);

            ////kids bedrooms

            while (roomH >= Parlor.Height)
                roomH = RandomRoomDimension();
            while (roomW >= Parlor.Width)
                roomW = RandomRoomDimension();
            area = new Rectangle(new Coord(mid - roomW, mid - roomH - roomH * 2), new Coord(mid, mid - roomH));
            SetCorners(area);
            SubAreas.Add(RoomType.BoysBedroom, new Area(RoomType.BoysBedroom.ToString() + suffix, _se, _ne, _nw, _sw));
            SubAreas.Add(RoomType.BoysCloset, AreaFactory.Closet(RoomType.BoysCloset.ToString() + suffix, _ne + new Coord(1, 1)));
            area = new Rectangle(new Coord(mid, mid - roomH - roomH), new Coord(mid + roomW, mid - roomH));
            SetCorners(area);
            SubAreas.Add(RoomType.GirlsBedroom, new Area(RoomType.GirlsBedroom.ToString() + suffix, _se, _ne, _nw, _sw));
            SubAreas.Add(RoomType.GirlsCloset, AreaFactory.Closet(RoomType.GirlsCloset.ToString() + suffix, _nw + new Coord(0, 3)));
        }*/

        private void DrawRoom(Area room, RoomType type)
        {
            for (int x = room.Left + 1; x < room.Right; x++)
            {
                for (int y = room.Top + 1; y < room.Bottom; y++)
                {
                    Coord location = new Coord(x, y) - Origin;
                    if (Map.Contains(location))
                    {
                        BasicTerrain floor;
                        ////Don't delete this, I'll come back to it later
                        //switch (type)
                        //{
                        //    case RoomType.BoysBedroom:
                        //    case RoomType.GirlsBedroom:
                        //    case RoomType.HallCloset:
                        //    case RoomType.MasterBedroom:
                        //    case RoomType.GuestBedroom: floor = TerrainFactory.OliveCarpet(location); break;
                        //    case RoomType.Kitchen:
                        //    case RoomType.GuestBathroom: floor = TerrainFactory.BathroomLinoleum(location); break;
                        //    default: floor = TerrainFactory.DarkHardwoodFloor(location); break;
                        //}

                        switch ((int)type % 8)
                        {
                            default:
                            case 0: floor = factory.OliveCarpet(location); break;
                            case 1: floor = factory.LightCarpet(location); break;
                            case 2: floor = factory.ShagCarpet(location); break;
                            case 3: floor = factory.BathroomLinoleum(location); break;
                            case 4: floor = factory.KitchenLinoleum(location); break;
                            case 5: floor = factory.DarkHardwoodFloor(location); break;
                            case 6: floor = factory.MediumHardwoodFloor(location); break;
                            case 7: floor = factory.LightHardwoodFloor(location); break;
                        }
                        Map.SetTerrain(floor);
                    }
                }
            }

            foreach (Coord location in room.OuterPoints)
                if (Map.Contains(location - Origin))
                    Map.SetTerrain(factory.Wall(location - Origin));

            foreach (Coord location in room.Connections)
                if (Map.Contains(location - Origin))
                    Map.SetTerrain(factory.Door(location - Origin));

        }
        private int RandomRoomDimension()
        {
            return Program.Settings.Random.Next(_minRoomSize, _maxRoomSize);
        }
    }
}
