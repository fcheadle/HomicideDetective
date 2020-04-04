using System;
using Engine.Creatures;
using Engine.Utils;
using GoRogue;
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

        public TerrainMap(int width, int height)
            // Allow multiple items on the same location only on the items layer.  This example uses 8-way movement, so Chebyshev distance is selected.
            : base(width, height, Enum.GetNames(typeof(MapLayer)).Length - 1, Distance.CHEBYSHEV, entityLayersSupportingMultipleItems: LayerMasker.DEFAULT.Mask((int)MapLayer.ITEMS))
        {
            ControlledGameObjectChanged += ControlledGameObjectTypeCheck<Player>; // Make sure we don't accidentally assign anything that isn't a Player type to ControlledGameObject
            FovVisibilityHandler = new DefaultFOVVisibilityHandler(this, ColorAnsi.BlackBright);


            MakeOutdoors(width, height);
            //MakePeople(width, height);
            //MakeHouses(width, height);
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
            Region area = new Region();
            area.OuterRect = new Rectangle(new Coord(0, 0), new Coord(width - 1, height - 1));
            area.InnerRect = new Rectangle(new Coord(1, 1), new Coord(width - 2, height - 2));
            area.IsLit = false;
            area.IsRectangle = true;
            area.IsVisited = false;
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
                        tile = new Terrain(Color.White, pos, '#', false, false);

                        area.OuterPoints.Add(pos);
                    }
                    else
                    {
                        if (foreground.B > foreground.G)
                        {
                            Color target;
                            int chance = Calculate.Chance();
                            if (chance < 10)
                                target = Color.DarkGreen;
                            else if (chance < 20)
                                target = Color.LightGreen;
                            else if (chance < 30)
                                target = Color.MediumSpringGreen;
                            else if (chance < 40)
                                target = Color.SpringGreen;
                            else if (chance < 50)
                                target = Color.DarkOliveGreen;
                            else if (chance < 60)
                                target = Color.DarkGray;
                            else if (chance < 70)
                                target = Color.Olive;
                            else if (chance < 80)
                                target = Color.OliveDrab;
                            else if (chance < 90)
                                target = Color.DarkSeaGreen;
                            else
                                target = Color.ForestGreen;

                            foreground = Colors.MutateBy(foreground, target);
                        }

                        if (foreground.R > foreground.G)
                        {
                            Color target = Color.Black;
                            int chance = Calculate.Chance();
                            if (chance < 10)
                                target = Color.DarkGreen;
                            else if (chance < 20)
                                target = Color.LightGreen;
                            else if (chance < 30)
                                target = Color.MediumSpringGreen;
                            else if (chance < 40)
                                target = Color.SpringGreen;
                            else if (chance < 50)
                                target = Color.DarkOliveGreen;
                            else if (chance < 60)
                                target = Color.DarkGray;
                            else if (chance < 70)
                                target = Color.Olive;
                            else if (chance < 80)
                                target = Color.OliveDrab;
                            else if (chance < 90)
                                target = Color.DarkSeaGreen;
                            else
                                target = Color.ForestGreen;

                            foreground = Colors.MutateBy(foreground, target);
                        }

                        tile = new Terrain(foreground,pos, '"', true, true);
                        area.InnerPoints.Add(pos);
                    }
                    SetTerrain(tile);
                }
            }

            for (int i = 0; i < 33; i++)
            {
                int x = GoRogue.Random.SingletonRandom.DefaultRNG.Next(Width);
                int y = GoRogue.Random.SingletonRandom.DefaultRNG.Next(Height);
                Coord c = new Coord(x, y);
                Terrain tree = new Terrain(Color.Brown, c,35, false, false);
                tree.IsVisible = true;
                area.InnerPoints.Add(c);
                SetTerrain(tree);
            }
            Coord c2 = new Coord(6, 6);
            Terrain tree2 = new Terrain(Color.Brown,c2, 'O', false, false);

            SetTerrain(tree2);

            //Regions.Add(area);//uncomment to make the entire map visible at once

        }
    }
}
