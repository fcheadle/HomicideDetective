using Engine.Maps;
using Engine.Maps.Areas;
using GoRogue;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tests
{
    [TestFixture]
    class AreaFactoryTests
    {
        private readonly Coord start = new Coord(1, 1);
        private readonly int width = 9;
        private readonly int height = 7;
        private readonly int rise = 1;
        private readonly int run = 4;
        Area room;

        [SetUp]
        public void SetUp()
        {
            room = AreaFactory.Rectangle("my office", start, width, height, rise, run);
        }
        [Test]
        public void RoomTest()
        {
            Coord nw = room.NorthWestCorner;
            Coord sw = room.SouthWestCorner;
            Coord se = room.SouthEastCorner;
            Coord ne = room.NorthEastCorner;

            Assert.AreEqual(nw, new Coord(1,1));

            Assert.Greater(nw.X, sw.X);
            Assert.Greater(sw.Y, nw.Y);
            Assert.Greater(se.X, sw.X);
            Assert.Greater(se.Y, sw.Y);
            Assert.Greater(ne.X, nw.X);
            Assert.Greater(ne.Y, nw.Y);
            Assert.Greater(ne.X, se.X);
            Assert.Greater(se.Y, ne.Y);

            double topDiff = DistanceBetween(nw, ne);
            double rightDiff = DistanceBetween(se, ne);
            double bottomDiff = DistanceBetween(sw, se);
            double leftDiff = DistanceBetween(nw, sw);
            Assert.AreEqual(topDiff, bottomDiff);
            Assert.AreEqual(leftDiff, rightDiff);
        }

        private double DistanceBetween(Coord start, Coord stop)
        {
            //a^2 + b^2 = c^2
            int xdiff = start.X > stop.X ? start.X - stop.X : stop.X - start.X;
            int ydiff = start.Y > stop.Y ? start.Y - stop.Y : stop.Y - start.Y;
            return Math.Sqrt(xdiff * xdiff + ydiff * ydiff);
        }
    }
}
