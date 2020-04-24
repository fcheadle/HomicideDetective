using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Creatures;
using GoRogue;
using GoRogue.GameFramework;
using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Maps;

namespace Engine.Maps
{
    internal class TownMap : BasicMap
    {
        private int _width;
        private int _height;
        public List<Region> Regions 
        { 
            get => new List<Region>()
                .Concat(Roads).ToList()
                .Concat(Blocks).ToList()
                .Concat(Houses).ToList()
                .Concat(Intersections.Values).ToList()
                .Concat(Rooms).ToList()
                ; }
        internal List<Road> Roads { get => HorizontalRoads.Values.ToList().Concat(VerticalRoads.Values).ToList(); }
        internal Dictionary<KeyValuePair<RoadNumbers, RoadNames>, RoadIntersection> Intersections { get; private set; } = new Dictionary<KeyValuePair<RoadNumbers, RoadNames>, RoadIntersection>();
        internal Dictionary<RoadNumbers, Road> HorizontalRoads { get; private set; } = new Dictionary<RoadNumbers, Road>();
        internal Dictionary<RoadNames, Road> VerticalRoads { get; private set; } = new Dictionary<RoadNames, Road>();
        internal List<Block> Blocks { get; private set; } = new List<Block>();
        internal List<Structure> Houses { get; private set; } = new List<Structure>();
        internal List<Room> Rooms { get; private set; } = new List<Room>();

        internal TownMap(int width, int height) : base(width, height, Calculate.EnumLength<MapLayers>(), Distance.MANHATTAN)
        {
            _width = width;
            _height = height;
            MakeOutdoors();
            MakeRoadsAndBlocks();
            MakeHouses();
            //MakePeople();
        }
        private void MakeRoadsAndBlocks()
        {
            RoadNames roadName = 0;
            RoadNumbers roadNum = 0;
            int offset = (Calculate.Percent() - 50) * 2;
            Road road;
            for (int i = 16; i < _width - 48; i += 96)
            {
                road = new Road(new Coord(-100, i), new Coord(_width + 100, i + offset), roadNum);
                HorizontalRoads.Add(roadNum, road);
                roadNum++;

                road = new Road(new Coord(i, -100), new Coord(i - offset, _height + 100), roadName);
                VerticalRoads.Add(roadName, road);
                roadName++;
            }
            foreach (Road hRoad in HorizontalRoads.Values)
            {
                foreach (Road vRoad in VerticalRoads.Values)
                {
                    List<Coord> overlap = vRoad.Overlap(hRoad).ToList();
                    RoadIntersection ri = new RoadIntersection(hRoad.StreetNumber, vRoad.StreetName, overlap);
                    vRoad.AddIntersection(ri);
                    hRoad.AddIntersection(ri);
                    Intersections.Add(new KeyValuePair<RoadNumbers, RoadNames>(hRoad.StreetNumber, vRoad.StreetName), ri);
                }
            }

            for(int i = 0; i < HorizontalRoads.Count - 1; i++)
            {
                Road h = HorizontalRoads[Calculate.EnumValueFromIndex<RoadNumbers>(i)];
                for (int j = 0; j < h.Intersections.Count - 1; j++)
                {
                    RoadIntersection nw = h.Intersections[j];
                    RoadIntersection ne = h.Intersections[j + 1];
                    Road r = HorizontalRoads[Calculate.EnumValueFromIndex<RoadNumbers>(i + 1)];
                    RoadIntersection sw = r.Intersections[j];
                    RoadIntersection se = r.Intersections[j + 1];
                    Blocks.Add(new Block(nw, sw, se, ne));
                }
            }
            foreach (Road r in Roads)
            {
                foreach (Coord c in r.InnerPoints)
                    if (this.Contains(c))
                        SetTerrain(TerrainFactory.Pavement(c));
            }

            foreach (Block block in Blocks)
            {
                //foreach (Coord c in block.OuterPoints)
                //    if (this.Contains(c))
                //        SetTerrain(TerrainFactory.TestSquare0(c));
                //foreach (Coord c in block.WestBoundary)
                //    if (this.Contains(c))
                //        SetTerrain(TerrainFactory.TestSquare1(c));
                //foreach (Coord c in block.EastBoundary)
                //    if (this.Contains(c))
                //        SetTerrain(TerrainFactory.TestSquare2(c));
                //foreach (Coord c in block.WestBoundary)
                //    if (this.Contains(c))
                //        SetTerrain(TerrainFactory.TestSquare3(c));
                //foreach (Coord c in block.WestBoundary)
                //    if (this.Contains(c))
                //        SetTerrain(TerrainFactory.TestSquare4(c));
                foreach (Coord c in block.GetFenceLocations())
                    if (this.Contains(c))
                        SetTerrain(TerrainFactory.Fence(c));
            }
            //foreach (RoadIntersection ri in Intersections.Values)
            //    foreach (Coord c in ri.OuterPoints)
            //        if (this.Contains(c))
            //            SetTerrain(TerrainFactory.TestSquare5(c));
        }
        private void MakeHouses()
        {
            int houseDistance = 25;
            Structure house;
            foreach (Block block in Blocks)
            {
                    for (int i = 0; i < block.WestBoundary.Count - houseDistance; i += houseDistance)
                    {
                        string address = block.Name[0] + "0" + ((i / houseDistance) * 2).ToString() + block.Name.Substring(9);
                        house = new Structure(houseDistance, houseDistance, block.WestBoundary[i] + new Coord(12, 8), StructureTypes.CentralPassageHouse, address);
                        foreach(Room room in house.Rooms.Values)
                        {
                            Coord start = new Coord(house.Origin.X + room.Left, house.Origin.Y + room.Top);
                            Coord stop = new Coord(house.Origin.X + room.Right, house.Origin.Y + room.Bottom);
                            //Rooms.Add(new Room(room.Name, new GoRogue.Rectangle(start, stop), room.Type));
                        }


                    int chance = Calculate.Percent();
                    if(chance < 50)
                    {
                        house.Map = house.Map.SwapXY();
                        house.Map = house.Map.ReverseHorizontal();
                    }
                    else
                    {
                        house.Map = house.Map.Rotate(90);
                    }


                    Houses.Add(house);

                    for (int j = 0; j < houseDistance; j++)
                    {
                        for (int k = 0; k < houseDistance; k++)
                        {
                            Coord c = new Coord(j, k);
                            if (house.Map.GetTerrain(c) != null && this.Contains(house.Origin + c))
                            {
                                SetTerrain(TerrainFactory.Copy(house.Map.GetTerrain<BasicTerrain>(c), house.Origin + c));
                            }
                        }
                    }
                }
            }
        }
        private void MakePeople()
        {
            for (int i = 0; i < 500; i++)
            {
                AddEntity(Creature.Person(new Coord(Settings.Random.Next(Width), Settings.Random.Next(Height))));
            }
            //for (int i = 0; i < 300; i++)
            //{
            //    AddEntity(Creature.Animal(new Coord(Settings.Random.Next(Width), Settings.Random.Next(Height))));
            //}
        }
        private void MakeOutdoors()
        {
            //for (int i = 0; i < 777; i++)
            //{
            //    Coord tree = new Coord(Settings.Random.Next(Width), Settings.Random.Next(Height));
            //    while (GetTerrain<BasicTerrain>(tree) != null)
            //    {
            //        tree = new Coord(Settings.Random.Next(Width), Settings.Random.Next(Height));
            //    }
            //    SetTerrain(TerrainFactory.Tree(tree));
            //}
         
            var f = Calculate.MasterFormula();
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    Coord pos = new Coord(i, j);
                    SetTerrain(TerrainFactory.Grass(pos, f(i, j))) ;
                }
            }
        }
    }
}
