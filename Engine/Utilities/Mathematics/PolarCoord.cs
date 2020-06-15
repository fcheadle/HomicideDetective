using GoRogue;
using System;
using System.Collections.Generic;

namespace Engine.Utilities.Mathematics
{
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
            if (left.Theta > right.Theta - 0.05 && left.Theta < right.Theta + 0.05)
                if (left.Radius > right.Radius - 0.05 && left.Radius < right.Radius + 0.05)
                    return true;

            return false;
        }

        public static bool operator !=(PolarCoord left, PolarCoord right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(PolarCoord))
                return false;
            else
                return this == (PolarCoord)obj;
        }

        public static List<Func<double, Coord>> FunctionsPolar => new List<Func<double, Coord>>
        {
            (theta) => //the butterfly curve
            {
                double radius = Math.Exp(Math.Sin(theta));
                radius -= 2 * Math.Cos(theta * 4);

                double powerBase = 2 * theta - Math.PI;
                powerBase /= 24;

                radius += Math.Pow(powerBase, 5);
                double x = radius * Math.Cos(theta);
                double y = radius * Math.Sin(theta);
                return new Coord((int)Math.Round(x, 0), (int)Math.Round(y, 0));
            }
        };


        public static Coord PolarToCartesian(double radius, double theta)
        {
            double x = radius * Math.Cos(theta);
            double y = radius * Math.Sin(theta);
            return new Coord((int)Math.Round(x, 0), (int)Math.Round(y, 0));
        }
        public static Coord PolarToCartesian(PolarCoord pc)
        {
            return PolarToCartesian(pc.Radius, pc.Theta);
        }
        public static PolarCoord CartesianToPolar(Coord c)
        {
            float radius = c.X * c.X + c.Y * c.Y;
            radius = (float)Math.Sqrt(radius);
            float theta = c.X == 0 ? 0 : 1 / (float)Math.Tan(c.Y / c.X);
            return new PolarCoord(radius, theta);
        }

        public static Coord ButterflyCurve(float theta)
        {
            float radius = (float)Math.Exp(Math.Sin(theta));
            radius -= 2 * (float)Math.Cos(theta * 4);
            float powerBase = 2 * theta - (float)Math.PI;
            powerBase /= 24;
            radius += (float)Math.Pow(powerBase, 5);
            return PolarToCartesian(radius, theta);
        }
        public static List<Coord> ButterflyCurve()
        {
            List<Coord> p = new List<Coord>();
            Coord lastP = new Coord();
            for (float theta = 0; theta < 48; theta += .01f)
            {
                Coord thisP = ButterflyCurve(theta);
                if (thisP != lastP)
                {
                    p.Add(thisP);
                    lastP = thisP;
                }
            }
            return p;
        }
    }
}
