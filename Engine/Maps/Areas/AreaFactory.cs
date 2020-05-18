using GoRogue;
using System;

namespace Engine.Maps.Areas
{
    public static class AreaFactory
    {
        public static Area Rectangle(string name, Coord start, int width, int height, int rise = 0, int run = 1)
        {
            if (run <= 0)
                throw new ArgumentOutOfRangeException("`run` must be a non-zero integer.");
            
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
            return Rectangle(name, origin, 2,2,0,1);
        }

        internal static Area RegularParallelogram(string name, Coord origin, int width, int height, int rotationDegrees)
        {
            Coord nw = new Coord(0, 0);
            Coord ne = new Coord(width, height);
            Coord se = new Coord(width, height * 2);
            Coord sw = new Coord(0, height);
            Area area = new Area(name, se, ne, nw, sw);
            //area = area.Rotate(rotationDegrees);
            return area;
        }
    }
}
