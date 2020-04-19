using GoRogue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Maps
{
    internal class Road : SadConsole.Maps.Region
    {
        internal Coord Start { get; }
        internal Coord Stop { get; }
        internal string Name 
        { 
            get 
            { 
                try { return StreetName.ToString(); }
                catch { return StreetNumber.ToString(); }
            }
        }
        internal int Value
        {
            get
            {
                try { return Convert.ToInt32(StreetName); }
                catch { return Convert.ToInt32(StreetNumber); }

            }
        }
        internal RoadNames StreetName { get; }
        internal RoadNumbers StreetNumber { get; }
        internal Direction.Types Orientation { get; }
        internal bool Horizontal { get => Orientation == Direction.Types.UP || Orientation == Direction.Types.DOWN; }
        internal List<RoadIntersection> Intersections { get; private set; } = new List<RoadIntersection>();
        internal Road(Coord start, Coord stop, RoadNames name)
        {
            Start = start;
            Stop = stop; 
            StreetName = name;
            InnerPoints = Calculate.PointsAlongStraightLine(start, stop, 8);
            Orientation = Direction.GetCardinalDirection(start, stop).Type;
            switch (Orientation)
            {
                case Direction.Types.UP:
                case Direction.Types.DOWN:
                    OuterPoints = Calculate.PointsAlongStraightLine(new Coord(start.X - 4, start.Y), new Coord(stop.X - 4, stop.Y)).ToList();
                    OuterPoints.AddRange(Calculate.PointsAlongStraightLine(new Coord(start.X + 4, start.Y), new Coord(stop.X + 4, stop.Y)).ToList());
                    OuterPoints.AddRange(Calculate.PointsAlongStraightLine(new Coord(start.X - 4, start.Y), new Coord(stop.X + 4, start.Y)).ToList());
                    OuterPoints.AddRange(Calculate.PointsAlongStraightLine(new Coord(start.X - 4, stop.Y), new Coord(stop.X + 4, stop.Y)).ToList());
                    break;
                case Direction.Types.LEFT:
                case Direction.Types.RIGHT:
                    OuterPoints = Calculate.PointsAlongStraightLine(new Coord(start.X, start.Y - 4), new Coord(stop.X, stop.Y - 4)).ToList();
                    OuterPoints.AddRange(Calculate.PointsAlongStraightLine(new Coord(start.X, start.Y + 4), new Coord(stop.X, stop.Y + 4)).ToList());
                    OuterPoints.AddRange(Calculate.PointsAlongStraightLine(new Coord(start.X, start.Y - 4), new Coord(start.X, start.Y + 4)).ToList());
                    OuterPoints.AddRange(Calculate.PointsAlongStraightLine(new Coord(stop.X, stop.Y - 4), new Coord(stop.X, stop.Y + 4)).ToList());
                    break;
                default: break;
            }            
        }
        internal Road(Coord start, Coord stop, RoadNumbers number)
        {
            Start = start;
            Stop = stop;
            StreetNumber = number;
            InnerPoints = Calculate.PointsAlongStraightLine(start, stop, 8);
            Orientation = Direction.GetCardinalDirection(start, stop).Type;
            switch (Orientation)
            {
                case Direction.Types.UP:
                case Direction.Types.DOWN:
                    OuterPoints = Calculate.PointsAlongStraightLine(new Coord(start.X - 4, start.Y), new Coord(stop.X - 4, stop.Y)).ToList();
                    OuterPoints.AddRange(Calculate.PointsAlongStraightLine(new Coord(start.X + 4, start.Y), new Coord(stop.X + 4, stop.Y)).ToList());
                    OuterPoints.AddRange(Calculate.PointsAlongStraightLine(new Coord(start.X - 4, start.Y), new Coord(stop.X + 4, start.Y)).ToList());
                    OuterPoints.AddRange(Calculate.PointsAlongStraightLine(new Coord(start.X - 4, stop.Y), new Coord(stop.X + 4, stop.Y)).ToList());
                    break;
                case Direction.Types.LEFT:
                case Direction.Types.RIGHT:
                    OuterPoints = Calculate.PointsAlongStraightLine(new Coord(start.X, start.Y - 4), new Coord(stop.X, stop.Y - 4)).ToList();
                    OuterPoints.AddRange(Calculate.PointsAlongStraightLine(new Coord(start.X, start.Y + 4), new Coord(stop.X, stop.Y + 4)).ToList());
                    OuterPoints.AddRange(Calculate.PointsAlongStraightLine(new Coord(start.X, start.Y - 4), new Coord(start.X, start.Y + 4)).ToList());
                    OuterPoints.AddRange(Calculate.PointsAlongStraightLine(new Coord(stop.X, stop.Y - 4), new Coord(stop.X, stop.Y + 4)).ToList());
                    break;
                default: break;
            }
        }
        internal void AddIntersection(Road road)
        {
            List<Coord> overlap = this.OverlappingPoints(road).ToList();
            Intersections.Add(new RoadIntersection(StreetNumber, road.StreetName, overlap));
        }
        internal void AddIntersection(RoadNumbers name, Road road)
        {
            List<Coord> overlap = this.OverlappingPoints(road).ToList();
            Intersections.Add(new RoadIntersection(name, StreetName, overlap));
        }
    }

    internal class RoadIntersection : SadConsole.Maps.Region
    {
        public RoadNumbers HorizontalStreet { get; }
        public RoadNames VerticalStreet { get; }
        internal RoadIntersection(RoadNumbers horizontalCrossStreet, RoadNames verticalCrossStreet, List<Coord> points)
        {
            HorizontalStreet = horizontalCrossStreet;
            VerticalStreet = verticalCrossStreet;
            InnerPoints = points;
            var byX = points.OrderBy(p => p.X);
            var byY = points.OrderBy(p => p.Y);
            int left = byX.First().X;
            int right = byX.Last().X;
            for (int i = left; i < right; i++)
            {
                int top =  byY.Where(p => p.X == i).First().Y;
                int bottom = byY.Where(p => p.X == i).Last().Y;
                OuterPoints.Add(new Coord(i, top));
                OuterPoints.Add(new Coord(i, bottom));
            }
        }
    }
}
