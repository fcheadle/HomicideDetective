using Engine.Entities;
using Engine.Maps;
using Engine.Maps.Areas;
using GoRogue;
using GoRogue.Pathing;
using NUnit.Framework;
using SadConsole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tests
{
    [TestFixture]
    public class HouseTests
    {
        [Test]
        public void NewHouseTest()
        {
            House house = new House(new Coord(0,0), HouseType.PrairieHome);
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
                        if (target != new Coord(0, 0))
                        {
                            Path path = house.Map.AStar.ShortestPath(new Coord(0, 0), target);
                            Assert.NotNull("House produced inaccessible locations");
                            Assert.Greater(path.Length, 0, "Path returned no steps");
                        }
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
        }

        [Test]
        public void RandomRoomDimensionTest()
        {
            House house = new House(new Coord(0, 0), HouseType.CentralPassageHouse);
            for (int i = 0; i < 25; i++)
            {
                int dim = house.RandomRoomDimension();
                Assert.GreaterOrEqual(dim, 3);
                Assert.LessOrEqual(dim, 7);
            }
        }
    }
}
