using Engine.Components.Creature;
using Engine.Entities;
using Engine.Maps;
using GoRogue;
using GoRogue.GameFramework;
using SadConsole;
using SadConsole.Maps.Generators.World;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Engine.Extensions
{
    public static class BasicMapExtensions
    {
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
                        t = TerrainFactory.Copy(t, c);
                        if (map.Contains(c))
                            map.SetTerrain(t);
                    }
                }
            }
            return map;
        }
        public static bool Contains(this BasicMap m, Coord location)
        {
            return (location.X >= 0 && location.Y >= 0 && location.X < m.Width && location.Y < m.Height);
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
                        t = TerrainFactory.Copy(t, target);
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
                        t = TerrainFactory.Copy(t, target);
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
                        t = TerrainFactory.Copy(t, target);
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
            BasicMap map = new BasicMap(m.Width, m.Height, 1, Distance.EUCLIDEAN);
            BasicMap rotated = new BasicMap(m.Width, m.Height, 1, Distance.EUCLIDEAN);
            double tan = Math.Tan(Calculate.DegreesToRadians(degrees));

            //only rotating the bottom right corner, and not even rotating it correctly
            if (degrees % 90 == 0)
            {
                //this rotates more than just the circle - bug, deal with it later
                rotated.Add(m.RotateDiscreet(degrees));
            }
            else
            {
                if (degrees <= 45 || degrees > -45)
                {
                    map.Add(m);
                }
                while (degrees > 45)
                {
                    degrees -= 90;
                    //rotates more than just this circle - bug - deal with it later
                    map.Add(m.RotateDiscreet(90));
                }
                while (degrees <= -45)
                {
                    degrees += 90;
                    //rotates more than just the circle - bug, fix later
                    map.Add(m.RotateDiscreet(270));
                }

                for (int x = -radius; x < radius; x++)
                {
                    for (int y = -radius; y < radius; y++)
                    {
                        Coord here = new Coord(x, y) + origin;
                        double sqrt = Math.Sqrt(Math.Abs(x * x - y * y));
                        if (radius >= -sqrt && radius <= sqrt)
                        {
                            int xRatio = (int)(here.Y * tan);
                            int yRatio = (int)(here.X * tan);
                            int yPosition = here.X + yRatio;
                            int xPosition = here.Y - xRatio;
                            Coord position = new Coord(xPosition, yPosition);
                            BasicTerrain t = map.GetTerrain<BasicTerrain>(here);
                            if (t != null && rotated.Contains(position))
                            {
                                t = TerrainFactory.Copy(t, position);
                                rotated.SetTerrain(t);
                            }
                        }
                    }
                }
            }
            return rotated;
        }
        public static void Add(this BasicMap m, BasicMap map) => Add(m, map, new Coord(0, 0));
        public static void Add(this BasicMap m, BasicMap map, Coord origin)
        {
            if ((m.Width < map.Width + origin.X && m.Height < map.Height + origin.X))
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
                        t1 = TerrainFactory.Copy(t1, target);
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
                        t = TerrainFactory.Copy(t, target);
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
                            else if (TerrainFactory.Pavement(c) == TerrainFactory.Copy(t, c)) //todo: fix this shit
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
                    if(m.GetTerrain<BasicTerrain>(new Coord(x,y)) != null)
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

            BasicMap map = new BasicMap(dx, dy, Calculate.EnumLength<MapLayer>(), Distance.EUCLIDEAN);
            map.ForXForY((Coord point) =>
            {
                if (m.Contains(point - offset))
                {
                    BasicTerrain terrain = m.GetTerrain<BasicTerrain>(point - offset);
                    if (terrain != null)
                    {
                        map.SetTerrain(TerrainFactory.Copy(terrain, point));
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
