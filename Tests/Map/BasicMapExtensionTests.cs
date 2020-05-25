using GoRogue;
using NUnit.Framework;
using SadConsole;
using System.Linq;
using System;
using Engine.Entities;
using Engine.Extensions;
using System.Collections.Generic;
using Engine.Maps.Areas;
using Color = Microsoft.Xna.Framework.Color;

namespace Tests.Map
{
    class BasicMapExtensionTests : TestBase
    {
        BasicMap _map;

        [SetUp]
        public void Setup()
        {
            _map = new BasicMap(8, 8, 2, Distance.MANHATTAN);
            for (int i = 0; i < _map.Width; i++)
            {
                for (int j = 0; j < _map.Height; j++)
                {
                    int glyph = i + j;
                    Coord c = new Coord(i, j);
                    _map.SetTerrain(TerrainFactory.Test(i + j, c));
                }
            }
        }
        [Test]
        public void SubsectionTest()
        {
            Coord start = new Coord(4, 3);
            Coord stop = new Coord(6, 7);
            BasicMap miniMap = _map.Subsection(start, stop);

            for (int i = 0; i < miniMap.Width; i++)
            {
                for (int j = 0; j < miniMap.Height; j++)
                {
                    Assert.AreEqual(miniMap.GetTerrain<BasicTerrain>(i, j).Glyph, _map.GetTerrain<BasicTerrain>(start.X + i, start.Y + j).Glyph);
                }
            }
        }
        [Test]
        public void ContainsTest()
        {
            Assert.IsTrue(_map.Contains(new Coord(0, 0)));
            Assert.IsTrue(_map.Contains(new Coord(1, 1)));
            Assert.IsTrue(_map.Contains(new Coord(7, 0)));
            Assert.IsTrue(_map.Contains(new Coord(0, 7)));
            Assert.IsTrue(_map.Contains(new Coord(7, 7)));
            Assert.IsTrue(_map.Contains(new Coord(5, 5)));

            Assert.IsFalse(_map.Contains(new Coord(8, 8)));
            Assert.IsFalse(_map.Contains(new Coord(-1, 5)));
            Assert.IsFalse(_map.Contains(new Coord(5, 19)));
            Assert.IsFalse(_map.Contains(new Coord(9, 4)));
        }
        [Test]
        public void ReverseHorizontalTest()
        {
            BasicMap reversed = _map.ReverseHorizontal();

            for (int i = 0; i < reversed.Width; i++)
            {
                for (int j = 0; j < reversed.Height; j++)
                {
                    Coord rev = new Coord(i, j);
                    Coord orig = new Coord(_map.Width - i - 1, j);
                    BasicTerrain r = reversed.GetTerrain<BasicTerrain>(rev);
                    BasicTerrain o = _map.GetTerrain<BasicTerrain>(orig);
                    Assert.AreEqual(r.Glyph, o.Glyph, "ReverseHorizontal() did not edit x/y values correctly at Coord(" + i.ToString() + ", " + j.ToString() + ").");
                }
            }
        }
        [Test]
        public void ReverseVerticalTest()
        {
            BasicMap reversed = _map.ReverseVertical();

            for (int i = 0; i < reversed.Width; i++)
            {
                for (int j = 0; j < reversed.Height; j++)
                {
                    Coord rev = new Coord(i, j);
                    Coord orig = new Coord(i, _map.Height - 1 - j);
                    BasicTerrain r = reversed.GetTerrain<BasicTerrain>(rev);
                    BasicTerrain o = _map.GetTerrain<BasicTerrain>(orig);
                    Assert.AreEqual(r.Glyph, o.Glyph, "ReverseVertical() did not edit x/y values correctly at Coord(" + i.ToString() + ", " + j.ToString() + ").");
                }
            }
        }
        [Test]
        public void SwapXYTest()
        {
            BasicMap reversed = _map.SwapXY();

            for (int i = 0; i < reversed.Width; i++)
            {
                for (int j = 0; j < reversed.Height; j++)
                {
                    Coord rev = new Coord(i, j);
                    Coord orig = new Coord(j, i);
                    BasicTerrain r = reversed.GetTerrain<BasicTerrain>(rev);
                    BasicTerrain o = _map.GetTerrain<BasicTerrain>(orig);
                    Assert.AreEqual(r.Glyph, o.Glyph, "SwapXY() did not swap the x/y values correctly at Coord(" + i.ToString() + ", " + j.ToString() + ").");
                }
            }
        }
        [Test]
        public void RotateDiscreetTest()
        {
            BasicMap rotated = _map.RotateDiscreet(90);
            //Top left corner becomes top right corner
            Coord a = new Coord(0, 0);
            Coord b = new Coord(_map.Width - 1, 0);
            BasicTerrain t1 = rotated.GetTerrain<BasicTerrain>(a);
            BasicTerrain t2 = _map.GetTerrain<BasicTerrain>(b);
            Assert.AreEqual(t1.Glyph, t2.Glyph, "Top left corner wasn't moved to the top right corner.");

            //top right corner becomes bottom right corner
            a = b;
            b = new Coord(_map.Width - 1, _map.Height - 1);
            t1 = rotated.GetTerrain<BasicTerrain>(a);
            t2 = _map.GetTerrain<BasicTerrain>(b);
            Assert.AreEqual(t1.Glyph, t2.Glyph, "Top right quadrant didn't move to the bottom right quadrant.");

            //bottom right corner become bottom left corner
            a = b;
            b = new Coord(0, _map.Height - 1);
            t1 = rotated.GetTerrain<BasicTerrain>(a);
            t2 = _map.GetTerrain<BasicTerrain>(b);
            Assert.AreEqual(t1.Glyph, t2.Glyph, "Bottom right Quadrant didn't move to the bottom left.");

            //bottom left corner becomes top left corner
            a = b;
            b = new Coord(0, 0);
            t1 = rotated.GetTerrain<BasicTerrain>(a);
            t2 = _map.GetTerrain<BasicTerrain>(b);
            Assert.AreEqual(t1.Glyph, t2.Glyph, "Bottom left quadrant didn't move to the top left.");
        }
        
        [Test]
        public void RotateTest()
        {
            _game = new MockGame(Rotate);
            MockGame.RunOnce();
            Assert.Pass();
        }
        private static void Rotate(Microsoft.Xna.Framework.GameTime time)
        {
            BasicMap map = new BasicMap(42, 42, 1, Distance.EUCLIDEAN);
            int radius = 20;
            for (int i = 0; i < map.Width; i++)
            {
                for (int j = 0; j < map.Height; j++)
                {
                    BasicTerrain t = new BasicTerrain(Color.White, Color.Black, (i*j + j) % 256, new Coord(i, j), true, true);
                    map.SetTerrain(t);
                }
            }

            Coord origin = new Coord(21, 21);
            BasicMap rotated = map.Rotate(origin, radius, 45);

            Assert.AreNotEqual(map, rotated, "map.Rotate() did not transform the map in any way.");
            BasicTerrain terrain = rotated.GetTerrain<BasicTerrain>(new Coord());
            Assert.IsNotNull(terrain, "somehow removed the NW corner of the map.");
            terrain = rotated.GetTerrain<BasicTerrain>(new Coord(0, 29));
            Assert.IsNotNull(terrain, "removed the NE corner of the map.");
            terrain = rotated.GetTerrain<BasicTerrain>(new Coord(29, 0));
            Assert.IsNotNull(terrain, "removed the SW corner of the map.");
            terrain = rotated.GetTerrain<BasicTerrain>(new Coord(29,29));
            Assert.IsNotNull(terrain, "removed the SE corner of the map.");

            for (int x = 0; x < map.Width; x++)
            {
                for (int y = 0; y < map.Height; y++)
                {
                    Coord here = new Coord(x, y);
                    Coord delta = origin - here;
                    if(radius > Math.Sqrt(delta.X*delta.X + delta.Y* delta.Y) + 1)
                    {
                        Assert.AreNotEqual(Math.Abs(x * y + y) % 256, rotated.GetTerrain<BasicTerrain>(x, y).Glyph);
                        Assert.AreEqual(Math.Abs(x * y + y) % 256, map.GetTerrain<BasicTerrain>(x, y).Glyph);
                    }
                    else
                    {
                        Assert.AreEqual(Math.Abs(x * y + y) % 256, rotated.GetTerrain<BasicTerrain>(x, y).Glyph);
                        Assert.AreEqual(Math.Abs(x * y + y) % 256, map.GetTerrain<BasicTerrain>(x, y).Glyph);
                    }
                }
            }
        }
        [Test]
        public void ForXTest()
        {

            int width = 33;
            int height = 80;
            BasicMap map = new BasicMap(width, height, 1, Distance.EUCLIDEAN);
            List<Coord> pointsSet = new List<Coord>();
            int counter = 0;
            map.ForX((int x) =>
            {
                double y  = 14.75 * Math.Sin(x / 30.0) + width / 2;
                Coord fx = new Coord(x, (int)y);
                pointsSet.Add(fx);
                map.SetTerrain(TerrainFactory.Test(counter, fx));
                counter++;
            });

            counter = 0;
            for (int x = 0; x < width; x++)
            {
                double y = 14.75 * Math.Sin(x / 30.0) + width / 2;
                Coord fx = new Coord(x, (int)y);
                BasicTerrain terrain = map.GetTerrain<BasicTerrain>(fx);
                Assert.NotNull(terrain);
                Assert.AreEqual(counter, terrain.Glyph);
                counter++;
            }
        }
        [Test]
        public void ForYTest()
        {
            int width = 33;
            int height = 80;
            BasicMap map = new BasicMap(width, height, 1, Distance.EUCLIDEAN);
            List<Coord> pointsSet = new List<Coord>();
            int counter = 0;
            map.ForY((int y) =>
            {
                double x = 14.75 * Math.Sin(y / 30.0) + width / 2;
                Coord fy = new Coord((int)x, y);
                pointsSet.Add(fy);
                map.SetTerrain(TerrainFactory.Test(counter, fy));
                counter++;
            });

            counter = 0;
            for (int y = 0; y < height; y++)
            {
                double x = 14.75 * Math.Sin(y / 30.0) + width / 2;
                BasicTerrain terrain = map.GetTerrain<BasicTerrain>(new Coord((int)x, y));
                Assert.NotNull(terrain);
                Assert.AreEqual(counter, terrain.Glyph);
                counter++;
            }
        }
        [Test]
        public void ForXForYTest()
        {
            BasicMap map = new BasicMap(25, 25, 1, Distance.EUCLIDEAN);
            int counter = 0;
            map.ForXForY((Coord point) =>
            {
                map.SetTerrain(TerrainFactory.Test(counter, point));
                counter++;
            });
            counter = 0;
            for(int i = 0; i < map.Width; i++)
            {
                for (int j = 0; j < map.Height; j++)
                {
                    BasicTerrain t = map.GetTerrain<BasicTerrain>(new Coord(i, j));
                    Assert.AreEqual(counter, t.Glyph);
                    counter++;
                }
            }
        }

        [Test]
        public void AddTest()
        {
            BasicMap largeMap = new BasicMap(18, 18, 1, Distance.MANHATTAN);
            largeMap.Add(_map);
            for (int i = 0; i < _map.Width; i++)
            {
                for (int j = 0; j < _map.Height; j++)
                {
                    BasicTerrain t1 = _map.GetTerrain<BasicTerrain>(new Coord(i, j));
                    BasicTerrain t2 = largeMap.GetTerrain<BasicTerrain>(new Coord(i, j));
                    Assert.AreEqual(t1.Glyph, t2.Glyph);
                }
            }
            BasicMap map = new BasicMap(8, 8, 2, Distance.MANHATTAN);
            map.Add(_map);
            for (int i = 0; i < _map.Width; i++)
            {
                for (int j = 0; j < _map.Height; j++)
                {
                    BasicTerrain t1 = _map.GetTerrain<BasicTerrain>(new Coord(i, j));
                    BasicTerrain t2 = map.GetTerrain<BasicTerrain>(new Coord(i, j));
                    Assert.AreEqual(t1.Glyph, t2.Glyph);
                }
            }

            for (int i = 0; i < map.Width; i++)
            {
                for (int j = 0; j < map.Height; j++)
                {
                    BasicTerrain t1 = _map.GetTerrain<BasicTerrain>(new Coord(i, j));
                    BasicTerrain t2 = map.GetTerrain<BasicTerrain>(new Coord(i, j));
                    Assert.AreEqual(t1.Glyph, t2.Glyph);
                }
            }
        }
        [Test]
        public void AddToLocationTest()
        {
            BasicMap largeMap = new BasicMap(18, 18, 1, Distance.MANHATTAN);
            largeMap.Add(_map, new Coord(_map.Width, _map.Height));
            for (int i = 0; i < _map.Width; i++)
            {
                for (int j = 0; j < _map.Height; j++)
                {
                    BasicTerrain t1 = _map.GetTerrain<BasicTerrain>(new Coord(i, j));
                    BasicTerrain t2 = largeMap.GetTerrain<BasicTerrain>(new Coord(i + _map.Width, j + _map.Height));
                    Assert.AreEqual(t1.Glyph, t2.Glyph);
                }
            }
        }
        [Test]
        public void AddDoesntOverWriteTest()
        {
            BasicMap newMap = new BasicMap(8, 8, 1, Distance.MANHATTAN);

            for (int i = 0; i < 8; i++)
            {
                newMap.SetTerrain(TerrainFactory.Test('#', new Coord(i, i)));
            }

            newMap.Add(_map);

            for (int i = 0; i < _map.Width; i++)
            {
                for (int j = 0; j < _map.Height; j++)
                {
                    BasicTerrain t1 = _map.GetTerrain<BasicTerrain>(new Coord(i, j));
                    BasicTerrain t2 = newMap.GetTerrain<BasicTerrain>(new Coord(i, j));
                    if (i == j)
                    {
                        Assert.AreEqual('#', t2.Glyph);
                    }
                    else
                    {
                        Assert.AreEqual(t1.Glyph, t2.Glyph);
                    }
                }
            }
        }
        [Test]
        public void ReplaceTilesTest()
        {
            BasicMap newMap = new BasicMap(8, 8, 1, Distance.MANHATTAN);

            for (int i = 0; i < 8; i++)
            {
                newMap.SetTerrain(TerrainFactory.Test(i, new Coord(i, i)));
            }

            newMap.ReplaceTiles(_map, new Coord(0, 0));

            for (int i = 0; i < _map.Width; i++)
            {
                for (int j = 0; j < _map.Height; j++)
                {
                    BasicTerrain t1 = _map.GetTerrain<BasicTerrain>(new Coord(i, j));
                    BasicTerrain t2 = newMap.GetTerrain<BasicTerrain>(new Coord(i, j));
                    Assert.AreEqual(t1.Glyph, t2.Glyph);
                }
            }
        }
        [Test]
        public void IsClearOfObstructionsTest()
        {
            Assert.IsTrue(_map.IsClearOfObstructions(new Coord(0, 0), 8));

            _map.SetTerrain(TerrainFactory.Wall(new Coord(0, 0)));

            Assert.IsFalse(_map.IsClearOfObstructions(new Coord(0, 0), 8));

            Assert.IsTrue(_map.IsClearOfObstructions(new Coord(1, 1), 8));
        }

        [Test]
        public void CropToContentTest()
        {
            BasicMap map = new BasicMap(8, 8, 2, Distance.MANHATTAN);
            for (int i = 1; i < map.Width - 1; i++)
            {
                for (int j = 1; j < map.Height - 1; j++)
                {
                    Coord c = new Coord(i, j);
                    map.SetTerrain(TerrainFactory.Test(i + j, c));
                }
            }

            map = map.CropToContent();
            Assert.AreEqual(_map.Width - 2, map.Width);
            Assert.AreEqual(_map.Height - 2, map.Height);
            Assert.AreEqual(2, map.GetTerrain<BasicTerrain>(0, 0).Glyph);


        }
    }
}