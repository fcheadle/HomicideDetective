using GoRogue;
using Microsoft.Xna.Framework;
using SadConsole;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Maps
{
    internal class Terrain : BasicTerrain
    {
        public Terrain(Color foreground, Coord position, int glyph, bool isWalkable = true, bool isTransparent = true) : base(foreground, Color.Black, glyph, position, isWalkable, isTransparent)
        {
        }

        internal static Terrain Grass(Coord position) => new Terrain(Color.Green, position, '"');
        internal static Terrain Pavement(Coord position) => new Terrain(Color.DarkGray, position, '=');
        internal static Terrain Carpet(Coord position) => new Terrain(Color.Maroon, position, 'o');
        internal static Terrain Linoleum(Coord position) => new Terrain(Color.Green, position, 4);
        internal static Terrain HardwoodFloor(Coord position) => new Terrain(Color.Green, position, 37);
    }
}
