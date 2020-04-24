using GoRogue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Line = System.Collections.Generic.List<GoRogue.Coord>;

namespace Engine.Maps
{
    /// <summary>
    /// Convenience class for specifying details of a subsection of the map
    /// </summary>
    class Area : SadConsole.Maps.Region
    {
        private Coord _southEastCorner;
        private Coord _northEastCorner;
        private Coord _northWestCorner;
        private Coord _southWestCorner;

        private Line _southBoundary;
        private Line _eastBoundary;
        private Line _northBoundary;
        private Line _westBoundary;

        private int _bottom;
        private int _left;
        private int _right;
        private int _top;

        private SadConsole.Orientation _orientation;
        public string Name { get; }
        public Coord Origin { get; }
        public Line SouthBoundary { get => _southBoundary; }
        public Line NorthBoundary { get => _northBoundary; }
        public Line EastBoundary { get => _eastBoundary; }
        public Line WestBoundary { get => _westBoundary; }
        internal int Left { get => _left; }
        internal int Right { get => _right; }
        internal int Top { get => _top; }
        internal int Bottom { get => _bottom; }
        internal Coord SouthEastCorner { get => _southEastCorner; }
        internal Coord SouthWestCorner { get => _southWestCorner; }
        internal Coord NorthWestCorner { get => _northWestCorner; }
        internal Coord NorthEastCorner { get => _northEastCorner; }
        public SadConsole.Orientation Orientation { get => _orientation; }
        internal int Width { get => _right - _left; }
        internal int Height { get => _bottom - _top; }
        internal int LeftAt(int y) => OuterPoints.Where(c => c.Y == y).OrderBy(c => c.X).First().X;
        internal int RightAt(int y) => OuterPoints.Where(c => c.Y == y).OrderBy(c => c.X).Last().X;
        internal int TopAt(int x) => OuterPoints.Where(c => c.X == x).OrderBy(c => c.Y).First().Y;
        internal int BottomAt(int x) => OuterPoints.Where(c => c.X == x).OrderBy(c => c.Y).Last().Y;
        public Area(string name, Coord se, Coord ne, Coord nw, Coord sw)
        {
            Name = name;
            _southEastCorner = se;
            _northEastCorner = ne;
            _northWestCorner = nw;
            _southWestCorner = sw;
            _westBoundary = Calculate.PointsAlongStraightLine(_northWestCorner, _southWestCorner).ToList();
            _southBoundary = Calculate.PointsAlongStraightLine(_southWestCorner, _southEastCorner).ToList();
            _eastBoundary = Calculate.PointsAlongStraightLine(_southEastCorner, _northEastCorner).ToList();
            _northBoundary = Calculate.PointsAlongStraightLine(_northEastCorner, _northWestCorner).ToList();

            _top = ne.Y > nw.Y ? ne.Y : nw.Y;
            _right = se.X > ne.X ? se.X : ne.X;
            _left = sw.X < nw.X ? sw.X : nw.X;
            _bottom = se.Y < sw.Y ? sw.Y : se.Y;


            OuterPoints.AddRange(_southBoundary);
            OuterPoints.AddRange(_northBoundary);
            OuterPoints.AddRange(_eastBoundary);
            OuterPoints.AddRange(_westBoundary);
            InnerPoints = Calculate.InnerFromOuterPoints(OuterPoints).ToList();
            _orientation = se.X + sw.X > se.Y + sw.Y ? SadConsole.Orientation.Horizontal : SadConsole.Orientation.Vertical;
        }

        public override string ToString()
        {
            return Name;
        }
        internal IEnumerable<Coord> Overlap(Area other)
        {
            foreach (Coord c in InnerPoints)
            {
                if (other.InnerPoints.Contains(c))
                    yield return c;
            }
        }
    }
}
