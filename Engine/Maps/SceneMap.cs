using Engine.Components;
using Engine.Entities.Creatures;
using Engine.Entities.Items;
using Engine.Entities.Terrain;
using Engine.Extensions;
using Engine.Maps.Areas;
using Engine.Utilities;
using GoRogue;
using Microsoft.Xna.Framework;
using SadConsole;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Maps
{
    public class SceneMap : BasicMap
    {
        private int _width;
        private int _height;
        private ISettings _settings;
        private ITerrainFactory _terrainFactory;
        private ICreatureFactory _creatureFactory;
        private IItemFactory _itemFactory;
       
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
        public FOVVisibilityHandler FovVisibilityHandler { get; }
        public SceneMap(int width, int height, ISettings settings = null, ITerrainFactory tFactory = null, ICreatureFactory cFactory = null, IItemFactory iFactory = null) : base(width, height, EnumUtils.EnumLength<MapLayer>(), Distance.MANHATTAN)
        {
            _width = width;
            _height = height;
            FovVisibilityHandler = new DefaultFOVVisibilityHandler(this, ColorAnsi.BlackBright);
            _settings = settings ?? new Settings();
            _terrainFactory = tFactory ?? new TerrainFactory();
            _creatureFactory = cFactory ?? new CreatureFactory();
            _itemFactory = iFactory ?? new ItemFactory();

            MakeOutdoors();
            //MakeBackrooms();
            MakeRoadsAndBlocks();
            MakeHouses();
            MakePeople();
            
        }
        private void MakeBackrooms()
        {
            House backrooms = new House("Backrooms", new Coord(0, 0), HouseType.Backrooms, Direction.Types.DOWN);
            backrooms.Generate();
            Houses.Add(backrooms);
            this.ForXForY(
                (Coord point) =>
                {
                    try
                    {
                        BasicTerrain t = backrooms.Map.GetTerrain<BasicTerrain>(point);
                        if (t != null)
                            SetTerrain(_terrainFactory.Copy(t, point));
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
            RoadNames roadName = (RoadNames)Calculate.Percent();
            RoadNumbers roadNum = 0;
            int offset = (Calculate.Percent() - 50) * 2;
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
                        SetTerrain(_terrainFactory.Floor(c));
                        //SetTerrain(_terrainFactory.Pavement(c));
            }

            foreach (Block block in Blocks)
            {
                foreach (Coord c in block.GetFenceLocations())
                    if (this.Contains(c))
                        SetTerrain(_terrainFactory.Generic(c, 44));
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
                                SetTerrain(_terrainFactory.Copy(house.Map.GetTerrain<BasicTerrain>(c), house.Origin + c));
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
                AddEntity(_creatureFactory.Person(new Coord(_settings.Random.Next(Width), _settings.Random.Next(Height))));
            }
            for (int i = 0; i < 300; i++)
            {
                AddEntity(_creatureFactory.Animal(new Coord(_settings.Random.Next(Width), _settings.Random.Next(Height))));
            }
        }
        private void MakeOutdoors()
        {
            var f = Calculate.MasterFormula();
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    Coord pos = new Coord(i, j);
                    SetTerrain(_terrainFactory.Grass(pos, f(i, j)));
                }
            }
        }
    }
}
