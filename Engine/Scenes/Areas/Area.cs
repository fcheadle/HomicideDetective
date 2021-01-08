using Engine.Utilities.Extensions;
using Engine.Utilities.Mathematics;
using GoRogue;
using SadConsole;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Scenes.Areas
{
    //Going away in favor of GoRogue Region
    public class Area
    {
        public string Name { get; set; }
        public int Rise { get; private set; }
        public int Run { get; private set; }
        public Orientation Orientation { get; private set; }
        public Dictionary<Enum, Area> SubAreas { get; set; } = new Dictionary<Enum, Area>();
        public List<Coord> OuterPoints { get; set; } = new List<Coord>();
        public List<Coord> InnerPoints { get; set; } = new List<Coord>();
        public List<Coord> SouthBoundary { get; private set; } //or southeast boundary in the case of diamonds
        public List<Coord> NorthBoundary { get; private set; }
        public List<Coord> EastBoundary { get; private set; }
        public List<Coord> WestBoundary { get; private set; }
        public List<Coord> Connections { get; private set; } = new List<Coord>();
        public int Left => SouthWestCorner.X <= NorthWestCorner.X ? SouthWestCorner.X : NorthWestCorner.X;
        public int Right => SouthEastCorner.X >= NorthEastCorner.X ? SouthEastCorner.X : NorthEastCorner.X;
        public int Top => NorthEastCorner.Y <= NorthWestCorner.Y ? NorthEastCorner.Y : NorthWestCorner.Y;
        public int Bottom => SouthEastCorner.Y <= SouthWestCorner.Y ? SouthWestCorner.Y : SouthEastCorner.Y;
        public Coord SouthEastCorner { get; private set; }
        public Coord SouthWestCorner { get; private set; }
        public Coord NorthWestCorner { get; private set; }
        public Coord NorthEastCorner { get; private set; }
        public int Width { get => Right - Left; }
        public int Height { get => Bottom - Top; }
        public List<Coord> Points { get => OuterPoints.Concat(InnerPoints).ToList(); }
        public Coord Center => new Coord((Left + Right) / 2, (Top + Bottom) / 2);
        public int LeftAt(int y) => OuterPoints.LeftAt(y);
        public int RightAt(int y) => OuterPoints.RightAt(y);
        public int TopAt(int x) => OuterPoints.TopAt(x);
        public int BottomAt(int x) => OuterPoints.BottomAt(x);

        public Area this[Enum e] => SubAreas[e];
        public Area(string name, Coord se, Coord ne, Coord nw, Coord sw)
        {
            Name = name;
            Generate(se, ne, nw, sw);
        }

        #region miscellaneous features
        public override string ToString()
        {
            return Name;
        }

        private void Generate(Coord se, Coord ne, Coord nw, Coord sw)
        {
            SouthEastCorner = se;
            NorthEastCorner = ne;
            NorthWestCorner = nw;
            SouthWestCorner = sw;
            Connections = new List<Coord>();
            WestBoundary = Calculate.PointsAlongStraightLine(NorthWestCorner, SouthWestCorner).ToList();
            SouthBoundary = Calculate.PointsAlongStraightLine(SouthWestCorner, SouthEastCorner).ToList();
            EastBoundary = Calculate.PointsAlongStraightLine(SouthEastCorner, NorthEastCorner).ToList();
            NorthBoundary = Calculate.PointsAlongStraightLine(NorthEastCorner, NorthWestCorner).ToList();

            Rise = se.Y - ne.Y;
            Run = se.X - sw.X;
            OuterPoints = new List<Coord>();
            OuterPoints.AddRange(SouthBoundary);
            OuterPoints.AddRange(NorthBoundary);
            OuterPoints.AddRange(EastBoundary);
            OuterPoints.AddRange(WestBoundary);
            OuterPoints = OuterPoints.Distinct().ToList();
            InnerPoints = InnerFromOuterPoints(OuterPoints).Distinct().ToList();
            Orientation = (NorthBoundary.Count() + SouthBoundary.Count()) / 2 > (EastBoundary.Count() + WestBoundary.Count()) / 2 ? Orientation.Horizontal : Orientation.Vertical;
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
            Area area = new Area(Name, SouthEastCorner + origin, NorthEastCorner + origin, NorthWestCorner + origin, SouthWestCorner + origin);
            foreach (var subArea in SubAreas)
            {
                area.SubAreas.Add(subArea.Key, subArea.Value);
            }
            return area;
        }

        public static IEnumerable<Coord> InnerFromOuterPoints(List<Coord> outer)
        {
            outer = outer.OrderBy(x => x.X).ToList();
            for (int i = outer.First().X; i <= outer.Last().X; i++)
            {
                List<Coord> chunk = outer.Where(w => w.X == i).OrderBy(o => o.Y).ToList();
                if(chunk.Count > 0)
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
            List<Coord> coords = a.OuterPoints.Where(here => b.OuterPoints.Contains(here) && !a.IsCorner(here) && !b.IsCorner(here)).ToList();
            foreach (Coord coord in coords)
            {
                possible.Add(coord);
            }
            if (possible.Count() <= 2) return;
            possible.Remove(possible.First());
            possible.Remove(possible.Last());

            Coord connection = possible.RandomItem();

            a.OuterPoints.Remove(connection);
            a.Connections.Add(connection);
            b.OuterPoints.Remove(connection);
            b.Connections.Add(connection);
        }

        private bool IsCorner(Coord here)
        {
            return here == NorthEastCorner || here == NorthWestCorner || here == SouthEastCorner || here == SouthWestCorner;
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
        public virtual Area Rotate(float degrees, bool doToSelf, Coord origin = default)
        {
            Coord center;
            double radians = Calculate.DegreesToRadians(degrees);
            
            List<Coord> corners = new List<Coord>();
            if (origin == default(Coord))
                center = new Coord((Left + Right) / 2, (Top + Bottom) / 2);
            else
                center = origin;

            while (degrees < 0)
                degrees += 360;

            while (degrees > 360)
                degrees -= 360;

            //while (degrees > 90)
            //{
            //    area = QuarterRotation(doToSelf, origin);
            //    quarterTurns++;
            //    degrees -= 90;
            //}
            Coord sw = SouthWestCorner - center;
            int x = (int)(sw.X * Math.Cos(radians) - sw.Y * Math.Sin(radians));
            int y = (int)(sw.X * Math.Sin(radians) + sw.Y * Math.Cos(radians));
            corners.Add(new Coord(x, y) + center);

            Coord se = SouthEastCorner - center;
            x = (int)(se.X * Math.Cos(radians) - se.Y * Math.Sin(radians));
            y = (int)(se.X * Math.Sin(radians) + se.Y * Math.Cos(radians));
            corners.Add(new Coord(x, y) + center);

            Coord nw = NorthWestCorner - center;
            x = (int)(nw.X * Math.Cos(radians) - nw.Y * Math.Sin(radians));
            y = (int)(nw.X * Math.Sin(radians) + nw.Y * Math.Cos(radians));
            corners.Add(new Coord(x, y) + center);

            Coord ne = NorthEastCorner - center;
            x = (int)(ne.X * Math.Cos(radians) - ne.Y * Math.Sin(radians));
            y = (int)(ne.X * Math.Sin(radians) + ne.Y * Math.Cos(radians));
            corners.Add(new Coord(x, y) + center);

            sw = corners.OrderBy(c => -c.Y).ThenBy(c => c.X).First();
            corners.Remove(sw);

            se = corners.OrderBy(c => -c.Y).First();
            corners.Remove(se);

            nw = corners.OrderBy(c => c.X).First();
            corners.Remove(nw);

            ne = corners.OrderBy(c => -c.X).First();

            if (doToSelf)
            {
                Generate(se, ne, nw, sw);

                foreach (Area subArea in SubAreas.Values)
                {
                    subArea.Rotate(degrees, true, center);
                }
                return this;
            }
            else
                return new Area(Name, se, ne, nw, sw);
        }

        #endregion
    }
}
