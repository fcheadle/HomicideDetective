using System.Collections.Generic;
using Engine;
using System.Linq;
using GoRogue;
using Microsoft.Xna.Framework;
using SadConsole;
using Engine.Entities;
using Engine.Extensions;

namespace Engine.Maps
{
    internal class TownMap : BasicMap
    {
        private int _width;
        private int _height;
        public List<Area> Regions 
        { 
            get => new List<Area>()
                .Concat(Roads)
                .Concat(Blocks)
                .Concat(Houses)
                .Concat(Intersections.Values)
                .Concat(Rooms).ToList()
                .ToList()
                ; 
        }
        internal List<Road> Roads { get => HorizontalRoads.Values.ToList().Concat(VerticalRoads.Values).ToList(); }
        internal Dictionary<KeyValuePair<RoadNumbers, RoadNames>, RoadIntersection> Intersections { get; private set; } = new Dictionary<KeyValuePair<RoadNumbers, RoadNames>, RoadIntersection>();
        internal Dictionary<RoadNumbers, Road> HorizontalRoads { get; private set; } = new Dictionary<RoadNumbers, Road>();
        internal Dictionary<RoadNames, Road> VerticalRoads { get; private set; } = new Dictionary<RoadNames, Road>();
        internal List<Block> Blocks { get; private set; } = new List<Block>();
        internal List<House> Houses { get; private set; } = new List<House>();
        internal List<Room> Rooms { get; private set; } = new List<Room>();
        public FOVVisibilityHandler FovVisibilityHandler { get; }
        internal TownMap(int width, int height) : base(width, height, Calculate.EnumLength<MapLayers>(), Distance.MANHATTAN)
        {
            _width = width;
            _height = height;
            FovVisibilityHandler = new DefaultFOVVisibilityHandler(this, ColorAnsi.BlackBright);
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
                foreach (Coord c in block.WestBoundary)
                    if (this.Contains(c))
                        SetTerrain(TerrainFactory.Test(4, c));
                foreach (Coord c in block.NorthBoundary)
                    if (this.Contains(c))
                        SetTerrain(TerrainFactory.Test(5, c));
                foreach (Coord c in block.EastBoundary)
                    if (this.Contains(c))
                        SetTerrain(TerrainFactory.Test(6, c));
                foreach (Coord c in block.SouthBoundary)
                    if (this.Contains(c))
                        SetTerrain(TerrainFactory.Test(3, c));
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
            int houseDistance = 20;
            House house;
            foreach (Block block in Blocks)
            {
                int i = 1;
                foreach (Coord houseOrigin in block.Addresses)
                {
                    string address = block.Name[0] + "0" + i.ToString() + block.Name.Substring(9);
                    house = new House(houseOrigin, Calculate.RandomEnumValue<HouseTypes>(), address/*, Direction.Types.RIGHT*/);
                    
                    Houses.Add(house);
                    i++;
                    
                    foreach(Room r in house.Rooms.Values)
                    {
                        Rooms.Add((Room)r.Shift());
                    }
                    
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

                    foreach (Coord c in house.OuterPoints)
                        if (this.Contains(c))
                            SetTerrain(TerrainFactory.Test(8, c));
                }
            }
        }
        private void MakePeople()
        {
            for (int i = 0; i < 500; i++)
            {
                AddEntity(CreatureFactory.Person(new Coord(Settings.Random.Next(Width), Settings.Random.Next(Height))));
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
