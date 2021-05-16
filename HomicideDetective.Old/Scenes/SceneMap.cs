using HomicideDetective.Old.Utilities.Extensions;
using GoRogue;
using Microsoft.Xna.Framework;
using SadConsole;
using System.Collections.Generic;
using System.Linq;
using HomicideDetective.Old.Scenes.Areas;
using HomicideDetective.Old.Scenes.Terrain;
using HomicideDetective.Old.Utilities;
using HomicideDetective.Old.Utilities.Mathematics;

namespace HomicideDetective.Old.Scenes
{
    //remove this in favor of RogueLikeMap
    //and refactor the generation parts into generation steps
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
                //.Concat(Rooms)
                .ToList()
                ;
        }
        internal List<Road> Roads { get => HorizontalRoads.Values.ToList().Concat(VerticalRoads.Values).ToList(); }
        internal Dictionary<KeyValuePair<RoadNumbers, RoadNames>, RoadIntersection> Intersections { get; private set; } = new Dictionary<KeyValuePair<RoadNumbers, RoadNames>, RoadIntersection>();
        internal Dictionary<RoadNumbers, Road> HorizontalRoads { get; private set; } = new Dictionary<RoadNumbers, Road>();
        internal Dictionary<RoadNames, Road> VerticalRoads { get; private set; } = new Dictionary<RoadNames, Road>();
        internal List<Block> Blocks { get; private set; } = new List<Block>();
        internal List<House> Houses { get; private set; } = new List<House>();

        public void RefreshRegion(Area area)
        {
            Area region = Regions.Where(x => x.Name == area.Name).First();
            Regions.Remove(region);
            Regions.Add(area);
            
        }

        public FOVVisibilityHandler FovVisibilityHandler { get; }
        public SceneMap(int width, int height) : base(width, height, EnumUtils.EnumLength<MapLayer>(), Game.Settings.DistanceType)
        {
            _width = width;
            _height = height;
            FovVisibilityHandler = new DefaultFOVVisibilityHandler(this, ColorAnsi.BlackBright);

            MakeOutdoors();
            //MakeBackrooms();//very, very, very slow
            MakeRoadsAndBlocks();
            MakeHouses();
            MakePeople();
        }
        public void MakeBackrooms()
        {
            House backrooms = new House("Backrooms", new Coord(0, 0), HouseType.Backrooms);
            backrooms.Generate();
            Houses.Add(backrooms);
            foreach (Coord floor in backrooms.Floor)
                if (this.Contains(floor))
                    SetTerrain(Game.TerrainFactory.Wall(floor));
            
            foreach (Coord wall in backrooms.Walls)
                if (this.Contains(wall))
                    SetTerrain(Game.TerrainFactory.Wall(wall));

            foreach (Coord door in backrooms.Doors)
                if (this.Contains(door))
                    SetTerrain(Game.TerrainFactory.Wall(door));

            
        }
        private void MakeRoadsAndBlocks()
        {
            RoadNames roadName = (RoadNames)Calculate.PercentValue();
            RoadNumbers roadNum = 0;
            int offset = (Calculate.PercentValue() - 50) * 2;

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

            //_rotationDegrees = Math.Atan; //guess?

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
                foreach (Coord c in r.InnerPoints)
                    if (this.Contains(c))
                        SetTerrain(Game.TerrainFactory.Floor(c));
             

            foreach (Block block in Blocks)
                foreach (Coord c in block.GetFenceLocations())
                    if (this.Contains(c))
                        SetTerrain(Game.TerrainFactory.Fence(c));
            
        }
        private void MakeHouses()
        {
            _rotationDegrees = Calculate.RandomInt(46, 90);
            House house;
            foreach (Block block in Blocks)
            {
                int i = 1;
                foreach (Coord houseOrigin in block.Addresses)
                {
                    string address = block.Name[0] + block.Name[1] + i.ToString() + block.Name.Substring(9);
                    house = new House(address, houseOrigin, HouseType.PrairieHome);
                    house.Generate();
                    house.Rotate(_rotationDegrees, true);
                    Houses.Add(house);
                    i++;
                }
            }

            foreach (House gennedHouse in Houses)
            {
                DefaultTerrainFactory factory = Game.TerrainFactory;
                BasicTerrain floor;
                int chance = Calculate.PercentValue();

                foreach (Coord target in gennedHouse.Floor)
                {
                    switch (chance % 8)
                    {
                        default: floor = factory.OliveCarpet(target); break;
                        case 1: floor = factory.LightCarpet(target); break;
                        case 2: floor = factory.ShagCarpet(target); break;
                        case 3: floor = factory.BathroomLinoleum(target); break;
                        case 4: floor = factory.KitchenLinoleum(target); break;
                        case 5: floor = factory.DarkHardwoodFloor(target); break;
                        case 6: floor = factory.MediumHardwoodFloor(target); break;
                        case 7: floor = factory.LightHardwoodFloor(target); break;
                    }

                    if (this.Contains(target) && GetTerrain<BasicTerrain>(target).IsWalkable)
                        SetTerrain(floor);
                }

                foreach (Coord target in gennedHouse.Walls)
                    if (this.Contains(target))
                        SetTerrain(factory.Wall(target));

                foreach (Coord target in gennedHouse.Doors)
                    if (this.Contains(target))
                        SetTerrain(factory.Door(target));
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
