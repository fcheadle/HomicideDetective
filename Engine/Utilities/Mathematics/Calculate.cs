using GoRogue;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Utilities.Mathematics
{
    public static class Calculate
    {
        #region map generation
        private static Random _random = new Random();
        private const float _piOverOneEighty = (float)(Math.PI / 180f);
        private const float _oneEightyOverPi = (float)(180 / Math.PI);
        public static float RadiansToDegrees(float theta) => theta * _oneEightyOverPi;
        public static float DegreesToRadians(int degrees) => degrees * _piOverOneEighty;
        #endregion

        #region chances
        public static bool PercentChance(int percentChance) => percentChance >= PercentValue();
        public static int PercentValue() => _random.Next(1, 101);
        #endregion

        #region MapUtils
        public static IEnumerable<Coord> PointsAlongStraightLine(Coord start, Coord stop)
        {
            int xOrigin = start.X;
            int yOrigin = start.Y;
            int xDestination = stop.X;
            int yDestination = stop.Y;

            int dx = Math.Abs(xDestination - xOrigin);
            int dy = Math.Abs(yDestination - yOrigin);

            int sx = xOrigin < xDestination ? 1 : -1;
            int sy = yOrigin < yDestination ? 1 : -1;
            int err = dx - dy;

            while (true)
            {
                yield return new Point(xOrigin, yOrigin);
                if (xOrigin == xDestination && yOrigin == yDestination)
                {
                    break;
                }
                int e2 = 2 * err;
                if (e2 > -dy)
                {
                    err = err - dy;
                    xOrigin = xOrigin + sx;
                }
                if (e2 < dx)
                {
                    err = err + dx;
                    yOrigin = yOrigin + sy;
                }
            }
        }

        public static float DistanceBetween(Coord start, Coord stop)
        {
            //a^2 + b^2 = c^2
            int xdiff = start.X > stop.X ? start.X - stop.X : stop.X - start.X;
            int ydiff = start.Y > stop.Y ? start.Y - stop.Y : stop.Y - start.Y;
            return (float)Math.Sqrt(xdiff * xdiff + ydiff * ydiff);
        }
        public static List<Coord> PointsAlongStraightLine(Coord start, Coord stop, int width)
        {
            if (width == 0)
                throw new ArgumentOutOfRangeException("width must be equal to or greater than 1.");

            List<Coord> points = PointsAlongStraightLine(start, stop).ToList();
            List<Coord> answer = new List<Coord>();
            int xDiff = Math.Abs(start.X - stop.X);
            int yDiff = Math.Abs(start.Y - stop.Y);
            foreach (Coord point in points)
            {
                if (xDiff > yDiff)
                {
                    for (int i = point.Y - width / 2; i < point.Y + width / 2; i++)
                    {
                        answer.Add(new Coord(point.X, i));
                    }
                }
                else
                {
                    for (int i = point.X - width / 2; i < point.X + width / 2; i++)
                    {
                        answer.Add(new Coord(i, point.Y));
                    }
                }
            }

            return answer.Distinct().ToList();
        }
        public static List<Coord> BorderLocations(GoRogue.Rectangle room)
        {
            int xMin = room.MinExtentX;
            int xMax = room.MaxExtentX;
            int yMin = room.MinExtentY;
            int yMax = room.MaxExtentY;
            Coord min = new Coord(xMin, yMin); //upper left
            Coord max = new Coord(xMax, yMax); //bottom right
            Coord bl = new Coord(xMin, yMax); //bottom left
            Coord tr = new Coord(xMax, yMin); //top right
            List<Coord> borderCells;
            borderCells = PointsAlongStraightLine(tr, max).ToList();
            borderCells.AddRange(PointsAlongStraightLine(min, tr).ToList());
            borderCells.AddRange(PointsAlongStraightLine(min, bl).ToList());
            borderCells.AddRange(PointsAlongStraightLine(bl, max).ToList());

            return borderCells.Distinct().ToList(); ;
        }

        internal static int RandomInt(int min, int max)
        {
            return _random.Next(min, max);
        }
        #endregion
    }
}
