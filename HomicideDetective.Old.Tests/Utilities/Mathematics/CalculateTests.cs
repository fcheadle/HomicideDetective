using System;
using System.Collections.Generic;
using System.Linq;
using HomicideDetective.Old.Utilities.Mathematics;
using GoRogue;
using NUnit.Framework;

namespace HomicideDetective.Old.Tests.Utilities.Mathematics
{
    [TestFixture]
    public class CalculateTests
    {
        [Test]
        [Category("NonGraphical")]
        public void PointsAlongStraightLineTest()
        {
            Coord start = new Coord(1, 1);
            Coord stop = new Coord(10, 4);

            List<Coord> line = Calculate.PointsAlongStraightLine(start, stop).ToList();
            Assert.Contains(start, line);
            Assert.Contains(stop, line);
            Assert.Contains(new Coord(3, 2), line);
            Assert.Contains(new Coord(4, 2), line);
            Assert.Contains(new Coord(6, 3), line);
            Assert.Contains(new Coord(7, 3), line);
            Assert.Contains(new Coord(9, 4), line);
        }
        [Test]
        [Category("NonGraphical")]
        public void BorderLocationsTest()
        {
            /*
             * 01234567890
             *0 
             *1 XxxX
             *2 x  x
             *3 x  x
             *4 x  x
             *5 XxxX
             *6 
             * 
             */
            int xmin = 1;
            int ymin = 1;
            int xmax = 4;
            int ymax = 4;
            var answer = Calculate.BorderLocations(new Rectangle(new Coord(xmin, ymin), new Coord(xmax, ymax)));
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    Coord c = new Coord(i, j);
                    if (
                        i == xmin && (j == ymin || j == ymax) ||
                        j == ymin && (i == xmin || i == xmax) ||
                        i == xmax && (j == ymin || j == ymax) ||
                        j == ymax && (i == xmin || i == xmax))
                    {
                        Assert.Contains(c, answer);
                    }
                }
            }

            Assert.AreEqual(12, answer.Count);
        }
        [Test]
        [Category("NonGraphical")]
        public void PolarToCartesianTest()
        {
            PolarCoord origin = new PolarCoord(3.7416573867739413d, 0.64209261593433065d);
            Coord target = PolarCoord.PolarToCartesian(origin);
            Assert.AreEqual(new Coord(3, 2), target);
        }
        [Test]
        [Category("NonGraphical")]
        public void CartesianToPolarTest()
        {
            Coord origin = new Coord(5, 5);
            PolarCoord target = PolarCoord.CartesianToPolar(origin);
            double expectedRadius = Math.Sqrt(50);
            double expectedTheta = 1 / Math.Tan(1);


            Assert.Less(expectedTheta - 0.01, target.Theta);
            Assert.Greater(expectedTheta + 0.01, target.Theta);
            Assert.Less(expectedRadius - 0.01, target.Radius);
            Assert.Greater(expectedRadius + 0.01, target.Radius);
        }
        [Test]
        [Category("NonGraphical")]
        public void PolarCoordTest()
        {
            PolarCoord target = PolarCoord.CartesianToPolar(new Coord(5, 5));
            PolarCoord origin = new PolarCoord(7.0710678118654755f, 0.64209261593433065f);
            Coord cartTarget = PolarCoord.PolarToCartesian(target);
            Coord cartOrigin = PolarCoord.PolarToCartesian(origin);

            Assert.AreEqual(target, origin);
            Assert.AreEqual(cartTarget, cartOrigin);
        }

        [Test]
        [Category("NonGraphical")]
        public void RadiansToDegreesTest()
        {
            Assert.Greater(Calculate.RadiansToDegrees((float)Math.PI), 179.99f);
            Assert.Less(Calculate.RadiansToDegrees((float)Math.PI), 180.01f);
        }
        [Test]
        [Category("NonGraphical")]
        public void DegreesToRadiansTest()
        {
            Assert.Greater(Calculate.DegreesToRadians(180), Math.PI - 0.01);
            Assert.Less(Calculate.DegreesToRadians(180), Math.PI + 0.01);
        }
        [Test]
        [Category("NonGraphical")]
        public void PercentTest()
        {
            List<int> previousChances = new List<int>();
            for (int i = 0; i < 50; i++)
            {
                int x = Calculate.PercentValue();
                previousChances.Add(x);
                Assert.Less(x, 101);
                Assert.Greater(x, 0);
            }

            Assert.Greater(previousChances.Distinct().Count(), 12);
        }
        [Test]
        [Category("NonGraphical")]
        public void DistanceBetweenTest()
        {
            double five = Calculate.DistanceBetween(new Coord(0, 0), new Coord(3, 4));
            Assert.AreEqual(5, five, "Got an answer other than 5");
        }
    }
}
