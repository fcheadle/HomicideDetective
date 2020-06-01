using Engine.Maps;
using GoRogue;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Tests.Map.Areas
{
    [TestFixture]
    public class BlockTests
    {
        Block block;

        RoadIntersection nwIntersection;
        RoadIntersection swIntersection;
        RoadIntersection neIntersection;
        RoadIntersection seIntersection;
        [SetUp]
        public void SetUp()
        {
            List<Coord> points = new List<Coord>()
            {
                new Coord(1,1),
                new Coord(1,2),
                new Coord(2,1),
                new Coord(2,2)
            };

            nwIntersection = new RoadIntersection(RoadNumbers.TwentyFirst, RoadNames.Neumann, points);

            points = new List<Coord>()
            {
                new Coord(5,2),
                new Coord(6,2),
                new Coord(5,3),
                new Coord(6,3)
            };

            neIntersection = new RoadIntersection(RoadNumbers.TwentyFirst, RoadNames.Olive, points);


            points = new List<Coord>()
            {
                new Coord(6,8),
                new Coord(7,8),
                new Coord(6,9),
                new Coord(7,9)
            };

            seIntersection = new RoadIntersection(RoadNumbers.TwentySecond, RoadNames.Neumann, points);


            points = new List<Coord>()
            {
                new Coord(4,6),
                new Coord(5,6),
                new Coord(4,7),
                new Coord(5,7)
            };

            swIntersection = new RoadIntersection(RoadNumbers.TwentySecond, RoadNames.Olive, points);

            block = new Block(nwIntersection, swIntersection, seIntersection, neIntersection);
        }
        [Test]
        public void NewBlockTest()
        {
            Assert.AreEqual(new Coord(5, 3), block.NorthEastCorner);
            Assert.AreEqual(new Coord(6, 8), block.SouthEastCorner);
            Assert.AreEqual(new Coord(2, 2), block.NorthWestCorner);
            Assert.AreEqual(new Coord(5, 6), block.SouthWestCorner);
        }
        [Test]
        public void ToStringOverrideTest()
        {
            Assert.AreEqual("2200 Block Olive", block.ToString());
        }
        [Test]
        public void GetFenceLocationsTest()
        {
            List<Coord> locations = block.GetFenceLocations().ToList();
            Assert.IsEmpty(locations);

            nwIntersection = new RoadIntersection(RoadNumbers.Seventh, RoadNames.Guthrow, new List<Coord>()
            {
                new Coord(0, 0),
                new Coord(0, 1),
                new Coord(1, 0),
                new Coord(1, 1),
            });
            swIntersection = new RoadIntersection(RoadNumbers.Eighth, RoadNames.MatrinLuthorKingJr, new List<Coord>()
            {
                new Coord(3, 40),
                new Coord(3, 41),
                new Coord(4, 40),
                new Coord(4, 41),
            });
            neIntersection = new RoadIntersection(RoadNumbers.Seventh, RoadNames.Guthrow, new List<Coord>()
            {
                new Coord(55, 0),
                new Coord(55, 1),
                new Coord(56, 0),
                new Coord(56, 1),
            });
            seIntersection = new RoadIntersection(RoadNumbers.Eighth, RoadNames.MatrinLuthorKingJr, new List<Coord>()
            {
                new Coord(50, 50),
                new Coord(50, 51),
                new Coord(51, 50),
                new Coord(51, 51),
            });

            block = new Block(nwIntersection, swIntersection, seIntersection, neIntersection);
            locations = block.GetFenceLocations().ToList();
            Assert.IsNotEmpty(locations);

            swIntersection = new RoadIntersection(RoadNumbers.Eighth, RoadNames.MatrinLuthorKingJr, new List<Coord>()
            {
                new Coord(3, 70),
                new Coord(3, 71),
                new Coord(4, 70),
                new Coord(4, 71),
            });

            block = new Block(nwIntersection, swIntersection, seIntersection, neIntersection);
            locations = block.GetFenceLocations().ToList();
            Assert.IsNotEmpty(locations);
        }
    }
}
