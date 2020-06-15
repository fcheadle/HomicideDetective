using Engine.Scenes.Areas;
using GoRogue;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Tests.Map.Areas
{
    [TestFixture]
    public class RoadIntersectionTests
    {

        [Test]
        public void NewRoadIntersectionTest()
        {
            List<Coord> points = new List<Coord>();
            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 25; j++)
                {
                    points.Add(new Coord(i, j));
                }
            }
            RoadIntersection intersection = new RoadIntersection(RoadNumbers.Eighteenth, RoadNames.Anaheim, points);

            Coord se = points.OrderBy(c => -c.Y).ThenBy(c => -c.X).ToList().First();
            Coord ne = points.OrderBy(c => c.Y).ThenBy(c => -c.X).ToList().First();
            Coord nw = points.OrderBy(c => c.Y).ThenBy(c => c.X).ToList().First();
            Coord sw = points.OrderBy(c => -c.Y).ThenBy(c => c.X).ToList().First();


            Assert.AreEqual(new Coord(0, 0), intersection.NorthWestCorner);
            Assert.AreEqual(new Coord(0, 24), intersection.SouthWestCorner);
            Assert.AreEqual(new Coord(14, 0), intersection.NorthEastCorner);
            Assert.AreEqual(new Coord(14, 24), intersection.SouthEastCorner);
        }
    }
}
