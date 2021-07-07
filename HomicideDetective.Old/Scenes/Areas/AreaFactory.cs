using GoRogue;
using System;

namespace HomicideDetective.Old.Scenes.Areas
{
    //going away in favor of GoRogue.Region
    public static class AreaFactory
    {
        public static Area Rectangle(string name, Coord start, int width, int height, double angleRads = 0)
        {
            if (angleRads <= -1 || angleRads >= 1)
                throw new ArgumentOutOfRangeException(nameof(angleRads));

            int hRatio = (int)(height * angleRads);
            int wRatio = (int)(width * angleRads);
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
        public static Area Closet(string name, Coord origin)
        {
            return Rectangle(name, origin, 3, 3);
        }
        public static Area FromRectangle(string name, Rectangle rectangle)
        {
            return Rectangle(name, rectangle.MinExtent, rectangle.Width, rectangle.Height);
        }
        public static Area RegularParallelogram(string name, Coord origin, int width, int height, int rotationDegrees)
        {
            Coord nw = origin;
            Coord ne = origin + new Coord(width, 0);
            Coord se = origin + new Coord(width * 2, height);
            Coord sw = origin + new Coord(width, height);
            Area area = new Area(name, se, ne, nw, sw);
            //area = area.Rotate(rotationDegrees);
            return area;
        }
    }
}
