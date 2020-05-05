using GoRogue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Maps
{
    public class Area
    {
        private Coord _southEastCorner;
        private Coord _northEastCorner;
        private Coord _northWestCorner;
        private Coord _southWestCorner;

        private List<Coord> _southBoundary;
        private List<Coord> _eastBoundary;
        private List<Coord> _northBoundary;
        private List<Coord> _westBoundary;

        private int _bottom;
        private int _left;
        private int _right;
        private int _top;

        private SadConsole.Orientation _orientation;
        public string Name { get; set; }
        public List<Coord> OuterPoints { get; set; } = new List<Coord>();
        public List<Coord> InnerPoints { get; set; } = new List<Coord>();
        public Coord Origin { get; set; }
        public List<Coord> SouthBoundary { get => _southBoundary; }
        public List<Coord> NorthBoundary { get => _northBoundary; }
        public List<Coord> EastBoundary { get => _eastBoundary; }
        public List<Coord> WestBoundary { get => _westBoundary; }
        public int Left { get => _left; }
        public int Right { get => _right; }
        public int Top { get => _top; }
        public int Bottom { get => _bottom; }
        public Coord SouthEastCorner { get => _southEastCorner; }
        public Coord SouthWestCorner { get => _southWestCorner; }
        public Coord NorthWestCorner { get => _northWestCorner; }
        public Coord NorthEastCorner { get => _northEastCorner; }
        public SadConsole.Orientation Orientation { get => _orientation; }
        public int Width { get => _right - _left; }
        public int Height { get => _bottom - _top; }
        public int LeftAt(int y) => OuterPoints.Where(c => c.Y == y).OrderBy(c => c.X).First().X;
        public int RightAt(int y) => OuterPoints.Where(c => c.Y == y).OrderBy(c => c.X).Last().X;
        public int TopAt(int x) => OuterPoints.Where(c => c.X == x).OrderBy(c => c.Y).First().Y;
        public int BottomAt(int x) => OuterPoints.Where(c => c.X == x).OrderBy(c => c.Y).Last().Y;
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

            _top = ne.Y < nw.Y ? ne.Y : nw.Y;
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
        public void Shift(Coord origin)
        {
            List<Coord> newOuter = new List<Coord>();
            List<Coord> newInner = new List<Coord>();
            List<Coord> newSouth = new List<Coord>();
            List<Coord> newNorth = new List<Coord>();
            List<Coord> newWest = new  List<Coord>();
            List<Coord> newEast = new List<Coord>();
            foreach (Coord c in OuterPoints)
            {
                newOuter.Add(c + origin);
            }

            foreach (Coord c in InnerPoints)
            {
                newInner.Add(c + origin);
            }

            _southEastCorner += origin;
            _northEastCorner += origin;
            _northWestCorner += origin;
            _southWestCorner += origin;

            foreach(Coord c in _southBoundary) newSouth.Add(c + origin);
            foreach(Coord c in _eastBoundary) newEast.Add(c + origin);
            foreach(Coord c in _northBoundary) newNorth.Add(c + origin);
            foreach(Coord c in _westBoundary) newWest.Add(c + origin);

            _top =    _northEastCorner.Y < _northWestCorner.Y ? _northEastCorner.Y : _northWestCorner.Y;
            _right =  _southEastCorner.X > _northEastCorner.X ? _southEastCorner.X : _northEastCorner.X;
            _left =   _southWestCorner.X < _northWestCorner.X ? _southWestCorner.X : _northWestCorner.X;
            _bottom = _southEastCorner.Y < _southWestCorner.Y ? _southWestCorner.Y : _southEastCorner.Y;
        }

        public virtual Area Shift()
        {
            Shift(Origin);
            return this;// new Area(Name, SouthEastCorner + Origin, NorthEastCorner + Origin, NorthWestCorner + Origin, SouthWestCorner + Origin);
        }
    }
}
