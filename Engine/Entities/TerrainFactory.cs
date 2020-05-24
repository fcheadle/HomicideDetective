using Engine.Components;
using Engine.Components.Terrain;
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
            t.AddComponent(new BlowsInWindComponent(glyph));
            return t;
        }
        public static BasicTerrain Pavement(Coord position) => new BasicTerrain(Color.DarkGray, Color.Black, 247, position, true, true);
        public static BasicTerrain Wall(Coord position) => new BasicTerrain(Color.White, Color.Black, '#', position, false, false);
        public static BasicTerrain Fence(Coord position) => new BasicTerrain(Color.LightGray, Color.Black, 140, position, false, true);
        public static BasicTerrain FenceGate(Coord position) => new BasicTerrain(Color.LightGray, Color.Black, 15, position, true, true);
        public static BasicTerrain Door(Coord position) => new BasicTerrain(Color.LightGray, Color.Black, 234, position, true, false);
        public static BasicTerrain OliveCarpet(Coord position) => new BasicTerrain(Color.Olive.Half(), Color.DarkOliveGreen.Half(), position.Y % 2 == 1 ? 16 : 17, position, true, true);
        public static BasicTerrain LightCarpet(Coord position) => new BasicTerrain((position.X + position.Y) % 2 == 1 ? Color.Gray : Color.GhostWhite, (position.X + position.Y) % 2 == 1 ? Color.GhostWhite : Color.Gray, 177, position, true, true);
        public static BasicTerrain ShagCarpet(Coord position) => new BasicTerrain(Color.DarkOrange, Color.Maroon, (position.X + position.Y) % 2 == 1 ? 178 : 176, position, true, true);
        public static BasicTerrain BathroomLinoleum(Coord position) => new BasicTerrain(Color.LightGray, Color.DarkGray, 4, position, true, true);
        public static BasicTerrain KitchenLinoleum(Coord position) => new BasicTerrain(Color.LightYellow, Color.DarkGoldenrod, (position.X + position.Y) % 2 == 1? 9 : 10, position, true, true);
        public static BasicTerrain Window(Coord position) => new BasicTerrain(Color.Transparent, Color.Black, 0, position, false, true);
        public static BasicTerrain DarkHardwoodFloor(Coord position) => new BasicTerrain(Color.SaddleBrown.Half(), Color.Brown.Half().Half(), position.Y % 2 == 1 ? 174 : 175, position, true, true);
        public static BasicTerrain MediumHardwoodFloor(Coord position) => new BasicTerrain(Color.SaddleBrown, Color.Brown.Half(), 240, position, true, true);
        public static BasicTerrain LightHardwoodFloor(Coord position) => new BasicTerrain(Color.RosyBrown, Color.Brown, position.Y % 2 == 1 ? 242 : 243, position, true, true);
        public static BasicTerrain Tree(Coord position) => new BasicTerrain(Color.Brown.MutateBy(Color.Black), Color.Black, '0', position, false, false);
        public static BasicTerrain Test(int glyph, Coord position) => new BasicTerrain(Color.Cyan, Color.Magenta, glyph, position, true, true);
    }
}