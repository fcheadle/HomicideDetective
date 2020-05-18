﻿using Engine.Entities;
using Engine.Extensions;
using Engine.Maps.Areas;
using GoRogue;
using SadConsole;
using System;
using System.Collections.Generic;
using System.Linq;
using Rectangle = GoRogue.Rectangle;

namespace Engine.Maps
{

    //TODO:
    //Need to rewrite houses entirely
    //first, get the tests passing
    //then, just delete all house stuff and start from scratch
    public class House : Area
    {
        public BasicMap Map;
        new internal Coord Origin;
        private const int _minRoomSize = 3;
        private const int _maxRoomSize = 7;
        private readonly HouseTypes StructureType;
        private readonly Direction.Types _facing;
        private Coord _se;
        private Coord _ne;
        private Coord _nw;
        private Coord _sw;

        private Area Parlor { get => SubAreas[RoomType.Parlor]; }
        private Area Hallway { get => SubAreas[RoomType.Hall]; }
        private Area Kitchen { get => SubAreas[RoomType.Kitchen]; }
        private Area GuestBathroom { get => SubAreas[RoomType.GuestBathroom]; }
        private Area MasterBedroom { get => SubAreas[RoomType.MasterBedroom]; }
        private Area MasterBathroom { get => SubAreas[RoomType.MasterBathroom]; }
        private Area MasterBedCloset { get => SubAreas[RoomType.MasterBedCloset]; }
        private Area BoysBedroom { get => SubAreas[RoomType.BoysBedroom]; }
        private Area BoysCloset { get => SubAreas[RoomType.BoysCloset]; }
        private Area GirlsBedroom { get => SubAreas[RoomType.GirlsBedroom]; }
        private Area GirlsCloset { get => SubAreas[RoomType.GirlsCloset]; }
        private Area DiningRoom { get => SubAreas[RoomType.DiningRoom]; }
        private Area HallCloset { get => SubAreas[RoomType.HallCloset]; }
        private Area ParlorCloset { get => SubAreas[RoomType.ParlorCloset]; }
        private List<Area> Bedrooms { get => new List<Area>() { SubAreas[RoomType.MasterBedroom], SubAreas[RoomType.BoysBedroom], SubAreas[RoomType.GirlsBedroom], SubAreas[RoomType.GuestBedroom] }; }
        private List<Area> Closets { get => new List<Area>() { SubAreas[RoomType.MasterBedCloset], SubAreas[RoomType.BoysCloset], SubAreas[RoomType.GirlsCloset], SubAreas[RoomType.GuestCloset] }; }
        private List<Area> Bathrooms { get => new List<Area>() { SubAreas[RoomType.MasterBathroom], SubAreas[RoomType.GuestBathroom] }; }
        internal string Address { get; set; }

        public House(Coord origin, HouseTypes type, string name = null, Direction.Types facing = Direction.Types.DOWN): base(
            name ?? origin.X.ToString() + origin.Y.ToString(),
            new Coord(24, 24) + origin,
            new Coord(24, 0) + origin,
            origin,
            new Coord(0,24) + origin
            )
        {
            StructureType = type;
            Map = new BasicMap(25, 25, 1, Distance.MANHATTAN);
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

            AddConnectionBetween(Hallway, MasterBedroom);
            //AddConnectionBetween(Hallway, BoysBedroom);
            //AddConnectionBetween(Hallway, GirlsBedroom);
            //AddConnectionBetween(Hallway, DiningRoom);
            //AddConnectionBetween(Hallway, Kitchen);
            //AddDoorBetween(Hallway, GuestBathroom);

            //AddDoorBetween(GuestBathroom, MasterBedroom);
            DistinguishSubAreas();
            Map.SetTerrain(TerrainFactory.Door(new Coord(Hallway.Left + 1, Hallway.Bottom)));
            Map.SetTerrain(TerrainFactory.Door(new Coord(MasterBedCloset.Left + 1, MasterBedCloset.Top)));
            Map.SetTerrain(TerrainFactory.Door(new Coord(BoysCloset.Left, BoysCloset.Top + 1)));
            Map.SetTerrain(TerrainFactory.Door(new Coord(GirlsCloset.Right, GirlsCloset.Top+1)));
            Map.SetTerrain(TerrainFactory.Door(new Coord(ParlorCloset.Right - 1, ParlorCloset.Bottom)));
        }

        private void AddDoorBetween(Area hallway, Area area)
        {
            List<Coord> overlap = hallway.OuterPoints.Where(point => area.OuterPoints.Contains(point)).ToList();
            Coord point = overlap.RandomItem();

            if(Map.Contains(point))
                Map.SetTerrain(TerrainFactory.Door(point));
        }

        private void CreateCentralPassageHouse()
        {
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
            area = new Rectangle(new Coord(mid - roomW - (roomW/2), mid - tempH), new Coord(mid - roomW/2, mid - tempH + roomH));
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

            CreateArch(DiningRoom, Kitchen);

            if (Calculate.Percent() % 2 == 1)
                Map = Map.ReverseHorizontal();

            //first bathroom then hall
            //area = new Rectangle(new Coord(mid - roomW - 5, mid - 2), new Coord(mid - 3, mid + 2));
            //SetCorners(area);
            //SubAreas.Add(RoomType.GuestBathroom, new Area(RoomType.GuestBathroom.ToString() + suffix, _se, _ne, _nw, _sw));

            area = new Rectangle(new Coord(mid - roomH/2, mid - roomW), new Coord(mid + roomH/ 2, mid + roomH));
            SetCorners(area);
            SubAreas.Add(RoomType.Hall, new Area(RoomType.Hall.ToString() + suffix, _se, _ne, _nw, _sw));

            CreateArch(Parlor, Hallway);

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
        }

        private void SetCorners(Rectangle area)
        {
            int xMax = area.MaxExtentX/* + Run*/;
            int xMin = area.MinExtentX;
            int yMax = area.MaxExtentY /*+ Rise*/;
            int yMin = area.MinExtentY;
            _se = new Coord(xMax, yMax);
            _ne = new Coord(xMax, yMin);
            _nw = new Coord(xMin, yMin);
            _sw = new Coord(xMin, yMax);
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
        public int RandomRoomDimension()
        {
            return Settings.Random.Next(_minRoomSize, _maxRoomSize);
        }
        public void CreateArch(Area host, Area imposing)
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
