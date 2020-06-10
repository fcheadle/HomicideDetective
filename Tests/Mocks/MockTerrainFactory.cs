using Engine.Components.Terrain;
using Engine.Extensions;
using GoRogue;
using Microsoft.Xna.Framework;
using SadConsole;

namespace Engine.Entities.Terrain
{
    public class MockTerrainFactory : ITerrainFactory
    {
        public BasicTerrain Generic(Coord position, int glyph)
        {
            Color fore;
            Color back;
            double z = Program.Settings.Random.NextDouble() * glyph;
            if (position.Y % 2 == 0)
            {
                fore = Color.Cyan.MutateToIndex(-z);
            }
            else
            {
                fore = Color.White.MutateToIndex(-z);
            }
            if (position.X % 2 == 0)
            {
                back = Color.Magenta.MutateToIndex(z);
            }
            else
            {
                back = Color.DarkMagenta.MutateToIndex(z);
            }
            return new BasicTerrain(fore, back, glyph, position, true, true);
        }
        public BasicTerrain Copy(BasicTerrain source, Coord target) => new BasicTerrain(source.Foreground, source.Background, source.Glyph, target, source.IsWalkable, source.IsTransparent);
        public BasicTerrain Floor(Coord position) => new BasicTerrain(Color.White, Color.Black, '.', position, true, true);
        public BasicTerrain Grass(Coord position, double z)
        {
            int[] animation = { '/', '|', '(', '\\', '|', ')' };
            BasicTerrain t = new BasicTerrain(Color.Green, Color.Black, '\'', position, true, true);
            t.AddComponent(new AnimateGlyphComponent('\'', animation));
            return t;
        }
        public BasicTerrain Door(Coord position) => new BasicTerrain(Color.LightGray, Color.Black, 234, position, true, false);
        public BasicTerrain Wall(Coord position) => new BasicTerrain(Color.White, Color.Black, '#', position, false, false);
        }
}