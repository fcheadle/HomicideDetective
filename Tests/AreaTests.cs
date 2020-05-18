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
            Assert.AreEqual(14, area.OuterPoints.Count);
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

        [Test]
        public void DistinguishSubAreasTest()
        {
            /* 0123456
             * XXXXXXX
             * X     X
             * X     X
             * X     X
             * X     Xxxx
             * X     X  x
             * XXXXXXX  x
             *    x     x
             *    xxxxxxx
             */
            Coord nw = new Coord(0, 0);
            Coord sw = new Coord(0, 9);
            Coord se = new Coord(9, 9);
            Coord ne = new Coord(9, 0);
            Area mainArea = new Area("parent area", ne: ne, nw: nw, se: se, sw: sw);

            nw = new Coord(1, 1);
            se = new Coord(5, 5);
            sw = new Coord(1, 5);
            ne = new Coord(5, 1);
            Area imposingSubArea = new Area("imposing sub area", ne: ne, nw: nw, se: se, sw: sw);

            nw = new Coord(4, 4);
            se = new Coord(8, 8);
            sw = new Coord(4, 8);
            ne = new Coord(8, 4);
            Area hostSubArea = new Area("host sub area", ne: ne, nw: nw, se: se, sw: sw);

            mainArea.SubAreas.Add(RoomType.Parlor, hostSubArea);
            mainArea.SubAreas.Add(RoomType.GuestBathroom, imposingSubArea);

            List<Coord> coords = new List<Coord>();
            mainArea.DistinguishSubAreas();
            foreach (Coord c in imposingSubArea.InnerPoints)
            {
                Assert.IsTrue(mainArea.Contains(c), "Main area somehow had a coord removed.");
                Assert.IsFalse(hostSubArea.Contains(c), "sub area contains a coordinate in imposing area.");
            }
            foreach (Coord c in hostSubArea.InnerPoints)
            {
                Assert.IsTrue(mainArea.Contains(c), "Main area somehow lost a coord.");
                Assert.IsFalse(imposingSubArea.Contains(c), "a coord should have been removed from the sub area but was not.");
            }
        }

        [Test]
        public void AddConnectionBetweenTest()
        {
            Area a = AreaFactory.Rectangle("test-tangle", new Coord(0, 0), 4,4);
            Area b = AreaFactory.Rectangle("rest-ert", new Coord(a.Right, a.Top + 1), 3, 3);
            Area.AddConnectionBetween(a, b);
            Assert.AreEqual(1, a.Connections.Count());
            Assert.AreEqual(1, b.Connections.Count());
        }
    }
}
