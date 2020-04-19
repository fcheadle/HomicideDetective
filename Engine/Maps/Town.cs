using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Creatures;
using GoRogue;
using GoRogue.GameFramework;
using Microsoft.Xna.Framework;
using SadConsole;

namespace Engine.Maps
{
    internal class Town
    {
        private int _width;
        private int _height;
        private BasicMap _map;

        internal Dictionary<RoadNumbers, Road> HorizontalRoads { get; private set; } = new Dictionary<RoadNumbers, Road>();
        internal Dictionary<RoadNames, Road> VerticalRoads { get; private set; } = new Dictionary<RoadNames, Road>();
        internal List<Block> Blocks { get; private set; } = new List<Block>();
        internal List<Structure> Houses { get; private set; } = new List<Structure>();
        internal BasicMap Map { get => _map; }
        internal Town(int width, int height)
        {
            _width = width;
            _height = height;
            _map = new BasicMap(width, height, Calculate.EnumLength<MapLayers>(), Distance.MANHATTAN);
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
            for (int i = 0; i < _width; i += 96)
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
                    vRoad.AddIntersection(hRoad);
                    hRoad.AddIntersection(vRoad);
                    foreach (Coord c in vRoad.InnerPoints)
                        if (Map.Contains(c))
                            Map.SetTerrain(TerrainFactory.Pavement(c));
                }

                foreach (Coord c in hRoad.InnerPoints)
                    if (Map.Contains(c))
                        Map.SetTerrain(TerrainFactory.Pavement(c));
            
            }
            for (int i = 0; i < HorizontalRoads.Count() - 2; i++)
            {
                Road r = HorizontalRoads[Calculate.EnumValueFromIndex<RoadNumbers>(i)];
                for (int j = 0; j < r.Intersections.Count - 2; j++)
                {
                    RoadIntersection sw = r.Intersections[j];
                    RoadIntersection se = r.Intersections[j + 1];
                    r = HorizontalRoads[Calculate.EnumValueFromIndex<RoadNumbers>(i + 1)];
                    RoadIntersection nw = r.Intersections[j];
                    RoadIntersection ne = r.Intersections[j + 1];
                    Blocks.Add(new Block(nw,sw,se,ne));
                }
            }
        }
        private void MakeHouses()
        {
            Structure house;
            foreach (Block block in Blocks)
            {
                foreach (Coord c in block.GetFenceLocations())
                {
                    if(Map.Contains(c))
                        Map.SetTerrain(TerrainFactory.Fence(c));
                }
            }
        }
        private void MakePeople()
        {
            for (int i = 0; i < 500; i++)
            {
                Map.AddEntity(Creature.Person(new Coord(Settings.Random.Next(Map.Width), Settings.Random.Next(Map.Height))));
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
                Coord tree = new Coord(Settings.Random.Next(Map.Width), Settings.Random.Next(Map.Height));
                while (Map.GetTerrain<BasicTerrain>(tree) != null)
                {
                    tree = new Coord(Settings.Random.Next(Map.Width), Settings.Random.Next(Map.Height));
                }
                Map.SetTerrain(TerrainFactory.Tree(tree));
            }
         
            var f = Calculate.MasterFormula();
            for (int i = 0; i < Map.Width; i++)
            {
                for (int j = 0; j < Map.Height; j++)
                {
                    Coord pos = new Coord(i, j);
                    if (Map.GetTerrain<BasicTerrain>(pos) == null)
                    {

                        BasicTerrain tile;
                        double z = f(i, j);

                        Color foreground = Color.Green;
                        for (double k = 0; k < z; k++)
                            foreground = Colors.Brighten(foreground);
                        for (double k = z; k < 0; k++)
                            foreground = Colors.Darken(foreground);

                        if (i == 0 || i == Map.Width - 1 || j == 0 || j == Map.Height - 1)
                        {
                            tile = Maps.TerrainFactory.Wall(pos);
                        }
                        else
                        {
                            tile = Maps.TerrainFactory.Grass(pos, z);
                        }
                        Map.SetTerrain(tile);
                    }
                }
            }

        }
    }
}
