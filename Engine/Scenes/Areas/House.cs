using Engine.Utilities.Extensions;
using GoRogue;
using System;
using System.Collections.Generic;
using System.Linq;
using Rectangle = GoRogue.Rectangle;

namespace Engine.Scenes.Areas
{
    //requires refactor after alpha
    public class House : Area
    {
        //public BasicMap Map;
        private const int MinRoomSize = 4;
        //private const int MaxRoomSize = 7;
        // ReSharper disable once InconsistentNaming
        private const int _width = 32;
        // ReSharper disable once InconsistentNaming
        private const int _height = 32;
        private readonly HouseType _structureType;
        
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
        public IEnumerable<Coord> Floor
        {
            get
            {
                foreach (Area area in SubAreas.Values)
                    foreach (Coord point in area.InnerPoints)
                        yield return point;

            }
        }

        public House(string name, Coord origin, HouseType type) :
            base(
                name ?? origin.X.ToString() + origin.Y.ToString(),
                se: origin + new Coord(_width, _height),
                ne: origin + new Coord(_width, 0),
                nw: origin,
                sw: origin + new Coord(0, _height)
            )
        {
            _structureType = type;
            Origin = origin;
            //Map = new BasicMap(25, 25, 1, Distance.MANHATTAN);

            
            //Generate();
        }

        public void Generate()
        {
            switch (_structureType)
            {
                case HouseType.PrairieHome: CreatePrairieHome(); break;
                case HouseType.Backrooms: CreateBackrooms(); break;
                //case HouseType.CentralPassageHouse: CreateCentralPassageHouse(); break;
            }

            foreach (KeyValuePair<Enum, Area> room in SubAreas)
            {
                //ConnectRoomToNeighbors(room.Value);
                ConnectRoomOnAllSides(room.Value);
            }

            //Draw();
        }

        private void CreateBackrooms()
        {
            //Map = new BasicMap(Map.Width, Map.Height, 1, Distance.EUCLIDEAN);
            Rectangle wholeHouse = new Rectangle(0, 0, _width, _height); //six is the magic number for some reason
            List<Rectangle> rooms = wholeHouse.RecursiveBisect(MinRoomSize).ToList();

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

            //Coord[] doors =
            //{
            //    EastBoundary.RandomItem(),
            //    SouthBoundary.RandomItem(),
            //    NorthBoundary.RandomItem(),
            //    WestBoundary.RandomItem(),
            //};

            //foreach (Coord door in doors)
            //{
            //    Map.SetTerrain(factory.Door(door - Origin));
            //}
        }
        public void Connect()
        {
            foreach (var room in SubAreas)
            {
                if((RoomType)room.Key != RoomType.Kitchen && (RoomType)room.Key != RoomType.Parlor)
                    ConnectRoomToNeighbors(room.Value);
            }
            ConnectRoomOnAllSides(this[RoomType.Parlor]);
            ConnectRoomOnAllSides(this[RoomType.Kitchen]);
        }
        private void ConnectRoomOnAllSides(Area area)
        {
            Coord door = area.NorthBoundary[area.NorthBoundary.Count / 2];
            area.Connections.Add(door);
            area.OuterPoints.Remove(door);

            door = area.WestBoundary[area.WestBoundary.Count / 2];
            area.Connections.Add(door);
            area.OuterPoints.Remove(door);

            door = area.EastBoundary[area.EastBoundary.Count / 2];
            area.Connections.Add(door);
            area.OuterPoints.Remove(door);

            door = area.SouthBoundary[area.SouthBoundary.Count / 2];
            area.Connections.Add(door);
            area.OuterPoints.Remove(door);
        }
        private void CreatePrairieHome()
        {
            Rectangle wholeHouse = new Rectangle(0, 0, _width - 12, _height / 2); 
            List<Rectangle> rooms = wholeHouse.RecursiveBisect(MinRoomSize).ToList();

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
            Area room = AreaFactory.FromRectangle(type + ", " + Name, plan);
            SubAreas.Add(type, room);
        }

        public override Area Rotate(float degrees, bool doToSelf, Coord origin = default)
        {
            var house = (House)base.Rotate(degrees, doToSelf, origin);
            //house.Connect();
            return house;
        }
    }
}
