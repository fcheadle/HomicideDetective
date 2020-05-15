using GoRogue;
using System;

namespace Engine.Maps.Areas
{
    public static class AreaFactory
    {
        public static Area Rectangle(string name, Coord start, int width, int height, int rise = 0, int run = 1)
        {
            if (run <= 0)
                throw new ArgumentOutOfRangeException("`run` must be a positive, non-zero integer.");
            
            int hRatio = height * rise / run;
            int wRatio = width * rise / run;
            int east = start.X + width - wRatio;
            int west = start.X - wRatio;
            int north = start.Y + hRatio;
            int south = start.Y + height + hRatio;
            return new Area(
                name,
                nw: start,
                se: new Coord(east - wRatio, south),
                ne: new Coord(east, north),
                sw: new Coord(west, south - hRatio)
            );
        }

        internal static Area Closet(string name, Coord origin)
        {
            return new Area(name, origin + 2, origin + new Coord(2, 0), origin, origin + new Coord(0, 2));
        }
    }
}
