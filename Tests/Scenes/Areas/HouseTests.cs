using Engine.Scenes;
using Engine.Scenes.Areas;
using GoRogue;
using NUnit.Framework;
using System.Linq;

namespace Tests.Map.Areas
{
    [TestFixture]
    public class HouseTests
    {
        [Datapoints]

        [Test]
        public void NewHouseTest()
        {
            House house = new House("humble abode", new Coord(5, 5), HouseType.PrairieHome, Direction.Types.DOWN);
            Assert.AreEqual("humble abode", house.Name);
            Assert.AreEqual(new Coord(5, 5), house.Origin);
            for (int x = 0; x < 24; x++)
            {
                for (int y = 0; y < 24; y++)
                {
                    Assert.IsTrue(house.Contains(new Coord(x, y) + 5));
                }
            }
        }

        [Test]
        public void ConnectRoomToNeighborsTest()
        {
            House house = new House("party mansion", new Coord(5, 5), HouseType.PrairieHome, Direction.Types.DOWN);
            Rectangle parlor = new Rectangle(0, 0, 12, 18);
            Rectangle dining = new Rectangle(parlor.Width, 7, 5, 5);
            house.CreateRoom(RoomType.Parlor, parlor);
            house.CreateRoom(RoomType.DiningRoom, dining);
            house.ConnectRoomToNeighbors(house[RoomType.Parlor]);
            Assert.AreEqual(2, house.Doors.Count(), "Did not add the both points");
            Assert.AreEqual(house.Doors.ToList()[0], house.Doors.ToList()[1], "Did not add the same connection point to both areas");
            Assert.AreNotEqual(new Coord(0, 0), house.Doors.ToList()[0]);
            Assert.AreNotEqual(new Coord(0, 0), house.Doors.ToList()[1]);
        }
        [Test]
        public void CreateRoomTest()
        {
            House house = new House("party mansion", new Coord(5, 5), HouseType.PrairieHome, Direction.Types.DOWN);
            Rectangle room = new Rectangle(6, 9, 12, 18);
            house.CreateRoom(RoomType.Parlor, room);
            Assert.AreEqual(1, house.SubAreas.Count());
            Area parlor = house[RoomType.Parlor];
            Assert.NotNull(parlor);
            Assert.AreEqual(room.Width, parlor.Width);
            Assert.AreEqual(room.Height, parlor.Height);
            //add another room
        }
    }
}
