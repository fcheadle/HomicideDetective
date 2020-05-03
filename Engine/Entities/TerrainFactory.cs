using Engine.Extensions;
using GoRogue;
using Microsoft.Xna.Framework;
using SadConsole;

namespace Engine.Entities
{
    public class TerrainFactory
    {
        public static BasicTerrain Copy(BasicTerrain source, Coord target) => new BasicTerrain(source.Foreground, source.Background, source.Glyph, target, source.IsWalkable, source.IsTransparent);
        public static BasicTerrain Grass(Coord position, double z = 0)
        {
            Color color = Color.Green.MutateToIndex(z);
            color = color.Greenify();
            int glyph;
            int chance = Calculate.Percent();
            if (chance < 10)
                glyph = '\'';
            else if (chance < 20)
                glyph = '`';
            else if (chance < 30)
                glyph = '.';
            else if (chance < 40)
                glyph = ',';
            else if (chance < 90)
                glyph = '"';
            else
                glyph = '*';
            BasicTerrain t = new BasicTerrain(color, Color.Black, glyph, position, true, true);
            return t;
        }
        public static BasicTerrain Pavement(Coord position) => new BasicTerrain(Color.DarkGray, Color.Black, 247, position, true, true);
        public static BasicTerrain Wall(Coord position) => new BasicTerrain(Color.White, Color.Black, '#', position, false, false);
        public static BasicTerrain Fence(Coord position) => new BasicTerrain(Color.LightGray, Color.Black, 140, position, false, true);
        public static BasicTerrain FenceGate(Coord position) => new BasicTerrain(Color.LightGray, Color.Black, 15, position, true, true);
        public static BasicTerrain Door(Coord position) => new BasicTerrain(Color.LightGray, Color.Black, 234, position, true, false);
        public static BasicTerrain Carpet(Coord position) => new BasicTerrain(Color.Olive.Half(), Color.DarkOliveGreen.Half(), position.Y % 2 == 1 ? 16 : 17, position, true, true);
        public static BasicTerrain Linoleum(Coord position) => new BasicTerrain(Color.LightGray, Color.DarkGray, 4, position, true, true);
        public static BasicTerrain Window(Coord position) => new BasicTerrain(Color.Transparent, Color.Black, 0, position, false, true);
        public static BasicTerrain HardwoodFloor(Coord position) => new BasicTerrain(Color.SaddleBrown.Half(), Color.Brown.Half(), position.Y % 2 == 1 ? 174 : 175, position, true, true);
        public static BasicTerrain Tree(Coord position) => new BasicTerrain(Color.Brown.MutateBy(Color.Black), Color.Black, '0', position, false, false);
        public static BasicTerrain Test(int glyph, Coord position) => new BasicTerrain(Color.Cyan, Color.Magenta, glyph, position, true, true);
    }
}