using GoRogue;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Utils
{
    public static class Calculate
    {
        private static Random r = new Random();
        public static List<Func<double, double>> Functions2d = new List<Func<double, double>> //use these for wind
        {
            x => { return Math.Sin(x) + (x/4); },
            x => { return Math.Tan(x / 50) + 50 - (x/3); },
            x => { return (Math.Cos(x) * x / 4) / x; }, //slow
            x => { return (Math.Cos(x)* 2) * (Math.Tan(x) * 2); },

        };
        public static List<Func<int, int, double>> Functions3d = new List<Func<int, int, double>>
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
        public static List<Func<int, int, int, double>> Functions4d = new List<Func<int, int, int, double>> //for future shenanigans
        {

        };
        public static List<Func<double, Point>> FunctionsPolar = new List<Func<double, Point>> //mind-bending stuff
        {
            (theta) => //the butterfly curve: incredibly beautiful, insanely slow. Would probably make a good seed for some cellular automata wind?
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

        public static Func<double, double> RandomFunction2d()
        {
            int index = r.Next(0, Functions2d.Count);
            return Functions2d[index];
        }

        public static Func<int, int, double> RandomFunction3d()
        {
            int index = r.Next(0, Functions3d.Count);
            return Functions3d[index];
        }

        public static Func<int, int, int, double> RandomFunction4d()
        {
            int index = r.Next(0, Functions4d.Count);
            return Functions4d[index];
        }

        public static Func<int, int, double> MasterFormula()
        {
            Func<int, int, double> f = RandomFunction3d();

            return f;
        }

        public static Point PolarToCartesian(double radius, double theta)
        {
            double x = radius * Math.Cos(theta);
            double y = radius * Math.Sin(theta);
            return new Point((int)x, (int)y);
        }

        public static PolarCoord CartesianToPolar(Point p)
        {
            double radius = p.X ^ 2 + p.Y ^ 2;
            radius = Math.Sqrt(radius);
            double theta = 1 / Math.Tan(p.Y / p.X);
            return new PolarCoord(radius, theta);
        }

        public static Point ButterflyCurve(double theta)//too beautiful for its own good. This operates insanely slowly
        {
            double radius = Math.Exp(Math.Sin(theta));
            radius -= (2 * Math.Cos(theta * 4));
            double powerBase = 2 * theta - Math.PI;
            powerBase /= 24;
            radius += Math.Pow(powerBase, 5);
            return PolarToCartesian(radius, theta);
        }

        public static List<Point> ButterflyCurve() //too beautiful for its own good. This operates insanely slowly
        {
            List<Point> p = new List<Point>();
            Point lastP = new Point();
            for (double theta = 0; theta < 48; theta += .01)
            {
                Point thisP = ButterflyCurve(theta);
                if (thisP != lastP)
                {
                    p.Add(thisP);
                    //draw line between thisP and lastP?
                    lastP = thisP;
                }
            }

            return p;
        }

        public static bool Chance(int percentChance)
        {
            return percentChance >= r.Next(1, 101);
        }

        internal static int Chance()
        {
            return r.Next(1, 101);
        }

        internal static IEnumerable<Coord> PointsAlongLine(Coord start, Coord stop)
        {
            int xOrigin = start.X;
            int yOrigin = start.Y;
            int xDestination = stop.X;            
            int yDestination = stop.Y;            
            xOrigin = xOrigin < 0 ? 0 : xOrigin;
            xOrigin = xOrigin > Settings.MapWidth ? 0 : xOrigin;
            yOrigin = yOrigin < 0 ? 0 : yOrigin;
            yOrigin = yOrigin > Settings.MapWidth ? 0 : yOrigin;
            xDestination = xDestination > Settings.MapWidth ? 0 : xDestination;
            xDestination = xDestination < 0 ? 0 : xDestination;
            yDestination = yDestination > Settings.MapHeight ? 0 : yDestination;
            yDestination = yDestination < 0 ? 0 : yDestination;

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
            borderCells = PointsAlongLine(tr, max).ToList();
            borderCells.AddRange(PointsAlongLine(min, tr).ToList());
            borderCells.AddRange(PointsAlongLine(min, bl).ToList());
            borderCells.AddRange(PointsAlongLine(bl, max).ToList());
            
            return borderCells.Distinct().ToList();
        }
    }
}
