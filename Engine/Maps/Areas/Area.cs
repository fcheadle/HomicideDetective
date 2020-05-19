using Engine.Extensions;
using GoRogue;
using SadConsole;
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
        public List<Coord> Connections { get; private set; } = new List<Coord>();
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
        public List<Coord> Points { get => OuterPoints.Concat(InnerPoints).ToList(); }

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

        #region miscellaneous features
        public override string ToString()
        {
            return Name;
        }

        public IEnumerable<Coord> SurroundingPoints(Coord point)
        {
            for (int i = -1; i <= 1; i++)
                for (int j = -1; j <= 1; j++)
                    yield return new Coord(i, j);
        }
        #endregion

        #region utilities
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
            List<Area> areasDistinguished = new List<Area>();
            foreach (Area area in SubAreas.Values.Reverse())
            {
                foreach (Coord point in area.Points.Distinct())
                {
                    foreach (Area distinguishedArea in areasDistinguished)
                    {
                        if (distinguishedArea.Contains(point))
                        {
                            while (area.OuterPoints.Contains(point))
                                area.OuterPoints.Remove(point);

                            while (area.InnerPoints.Contains(point))
                                area.InnerPoints.Remove(point);
                        }
                    }
                }

                areasDistinguished.Add(area);
            }
        }

        public static void AddConnectionBetween(Area a, Area b)
        {
            List<Coord> possible = new List<Coord>();

            foreach(Coord coord in a.OuterPoints.Where(here => b.OuterPoints.Contains(here)))
            {
                possible.Add(coord);
            }

            possible.Remove(possible.First());
            possible.Remove(possible.Last());

            Coord connection = possible.RandomItem();

            a.OuterPoints.Remove(connection);
            a.Connections.Add(connection);
            b.OuterPoints.Remove(connection);
            b.Connections.Add(connection);
        }
        public void RemoveOverlappingOuterpoints(Area imposing)
        {
            foreach (Coord c in imposing.OuterPoints)
            {
                while (OuterPoints.Contains(c))
                    OuterPoints.Remove(c);
                while (InnerPoints.Contains(c))
                    InnerPoints.Remove(c);
            }
        }
        public void RemoveOverlappingInnerpoints(Area imposing)
        {
            foreach (Coord c in imposing.InnerPoints)
            {
                while (OuterPoints.Contains(c))
                    OuterPoints.Remove(c);
                while (InnerPoints.Contains(c))
                    InnerPoints.Remove(c);
            }
        }
        public void RemoveOverlappingPoints(Area imposing)
        {
            foreach (Coord c in imposing.OuterPoints.Concat(imposing.InnerPoints))
            {
                while (OuterPoints.Contains(c))
                    OuterPoints.Remove(c);
                while (InnerPoints.Contains(c))
                    InnerPoints.Remove(c);
            }
        }
        #endregion
    }
}
