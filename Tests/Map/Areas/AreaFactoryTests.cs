using Engine;
using Engine.Maps.Areas;
using Engine.Mathematics;
using GoRogue;
using NUnit.Framework;
using System.Linq;

namespace Tests.Map.Areas
{
    [TestFixture]
    class AreaFactoryTests
    {
        private readonly Coord start = new Coord(1, 1);
        private readonly int width = 9;
        private readonly int height = 7;
        private readonly int rise = 1;
        private readonly int run = 4;
        private readonly double angleRadians = 0.25;
        [SetUp]
        public void SetUp()
        {
        }
        [Test]
        public void RectangleTest()
        {

            Area room = AreaFactory.Rectangle("my office", start, width, height, angleRadians);

            Coord nw = room.NorthWestCorner;
            Coord sw = room.SouthWestCorner;
            Coord se = room.SouthEastCorner;
            Coord ne = room.NorthEastCorner;

            Assert.AreEqual(nw, new Coord(1, 1));

            Assert.Greater(nw.X, sw.X);
            Assert.Greater(sw.Y, nw.Y);
            Assert.Greater(se.X, sw.X);
            Assert.Greater(se.Y, sw.Y);
            Assert.Greater(ne.X, nw.X);
            Assert.Greater(ne.Y, nw.Y);
            Assert.Greater(ne.X, se.X);
            Assert.Greater(se.Y, ne.Y);

            double topDiff = Calculate.DistanceBetween(nw, ne);
            double rightDiff = Calculate.DistanceBetween(se, ne);
            double bottomDiff = Calculate.DistanceBetween(sw, se);
            double leftDiff = Calculate.DistanceBetween(nw, sw);
            Assert.AreEqual(topDiff, bottomDiff);
            Assert.AreEqual(leftDiff, rightDiff);
        }
        [Test]
        public void ClosetTest()
        {
            Area closet = AreaFactory.Closet("pantry", new Coord(0, 0));
            Assert.AreEqual(3, closet.Right);
            Assert.AreEqual(3, closet.Bottom);
        }
        [Test]
        public static void FromRectangleTest()
        {
            Rectangle rectangle = new Rectangle(new Coord(1, 1), new Coord(5, 5));
            Area area = AreaFactory.FromRectangle("square", rectangle);
            //Assert.AreEqual(25, area.OuterPoints.Count());
            Assert.AreEqual(rectangle.Width, area.Width);
            Assert.AreEqual(rectangle.Height, area.Height);
            Assert.AreEqual(20, area.OuterPoints.Count());
            Assert.AreEqual(6, area.NorthBoundary.Count());
            Assert.AreEqual(6, area.SouthBoundary.Count());
            Assert.AreEqual(6, area.EastBoundary.Count());
            Assert.AreEqual(6, area.WestBoundary.Count());
        }
        [Test]
        public void RegularParallelogramTest()
        {
            int length = 25;
            Coord origin = new Coord(length, length);
            Coord horizontalBound = origin + new Coord(length, 0);
            Coord verticalBound = origin + new Coord(0, length);
            Area parallelogram = AreaFactory.RegularParallelogram("My Parallelogram", origin, length, length, 0);
            Assert.IsTrue(parallelogram.Contains(origin), "Didn't contain the origin.");
            Assert.IsTrue(parallelogram.Contains(origin + length), "Didn't contain expected coordinate of " + (origin + length).ToString());
            Assert.IsTrue(parallelogram.Contains(horizontalBound), "Didn't contain expected top-right corner");
            Assert.IsFalse(parallelogram.Contains(origin + new Coord(0, 2)), "Contained unexpected spot due south of the origin.");
        }
    }
}
