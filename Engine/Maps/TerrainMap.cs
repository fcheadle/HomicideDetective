using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Creatures;
using Engine.Utils;
using GoRogue;
using Microsoft.Xna.Framework;
using SadConsole;

namespace Engine.Maps
{
    internal enum MapLayers
    {
        TERRAIN,
        FURNITURE,
        CREATURES,
        PLAYER,
        ITEMS,
    }

    public enum Directions
    {
        North,
        NorthNorthEast,
        NorthEast,
        EastNorthEast,
        East,
        EastSouthEast,
        SouthEast,
        SouthSouthEast,
        South,
        SouthSouthWest,
        SouthWest,
        WestSouthWest,
        West,
        WestNorthWest,
        NorthWest,
        NorthNorthWest,
        Up,
        Down,
        None,
        All
    }

    //for now
    public enum RoadNames
    {
        Alder,
        Birch,
        Cedar,
        Dogwood,
        Elm,
        Fir,
        Gomez,
        Holly,
        Ibex,
        Juniper,
        Kuiper,
        Lark,
        Menendez,
        Norwood,
        Oak,
        Pear,
        Quincy,
        Redman,
        Spruce,
        Terweiliger,
        Utah,
        VillaNova,
        Waterfront,
        Xavier,
        Yew,
        Zedd,

        Alm,
        Baskins,
        Cherry,
        Danforth,
        Elders,
        Franklin,
        Gabriel,
        Hazelnut,
        Ipanema,
        Justice, 
        Killinger,
        Lamont,
        Montoya,
        Neumann,
        Olive,
        Peach,
        Quenton,
        Raspberry,
        Spear,
        Thompson,
        Underbridge,
        VanStrom,
        Williams,
        Xanadu,
        YellowLine,
        Zephraim,

        Apple,
        Burk,
        Chestnut,
        Quebec,
        Ash,
        Cottonwood,
        Aspen,
        Cypress,
    }

    public class TerrainMap : BasicMap
    {
        public List<Road> Roads { get; private set; } = new List<Road>();
        public List<Structure> Structures { get; private set; } = new List<Structure>();
        public FOVVisibilityHandler FovVisibilityHandler { get; }
        internal new Player ControlledGameObject
        {
            get => (Player)base.ControlledGameObject;
            set => base.ControlledGameObject = value;
        }

        public TerrainMap(int width, int height, bool generate = true)
            // Allow multiple items on the same location only on the items layer.  This example uses 8-way movement, so Chebyshev distance is selected.
            : base(width, height, Enum.GetNames(typeof(MapLayers)).Length - 1, Distance.CHEBYSHEV, entityLayersSupportingMultipleItems: LayerMasker.DEFAULT.Mask((int)MapLayers.ITEMS))
        {
            ControlledGameObjectChanged += ControlledGameObjectTypeCheck<Player>; // Make sure we don't accidentally assign anything that isn't a Player type to ControlledGameObject
            FovVisibilityHandler = new DefaultFOVVisibilityHandler(this, ColorAnsi.BlackBright);

            if (generate)
            {
                MakeRoads();
                MakeHouses();
                MakeOutdoors();
                MakePeople();
            }
        }

        private void MakeRoads()
        {
            RoadNames roadName = 0;
            List<Coord> roadCoords = new List<Coord>();
            for (int i = 0; i < Width - 160; i += 80)
            {
                //east/west
                Road road = new Road(new Coord(0, i), new Coord(Width + i + 400, 0), Enum.GetName(typeof(RoadNames), roadName) + " Street");
                roadCoords.AddRange(road.InnerPoints);
                Roads.Add(road);
                roadName++;

                //north/south
                //runs 1 / rises -4
                road = new Road(new Coord(i, 0), new Coord(Width, Height + i + 400), Enum.GetName(typeof(RoadNames), roadName) + " Street");
                roadCoords.AddRange(road.InnerPoints);
                Roads.Add(road);
                roadName++;
            }

            foreach(Coord coord in roadCoords)
            {
                if(Contains(coord))
                    SetTerrain(Maps.Terrain.Pavement(coord));
            }
        }

        internal TerrainMap Subsection(Coord start, Coord stop)
        {
            int xDiff = stop.X - start.X;
            int yDiff = stop.Y - start.Y;
            int size = xDiff > yDiff ? xDiff : yDiff;
            TerrainMap map = new TerrainMap(size + 2, size + 2, false);
            for (int i = start.X; i < stop.X+1; i++)
            {
                for (int j = start.Y; j < stop.Y+1; j++)
                {
                    Terrain t = GetTerrain<Terrain>(new Coord(i, j));
                    if (t != null)
                    {
                        Coord c = new Coord(i - start.X, j - start.Y);
                        t = Maps.Terrain.Copy(t, c);
                        if(map.Contains(c))
                            map.SetTerrain(t);
                    }
                }
            }
            return map;
        }

        private void MakeHouses()
        {
            foreach(Road road in Roads)
            {
                road.Populate();
                foreach (KeyValuePair<Coord, Structure> address in road.Addresses)
                {
                    Structure house = address.Value;
                    house.Address += " Street";
                    //if(IsClearOfObstructions(house.Origin, house.Map.Width))
                        Add(house.Map, house.Origin);
                    Structures.Add(house);
                }
            }
        }

        private bool IsClearOfObstructions(Coord origin, int width)
        {
            for (int i = origin.X; i < origin.X + width; i++)
            {
                for (int j = origin.Y; j < origin.Y + width; j++)
                {
                    Coord c = new Coord(i, j);
                    if (Contains(c))
                    {
                        Terrain t = GetTerrain<Terrain>(c);
                        if (t != null)
                        {
                            if (!t.IsWalkable)
                                return false;
                            else if (Maps.Terrain.Pavement(c) == Maps.Terrain.Copy(t, c)) //todo: fix this shit
                                return false;
                        }
                    }
                }
            }

            return true;
        }

        public bool Contains(Coord location)
        {
            return (location.X >= 0 && location.Y >= 0 && location.X < Width - 1 && location.Y < Height - 1);
        }

        internal void Add(TerrainMap map, Point origin)
        {
            for (int x = 0; x < map.Width; x++)
            {
                for (int y = 0; y < map.Height; y++)
                {
                    Coord source = new Coord(x, y);
                    Coord local = new Coord(x + origin.X, y + origin.Y);
                    Terrain terrain = map.GetTerrain<Terrain>(source);
                    if (terrain != null)
                    {
                        if (Contains(local))
                        {
                            Terrain newTerrain = Maps.Terrain.Copy(terrain, local);

                            SetTerrain(newTerrain);
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
            for (int i = 0; i < 777; i++)
            {
                Coord tree = new Coord(Settings.Random.Next(Width), Settings.Random.Next(Height));
                while (GetTerrain<Terrain>(tree) != null)
                {
                    tree = new Coord(Settings.Random.Next(Width), Settings.Random.Next(Height));
                }
                SetTerrain(Maps.Terrain.Tree(tree));
            }
         
            var f = Calculate.MasterFormula();
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    Coord pos = new Coord(i, j);
                    if (GetTerrain<Terrain>(pos) == null)
                    {

                        Terrain tile;
                        double z = f(i, j);

                        Color foreground = Color.Green;
                        for (double k = 0; k < z; k++)
                            foreground = Colors.Brighten(foreground);
                        for (double k = z; k < 0; k++)
                            foreground = Colors.Darken(foreground);

                        if (i == 0 || i == Width - 1 || j == 0 || j == Height - 1)
                        {
                            tile = Maps.Terrain.Wall(pos);
                        }
                        else
                        {
                            tile = Maps.Terrain.Grass(pos, z);
                        }
                        SetTerrain(tile);
                    }
                }
            }

        }

        public void ReverseHorizontal()
        {
            TerrainMap map = new TerrainMap(Width, Height, false);
            for (int i = Width - 1; i >= 0; i--)
            {
                for (int j = 0; j < Height; j++)
                {
                    Point source = new Point(i, j);
                    Point target = new Point(map.Width - i - 1, j);
                    Terrain t = GetTerrain<Terrain>(source);
                    if (t != null)
                    {
                        t = Maps.Terrain.Copy(t, target);
                        map.SetTerrain(t);
                    }
                }
            }
            Add(map);
        }

        private void Add(TerrainMap map) => Add(map, new Coord(0, 0));
        private void Add(TerrainMap map, Coord origin)
        {
            for (int i = 0; i < map.Width; i++)
            {
                for (int j = 0; j < map.Height; j++)
                {
                    Coord target = new Coord(i+origin.X, j+origin.Y);
                    Terrain t = map.GetTerrain<Terrain>(new Coord(i, j));
                    if (t != null && Contains(target))
                    {
                        t = Maps.Terrain.Copy(t,target);
                        SetTerrain(t);
                    }
                }
            }
        }

        public void ReverseVertical()
        {
            TerrainMap map = new TerrainMap(Width, Height, false);
            for (int i = 0; i < Width; i++)
            {
                for (int j = Height - 1; j > 0; j--)
                {
                    Point source = new Point(i, j);
                    Point target = new Point(map.Width - i - 1, j);
                    Terrain t = GetTerrain<Terrain>(source);
                    if (t != null)
                    {
                        t = Maps.Terrain.Copy(t, target);
                        SetTerrain(t);
                    }
                }
            }

            Add(map);
        }

        public void SwapXY()
        {

            TerrainMap map = new TerrainMap(Width, Height, false);
        
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    Point original = new Point(j, i);
                    Point target = new Point(i, j);
                    Terrain t = GetTerrain<Terrain>(original);
                    if (t != null)
                    {
                        t= Maps.Terrain.Copy(t, target);
                        SetTerrain(t);
                    }
                }
            }

            Add(map);
        }

        /// <summary>
        /// Too slow. Don't use.
        /// </summary>
        /// <param name="degrees"></param>
        public void Rotate(int degrees)
        {
            //too slow
            TerrainMap newMap = new TerrainMap(Width, Height, false);
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    Terrain t = GetTerrain<Terrain>(new Coord(i, j));
                    if (t != null)
                    {
                        PolarCoord pc = Calculate.CartesianToPolar(new Coord(i, j));
                        pc.theta += Calculate.DegreesToRadians(degrees);
                        Coord c = Calculate.PolarToCartesian(pc);
                        try
                        {
                            t = Maps.Terrain.Copy(t, c);
                            newMap.SetTerrain(t);
                        }
                        catch
                        {

                        }
                    }
                }
            }

            Add(newMap);
        }
    }
}
