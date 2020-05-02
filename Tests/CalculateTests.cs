using Engine;
using GoRogue;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tests
{
    [TestFixture]
    public class CalculateTests
    {
        [Test]
        public void RandomFunction2dTest()
        {
            var f = Calculate.RandomFunction2d();
            double answer = f(77.5);
            Assert.AreNotEqual(0.000, answer); //I mean, statistically...
        }
        [Test]
        public void RandomFunction3dTest()
        {
            var f = Calculate.RandomFunction3d();
            double answer = f(77, -14);
            Assert.AreNotEqual(0.000, answer); //I mean, statistically...
        }
        [Test]
        public void RandomFunction4dTest()
        {
            var f = Calculate.RandomFunction4d();
            double answer = f(63, 35, TimeSpan.FromMilliseconds(100));
            Assert.AreNotEqual(0.000, answer); //I mean, statistically...
        }
        [Test]
        public void InnerFromOuterPointsTest()
        {
            List<Coord> inner = new List<Coord>();
            List<Coord> outer = new List<Coord>();

            outer.AddRange(Calculate.PointsAlongStraightLine(new Coord(2, 3), new Coord(8, 9)));
            outer.AddRange(Calculate.PointsAlongStraightLine(new Coord(8, 9), new Coord(5, 14)));
            outer.AddRange(Calculate.PointsAlongStraightLine(new Coord(5, 14), new Coord(-1, 7)));
            outer.AddRange(Calculate.PointsAlongStraightLine(new Coord(-1, 7), new Coord(2,3)));

            inner = Calculate.InnerFromOuterPoints(outer).ToList();
            Assert.Greater(outer.Count * 10, inner.Count);
        }
        [Test]
        public void PointsAlongStraightLineTest()
        {
            Coord start = new Coord(1, 1);
            Coord stop = new Coord(10, 4);

            List<Coord> line = Calculate.PointsAlongStraightLine(start, stop).ToList();
            Assert.Contains(start, line);
            Assert.Contains(stop, line);
            Assert.Contains(new Coord(3,2), line);
            Assert.Contains(new Coord(4,2), line);
            Assert.Contains(new Coord(6,3), line);
            Assert.Contains(new Coord(7,3), line);
            Assert.Contains(new Coord(9, 4), line);
        }
        [Test]
        public void BorderLocationsTest()
        {
            int xmin = 1;
            int ymin = 1;
            int xmax = 4;
            int ymax = 5;
            var answer = Calculate.BorderLocations(new Rectangle(new Coord(xmin, ymin), new Coord(xmax, ymax)));
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    Coord c = new Coord(i, j);
                    if ((i == xmin && j >= ymin && j <= ymax) || (j == ymin && i >= xmin && i <= xmax))
                        Assert.Contains(c, answer);
                    if ((i == xmax && j >= ymin && j <= ymax) || (j == ymax && i >= xmin && i <= xmax))
                        Assert.Contains(c, answer);
                }
            }

            Assert.AreEqual(14, answer.Count);
        }
        [Test]
        public void EnumValueFromIndexTest()
        {
            BloodTypes first = Calculate.EnumValueFromIndex<BloodTypes>(0);
            int amountOfTypes = Calculate.EnumLength<BloodTypes>();
            BloodTypes last = Calculate.EnumValueFromIndex<BloodTypes>(amountOfTypes - 1);
            Assert.AreNotEqual(first, last);
            last -= amountOfTypes;
            last++;
            Assert.AreEqual(first, last);
        }
        [Test]
        public void EnumLengthTest()
        {
            int answer = Calculate.EnumLength<BloodTypes>();
            Assert.AreEqual(4, answer);
        }
        [Test]
        public void RandomEnumValueTest()
        {
            BloodTypes type = Calculate.RandomEnumValue<BloodTypes>();
            bool makesnew = false;
            bool makesA = false;
            bool makesAB = false;
            bool makesO = false;
            bool makesB = false;
            for (int i = 0; i < 50; i++)
            {
                if (Calculate.RandomEnumValue<BloodTypes>() != type)
                    makesnew = true;
                if (type == BloodTypes.A)
                    makesA = true;
                if (type == BloodTypes.B)
                    makesB = true;
                if (type == BloodTypes.AB)
                    makesAB = true;
                if (type == BloodTypes.O)
                    makesO = true;

                type = Calculate.RandomEnumValue<BloodTypes>();
            }

            Assert.IsTrue(makesnew);
            Assert.IsTrue(makesA);
            Assert.IsTrue(makesB);
            Assert.IsTrue(makesAB);
            Assert.IsTrue(makesO);

        }
        [Test]
        public void MasterFormulaTest()
        {
            var f = Calculate.MasterFormula();
            double z = f(34, -77);
            Assert.NotZero(z); //i mean, statistically...
        }
        [Test]
        public void PolarToCartesianTest()
        {
            PolarCoord origin = new PolarCoord(3.7416573867739413, 0.64209261593433065);
            Coord target = Calculate.PolarToCartesian(origin);
            Assert.AreEqual(new Coord(2, 2), target);
        }
        [Test]
        public void CartesianToPolarTest()
        {
            Coord origin = new Coord(5, 5);
            PolarCoord target = Calculate.CartesianToPolar(origin);
            double expectedRadius = Math.Sqrt(50);
            double expectedTheta = 1/Math.Tan(1);


            Assert.Less(expectedTheta - 0.01, target.Theta);
            Assert.Greater(expectedTheta + 0.01, target.Theta);
            Assert.Less(expectedRadius - 0.01, target.Radius);
            Assert.Greater(expectedRadius + 0.01, target.Radius);

            //origin = new Coord(3, 13);
            //target = Calculate.CartesianToPolar(origin);
            //Assert.Less(0.86, target.theta);
            //Assert.Greater(0.87, target.theta);
            //Assert.AreEqual(4, target.radius);
        }
        [Test]
        public void PolarCoordTest()
        {
            PolarCoord target = Calculate.CartesianToPolar(new Coord(5, 5));
            PolarCoord origin = new PolarCoord(7.0710678118654755d, 0.64209261593433065);
            Assert.AreEqual(target.Radius, origin.Radius);
            Assert.AreEqual(target.Theta, origin.Theta);
            Coord cartTarget = Calculate.PolarToCartesian(target);
            Coord cartOrigin = Calculate.PolarToCartesian(origin);
            Assert.AreEqual(cartOrigin, cartTarget);
        }

        [Test]
        public void RadiansToDegreesTest()
        {
            Assert.Greater(Calculate.RadiansToDegrees(Math.PI), 179.95f);
            Assert.Less(Calculate.RadiansToDegrees(Math.PI), 180.05f);
        }
        [Test]
        public void DegreesToRadiansTest()
        {
            Assert.Greater(Calculate.DegreesToRadians(180), Math.PI - 0.05);
            Assert.Less(Calculate.DegreesToRadians(180), Math.PI + 0.05);
        }
        [Test]
        public void PercentTest()
        {
            for (int i = 0; i < 50; i++)
            {
                Assert.Less(Calculate.Percent(), 101);
                Assert.Greater(Calculate.Percent(), 0);
            }
        }
    }
}
