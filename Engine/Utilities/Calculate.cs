using GoRogue;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine
{
    internal static class Calculate
    {
        #region generation
        /// <summary>
        /// List of 2d functions. Use these for calculating wind effects
        /// </summary>
        internal static List<Func<double, double>> Functions2d { get; } = new List<Func<double, double>>
        {
            x => { return Math.Sin(x) + (x/4); },
            x => { return Math.Tan(x / 50) + 50 - (x/3); },
            x => { return (Math.Cos(x) * x / 4) / x; }, //slow
            x => { return (Math.Cos(x)* 2) * (Math.Tan(x) * 2); },

        };

        /// <summary>
        /// List of 3d functions. Use this to calculate Z-Levels according to whatever formulae you like
        /// </summary>
        internal static List<Func<int, int, double>> Functions3d = new List<Func<int, int, double>>
        {
            (x,y) => //pretty, simple, fast
            {
                return (Math.Sin(x + (x * 2.25)) + Math.Cos(y + (y/7))) * 7;
            },

            //(x,y) => //beautiful, but slow
            //{
            //    double answer = Math.Sin(Math.Sin(Math.Sin(x))) * Math.Sin(Math.Sin(Math.Sin(y)));
            //    answer += (x / 5) + (y / 13);
            //    return answer;
            //},

            //(x,y) => //nice, simple, fast
            //{
            //    double answer = Math.Exp(Math.Sin(x)) - (2*Math.Cos(4* y)) + Math.Pow(Math.Sin((2*x*y - Math.PI)/24), 5);
            //    answer *= 3;
            //    return answer;
            //},

            //(x,y) => //nice and simple
            //{
            //    double answer = (x *x) + (y * y);
            //    answer = Math.Cos(answer);
            //    answer *= 5.55;
            //    return answer;
            //},

            //(x,y) =>
            //{
            //    return Math.Cos(x + y * Math.PI / 180) + Math.Sin(x + y * Math.PI / 180) * 3.33;
            //},

            //(x,y) => 
            //{
            //    return Math.Tan(x) - Math.Tan(y) / 4;
            //},

            //(x,y) => //beautiful, quite slow
            //{
            //    return Math.Tan(x) - Math.Tan(y) + x;
            //},

            //(x,y) => //beautiful, quite slow
            //{
            //    return Math.Tan(x) - Math.Tan(y) + y;
            //},

            //(x,y) => //beautiful, quite slow
            //{
            //    return Math.Tan(x) + Math.Tan(y) + x + y;
            //},

            //(x,y) => //beautiful, but slow
            //{
            //    double sin = Math.Sin(x);
            //    double cos = Math.Cos(y);
            //    double answer = sin + cos + (x / 3) + (y / 5);
            //    //answer *= -1;
            //    return answer;
            //},

            //(x, y) => //beautiful, but slow
            //{
            //    double sin = Math.Sin(x);
            //    double cos = Math.Cos(y);
            //    double answer = sin + cos + x + y;
            //    //answer *= 15;
            //    return answer;
            //},

            //(x,y) => //too slow
            //{
            //    //y *= -1;
            //    double sin = Math.Sin(x * Math.PI / 180);
            //    double cos = Math.Cos(y * Math.PI / 180);
            //    double answer = sin + cos  + (x) + (y);
            //    answer *= 3;
            //    return answer;
            //},
        };

        internal static IEnumerable<Coord> InnerFromOuterPoints(List<Coord> outer)
        {
            List<Coord> inner = new List<Coord>();
            outer = outer.OrderBy(x => x.X).ToList();
            for (int i = outer.First().X; i < outer.Last().X; i++)
            {
                List<Coord> chunk = outer.Where(w=> w.X==i).OrderBy(o => o.Y).ToList();
                for (int j = 0; j < chunk.Last().Y; j++)
                {
                    yield return new Coord(i, j);
                }
            }
        }

        internal static T EnumValueFromIndex<T>(int i) where T : Enum
        {
            Array values = Enum.GetValues(typeof(T));
            T value = (T)values.GetValue(i);
            return value;
        }

    internal static int EnumLength<T>()
        {
            return Enum.GetNames(typeof(T)).Length;
        }

        /// <summary>
        /// List of 4d functions. Use these to animate trippy effects map-wide, such as when the player is hallucinating, during an earthquake or meteor strike, or if some magic effect is changing the landscape.
        /// </summary>
        internal static List<Func<int, int, int, double>> Functions4d = new List<Func<int, int, int, double>> //for future shenanigans
        {

        };

        /// <summary>
        /// List of Polar Functions. Useful for wind generation, Tornado and Hurricane Effects, and Distribution of town buildings around traffic circles. Unfortunately, this is too slow to be truly usable.
        /// </summary>
        internal static List<Func<double, Point>> FunctionsPolar = new List<Func<double, Point>> 
        {
            (theta) => //the butterfly curve
            {
                double radius = Math.Exp(Math.Sin(theta));
                radius -= (2 * Math.Cos(theta * 4));

                double powerBase = 2 * theta - Math.PI;
                powerBase /= 24;

                radius += Math.Pow(powerBase, 5);
                double x = radius * Math.Cos(theta);
                double y = radius * Math.Sin(theta);
                return new Point((int)x, (int)y);
            }
        };

        /// <summary>
        /// Returns a random 2d function hardcoded into Calculate.cs
        /// </summary>
        /// <returns>A line.</returns>
        internal static Func<double, double> RandomFunction2d()
        {
            int index = Settings.Random.Next(0, Functions2d.Count);
            return Functions2d[index];
        }

        /// <summary>
        /// Returns a random 3d function hardcoded into Calculate.cs
        /// </summary>
        /// <returns>A heightmap.</returns>
        internal static Func<int, int, double> RandomFunction3d()
        {
            int index = Settings.Random.Next(0, Functions3d.Count);
            return Functions3d[index];
        }

        /// <summary>
        /// returns a random enum value from the type provided
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        internal static T RandomEnumValue<T>()
        {
            var v = Enum.GetValues(typeof(T));
            return (T)v.GetValue(new Random().Next(v.Length));
        }

        /// <summary>
        /// Returns a random 4d function hardcoded into Calculate.cs
        /// </summary>
        /// <returns>A heightmap which changes over time.</returns>
        internal static Func<int, int, int, double> RandomFunction4d()
        {
            int index = Settings.Random.Next(0, Functions4d.Count);
            return Functions4d[index];
        }

        /// <summary>
        /// Picks a random terrain generation formula from the list of 3d functions in Calculate.cs
        /// </summary>
        /// <returns>The formula for terrain generation.</returns>
        internal static Func<int, int, double> MasterFormula()
        {
            Func<int, int, double> f = RandomFunction3d();

            return f;
        }

        /// <summary>
        /// Converts a Polar Coordinate to a Cartesian Coordinate
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="theta"></param>
        /// <returns></returns>
        internal static Coord PolarToCartesian(double radius, double theta)
        {
            double x = radius * Math.Cos(theta);
            double y = radius * Math.Sin(theta);
            return new Coord((int)x, (int)y);
        }

        /// <summary>
        /// Another way of changing a polar coordinate to a cartesian coordinate
        /// </summary>
        /// <param name="pc"></param>
        /// <returns></returns>
        internal static Coord PolarToCartesian(PolarCoord pc)
        {
            return PolarToCartesian(pc.radius, pc.theta);
        }

        /// <summary>
        /// Converts a Cartesian Coordinate to a Polar Coordinate
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        internal static PolarCoord CartesianToPolar(Coord c)
        {
            double radius = (c.X ^ 2) + (c.Y ^ 2);
            radius = Math.Sqrt(radius);
            double theta = c.X == 0 ? 0 : 1 / Math.Tan(c.Y / c.X);
            return new PolarCoord(radius, theta);
        }

        /// <summary>
        /// Converts a given Radians to a degree value
        /// </summary>
        /// <param name="degrees"></param>
        /// <returns></returns>
        internal static double RadiansToDegrees(double theta)
        {
            return theta * 180.0f / Math.PI ;
        }

        /// <summary>
        /// Converts a given Degree to a radian value
        /// </summary>
        /// <param name="degrees"></param>
        /// <returns></returns>
        internal static double DegreesToRadians(int degrees)
        {
            return degrees * Math.PI / 180.0f;
        }

        /// <summary>
        /// Calculates the radius for a given theta according to the butterfly curve. Too beautiful for its own good, this operates too slowly to use.
        /// </summary>
        /// <param name="theta"></param>
        /// <returns></returns>
        internal static Point ButterflyCurve(double theta)
        {
            double radius = Math.Exp(Math.Sin(theta));
            radius -= (2 * Math.Cos(theta * 4));
            double powerBase = 2 * theta - Math.PI;
            powerBase /= 24;
            radius += Math.Pow(powerBase, 5);
            return PolarToCartesian(radius, theta);
        }

        /// <summary>
        /// Calculates all the points along the butterfly curve.  Too beautiful for its own good, this operates too slowly to use.
        /// </summary>
        /// <returns></returns>
        internal static List<Point> ButterflyCurve() 
        {
            List<Point> p = new List<Point>();
            Point lastP = new Point();
            for (double theta = 0; theta < 48; theta += .01)
            {
                Point thisP = ButterflyCurve(theta);
                if (thisP != lastP)
                {
                    p.Add(thisP);
                    lastP = thisP;
                }
            }

            return p;
        }
        #endregion

        #region chances
        /// <summary>
        /// Returns or not that a given percent chance of something happening, happens 
        /// </summary>
        /// <param name="percentChance">Odds of this particular thing</param>
        /// <returns></returns>
        internal static bool Percent(int percentChance)=> percentChance >= Settings.Random.Next(1, 101);

        /// <summary>
        /// Gets a random number between 1 and 100
        /// </summary>
        /// <returns></returns>
        internal static int Percent() => Settings.Random.Next(1, 101);
        #endregion

        #region MapUtils
        /// <summary>
        /// Calculates the points along a line
        /// </summary>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        internal static IEnumerable<Coord> PointsAlongStraightLine(Coord start, Coord stop)
        {
            int xOrigin = start.X;
            int yOrigin = start.Y;
            int xDestination = stop.X;
            int yDestination = stop.Y;
            //xOrigin = xOrigin < 0 ? 0 : xOrigin;
            //xOrigin = xOrigin > Settings.MapWidth ? 0 : xOrigin;
            //yOrigin = yOrigin < 0 ? 0 : yOrigin;
            //yOrigin = yOrigin > Settings.MapWidth ? 0 : yOrigin;
            //xDestination = xDestination > Settings.MapWidth ? 0 : xDestination;
            //xDestination = xDestination < 0 ? 0 : xDestination;
            //yDestination = yDestination > Settings.MapHeight ? 0 : yDestination;
            //yDestination = yDestination < 0 ? 0 : yDestination;

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

        /// <summary>
        /// Gets the points along the ling with a specified width, aka the thickness of the line.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        internal static List<Coord> PointsAlongStraightLine(Coord start, Coord stop, int width)
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
                    for (int i = point.Y - (width / 2); i < point.Y + (width / 2); i++)
                    {
                        answer.Add(new Coord(point.X, i));
                    }
                }
                else
                {
                    for (int i = point.X - (width / 2); i < point.X + (width / 2); i++)
                    {
                        answer.Add(new Coord(i, point.Y));
                    }
                }
            }

            return answer.Distinct().ToList();
        }

        /// <summary>
        /// Calculates all points which are considered "bordering" the given rectangle
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        internal static List<Coord> BorderLocations(GoRogue.Rectangle room)
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
        #endregion
    }

    //too slow
    internal class PolarCoord
    {   
        internal double radius;
        internal double theta;
        
        internal PolarCoord(double radius, double theta)
        {
            this.radius = radius;
            this.theta = theta;
        }
        
    }
}
