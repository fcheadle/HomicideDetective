using Engine.Creatures;
using GoRogue;
using GoRogue.GameFramework;
using SadConsole;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Maps
{
    public static class BasicMapExtensions
    {
        public static BasicMap Subsection(this BasicMap m, Coord start, Coord stop)
        {
            int xDiff = stop.X - start.X;
            int yDiff = stop.Y - start.Y;
            int size = xDiff > yDiff ? xDiff : yDiff;
            BasicMap map = new BasicMap(size + 2, size + 2, Enum.GetNames(typeof(MapLayers)).Length, Distance.MANHATTAN);
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

            for (int i = m.Width - 1; i > 0; i--)
            {
                for (int j = 0; j < m.Height; j++)
                {
                    Coord source = new Coord(i, j);
                    Coord target = new Coord(map.Width - i - 1, j);
                    BasicTerrain t = m.GetTerrain<BasicTerrain>(source);
                    if (t != null)
                    {
                        t = Maps.TerrainFactory.Copy(t, target);
                        map.SetTerrain(t);
                    }
                }
            }
            return map;        }
        public static BasicMap ReverseVertical(this BasicMap m)
        {
            BasicMap map = new BasicMap(m.Width, m.Height, 1, Distance.MANHATTAN);
            for (int i = 0; i < m.Width; i++)
            {
                for (int j = m.Height - 1; j > 0; j--)
                {
                    Coord source = new Coord(i, j);
                    Coord target = new Coord(map.Width - i - 1, j);
                    BasicTerrain t = m.GetTerrain<BasicTerrain>(source);
                    if (t != null)
                    {
                        t = TerrainFactory.Copy(t, target);
                        m.SetTerrain(t);
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
                        m.SetTerrain(t);
                    }
                }
            }

            return map;
        }

        public static BasicMap Rotate(this BasicMap m, int degrees)
        {
            if (degrees % 90 != 0)
                throw new ArgumentOutOfRangeException("Degrees must be a multiple of 90.");

            if (m.Width != m.Height)
                throw new Exception("Map must be square (Width == Height) to rotate");

            BasicMap map = new BasicMap(m.Width, m.Height, 1, Distance.MANHATTAN);
            int center = m.Width / 2;
            center--;
            switch (degrees)
            {
                case 90:
                    for (int homeX = 0; homeX < m.Width; homeX++)
                    {
                        for (int homeY = 0; homeY < m.Height; homeY++)
                        {
                            int targetX = m.Height - 1 - homeY;
                            int targetY = homeX;
                            Coord original = new Coord(homeX, homeY);
                            Coord target = new Coord(targetX, targetY);
                            BasicTerrain t = m.GetTerrain<BasicTerrain>(original);
                            if (t != null)
                            {
                                t = TerrainFactory.Copy(t, target);
                                map.SetTerrain(t);
                            }
                        }
                    }
                    break;
                case 180:
                    for (int homeX = 0; homeX < m.Width; homeX++)
                    {
                        for (int homeY = 0; homeY < m.Height; homeY++)
                        {
                            int targetX = m.Height - 1 - homeY;
                            int targetY = m.Width - 1 - homeX;
                            Coord original = new Coord(homeX, homeY);
                            Coord target = new Coord(targetX, targetY);
                            BasicTerrain t = m.GetTerrain<BasicTerrain>(original);
                            if (t != null)
                            {
                                t = TerrainFactory.Copy(t, target);
                                map.SetTerrain(t);
                            }
                        }
                    }
                    break;
                case 270:
                    for (int homeX = 0; homeX < m.Width; homeX++)
                    {
                        for (int homeY = 0; homeY < m.Height; homeY++)
                        {
                            int targetX = homeY;
                            int targetY = m.Width - 1 - homeX;
                            Coord original = new Coord(homeX, homeY);
                            Coord target = new Coord(targetX, targetY);
                            BasicTerrain t = m.GetTerrain<BasicTerrain>(original);
                            if (t != null)
                            {
                                t = TerrainFactory.Copy(t, target);
                                map.SetTerrain(t);
                            }
                        }
                    }
                    break;
            }

            return map;
        }
        public static void Add(this BasicMap m, BasicMap map) => Add(m, map, new Coord(0, 0));
        public static void Add(this BasicMap m, BasicMap map, Coord origin)
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
        public static void ReplaceTiles(this BasicMap m, BasicMap map, Coord origin)
        {
            if ((m.Width >= map.Width + origin.X && m.Height >= map.Height + origin.X))
                throw new ArgumentOutOfRangeException("Map raplacing must be larger than the map we're replacing tiles with");

            for (int i = 0; i < map.Width; i++)
            {
                for (int j = 0; j < map.Height; j++)
                {
                    m.SetTerrain(TerrainFactory.Grass(new Coord(i + origin.X, j + origin.Y)));
                    Coord c = new Coord(i, j);
                    BasicTerrain t = m.GetTerrain<BasicTerrain>(c);
                    if (t != null)
                        m.SetTerrain(TerrainFactory.Copy(t, c));
                }
            }
        }
        public static IEnumerable<Creature> GetCreatures(this BasicMap m)
        {
            foreach (ISpatialTuple<IGameObject> e in m.Entities)
            {
                IGameObject entity = e.Item;
                if (entity.GetType() == typeof(Creature) || entity.GetType().IsAssignableFrom(typeof(IActor)))
                {
                    yield return (Creature)entity;
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
    }
}
