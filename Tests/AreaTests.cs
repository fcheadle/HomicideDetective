using Engine.Maps;
using GoRogue;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tests
{
    [TestFixture]
    class AreaTests
    {
        private readonly Coord sw = new Coord(3, 4);
        private readonly Coord nw = new Coord(1, 1);
        private readonly Coord ne = new Coord(5, 0);
        private readonly Coord se = new Coord(7, 3);
        Area area;
        [SetUp]
        public void SetUp()
        {
            area = new Area("forbidden zone", se, ne, nw, sw);
            area.Origin = new Coord(1, 1);
        }
        [Test]
        public void AreaTest()
        {
            Assert.AreEqual(18, area.OuterPoints.Count);
            Assert.AreEqual(23, area.InnerPoints.Count);
            Assert.AreEqual(5, area.NorthBoundary.Count);
            Assert.AreEqual(4, area.WestBoundary.Count);
            Assert.AreEqual(5, area.SouthBoundary.Count);
            Assert.AreEqual(4, area.EastBoundary.Count);
        }
        [Test]
        public void ToStringOverrideTest()
        {
            Assert.AreEqual("forbidden zone", area.ToString());
        }
        [Test]
        public void ContainsTest()
        {
            Area area = new Area("forbidden zone", se, ne, nw, sw); 
            Assert.IsFalse(area.Contains(new Coord(-5, -5)));
            Assert.IsFalse(area.Contains(new Coord(1, 2)));
            Assert.IsFalse(area.Contains(new Coord(9, 8)));
            Assert.IsFalse(area.Contains(new Coord(6, 15)));
            Assert.IsTrue(area.Contains(new Coord(2, 1)));
            Assert.IsTrue(area.Contains(new Coord(4, 1)));
            Assert.IsTrue(area.Contains(new Coord(6, 3)));
            Assert.IsTrue(area.Contains(new Coord(3, 3)));
        }
        [Test]
        public void OverlapTest()
        {
            Coord tl = new Coord(0, 0);
            Coord tr = new Coord(2, 0);
            Coord br = new Coord(2, 2);
            Coord bl = new Coord(0, 2);
            Area a2 = new Area("zone of terror", br, tr, tl, bl);
            List<Coord> answer = area.Overlap(a2).ToList();

            foreach (Coord c in answer)
            {
                Assert.IsTrue(area.Contains(c));
                Assert.IsTrue(a2.Contains(c));
            }
        }
        [Test]
        public void LeftAtTest()
        {
            Assert.AreEqual(nw.X, area.LeftAt(nw.Y));
            Assert.AreEqual(3, area.LeftAt(area.Top)); //the rise/run of the top left-right meetx y=0 at x=3
            Assert.AreEqual(3, area.LeftAt(area.Bottom));
        }
        [Test]
        public void RightAtTest()
        {
            Assert.AreEqual(ne.X, area.RightAt(ne.Y));
            Assert.AreEqual(5, area.RightAt(area.Top)); 
            Assert.AreEqual(5, area.RightAt(area.Bottom));
        }
        [Test]
        public void TopAtTest()
        {
            Assert.AreEqual(ne.Y, area.TopAt(ne.X));
            Assert.AreEqual(nw.Y, area.TopAt(nw.X));
            Assert.AreEqual(0, area.TopAt(5));
        }
        [Test]
        public void BottomAtTest()
        {
            Assert.AreEqual(se.Y, area.BottomAt(se.X));
            Assert.AreEqual(sw.Y, area.BottomAt(sw.X));
            Assert.AreEqual(4, area.BottomAt(5));
        }
        [Test]
        public void TopTest()
        {
            Assert.AreEqual(0, area.Top);
        }
        [Test]
        public void BottomTest()
        {
            Assert.AreEqual(4, area.Bottom);
        }
        [Test]
        public void LeftTest()
        {
            Assert.AreEqual(1, area.Left);
        }
        [Test]
        public void RightTest()
        {
            Assert.AreEqual(7, area.Right);
        }
        [Test]
        public void ShiftTest()
        {
            Area a1 = area;
            Area a2 = area.Shift();
            Coord one = new Coord(1, 1);

            foreach (Coord inner in a2.InnerPoints)
                Assert.IsTrue(a1.Contains(inner - one));

        }
        
        [Test]
        public void ShiftWithParametersTest()
        {
            Coord two = new Coord(2, 2);
            Area a1 = area;
            Area a2 = area.Shift(two);

            foreach (Coord inner in a2.InnerPoints)
                Assert.IsTrue(a1.Contains(inner - two));
        }
    }
}
