using Engine.Extensions;
using GoRogue;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tests
{
    [TestFixture]
    public class RectangleExtensionsTests
    {
        [Test]
        public void RecursiveBisectTest()
        {
            Assert.Fail("Not Implemented Test");
            Rectangle rectangle = new Rectangle(0, 0, 20, 20);
            List<Rectangle> rectangles = rectangle.RecursiveBisect(3).ToList();
            Assert.GreaterOrEqual(rectangles.Count(), 5);
        }
        [Test]
        public void BisectTest()
        {
            Rectangle rectangle = new Rectangle(0, 0, 5, 10);
            List<Rectangle> rectangles = rectangle.BisectHorizontally().ToList();
            foreach (Coord c in rectangles[0].Positions())
            {
                Assert.IsTrue(rectangle.Contains(c));
                if (c.Y != rectangles[0].MinExtentY && c.Y != rectangles[1].MinExtentY && c.Y != rectangles[0].MaxExtentY && c.Y != rectangles[1].MaxExtentY)
                    Assert.IsFalse(rectangles[1].Contains(c));
            }

            rectangle = new Rectangle(0, 0, 10, 5);
            rectangles = rectangle.BisectVertically().ToList();
            foreach (Coord c in rectangles[0].Positions())
            {
                Assert.IsTrue(rectangle.Contains(c));
                if (c.X != rectangles[0].MinExtentX && c.X != rectangles[1].MinExtentX && c.X != rectangles[0].MaxExtentX && c.X != rectangles[1].MaxExtentX)
                    Assert.IsFalse(rectangles[1].Contains(c));
            }
        }
        [Test]
        public void BisectHorizontallyTest()
        {
            Rectangle rectangle = new Rectangle(0, 0, 5, 10);
            List<Rectangle> rectangles = rectangle.BisectHorizontally().ToList();
            foreach (Coord c in rectangles[0].Positions())
            {
                Assert.IsTrue(rectangle.Contains(c));
                if (c.Y != rectangles[0].MinExtentY && c.Y != rectangles[1].MinExtentY && c.Y != rectangles[0].MaxExtentY && c.Y != rectangles[1].MaxExtentY)
                    Assert.IsFalse(rectangles[1].Contains(c));
            }
        }
        [Test]
        public void BisectVerticallyTest()
        {
            Rectangle rectangle = new Rectangle(0, 0, 10, 5);
            List<Rectangle> rectangles = rectangle.BisectVertically().ToList();
            foreach (Coord c in rectangles[0].Positions())
            {
                Assert.IsTrue(rectangle.Contains(c));
                if (c.X != rectangles[0].MinExtentX && c.X != rectangles[1].MinExtentX && c.X != rectangles[0].MaxExtentX && c.X != rectangles[1].MaxExtentX)
                    Assert.IsFalse(rectangles[1].Contains(c));
            }
        }
    }
}
