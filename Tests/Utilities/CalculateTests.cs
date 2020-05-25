using Engine;
using Engine.Components.Creature;
using GoRogue;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tests.Utilities
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
            bool answer = f(63, 35, TimeSpan.FromMilliseconds(100));
            Assert.AreNotEqual(0.000, answer); //I mean, statistically...
            //Assert.Fail();
        }
        [Test]
        public void InnerFromOuterPointsTest()
        {
            List<Coord> inner = new List<Coord>();
            List<Coord> outer = new List<Coord>();

            outer.AddRange(Calculate.PointsAlongStraightLine(new Coord(1, 1), new Coord(5, 0)));
            outer.AddRange(Calculate.PointsAlongStraightLine(new Coord(5, 0), new Coord(7, 3)));
            outer.AddRange(Calculate.PointsAlongStraightLine(new Coord(7, 3), new Coord(3, 4)));
            outer.AddRange(Calculate.PointsAlongStraightLine(new Coord(3, 4), new Coord(1, 1)));

            inner = Calculate.InnerFromOuterPoints(outer).ToList();
            Assert.Greater(outer.Count * 10, inner.Count);

            Assert.IsFalse(inner.Contains(new Coord(-5, -5)));
            Assert.IsFalse(inner.Contains(new Coord(1, 2)));
            Assert.IsFalse(inner.Contains(new Coord(9, 8)));
            Assert.IsFalse(inner.Contains(new Coord(6, 15)));

            Assert.IsTrue(inner.Contains(new Coord(2, 1)));
            Assert.IsTrue(inner.Contains(new Coord(4, 1)));
            Assert.IsTrue(inner.Contains(new Coord(6, 3)));
            Assert.IsTrue(inner.Contains(new Coord(3, 3)));
        }
        [Test]
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
        public void EnumValueFromIndexTest()
        {
            BloodType first = Calculate.EnumValueFromIndex<BloodType>(0);
            int amountOfTypes = Calculate.EnumLength<BloodType>();
            BloodType last = Calculate.EnumValueFromIndex<BloodType>(amountOfTypes - 1);
            Assert.AreNotEqual(first, last);
            last -= amountOfTypes;
            last++;
            Assert.AreEqual(first, last);
        }
        [Test]
        public void EnumLengthTest()
        {
            int answer = Calculate.EnumLength<BloodType>();
            Assert.AreEqual(4, answer);
        }
        [Test]
        public void RandomEnumValueTest()
        {
            BloodType type = Calculate.RandomEnumValue<BloodType>();
            bool makesnew = false;
            bool makesA = false;
            bool makesAB = false;
            bool makesO = false;
            bool makesB = false;
            for (int i = 0; i < 50; i++)
            {
                if (Calculate.RandomEnumValue<BloodType>() != type)
                    makesnew = true;
                if (type == BloodType.A)
                    makesA = true;
                if (type == BloodType.B)
                    makesB = true;
                if (type == BloodType.AB)
                    makesAB = true;
                if (type == BloodType.O)
                    makesO = true;

                type = Calculate.RandomEnumValue<BloodType>();
            }

            Assert.IsTrue(makesnew);
            Assert.IsTrue(makesA);
            Assert.IsTrue(makesB);
            Assert.IsTrue(makesAB);
            Assert.IsTrue(makesO);

        }

        [Test] //skip for now
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
            double expectedTheta = 1 / Math.Tan(1);


            Assert.Less(expectedTheta - 0.01, target.Theta);
            Assert.Greater(expectedTheta + 0.01, target.Theta);
            Assert.Less(expectedRadius - 0.01, target.Radius);
            Assert.Greater(expectedRadius + 0.01, target.Radius);
        }
        [Test]
        public void PolarCoordTest()
        {
            PolarCoord target = Calculate.CartesianToPolar(new Coord(5, 5));
            PolarCoord origin = new PolarCoord(7.0710678118654755d, 0.64209261593433065);
            Coord cartTarget = Calculate.PolarToCartesian(target);
            Coord cartOrigin = Calculate.PolarToCartesian(origin);

            Assert.AreEqual(target.Radius, origin.Radius);
            Assert.AreEqual(target.Theta, origin.Theta);
            Assert.AreEqual(cartOrigin, cartTarget);
        }

        [Test]
        public void RadiansToDegreesTest()
        {
            Assert.Greater(Calculate.RadiansToDegrees(Math.PI), 179.99f);
            Assert.Less(Calculate.RadiansToDegrees(Math.PI), 180.01f);
        }
        [Test]
        public void DegreesToRadiansTest()
        {
            Assert.Greater(Calculate.DegreesToRadians(180), Math.PI - 0.01);
            Assert.Less(Calculate.DegreesToRadians(180), Math.PI + 0.01);
        }
        [Test]
        public void PercentTest()
        {
            List<int> previousChances = new List<int>();
            for (int i = 0; i < 50; i++)
            {
                int x = Calculate.Percent();
                previousChances.Add(x);
                Assert.Less(x, 101);
                Assert.Greater(x, 0);
            }

            Assert.Greater(previousChances.Distinct().Count(), 12);
        }
        [Test]
        public void DistanceBetweenTest()
        {
            double five = Calculate.DistanceBetween(new Coord(0, 0), new Coord(3, 4));
            Assert.AreEqual(5, five, "Got an answer other than 5");
        }

        [Test]
        public void BoundedTanTest()
        {
            for (int i = 0; i < 100; i++)
            {
                double fx = Calculate.BoundedTan(i);
                Assert.Greater(1.01, fx);
                Assert.Less(-1.01, fx);
            }
        }
    }
}
