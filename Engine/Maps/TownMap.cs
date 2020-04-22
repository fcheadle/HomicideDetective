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
        public List<Region> Regions { get => new List<Region>().Concat(Roads).ToList().Concat(Blocks).ToList().Concat(Houses).ToList(); }
        public List<Road> Roads { get => HorizontalRoads.Values.ToList().Concat(VerticalRoads.Values).ToList(); }
        public Dictionary<RoadNumbers, Road> HorizontalRoads { get; private set; } = new Dictionary<RoadNumbers, Road>();
        public Dictionary<RoadNames, Road> VerticalRoads { get; private set; } = new Dictionary<RoadNames, Road>();
        internal List<Block> Blocks { get; private set; } = new List<Block>();
        internal List<Structure> Houses { get; private set; } = new List<Structure>();
        internal TownMap(int width, int height) : base(width, height, Calculate.EnumLength<MapLayers>(), Distance.MANHATTAN)
        {
            _width = width;
            _height = height;
            MakeRoadsAndBlocks();
            MakeHouses();
            MakeOutdoors();
            MakePeople();
        }
        private void MakeRoadsAndBlocks()
        {
            RoadNames roadName = 0;
            RoadNumbers roadNum = 0;
            Road road;
            for (int i = 0; i < _width; i += 96 )
            {
                road = new Road(new Coord(-100, i), new Coord(_width + 100,  (_height/9) + i), roadNum);
                HorizontalRoads.Add(roadNum, road);       
                roadNum++;

                road = new Road(new Coord(i, -100), new Coord(i - _width / 4, _height + 100), roadName);
                VerticalRoads.Add(roadName, road);
                roadName++;
            }
            foreach (Road hRoad in HorizontalRoads.Values)
            {
                foreach (Road vRoad in VerticalRoads.Values)
                {
                    RoadIntersection ri = new RoadIntersection(hRoad.StreetNumber, vRoad.StreetName, vRoad.Overlap(hRoad).ToList());
                    vRoad.AddIntersection(ri);
                    hRoad.AddIntersection(ri);
                }
            }

            foreach(Road ro in HorizontalRoads.Values)
            {
                for (int j = 0; j < ro.Intersections.Count-1; j++)
                {
                    Road r = HorizontalRoads[Calculate.EnumValueFromIndex<RoadNumbers>(j)];
                    RoadIntersection sw = r.Intersections[j];
                    RoadIntersection se = r.Intersections[j + 1];
                    r = HorizontalRoads[Calculate.EnumValueFromIndex<RoadNumbers>(j + 1)];
                    RoadIntersection nw = r.Intersections[j];
                    RoadIntersection ne = r.Intersections[j + 1];
                    Blocks.Add(new Block(nw,sw,se,ne));
                }
            }
            foreach (Road r in Roads)
            {
                foreach (Coord c in r.InnerPoints)
                    if (this.Contains(c))
                        SetTerrain(TerrainFactory.Pavement(c));

                foreach(RoadIntersection i in r.Intersections)
                    foreach(Coord c in i.OuterPoints)
                        if (this.Contains(c))
                            SetTerrain(TerrainFactory.TestSquare1(c));

            }

            foreach (Block block in Blocks)
                foreach(Coord c in block.OuterPoints)
                    if(this.Contains(c))
                        SetTerrain(TerrainFactory.TestSquare2(c));
        }
        private void MakeHouses()
        {
            Structure house;
            foreach (Block block in Blocks)
            {
                foreach (Coord c in block.GetFenceLocations())
                {
                    if(this.Contains(c))
                        SetTerrain(TerrainFactory.Fence(c));
                }
                //block.Populate(); //left off here
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
            for (int i = 0; i < 777; i++)
            {
                Coord tree = new Coord(Settings.Random.Next(Width), Settings.Random.Next(Height));
                while (GetTerrain<BasicTerrain>(tree) != null)
                {
                    tree = new Coord(Settings.Random.Next(Width), Settings.Random.Next(Height));
                }
                SetTerrain(TerrainFactory.Tree(tree));
            }
         
            var f = Calculate.MasterFormula();
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    Coord pos = new Coord(i, j);
                    if (GetTerrain<BasicTerrain>(pos) == null)
                    {

                        BasicTerrain tile;
                        double z = f(i, j);

                        Color foreground = Color.Green;
                        for (double k = 0; k < z; k++)
                            foreground = Colors.Brighten(foreground);
                        for (double k = z; k < 0; k++)
                            foreground = Colors.Darken(foreground);

                        if (i == 0 || i == Width - 1 || j == 0 || j == Height - 1)
                        {
                            tile = Maps.TerrainFactory.Wall(pos);
                        }
                        else
                        {
                            tile = Maps.TerrainFactory.Grass(pos, z);
                        }
                        SetTerrain(tile);
                    }
                }
            }

        }
    }
}
