using Engine.Entities;
using Engine.Extensions;
using Engine.Maps;
using Engine.Maps.Areas;
using GoRogue;
using GoRogue.Pathing;
using NUnit.Framework;
using SadConsole;
using System.Collections.Generic;
using System.Linq;

namespace Tests.Integration
{
    class HouseGenerationIntegrationTests
    {
        [Test]
        public void GenerateTest()
        {
            House house = new House("paddys pub", new Coord(5, 5), HouseType.PrairieHome, Direction.Types.DOWN);
            house.Generate();
            int nonWalkableCount = 0;
            int doorCount = 0;

            for (int i = 0; i < house.Map.Width; i++)
            {
                for (int j = 0; j < house.Map.Height; j++)
                {
                    Coord target = new Coord(i, j);
                    BasicTerrain terrain = house.Map.GetTerrain<BasicTerrain>(target);
                    if (terrain == null)
                        house.Map.SetTerrain(TerrainFactory.Pavement(target));

                    terrain = house.Map.GetTerrain<BasicTerrain>(target);
                    if (terrain.IsWalkable)
                    {
                        Path path = house.Map.AStar.ShortestPath(new Coord(house.Map.Width / 2, house.Map.Height / 2), target);
                        Assert.NotNull(path, "House produced inaccessible locations from coord " + target.ToString());
                        //Assert.Greater(path.Length, 0, "Path returned no steps");

                        if (!terrain.IsTransparent)
                        {
                            //it's a door
                            doorCount++;
                        }
                    }
                    else
                    {
                        nonWalkableCount++;
                    }
                }
            }

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
            House house = new House("Testatorium", new Coord(0, 0), HouseType.PrairieHome, Direction.Types.DOWN);
            List<Rectangle> rooms = new Rectangle(0, 0, 24, 24).RecursiveBisect(5).ToList();
            List<Area> areas = new List<Area>();
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
