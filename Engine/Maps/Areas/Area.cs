using Engine.Extensions;
using GoRogue;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Maps
{
    public class Area
    {
        public string Name { get; set; }
        public Coord Origin { get; set; }
        public int Rise { get; internal set; }
        public int Run { get; internal set; }
        public SadConsole.Orientation Orientation { get; }
        public Dictionary<Enum, Area> SubAreas { get; set; } = new Dictionary<Enum, Area>();
        public List<Coord> OuterPoints { get; set; } = new List<Coord>();
        public List<Coord> InnerPoints { get; set; } = new List<Coord>();
        public List<Coord> SouthBoundary { get; } //or southeast boundary in the case of diamonds
        public List<Coord> NorthBoundary { get; }
        public List<Coord> EastBoundary { get; }
        public List<Coord> WestBoundary { get; }
        public int Left { get; }
        public int Right { get; }
        public int Top { get; }
        public int Bottom { get; }
        public Coord SouthEastCorner { get; }
        public Coord SouthWestCorner { get; }
        public Coord NorthWestCorner { get; }
        public Coord NorthEastCorner { get; }
        public int Width { get => Right - Left; }
        public int Height { get => Bottom - Top; }
        public int LeftAt(int y) => OuterPoints.LeftAt(y);
        public int RightAt(int y) => OuterPoints.RightAt(y);
        public int TopAt(int x) => OuterPoints.TopAt(x);
        public int BottomAt(int x) => OuterPoints.BottomAt(x);

        public Area this[Enum e] => SubAreas[e];
        public Area(string name, Coord se, Coord ne, Coord nw, Coord sw)
        {
            Name = name;
            SouthEastCorner = se;
            NorthEastCorner = ne;
            NorthWestCorner = nw;
            SouthWestCorner = sw;
            
            WestBoundary = Calculate.PointsAlongStraightLine(NorthWestCorner, SouthWestCorner).ToList();
            SouthBoundary = Calculate.PointsAlongStraightLine(SouthWestCorner, SouthEastCorner).ToList();
            EastBoundary = Calculate.PointsAlongStraightLine(SouthEastCorner, NorthEastCorner).ToList();
            NorthBoundary = Calculate.PointsAlongStraightLine(NorthEastCorner, NorthWestCorner).ToList();
            
            Rise = se.Y - ne.Y;
            Run = se.X - sw.X;

            Top = ne.Y < nw.Y ? ne.Y : nw.Y;
            Right = se.X > ne.X ? se.X : ne.X;
            Left = sw.X < nw.X ? sw.X : nw.X;
            Bottom = se.Y < sw.Y ? sw.Y : se.Y;

            OuterPoints.AddRange(SouthBoundary);
            OuterPoints.AddRange(NorthBoundary);
            OuterPoints.AddRange(EastBoundary);
            OuterPoints.AddRange(WestBoundary);
            OuterPoints = OuterPoints.Distinct().ToList();
            InnerPoints = InnerFromOuterPoints(OuterPoints).Distinct().ToList();
            Orientation = (NorthBoundary.Count() + SouthBoundary.Count()) / 2 > (EastBoundary.Count() + WestBoundary.Count()) / 2 ? SadConsole.Orientation.Horizontal : SadConsole.Orientation.Vertical;
        }

        public override string ToString()
        {
            return Name;
        }
        public IEnumerable<Coord> Overlap(Area other)
        {
            foreach (Coord c in InnerPoints)
            {
                if (other.InnerPoints.Contains(c))
                    yield return c;
            }
        }
        public bool Contains(Coord c)
        {
            return InnerPoints.Contains(c);
        }
        public Area Shift(Coord origin)
        {
            return new Area(Name, SouthEastCorner + origin, NorthEastCorner + origin, NorthWestCorner + origin, SouthWestCorner + origin);
        }

        public Area Shift()
        {
            return Shift(Origin);
        }

        public static IEnumerable<Coord> InnerFromOuterPoints(List<Coord> outer)
        {
            List<Coord> inner = new List<Coord>();
            outer = outer.OrderBy(x => x.X).ToList();
            for (int i = outer.First().X; i <= outer.Last().X; i++)
            {
                List<Coord> chunk = outer.Where(w => w.X == i).OrderBy(o => o.Y).ToList();
                for (int j = chunk.First().Y; j <= chunk.Last().Y; j++)
                {
                    yield return new Coord(i, j);
                }
            }
        }

        public void DistinguishSubAreas()
        {
            List<Coord> existing = new List<Coord>();
            foreach(Area area in SubAreas.Values)
            {
                List<Coord> remove = new List<Coord>();
                List<Coord> removeOuter = new List<Coord>();

                foreach (Coord point in area.InnerPoints)
                {
                    if (!existing.Contains(point))
                        existing.Add(point);
                    else if (!area.OuterPoints.Contains(point))
                        remove.Add(point);
                }

                foreach (Coord point in remove)
                {
                    while (area.InnerPoints.Contains(point))
                        area.InnerPoints.Remove(point);
                    while (area.OuterPoints.Contains(point))
                        area.OuterPoints.Remove(point);
                }


                foreach (Coord point in removeOuter)
                    while (area.OuterPoints.Contains(point))
                        area.OuterPoints.Remove(point);
            }
        }
    }
}
