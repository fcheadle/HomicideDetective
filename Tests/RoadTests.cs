using Engine.Maps;
using GoRogue;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests
{
    [TestFixture]
    public class RoadTests
    {
        Road road;
        Coord start = new Coord(15, 0);
        Coord stop = new Coord(35, 20);
        [Test]
        public void NewHorizontlRoadTest()
        {
            road = new Road(start, stop, RoadNumbers.Thirteenth);
            Assert.Fail();
        }
        [Test]
        public void NewVerticalRoadTest()
        {
            Assert.Fail();
        }
        [Test]
        public void AddIntersectionWithRoadNumber()
        {
            Assert.Fail();
        }
        [Test]
        public void AddIntersectionWithRoadName()
        {
            Assert.Fail();
        }
        [Test]
        public void AddIntersectionTest()
        {
            Assert.Fail();
        }
    }
}
