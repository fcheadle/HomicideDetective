using Engine.Utilities.Extensions;
using Engine.Utilities.Mathematics;
using GoRogue;
using NUnit.Framework;
using System.Collections.Generic;

namespace Tests.Utilities.Extensions
{
    [TestFixture]
    public class ListOfCoordExtensionsTests
    {
        List<Coord> _hardCodedRange = new List<Coord>();

        private readonly Coord _sw = new Coord(3, 4);
        private readonly Coord _nw = new Coord(1, 1);
        private readonly Coord _ne = new Coord(5, 0);
        private readonly Coord _se = new Coord(7, 3);
        [SetUp]
        public void SetUp()
        {

            _hardCodedRange.AddRange(Calculate.PointsAlongStraightLine(_nw, _ne));
            _hardCodedRange.AddRange(Calculate.PointsAlongStraightLine(_ne, _se));
            _hardCodedRange.AddRange(Calculate.PointsAlongStraightLine(_se, _sw));
            _hardCodedRange.AddRange(Calculate.PointsAlongStraightLine(_sw, _nw));
        }
        [Test]
        [Category("NonGraphical")]
        public void LeftAtTest()
        {
            Assert.AreEqual(_nw.X, _hardCodedRange.LeftAt(_nw.Y));
            Assert.AreEqual(4, _hardCodedRange.LeftAt(0));
            Assert.AreEqual(3, _hardCodedRange.LeftAt(4));
        }
        [Test]
        [Category("NonGraphical")]
        public void RightAtTest()
        {
            Assert.AreEqual(_ne.X, _hardCodedRange.RightAt(_ne.Y));
            Assert.AreEqual(_se.X, _hardCodedRange.RightAt(_se.Y));
        }
        [Test]
        [Category("NonGraphical")]
        public void TopAtTest()
        {
            Assert.AreEqual(_ne.Y, _hardCodedRange.TopAt(_ne.X));
            Assert.AreEqual(_nw.Y, _hardCodedRange.TopAt(_nw.X));
        }
        [Test]
        [Category("NonGraphical")]
        public void BottomAtTest()
        {
            Assert.AreEqual(_se.Y, _hardCodedRange.BottomAt(_se.X));
            Assert.AreEqual(_sw.Y, _hardCodedRange.BottomAt(_sw.X));
        }
    }

}
