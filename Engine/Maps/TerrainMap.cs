using System;
using Engine.Creatures;
using Engine.Utils;
using GoRogue;
using Microsoft.Xna.Framework;
using SadConsole;

namespace Engine.Maps
{
    internal enum MapLayer
    {
        TERRAIN,
        ITEMS,
        MONSTERS,
        PLAYER
    }

    internal class TerrainMap : BasicMap
    {
        // Handles the changing of tile/entity visiblity as appropriate based on Map.FOV.
        public FOVVisibilityHandler FovVisibilityHandler { get; }


        // Since we'll want to access the player as our Player type, create a property to do the cast for us.  The cast must succeed thanks to the ControlledGameObjectTypeCheck
        // implemented in the constructor.
        public new Player ControlledGameObject
        {
            get => (Player)base.ControlledGameObject;
            set => base.ControlledGameObject = value;
        }

        public TerrainMap(int width, int height, bool generate = true)
            // Allow multiple items on the same location only on the items layer.  This example uses 8-way movement, so Chebyshev distance is selected.
            : base(width, height, Enum.GetNames(typeof(MapLayer)).Length - 1, Distance.CHEBYSHEV, entityLayersSupportingMultipleItems: LayerMasker.DEFAULT.Mask((int)MapLayer.ITEMS))
        {
            ControlledGameObjectChanged += ControlledGameObjectTypeCheck<Player>; // Make sure we don't accidentally assign anything that isn't a Player type to ControlledGameObject
            FovVisibilityHandler = new DefaultFOVVisibilityHandler(this, ColorAnsi.BlackBright);

            if (generate)
            {
                MakeHouses();
                MakeOutdoors();
                //MakePeople();
            }
        }

        private void MakeHouses()
        {
            for (int x = 1; x < Width - 49; x+=48)
            {
                for (int y = 1; y < Height - 49; y+=48)
                {
                    Point origin = new Point(x, y);
                    Structure house = new Structure(48, 48, origin);
                    Add(house.Map, origin);
                }
            }
        }

        public bool Contains(Coord location)
        {
            return (location.X >= 0 && location.Y >= 0 && location.X < Width && location.Y < Height);
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
                        Terrain newTerrain = GetTerrain<Terrain>(local);
                        newTerrain = Maps.Terrain.Copy(terrain, local);
                        //try
                        //{
                        SetTerrain(newTerrain);
                        //}
                        //catch
                        //{
                        //    //do nothing
                        //    ;
                        //}
                    }
                }
            }
        }
        private void MakePeople()
        {
            for (int i = 0; i < 100; i++)
            {
                AddEntity(Creature.Person(new Coord(Settings.Random.Next(Width), Settings.Random.Next(Height))));
            }
            for (int i = 0; i < 300; i++)
            {
                AddEntity(Creature.Animal(new Coord(Settings.Random.Next(Width), Settings.Random.Next(Height))));
            }
        }

        private void MakeOutdoors()
        {
            var f = Calculate.MasterFormula();
            int temp = 0;
            while (temp < 300)
            {
                int x = GoRogue.Random.SingletonRandom.DefaultRNG.Next(Width);
                int y = GoRogue.Random.SingletonRandom.DefaultRNG.Next(Height);
                Coord tree = new Coord(x, y);
                if (GetTerrain<Terrain>(tree) == null)
                {
                    SetTerrain(Maps.Terrain.Tree(new Coord(x, y)));
                    temp++;
                }
            }

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
                        t.Position = target;
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
                    if (t != null)
                    {
                        t.Position = target;
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
                        t.Position = target;
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
                        t.Position = target;
                        SetTerrain(t);
                    }
                }
            }

            Add(map);
        }
    }
}
