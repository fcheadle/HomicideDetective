using Engine;
using Engine.Utilities.Extensions;
using Engine.Utilities.Mathematics;
using GoRogue;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Tests.Utilities.Extensions
{
    [TestFixture]
    public class ListOfCoordExtensionsTests
    {
        List<Coord> _hardCodedRange = new List<Coord>();

        private readonly Coord sw = new Coord(3, 4);
        private readonly Coord nw = new Coord(1, 1);
        private readonly Coord ne = new Coord(5, 0);
        private readonly Coord se = new Coord(7, 3);
        [SetUp]
        public void SetUp()
        {

            _hardCodedRange.AddRange(Calculate.PointsAlongStraightLine(nw, ne));
            _hardCodedRange.AddRange(Calculate.PointsAlongStraightLine(ne, se));
            _hardCodedRange.AddRange(Calculate.PointsAlongStraightLine(se, sw));
            _hardCodedRange.AddRange(Calculate.PointsAlongStraightLine(sw, nw));
        }
        [Test]
        [Category("NonGraphical")]
        public void LeftAtTest()
        {
            Assert.AreEqual(nw.X, _hardCodedRange.LeftAt(nw.Y));
            Assert.AreEqual(4, _hardCodedRange.LeftAt(0));
            Assert.AreEqual(3, _hardCodedRange.LeftAt(4));
        }
        [Test]
        [Category("NonGraphical")]
        public void RightAtTest()
        {
            Assert.AreEqual(ne.X, _hardCodedRange.RightAt(ne.Y));
            Assert.AreEqual(se.X, _hardCodedRange.RightAt(se.Y));
        }
        [Test]
        [Category("NonGraphical")]
        public void TopAtTest()
        {
            Assert.AreEqual(ne.Y, _hardCodedRange.TopAt(ne.X));
            Assert.AreEqual(nw.Y, _hardCodedRange.TopAt(nw.X));
        }
        [Test]
        [Category("NonGraphical")]
        public void BottomAtTest()
        {
            Assert.AreEqual(se.Y, _hardCodedRange.BottomAt(se.X));
            Assert.AreEqual(sw.Y, _hardCodedRange.BottomAt(sw.X));
        }
    }

}
