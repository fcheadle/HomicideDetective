using GoRogue;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine
{
    public static class Calculate
    {
        #region map generation
        internal static List<Func<double, double>> Functions2d = new List<Func<double, double>>
        {
            //Not sure that I'm using these for ANYTHING right now
            x => { return Math.Sin(x) + (x/4); },
            x => { return Math.Tan(x / 50) + 50 - (x/3); },
            x => { return (Math.Cos(x) * x / 4) / x; }, //slow
            x => { return (Math.Cos(x)* 2) * (Math.Tan(x) * 2); },

        };
        internal static List<Func<int, int, double>> Functions3d = new List<Func<int, int, double>>
        {
            //Terrain Generation formulae - starts at Color.Green and mutatesToZ of the returned value.
            //Currently, the color formula is counting down by 1, but we would like for it to count down by 0.01,
            //and then it should be bound between -2 and 2
            //(x,y) => 0.00, //for testing purposes
            (x,y) => 07.00 * (Math.Sin(x + (x * 2.25)) + Math.Cos(y + (y/7))), //strange curves
            (x,y) => 04.65 * (Math.Sin(x) - (2*Math.Cos(4* y))), //freckles
            (x,y) => 07.77 * (Math.Cos((x *x) + (y * y))), //static
            (x,y) => 04.44 * (Math.Cos(x + y * Math.PI / 180) + Math.Sin(x + y * Math.PI / 180)), //smooth vertical lines
            (x,y) => 08.88 * (Math.Tan(x / 13 % 1.99) + Math.Tan(y % 1.91)), //horizontal lines
            (x,y) => 05.11 * (Math.Cos(Math.Sqrt(Math.Abs((x*x) + (y*y))))), //concentric circles origination from the top left
            (x,y) => 08.75 * (Math.Cos(Math.Sqrt(Math.Abs((-x*x) + (y*y))))), //strange, curved lines
            (x,y) => 13.31 * (Math.Tan((-2 * x * y) % 1.99)), //a little slow, but produces a sunburst pattern
            (x,y) => 11.11 * (Math.Tan(Math.Sqrt(x*y))), //nice, indiscernible pattern
            (x,y) => 11.11 * (Math.Tan(x/(y+1)) / (Math.Abs(y/3 - x / 17) +1)), //odd cones
            (x,y) => 13.31 * (Math.Tan(-x/(y+1))), //more cones
            (x,y) => 12.00 * (Math.Cos(y) * Math.Tan(x)), //looks natural
            (x,y) => 15.00 * (Math.Sin(x) * Math.Tan(y)), //looks natural
            (x,y) => 08.88 * (Math.Sin(x / (y+1)) * Math.Tan(y * x)), //looks natural
            (x,y) => 19.19 * (Math.Sin(x + y) * Math.Cos(y * x)), //looks natural
        };
        public static List<Func<int, int, TimeSpan, bool>> Functions4d = new List<Func<int, int, TimeSpan, bool>> 
        {
            //Curently used for wind.
            //TODO:  change the return type from bool to double, 
            //(x,y,t) => false, //for testing purposes
            //(x,y,t) => true, //for testing purposes
            (x,y,t) => Math.Cos(t.TotalMilliseconds / 222 + Math.Sqrt((x*x) + (y*y))) > 0.95, //concentric waves converging on northwest
            (x,y,t) => Math.Cos(-t.TotalMilliseconds / 333 - Math.Sqrt(Math.Abs((-x*x) + (y*y)))) > 0.9, //spiraling waves going northeast
            (x,y,t) => Math.Cos(-t.TotalMilliseconds / 444 + Math.Sqrt((x*x) + (y*y))) > 0.9, //concentric waves going southeast
            (x,y,t) => Math.Cos(t.TotalMilliseconds / 555 + Math.Sqrt(Math.Abs((x*x) + (y * 20)))) > 0.9, //spiraling towards northwest
            (x,y,t) => Math.Cos(t.TotalMilliseconds / 666 + Math.Sqrt(Math.Abs((x * y) + y))) > 0.9, //waves going northwest
            (x,y,t) => Math.Cos(t.TotalMilliseconds / 777 + Math.Sqrt(y * x)) > 0.95, //waves going nothwest and curling as they go
            (x,y,t) => Math.Sin(x / 3.33 + t.TotalSeconds) + Math.Cos(y * 3.33 + t.TotalSeconds) > 1.0, //slow lines west-northwest
            (x,y,t) => Math.Sin(x * 7.77 + t.TotalSeconds) + Math.Cos(y / 7.77 + t.TotalSeconds) > 1.1, //lines snaking north-northwest
            (x,y,t) => Math.Sin(x / 3.33 + t.TotalMilliseconds / 666) + Math.Cos(y * 8.76 + t.TotalMilliseconds / 222) > 1.25, //fast north-northwest bubbles
            (x,y,t) => Math.Cos(x * 1.125 - t.TotalMilliseconds / 111) + Math.Sin(y + t.TotalMilliseconds / 999) > 1.55, //wavy line crawling north
            (x,y,t) => Math.Cos(y + t.TotalSeconds) + Math.Sin(x * y - t.TotalMilliseconds / 666) > 1.55, //marching breeze north
            (x,y,t) => Math.Cos(x * y - t.TotalMilliseconds / 222) + Math.Sin(y * 4 - t.TotalMilliseconds / 222) > 1.55, //chaotic breeze northward with cross-breezes
            (x,y,t) => (int)(Math.Cos(x)* 3.75 + x + y * 4.15 + t.TotalSeconds * 25) % 64 <= 5,//large, sloped sin waves marching north
            (x,y,t) => (int)(Math.Cos(x) + x * 3.75 + y * 4.15 + t.TotalSeconds * t.TotalSeconds) % 64 <= 5,//gradually escalating northwest storm
            (x,y,t) => Math.Cos(t.TotalMilliseconds / 777 + Math.Sqrt(Math.Abs(y * y * 0.75 - x * 3.5))) > 0.9, //odd waves going east
            (x,y,t) => Math.Cos(- t.TotalMilliseconds / 777 + Math.Sqrt(Math.Abs(y * y * 0.75 - x * 3.5))) > 0.9, //odd waves going west
            (x,y,t) => Math.Cos(t.TotalMilliseconds / 777 - Math.Sqrt(y * y * x * x)) > 0.95, //omnidirectional chaos
            (x,y,t) => Math.Sin(t.TotalMilliseconds / 777 + Math.Sqrt(y * y * x)) > 0.95, //omnidirectional waves with many nexi
            (x,y,t) => Math.Sin(t.TotalMilliseconds / 777 - Math.Sqrt(y * x * x)) > 0.95, //omnidirectional waves with many nexi
            (x,y,t) => Math.Sin(-t.TotalMilliseconds / 777 + Math.Sqrt(y * y * x)) > 0.95, //omnidirectional, spiraling chaos with many nexi
            (x,y,t) => Math.Sin(-t.TotalMilliseconds / 777 - Math.Sqrt(y * x * x)) > 0.95, //omnidirectional wave form with many nexi
            (x,y,t) => Math.Sin(Math.Sqrt(Math.Abs((y*y) - (x*x))) - t.TotalMilliseconds / 888) > 0.9, //beautiful lines come from an axis northwest-southeast and curl to the other corners
            (x,y,t) => Math.Cos(-t.TotalMilliseconds / 999 + Math.Sqrt(Math.Abs((x*x) - (y*y)))) > 0.9, //beautiful lines come from northeast and southwest to the center
            (x,y,t) => Math.Sin(x + t.TotalMilliseconds / 444) + Math.Cos(y + t.TotalMilliseconds / 333) > 1.25,//bubbles going northwest
            (x,y,t) => Math.Sin(x - t.TotalMilliseconds / 111) + Math.Cos(y + t.TotalMilliseconds / 333) > 1.25, //lines zig-zagging north
            (x,y,t) => Math.Sin(x + t.TotalMilliseconds / 111) + Math.Cos(y - t.TotalMilliseconds / 555) > 1.25, //waves zig-zagging south
            (x,y,t) => Math.Sin(x - t.TotalMilliseconds / 650) + Math.Cos(y - t.TotalMilliseconds / 325) > 1.25, //bubbles going southeast
            (x,y,t) => Math.Sin(x - t.TotalMilliseconds / 650) - Math.Cos(y - t.TotalMilliseconds / 325) < -1.25, //bubbles snake south-southeast
            (x,y,t) => -Math.Cos(x * 3.45 - t.TotalMilliseconds / 777) - Math.Sin(y*0.77 - t.TotalMilliseconds / 77) < -1.5, //bubbles snaking east
            (x,y,t) => Math.Cos(x * 3.45 - t.TotalMilliseconds / 500) - Math.Sin(y*0.77 - t.TotalMilliseconds / 250) < -1.33, //bubbles waving south
            (x,y,t) => Math.Cos(x * 0.88 + t.TotalMilliseconds / 333) + Math.Sin(y*1.125 - t.TotalMilliseconds / 777) > 1.55, //bubbles slowling going southwest
            (x,y,t) => Math.Cos(x * y - t.TotalMilliseconds / 999) + Math.Sin(y - t.TotalMilliseconds / 111) > 1.55, //chaotic blowing south with cross-breezes 
            (x,y,t) => Math.Cos(x * y + t.TotalMilliseconds / 888) + Math.Sin(y * x - t.TotalMilliseconds / 555) > 1.75, //starts and stops, nondirectional 
            (x,y,t) => Math.Cos(x * y + t.TotalMilliseconds / 666) - Math.Sin(-1.5 * y * x - t.TotalMilliseconds / 222) < -1.55, //omnidirectional chaos
            (x,y,t) => Math.Cos(x * y + t.TotalMilliseconds / 666) + Math.Sin(y * x) < -1.55, //periodic, omnidirectional chaos
            (x,y,t) => Math.Cos(y) + Math.Sin(x * y + t.TotalMilliseconds / 666) > 1.33, //east-west constant winds of varying intensities
            (x,y,t) => (int)(x * (x / 8) + Math.Sin(y)*4 + t.TotalSeconds * 25) % 77 == 1,//periodic gentle west breeze
            (x,y,t) => (int)(Math.Tan(x*0.875) + Math.Tan(y*1.875) + t.TotalSeconds * 11) % 99 == 1,//periodic beautiful chaos
            (x,y,t) => (int)(x*1.111 + Math.Tan(y*.875) - t.TotalMilliseconds / 250) % 23 == 1,//odd lines marching west
            (x,y,t) => (int)(y*4.4 + Math.Tan(x*0.666) - t.TotalMilliseconds / 188) % 69 == 1,//gentle south waves
            (x,y,t) => Math.Cos(Math.Sqrt(2 * t.TotalSeconds * t.TotalSeconds + x *y)) > 0.5,//counterclockwise spiral from a center of the southeast corner, speeds up over time
        };
        public static List<Func<double, Point>> FunctionsPolar = new List<Func<double, Point>> 
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
        public static Func<double, double> RandomFunction2d()
        {
            int index = Settings.Random.Next(0, Functions2d.Count);
            return Functions2d[index];
        }
        public static Func<int, int, double> RandomFunction3d()
        {
            int index = Settings.Random.Next(0, Functions3d.Count);
            return Functions3d[index];
        }
        public static T RandomEnumValue<T>()
        {
            var v = Enum.GetValues(typeof(T));
            return (T)v.GetValue(new Random().Next(v.Length));
        }
        public static IEnumerable<Coord> InnerFromOuterPoints(List<Coord> outer)
        {
            List<Coord> inner = new List<Coord>();
            outer = outer.OrderBy(x => x.X).ToList();
            for (int i = outer.First().X; i <= outer.Last().X; i++)
            {
                List<Coord> chunk = outer.Where(w => w.X == i).OrderBy(o => o.Y).ToList();
                for (int j = chunk.First().Y; j <= chunk.Last().Y; j++)
                {
                    yield return new Coord(i, j);
                }
            }
        }

        public static T EnumValueFromIndex<T>(int i) where T : Enum
        {
            Array values = Enum.GetValues(typeof(T));
            T value = (T)values.GetValue(i);
            return value;
        }

        public static int EnumLength<T>()
        {
            return Enum.GetNames(typeof(T)).Length;
        }
        public static Func<int, int, TimeSpan, bool> RandomFunction4d()
        {
            int index = Settings.Random.Next(0, Functions4d.Count);
            return Functions4d[index];
        }

        internal static int EnumIndexFromValue<T>(T mune) 
        {
            return Convert.ToInt32(mune);
        }
        public static Func<int, int, double> MasterFormula()
        {
            Func<int, int, double> f = RandomFunction3d();

            return f;
        }
        public static Coord PolarToCartesian(double radius, double theta)
        {
            double x = radius * Math.Cos(theta);
            double y = radius * Math.Sin(theta);
            return new Coord((int)x, (int)y);
        }

        public static Coord PolarToCartesian(PolarCoord pc)
        {
            return PolarToCartesian(pc.Radius, pc.Theta);
        }

        public static PolarCoord CartesianToPolar(Coord c)
        {
            double radius = (c.X * c.X) + (c.Y * c.Y);
            radius = Math.Sqrt(radius);
            double theta = c.X == 0 ? 0 : 1 / Math.Tan(c.Y / c.X);
            return new PolarCoord(radius, theta);
        }
        public static double RadiansToDegrees(double theta)
        {
            return theta * 180.0f / Math.PI ;
        }
        public static double DegreesToRadians(int degrees)
        {
            return degrees * Math.PI / 180.0f;
        }
        public static Point ButterflyCurve(double theta)
        {
            double radius = Math.Exp(Math.Sin(theta));
            radius -= (2 * Math.Cos(theta * 4));
            double powerBase = 2 * theta - Math.PI;
            powerBase /= 24;
            radius += Math.Pow(powerBase, 5);
            return PolarToCartesian(radius, theta);
        }
        public static List<Point> ButterflyCurve() 
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
        public static bool Percent(int percentChance)=> percentChance >= Percent();
        public static int Percent() => Settings.Random.Next(1, 101);
        #endregion

        #region MapUtils
        public static IEnumerable<Coord> PointsAlongStraightLine(Coord start, Coord stop)
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
        #endregion
    }

    public class PolarCoord
    {   
        public readonly double Radius; //in meters
        public readonly double Theta; //in degrees clockwise

        public PolarCoord(double radius, double theta)
        {
            Radius = radius;
            Theta = theta;
        }
        
        public static bool operator ==(PolarCoord left, PolarCoord right)
        {
            if (left.Theta > right.Theta - 0.05 && left.Theta < right.Theta - 0.05)
                if (left.Radius > right.Radius - 0.05 && left.Radius < right.Radius + 0.05)
                    return true;

            return false;
        }

        public static bool operator !=(PolarCoord left, PolarCoord right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(PolarCoord))
                return false;
            else
                return this == (PolarCoord)obj;
        }
    }
}
