using GoRogue;
using Microsoft.Xna.Framework;
using SadConsole;

namespace Engine.Maps
{
    internal class Terrain : BasicTerrain
    {
        public Terrain(Color foreground, Coord position, int glyph, bool isWalkable = true, bool isTransparent = true) : base(foreground, Color.Black, glyph, position, isWalkable, isTransparent)
        {

        }

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
            if (chance < 10)
                glyph = '\'';
            else if (chance < 20)
                glyph = ',';
            else if (chance < 30)
                glyph = '.';
            else
                glyph = '"';
            Terrain t = new Terrain(color, position, glyph);
            return t;
        }

        internal static Terrain Tree(Coord position)
        {
            int[] glyphs = { 'o', 'O', '0', 9, 237 };
            int glyph = glyphs[Settings.Random.Next(0, glyphs.Length)];
            return new Terrain(Color.Brown, position, glyph, false, false);
        }
        internal static Terrain Pavement(Coord position) => new Terrain(Color.DarkGray, position, 247);
        internal static Terrain Wall(Coord position) => new Terrain(Color.White, position, '#',false,false);
        internal static Terrain Carpet(Coord position)
        {
            Color color = Color.LightGoldenrodYellow;
            int glyph = position.Y % 2 == 1 ? 16 : 17;
            Terrain t = new Terrain(color, position, glyph);
            t.Background = Color.BurlyWood;
            return t;
        }
        internal static Terrain Linoleum(Coord position)
        {
            Color color = Color.LightGray;
            int glyph = 4;
            Terrain t = new Terrain(color, position, glyph);
            t.Background = Color.DarkGray;
            return t;
        }
        internal static Terrain HardwoodFloor(Coord position)
        {
            Color color = Color.BurlyWood;
            int glyph;
            //if(Utils.Calculate.Chance(50))
                glyph = position.Y % 2 == 1 ? 0171 : 0187;
            //else
                //glyph = 240;
            Terrain t = new Terrain(color, position, glyph);
            t.Background = Color.Brown;
            return t;
        }
    }
}