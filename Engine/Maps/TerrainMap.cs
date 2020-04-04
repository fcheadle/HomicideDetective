using System;
using Engine.Creatures;
using Engine.Utils;
using GoRogue;
using GoRogue.GameFramework;
using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Maps;
using Rectangle = GoRogue.Rectangle;

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
                MakeOutdoors(width, height);
                //MakePeople(width, height);
                //MakeHouses(width, height);
            }
        }

        private void MakePeople(int width, int height)
        {
            //Person person;
            //for (int i = 0; i < 50; i++)
            //{
            //    person = new Person(this, new Coord(GoRogue.Random.SingletonRandom.DefaultRNG.Next(Width), GoRogue.Random.SingletonRandom.DefaultRNG.Next(Height)));
            //    AddEntity(person);
            //}
            //person = new Person(this, new Coord(3, 3));// debug fellow right next to us to start

            //AddEntity(person);
        }

        private void MakeOutdoors(int width, int height)
        {
            var f = Calculate.MasterFormula();
            
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Terrain tile;
                    double z = f(i, j);
                    Coord pos = new Coord(i, j);

                    Color foreground = Color.Green;
                    for (double k = 0; k < z; k++)
                        foreground = Colors.Brighten(foreground);
                    for (double k = z; k < 0; k++)
                        foreground = Colors.Darken(foreground);

                    if (i == 0 || i == width - 1 || j == 0 || j == height - 1)
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

            for (int i = 0; i < 300; i++)
            {
                int x = GoRogue.Random.SingletonRandom.DefaultRNG.Next(Width);
                int y = GoRogue.Random.SingletonRandom.DefaultRNG.Next(Height);
                SetTerrain(Maps.Terrain.Tree(new Coord(x, y)));
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
                    t.Position = target;
                    map.SetTerrain(t);
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
                for (int j = Height - 1; j >= 0; j--)
                {
                    Point source = new Point(i, j);
                    Point target = new Point(map.Width - i - 1, j);
                    Terrain t = GetTerrain<Terrain>(source);
                    t.Position = target;
                    map.SetTerrain(t);
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
                    t.Position = target;
                    map.SetTerrain(t);
                }
            }

            Add(map);
        }
    }
}
