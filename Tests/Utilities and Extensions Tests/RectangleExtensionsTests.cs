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
            //Assert.Fail("This test is causing the rest not to run, so failing automatically.");
            Rectangle rectangle = new Rectangle(0, 0, 30, 30);
            List<Rectangle> rectangles = rectangle.RecursiveBisect(5).ToList();
            Assert.GreaterOrEqual(rectangles.Count(), 6);
            for (int i = 0; i < 30; i++)
            {
                for (int j = 0; j < 30; j++)
                {
                    int count = rectangles.Where(r => r.Contains(new Coord(i, j))).Count();
                    Assert.GreaterOrEqual(4, count);
                }
            }
        }
        [Test]
        public void BisectTest()
        {
            Rectangle rectangle = new Rectangle(0, 0, 5, 13);
            List<Rectangle> rectangles = rectangle.Bisect().ToList();
            foreach (Coord c in rectangles[0].Positions())
            {
                Assert.IsTrue(rectangle.Contains(c));
                Assert.IsFalse(rectangles[1].Contains(c));
            }
            foreach (Coord c in rectangles[1].Positions())
            {
                Assert.IsTrue(rectangle.Contains(c));
                Assert.IsFalse(rectangles[0].Contains(c));
            }
            Assert.GreaterOrEqual(rectangles[0].Height, 3);
            Assert.LessOrEqual(rectangles[0].Height, 11);
            Assert.GreaterOrEqual(rectangles[1].Height, 3);
            Assert.LessOrEqual(rectangles[1].Height, 11);
            Assert.AreEqual(5, rectangles[0].Width);
            Assert.AreEqual(5, rectangles[1].Width);

            rectangle = new Rectangle(0, 0, 13, 5);
            rectangles.AddRange(rectangle.Bisect().ToList());
            foreach (Coord c in rectangles[2].Positions())
            {
                Assert.IsTrue(rectangle.Contains(c));
                Assert.IsFalse(rectangles[3].Contains(c));
            }
            foreach (Coord c in rectangles[3].Positions())
            {
                Assert.IsTrue(rectangle.Contains(c));
                Assert.IsFalse(rectangles[2].Contains(c));
            }
            Assert.GreaterOrEqual(rectangles[2].Width, 3);
            Assert.LessOrEqual(rectangles[2].Width, 10);
            Assert.GreaterOrEqual(rectangles[3].Width, 3);
            Assert.LessOrEqual(rectangles[3].Width, 10);
            Assert.AreEqual(5, rectangles[2].Height);
            Assert.AreEqual(5, rectangles[3].Height);
        }
        [Test]
        public void BisectHorizontallyTest()
        {
            Rectangle rectangle = new Rectangle(0, 0, 5, 13);
            List<Rectangle> rectangles = rectangle.BisectHorizontally().ToList();
            foreach (Coord c in rectangles[0].Positions())
            {
                Assert.IsTrue(rectangle.Contains(c));
                Assert.IsFalse(rectangles[1].Contains(c));
            }
            Assert.GreaterOrEqual(rectangles[0].Height, 3);
            Assert.LessOrEqual(rectangles[0].Height, 10);
            Assert.GreaterOrEqual(rectangles[1].Height, 3);
            Assert.LessOrEqual(rectangles[1].Height, 10);
            Assert.AreEqual(5, rectangles[0].Width);
            Assert.AreEqual(5, rectangles[1].Width);
            
        }
        [Test]
        public void BisectVerticallyTest()
        {
            Rectangle rectangle = new Rectangle(0, 0, 13, 5);
            List<Rectangle> rectangles = rectangle.BisectVertically().ToList();
            foreach (Coord c in rectangles[0].Positions())
            {
                Assert.IsTrue(rectangle.Contains(c));
                Assert.IsFalse(rectangles[1].Contains(c));
            }
            Assert.GreaterOrEqual(rectangles[0].Width, 3);
            Assert.LessOrEqual(rectangles[0].Width, 10);
            Assert.GreaterOrEqual(rectangles[1].Width, 3);
            Assert.LessOrEqual(rectangles[1].Width, 10);
            Assert.AreEqual(5, rectangles[0].Height);
            Assert.AreEqual(5, rectangles[1].Height);
        }
    }
}
