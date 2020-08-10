using Engine.Scenes;
using Engine.Scenes.Areas;
using Engine.Utilities.Extensions;
using GoRogue;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Tests.Scenes.Areas
{
    [TestFixture]
    public class HouseTests
    {

        [Test]
        public void NewHouseTest()
        {
            House house = new House("humble abode", new Coord(5, 5), HouseType.PrairieHome);
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
            House house = new House("party mansion", new Coord(5, 5), HouseType.PrairieHome);
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
            House house = new House("party mansion", new Coord(5, 5), HouseType.PrairieHome);
            Rectangle room = new Rectangle(6, 9, 12, 18);
            house.CreateRoom(RoomType.Parlor, room);
            Assert.AreEqual(1, house.SubAreas.Count());
            Area parlor = house[RoomType.Parlor];
            Assert.NotNull(parlor);
            Assert.AreEqual(room.Width, parlor.Width);
            Assert.AreEqual(room.Height, parlor.Height);
            //add another room
        }

        [Test]
        public void GenerateTest()
        {
            
            House house = new House("paddys pub", new Coord(5, 5), HouseType.PrairieHome);
            house.Generate();
            int nonWalkableCount = house.Walls.Count();
            int doorCount = house.Doors.Count();

            Assert.Greater(house.SubAreas.Count(), 5);
            Assert.Greater(nonWalkableCount, 25); //i mean, statistically....
            Assert.Greater(doorCount, 6); //i mean, statistically....
            Assert.NotNull(house[RoomType.Parlor]);
            Assert.NotNull(house[RoomType.GuestBathroom]);
            Assert.NotNull(house[RoomType.MasterBedroom]);
            Assert.NotNull(house[RoomType.DiningRoom]);
            Assert.NotNull(house[RoomType.Kitchen]);
        }

        [Test]
        public void RecursiveBisectedRectanglesCanConnect()
        {
            //we need to know for sure that the rectangles returned from this are going to produce connectable rooms
            
            House house = new House("Testatorium", new Coord(0, 0), HouseType.PrairieHome);
            List<Rectangle> rooms = new Rectangle(0, 0, 24, 24).RecursiveBisect(5).ToList();


            int index = 0;
            foreach (Rectangle room in rooms)
            {
                house.CreateRoom((RoomType)index, room);
                index++;
            }

            foreach (Area area in house.SubAreas.Values)
            {
                house.ConnectRoomToNeighbors(area);
            }
            Assert.AreEqual(rooms.Count(), house.SubAreas.Count());
            Assert.Less(rooms.Count() * 2, house.Doors.Count(), "RecursiveBisect() is not returning rectangles which can be used for house generation.");
        }
    }
}
