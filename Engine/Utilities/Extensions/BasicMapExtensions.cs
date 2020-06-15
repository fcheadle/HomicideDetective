using Engine.Creatures.Components;
using Engine.Scenes;
using Engine.Scenes.Terrain;
using Engine.Utilities.Mathematics;
using GoRogue;
using GoRogue.GameFramework;
using SadConsole;
using System;
using System.Collections.Generic;

namespace Engine.Utilities.Extensions
{
    public static class BasicMapExtensions
    {
        static TerrainFactory _factory = new TerrainFactory();
        public static BasicMap Subsection(this BasicMap m, Coord start, Coord stop)
        {
            int xDiff = stop.X - start.X;
            int yDiff = stop.Y - start.Y;
            BasicMap map = new BasicMap(xDiff, yDiff, Enum.GetNames(typeof(MapLayer)).Length, Distance.MANHATTAN);
            for (int i = start.X; i < stop.X + 1; i++)
            {
                for (int j = start.Y; j < stop.Y + 1; j++)
                {
                    BasicTerrain t = m.GetTerrain<BasicTerrain>(new Coord(i, j));
                    if (t != null)
                    {
                        Coord c = new Coord(i - start.X, j - start.Y);
                        t = _factory.Copy(t, c);
                        if (map.Contains(c))
                            map.SetTerrain(t);
                    }
                }
            }
            return map;
        }
        public static bool Contains(this BasicMap m, Coord location)
        {
            return location.X >= 0 && location.Y >= 0 && location.X < m.Width && location.Y < m.Height;
        }
        public static BasicMap ReverseHorizontal(this BasicMap m)
        {
            BasicMap map = new BasicMap(m.Width, m.Height, 1, Distance.MANHATTAN);

            for (int i = 0; i < m.Width; i++)
            {
                for (int j = 0; j < m.Height; j++)
                {
                    Coord source = new Coord(i, j);
                    Coord target = new Coord(map.Width - i - 1, j);
                    BasicTerrain t = m.GetTerrain<BasicTerrain>(source);
                    if (t != null)
                    {
                        t = _factory.Copy(t, target);
                        map.SetTerrain(t);
                    }
                }
            }
            return map;
        }
        public static BasicMap ReverseVertical(this BasicMap m)
        {
            BasicMap map = new BasicMap(m.Width, m.Height, 1, Distance.MANHATTAN);
            for (int i = 0; i < m.Width; i++)
            {
                for (int j = 0; j < m.Height; j++)
                {
                    Coord source = new Coord(i, j);
                    Coord target = new Coord(i, m.Height - 1 - j);
                    BasicTerrain t = m.GetTerrain<BasicTerrain>(source);
                    if (t != null)
                    {
                        t = _factory.Copy(t, target);
                        map.SetTerrain(t);
                    }
                }
            }

            return map;
        }
        public static BasicMap SwapXY(this BasicMap m)
        {
            BasicMap map = new BasicMap(m.Width, m.Height, 1, Distance.MANHATTAN);

            for (int i = 0; i < m.Width; i++)
            {
                for (int j = 0; j < m.Height; j++)
                {
                    Coord original = new Coord(j, i);
                    Coord target = new Coord(i, j);
                    BasicTerrain t = m.GetTerrain<BasicTerrain>(original);
                    if (t != null)
                    {
                        t = _factory.Copy(t, target);
                        map.SetTerrain(t);
                    }
                }
            }

            return map;
        }
        public static BasicMap RotateDiscreet(this BasicMap m, int degrees)
        {
            if (degrees % 90 != 0)
                throw new ArgumentOutOfRangeException("degrees must be some multiple of 90.");

            BasicMap map = m;
            for (int i = degrees; i > 0; i -= 90)
            {
                map = map.SwapXY();
                map = map.ReverseVertical();
            }

            return map;
        }
        public static BasicMap Rotate(this BasicMap m, Coord origin, int radius, int degrees)
        {
            BasicMap rotated = new BasicMap(m.Width, m.Height, 1, Distance.EUCLIDEAN);

            //only rotating the bottom right corner, and not even rotating it correctly
            if (degrees % 90 == 0)
            {
                //this rotates more than just the circle - bug, deal with it later
                rotated.Add(m.RotateDiscreet(degrees));
            }
            else
            {
                BasicMap map = m.RotateDiscreet(360);
                while (degrees > 45)
                {
                    degrees -= 90;
                    //rotates more than just this circle - bug - deal with it later
                    map = m.RotateDiscreet(90);
                }
                while (degrees <= -45)
                {
                    degrees += 90;
                    //rotates more than just the circle - bug, fix later
                    map = m.RotateDiscreet(270);
                }
                double radians = Calculate.DegreesToRadians(degrees);

                for (int x = -radius; x < radius; x++)
                {
                    for (int y = -radius; y < radius; y++)
                    {
                        if (radius > Math.Sqrt(x * x + y * y))
                        {
                            int xPrime = (int)(x * Math.Cos(radians) - y * Math.Sin(radians));
                            int yPrime = (int)(x * Math.Sin(radians) + y * Math.Cos(radians));
                            Coord source = new Coord(x, y) + origin;
                            Coord target = new Coord(xPrime, yPrime) + origin;
                            BasicTerrain terrain = map.GetTerrain<BasicTerrain>(source);
                            if (terrain != null && rotated.Contains(target))
                            {
                                rotated.SetTerrain(_factory.Copy(terrain, target));
                            }
                        }
                    }
                }
            }
            return rotated;
        }
        public static void Add(this BasicMap m, BasicMap map) => m.Add(map, new Coord(0, 0));
        public static void Add(this BasicMap m, BasicMap map, Coord origin)
        {
            if (m.Width < map.Width + origin.X && m.Height < map.Height + origin.X)
                throw new ArgumentOutOfRangeException("Parent Map must be larger than or equal to the map we're adding.");
            for (int i = 0; i < map.Width; i++)
            {
                for (int j = 0; j < map.Height; j++)
                {
                    Coord target = new Coord(i + origin.X, j + origin.Y);
                    BasicTerrain t1 = map.GetTerrain<BasicTerrain>(new Coord(i, j));
                    BasicTerrain t2 = m.GetTerrain<BasicTerrain>(new Coord(i, j));
                    if (t1 != null && t2 == null && m.Contains(target))
                    {
                        t1 = _factory.Copy(t1, target);
                        m.SetTerrain(t1);
                    }
                }
            }
        }
        public static void ReplaceTiles(this BasicMap m, BasicMap map, Coord origin)
        {
            for (int i = 0; i < map.Width; i++)
            {
                for (int j = 0; j < map.Height; j++)
                {
                    Coord target = new Coord(i + origin.X, j + origin.Y);
                    BasicTerrain t = map.GetTerrain<BasicTerrain>(new Coord(i, j));
                    if (t != null && m.Contains(target))
                    {
                        t = _factory.Copy(t, target);
                        m.SetTerrain(t);
                    }
                }
            }
        }
        public static IEnumerable<BasicEntity> GetCreatures(this BasicMap m)
        {
            foreach (ISpatialTuple<IGameObject> e in m.Entities)
            {
                IGameObject entity = e.Item;
                if (entity.GetType() == typeof(BasicEntity) && entity.GetComponent<ActorComponent>() != null)
                {
                    yield return (BasicEntity)entity;
                }
            }
        }
        public static bool IsClearOfObstructions(this BasicMap m, Coord origin, int width)
        {
            for (int i = origin.X; i < origin.X + width; i++)
            {
                for (int j = origin.Y; j < origin.Y + width; j++)
                {
                    Coord c = new Coord(i, j);
                    if (m.Contains(c))
                    {
                        BasicTerrain t = m.GetTerrain<BasicTerrain>(c);
                        if (t != null)
                        {
                            if (!t.IsWalkable)
                                return false;
                            else if (_factory.Pavement(c) == _factory.Copy(t, c)) //todo: fix this shit
                                return false;
                        }
                    }
                }
            }

            return true;
        }
        public static BasicMap CropToContent(this BasicMap m)
        {
            int minX = m.Width;
            int maxX = 0;
            int minY = m.Height;
            int maxY = 0;

            for (int x = 0; x < m.Width; x++)
            {
                for (int y = 0; y < m.Height; y++)
                {
                    if (m.GetTerrain<BasicTerrain>(new Coord(x, y)) != null)
                    {
                        if (minX > x) minX = x;
                        if (minY > y) minY = y;
                        if (maxX < x) maxX = x;
                        if (maxY < y) maxY = y;
                    }
                }
            }

            int dx = Math.Abs(maxX - minX + 1);
            int dy = Math.Abs(maxY - minY + 1);
            Coord offset = new Coord(-minX, -minY);

            BasicMap map = new BasicMap(dx, dy, EnumUtils.EnumLength<MapLayer>(), Distance.EUCLIDEAN);
            map.ForXForY((point) =>
            {
                if (m.Contains(point - offset))
                {
                    BasicTerrain terrain = m.GetTerrain<BasicTerrain>(point - offset);
                    if (terrain != null)
                    {
                        map.SetTerrain(_factory.Copy(terrain, point));
                    }
                }
            });

            return map;
        }
        public static bool ForXForY(this BasicMap m, Action<Coord> f)
        {//makes it hard to debug, so I don't really recommend using this one any more.
            bool success = true;
            for (int i = 0; i < m.Width; i++)
            {
                for (int j = 0; j < m.Height; j++)
                {
                    Coord point = new Coord(i, j);
                    f(point);
                }
            }
            return success;
        }
        public static bool ForX(this BasicMap m, Action<int> f)
        {
            for (int i = 0; i < m.Width; i++)
            {
                f(i);
            }
            return true;
        }
        public static bool ForY(this BasicMap m, Action<int> f)
        {
            for (int i = 0; i < m.Height; i++)
            {
                f(i);
            }
            return true;
        }
    }
}
