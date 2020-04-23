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
        public List<Region> Regions { get => new List<Region>().Concat(Roads).ToList().Concat(Blocks).ToList().Concat(Houses).ToList().Concat(Intersections.Values).ToList(); }
        public List<Road> Roads { get => HorizontalRoads.Values.ToList().Concat(VerticalRoads.Values).ToList(); }
        public Dictionary<string, RoadIntersection> Intersections { get; private set; } = new Dictionary<string, RoadIntersection>();
        public Dictionary<RoadNumbers, Road> HorizontalRoads { get; private set; } = new Dictionary<RoadNumbers, Road>();
        public Dictionary<RoadNames, Road> VerticalRoads { get; private set; } = new Dictionary<RoadNames, Road>();
        internal List<Block> Blocks { get; private set; } = new List<Block>();
        internal List<Structure> Houses { get; private set; } = new List<Structure>();
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
            int offset = Calculate.Percent();
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
                    Intersections.Add(ri.Name, ri);
                }
            }

            foreach (Road ro in HorizontalRoads.Values)
            {
                for (int j = 0; j < ro.Intersections.Count - 1; j++)
                {
                    Road r = HorizontalRoads[Calculate.EnumValueFromIndex<RoadNumbers>(j)];
                    RoadIntersection sw = r.Intersections[j];
                    RoadIntersection se = r.Intersections[j + 1];
                    r = HorizontalRoads[Calculate.EnumValueFromIndex<RoadNumbers>(j + 1)];
                    RoadIntersection nw = r.Intersections[j];
                    RoadIntersection ne = r.Intersections[j + 1];
                    Blocks.Add(new Block(nw, sw, se, ne));
                }
            }
            foreach (Road r in Roads)
            {
                foreach (Coord c in r.InnerPoints)
                    if (this.Contains(c))
                        SetTerrain(TerrainFactory.Pavement(c));

                foreach (RoadIntersection i in r.Intersections)
                    foreach (Coord c in i.OuterPoints)
                        if (this.Contains(c))
                            SetTerrain(TerrainFactory.TestSquare1(c));

            }

            foreach (Block block in Blocks)
            {
                foreach (Coord c in block.OuterPoints)
                    if (this.Contains(c))
                        SetTerrain(TerrainFactory.TestSquare2(c));
                foreach (Coord c in block.GetFenceLocations())
                    if (this.Contains(c))
                        SetTerrain(TerrainFactory.Fence(c));
            }
        }
        private void MakeHouses()
        {
            Structure house;
            foreach (Block block in Blocks)
            {
                for (int i = 0; i < block.WestBoundary.Count - 48; i+=48)
                {
                    house = new Structure(48, 48, block.WestBoundary[i] + new Coord(12,0), StructureTypes.CentralPassageHouse);
                    Houses.Add(house);
                    for (int j = 0; j < 48; j++)
                    {
                        for (int k = 0; k < 48; k++)
                        {
                            Coord c = new Coord(j, k);
                            if(house.Map.GetTerrain(c) != null)
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
