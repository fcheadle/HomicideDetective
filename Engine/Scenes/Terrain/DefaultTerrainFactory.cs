using Engine.Scenes.Components;
using Engine.Utilities.Extensions;
using Engine.Utilities.Mathematics;
using GoRogue;
using Microsoft.Xna.Framework;
using SadConsole;

namespace Engine.Scenes.Terrain
{
    public class DefaultTerrainFactory : ITerrainFactory
    {
        public BasicTerrain Generic(Coord position, int glyph)
        {
            Color fore;
            Color back;
            double z = Game.Settings.Random.NextDouble() * glyph;
            if (position.Y + position.X % 2 == 0)
            {
                fore = Color.White.MutateToIndex(-z);
            }
            else
            {
                fore = Color.LightGray.MutateToIndex(-z);
            }
            back = Color.Black;
            return new BasicTerrain(fore, back, glyph, position, true, true);
        }
        public BasicTerrain Copy(BasicTerrain source, Coord target) => new BasicTerrain(source.Foreground, source.Background, source.Glyph, target, source.IsWalkable, source.IsTransparent);
        public BasicTerrain Floor(Coord position) => new BasicTerrain(Color.White, Color.Black, '.', position, true, true);

        private int GrassGlyph()
        {
            int glyph;
            int chance = Calculate.PercentValue();
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
            return glyph;
        }
        private Color Green(double z)
        {
            Color color = Color.Green.MutateToIndex(z);
            color = color.Greenify();
            return color;
        }
        public BasicTerrain Grass(Coord position) => Grass(position, 0.0f);

        public BasicTerrain Grass(Coord position, double z)
        {
            int glyph = GrassGlyph();
            int[] animation = { '/', '|', '(', '\\', '|', ')' };
            BasicTerrain t = new BasicTerrain(Green(z), Color.Black, glyph, position, true, true);
            t.AddComponent(new AnimateGlyphComponent(glyph, animation));
            return t;
        }
        public BasicTerrain Pavement(Coord position) => new BasicTerrain(Color.DarkGray, Color.Black, 247, position, true, true);
        public BasicTerrain Fence(Coord position) => new BasicTerrain(Color.LightGray, Color.Black, 140, position, false, true);
        public BasicTerrain DarkHardwoodFloor(Coord position) => new BasicTerrain(Color.SaddleBrown.Half(), Color.Brown.Half().Half(), position.Y % 2 == 1 ? 174 : 175, position, true, true);
        public BasicTerrain MediumHardwoodFloor(Coord position) => new BasicTerrain(Color.SaddleBrown, Color.Brown.Half(), 240, position, true, true);
        public BasicTerrain LightHardwoodFloor(Coord position) => new BasicTerrain(Color.RosyBrown, Color.Brown, position.Y % 2 == 1 ? 242 : 243, position, true, true);
        public BasicTerrain FenceGate(Coord position) => new BasicTerrain(Color.LightGray, Color.Black, 15, position, true, true);
        public BasicTerrain Door(Coord position) => new BasicTerrain(Color.LightGray, Color.Black, 234, position, true, false);
        public BasicTerrain OliveCarpet(Coord position) => new BasicTerrain(Color.Olive.Half(), Color.DarkOliveGreen.Half(), position.Y % 2 == 1 ? 16 : 17, position, true, true);
        public BasicTerrain LightCarpet(Coord position) => new BasicTerrain((position.X + position.Y) % 2 == 1 ? Color.Gray : Color.GhostWhite, (position.X + position.Y) % 2 == 1 ? Color.GhostWhite : Color.Gray, 177, position, true, true);
        public BasicTerrain ShagCarpet(Coord position) => new BasicTerrain(Color.DarkOrange, Color.Maroon, (position.X + position.Y) % 2 == 1 ? 178 : 176, position, true, true);
        public BasicTerrain BathroomLinoleum(Coord position) => new BasicTerrain(Color.LightGray, Color.DarkGray, 4, position, true, true);
        public BasicTerrain KitchenLinoleum(Coord position) => new BasicTerrain(Color.LightYellow, Color.DarkGoldenrod, (position.X + position.Y) % 2 == 1 ? 9 : 10, position, true, true);
        public BasicTerrain Wall(Coord position) => new BasicTerrain(Color.White, Color.Black, '#', position, false, false);
        public BasicTerrain Window(Coord position) => new BasicTerrain(Color.Transparent, Color.Black, 0, position, false, true);
        public BasicTerrain Tree(Coord position) => new BasicTerrain(Color.Brown.MutateBy(Color.Black), Color.Black, '0', position, false, false);
    }
}