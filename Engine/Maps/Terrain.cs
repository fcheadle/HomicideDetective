using Engine.Utils;
using GoRogue;
using GoRogue.GameFramework;
using Microsoft.Xna.Framework;
using SadConsole;
using System;

namespace Engine.Maps
{
    internal class Terrain : BasicTerrain
    {
        public Terrain(Color foreground, Color background, Coord position, int glyph, bool isWalkable = true, bool isTransparent = true) : base(foreground, background, glyph, position, isWalkable, isTransparent)
        {

        }
        internal static Terrain Copy(Terrain source, Coord target) => new Terrain(source.Foreground, source.Background, target, source.Glyph, source.IsWalkable, source.IsTransparent);
        internal static Terrain Grass(Coord position, double z = 0)
        {
            Color color = Color.Green;
            int chance;
            for (double k = 0; k < z; k++)
                color = Utils.Colors.Brighten(color);
            for (double k = z; k < 0; k++)
                color = Utils.Colors.Darken(color);
            
            if (color.B > color.G)
            {
                Color target;
                chance = Utils.Calculate.Chance();
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

                color = Utils.Colors.MutateBy(color, target);
            }

            if (color.R > color.G)
            {
                Color target = Color.Black;
                chance = Utils.Calculate.Chance();
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

                color = Utils.Colors.MutateBy(color, target);
            }

            int glyph = '"';
            chance = Utils.Calculate.Chance();
            if (chance < 15)
                glyph = '\'';
            else if (chance < 30)
                glyph = ',';
            else if (chance < 45)
                glyph = '.';
            else
                glyph = '"';
            Terrain t = new Terrain(color, Color.Black, position, glyph);
            return t;
        }
        internal static Terrain Pavement(Coord position) => new Terrain(Color.DarkGray, Color.Black, position, 247);
        internal static Terrain Wall(Coord position) => new Terrain(Color.White, Color.Black, position, '#',false,false);
        internal static Terrain Fence(Coord position) => new Terrain(Color.LightGray, Color.Black, position, 140, false, true);
        internal static Terrain FenceGate(Coord position) => new Terrain(Color.LightGray, Color.Black, position, 15);
        internal static Terrain Door(Coord position) => new Terrain(Color.LightGray, Color.Black, position, 234,true,false);
        internal static Terrain Carpet(Coord position) => new Terrain(Colors.Half(Color.Olive), Colors.Half(Color.DarkOliveGreen), position, position.Y % 2 == 1 ? 16 : 17);
        internal static Terrain Linoleum(Coord position) => new Terrain(Color.LightGray, Color.DarkGray, position, 4);
        internal static Terrain Window(Coord position) => new Terrain(Color.Transparent, Color.Black, position, 0, false, true);
        internal static Terrain HardwoodFloor(Coord position) => new Terrain(Colors.Half(Color.SaddleBrown), Colors.MutateBy(Color.Brown, Color.Black), position, position.Y % 2 == 1 ? 174 : 175);
        internal static Terrain Tree(Coord position)=> new Terrain(Color.Brown, Color.Black, position, 'O', false, false);
    }
}