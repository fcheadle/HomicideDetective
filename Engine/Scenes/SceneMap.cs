using Engine.Scenes.Areas;
using Engine.Utilities;
using Engine.Utilities.Extensions;
using Engine.Utilities.Mathematics;
using GoRogue;
using Microsoft.Xna.Framework;
using SadConsole;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Scenes
{
    public class SceneMap : BasicMap
    {
        private int _width;
        private int _height;
        private float _rotationDegrees;
        public List<Area> Regions
        {
            get => new List<Area>()
                .Concat(Roads)
                .Concat(Blocks)
                .Concat(Houses)
                .Concat(Intersections.Values)
                .Concat(Rooms)
                .ToList()
                ;
        }
        internal List<Road> Roads { get => HorizontalRoads.Values.ToList().Concat(VerticalRoads.Values).ToList(); }
        internal Dictionary<KeyValuePair<RoadNumbers, RoadNames>, RoadIntersection> Intersections { get; private set; } = new Dictionary<KeyValuePair<RoadNumbers, RoadNames>, RoadIntersection>();
        internal Dictionary<RoadNumbers, Road> HorizontalRoads { get; private set; } = new Dictionary<RoadNumbers, Road>();
        internal Dictionary<RoadNames, Road> VerticalRoads { get; private set; } = new Dictionary<RoadNames, Road>();
        internal List<Block> Blocks { get; private set; } = new List<Block>();
        internal List<House> Houses { get; private set; } = new List<House>();
        internal List<Area> Rooms { get; private set; } = new List<Area>();

        public void RefreshRegion(Area area)
        {
            Area region = Regions.Where(x => x.Name == area.Name).First();
            Regions.Remove(region);
            Regions.Add(area);
            
        }

        public FOVVisibilityHandler FovVisibilityHandler { get; }
        public SceneMap(int width, int height) : base(width, height, EnumUtils.EnumLength<MapLayer>(), Distance.MANHATTAN)
        {
            _width = width;
            _height = height;
            FovVisibilityHandler = new DefaultFOVVisibilityHandler(this, ColorAnsi.BlackBright);

            MakeOutdoors();
            //MakeBackrooms();
            MakeRoadsAndBlocks();
            MakeHouses();
            MakePeople();
        }
        public void MakeBackrooms()
        {
            House backrooms = new House("Backrooms", new Coord(0, 0), HouseType.Backrooms, Direction.Types.DOWN);
            backrooms.Generate();
            Houses.Add(backrooms);
            this.ForXForY(
                (point) =>
                {
                    try
                    {
                        BasicTerrain t = backrooms.Map.GetTerrain<BasicTerrain>(point);
                        if (t != null)
                            SetTerrain(Game.TerrainFactory.Copy(t, point));
                    }
                    catch
                    {
                        //don't care
                    }

                }
            );
        }
        private void MakeRoadsAndBlocks()
        {
            RoadNames roadName = (RoadNames)Calculate.PercentValue();
            RoadNumbers roadNum = 0;
            int offset = (Calculate.PercentValue() - 50) * 2;
            _rotationDegrees = (float)(offset * 1.75); //guess?
            Road road;
            for (int i = 16; i < _width - 48; i += 96)
            {
                road = new Road(new Coord(-100, i), new Coord(_width + 100, i + offset), roadNum);
                HorizontalRoads.Add(roadNum, road);
                roadNum++;

                if ((int)roadNum % 2 == 1)
                {
                    road = new Road(new Coord(i, -100), new Coord(i - offset, _height + 100), roadName);
                    VerticalRoads.Add(roadName, road);
                    roadName++;
                }
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

            for (int i = 0; i < HorizontalRoads.Count - 1; i++)
            {
                Road h = HorizontalRoads[(RoadNumbers)i];
                for (int j = 0; j < h.Intersections.Count - 1; j++)
                {
                    RoadIntersection nw = h.Intersections[j];
                    RoadIntersection ne = h.Intersections[j + 1];
                    Road r = HorizontalRoads[(RoadNumbers)i + 1];
                    RoadIntersection sw = r.Intersections[j];
                    RoadIntersection se = r.Intersections[j + 1];
                    Blocks.Add(new Block(nw, sw, se, ne));
                }
            }
            foreach (Road r in Roads)
            {
                foreach (Coord c in r.InnerPoints)
                    if (this.Contains(c))
                        SetTerrain(Game.TerrainFactory.Floor(c));
                //SetTerrain(_terrainFactory.Pavement(c));
            }

            foreach (Block block in Blocks)
            {
                foreach (Coord c in block.GetFenceLocations())
                    if (this.Contains(c))
                        SetTerrain(Game.TerrainFactory.Generic(c, 44));
                //SetTerrain(_terrainFactory.Fence(c));
            }
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
                    string address = block.Name[0] + block.Name[1] + i.ToString() + block.Name.Substring(9);
                    house = new House(address, houseOrigin, HouseType.PrairieHome, Direction.Types.DOWN);
                    house.Generate();
                    house.Rotate(_rotationDegrees, true);
                    house.Draw();
                    Houses.Add(house);
                    i++;

                    foreach (Area room in house.SubAreas.Values)
                    {
                        Rooms.Add(room);
                    }

                    for (int j = 0; j < houseDistance; j++)
                    {
                        for (int k = 0; k < houseDistance; k++)
                        {
                            Coord c = new Coord(j, k);
                            if (house.Map.GetTerrain(c) != null && this.Contains(house.Origin + c))
                            {
                                SetTerrain(Game.TerrainFactory.Copy(house.Map.GetTerrain<BasicTerrain>(c), house.Origin + c));
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
                AddEntity(Game.CreatureFactory.Person(new Coord(Game.Settings.Random.Next(Width), Game.Settings.Random.Next(Height))));
            }
            for (int i = 0; i < 300; i++)
            {
                AddEntity(Game.CreatureFactory.Animal(new Coord(Game.Settings.Random.Next(Width), Game.Settings.Random.Next(Height))));
            }
        }
        private void MakeOutdoors()
        {
            var f = Formulae.RandomTerrainGenFormula();
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    Coord pos = new Coord(i, j);
                    SetTerrain(Game.TerrainFactory.Grass(pos, f(i, j)));
                }
            }
        }
        public IEnumerable<Area> GetRegions(Coord position)
        {
            foreach (Area area in Regions)
            {
                if (area.InnerPoints.Contains(position))
                    yield return area;
            }
        }
    }
}
