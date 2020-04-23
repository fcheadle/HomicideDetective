using GoRogue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Maps
{
    internal class Road : Area
    {
        internal Coord Start { get; }
        internal Coord Stop { get; }
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
        internal List<RoadIntersection> Intersections { get; private set; } = new List<RoadIntersection>();
        internal Road(Coord start, Coord stop, RoadNames name) :
            base(
                name.ToString(),
                new Coord(start.X > stop.X ? start.X : stop.X, start.Y > stop.Y ? start.Y : stop.Y),
                new Coord(start.X > stop.X ? start.X : stop.X, start.Y < stop.Y ? start.Y : stop.Y),
                new Coord(start.X < stop.X ? start.X : stop.X, start.Y > stop.Y ? start.Y : stop.Y),
                new Coord(start.X < stop.X ? start.X : stop.X, start.Y < stop.Y ? start.Y : stop.Y)
            )
        {
            Start = start;
            Stop = stop; 
            StreetName = name;
            InnerPoints = Calculate.PointsAlongStraightLine(start, stop, 8);
            switch (Orientation)
            {                
                default:
                case SadConsole.Orientation.Vertical:
                    OuterPoints = Calculate.PointsAlongStraightLine(new Coord(start.X - 4, start.Y), new Coord(stop.X - 4, stop.Y)).ToList();
                    OuterPoints.AddRange(Calculate.PointsAlongStraightLine(new Coord(start.X + 4, start.Y), new Coord(stop.X + 4, stop.Y)).ToList());
                    OuterPoints.AddRange(Calculate.PointsAlongStraightLine(new Coord(start.X - 4, start.Y), new Coord(stop.X + 4, start.Y)).ToList());
                    OuterPoints.AddRange(Calculate.PointsAlongStraightLine(new Coord(start.X - 4, stop.Y), new Coord(stop.X + 4, stop.Y)).ToList());
                    break;
                case SadConsole.Orientation.Horizontal:
                    OuterPoints = Calculate.PointsAlongStraightLine(new Coord(start.X, start.Y - 4), new Coord(stop.X, stop.Y - 4)).ToList();
                    OuterPoints.AddRange(Calculate.PointsAlongStraightLine(new Coord(start.X, start.Y + 4), new Coord(stop.X, stop.Y + 4)).ToList());
                    OuterPoints.AddRange(Calculate.PointsAlongStraightLine(new Coord(start.X, start.Y - 4), new Coord(start.X, start.Y + 4)).ToList());
                    OuterPoints.AddRange(Calculate.PointsAlongStraightLine(new Coord(stop.X, stop.Y - 4), new Coord(stop.X, stop.Y + 4)).ToList());
                    break;

            }            
        }


        internal Road(Coord start, Coord stop, RoadNumbers number) :
            base(
                number.ToString(),
                new Coord(start.X > stop.X ? start.X : stop.X, start.Y > stop.Y ? start.Y : stop.Y),
                new Coord(start.X > stop.X ? start.X : stop.X, start.Y < stop.Y ? start.Y : stop.Y),
                new Coord(start.X < stop.X ? start.X : stop.X, start.Y > stop.Y ? start.Y : stop.Y),
                new Coord(start.X < stop.X ? start.X : stop.X, start.Y < stop.Y ? start.Y : stop.Y)
                )
        {
            Start = start;
            Stop = stop;
            StreetNumber = number;
            InnerPoints = Calculate.PointsAlongStraightLine(start, stop, 8);
            switch (Orientation)
            {
                default:
                case SadConsole.Orientation.Vertical:
                    OuterPoints = Calculate.PointsAlongStraightLine(new Coord(start.X - 4, start.Y), new Coord(stop.X - 4, stop.Y)).ToList();
                    OuterPoints.AddRange(Calculate.PointsAlongStraightLine(new Coord(start.X + 4, start.Y), new Coord(stop.X + 4, stop.Y)).ToList());
                    OuterPoints.AddRange(Calculate.PointsAlongStraightLine(new Coord(start.X - 4, start.Y), new Coord(stop.X + 4, start.Y)).ToList());
                    OuterPoints.AddRange(Calculate.PointsAlongStraightLine(new Coord(start.X - 4, stop.Y), new Coord(stop.X + 4, stop.Y)).ToList());
                    break;
                case SadConsole.Orientation.Horizontal:
                    OuterPoints = Calculate.PointsAlongStraightLine(new Coord(start.X, start.Y - 4), new Coord(stop.X, stop.Y - 4)).ToList();
                    OuterPoints.AddRange(Calculate.PointsAlongStraightLine(new Coord(start.X, start.Y + 4), new Coord(stop.X, stop.Y + 4)).ToList());
                    OuterPoints.AddRange(Calculate.PointsAlongStraightLine(new Coord(start.X, start.Y - 4), new Coord(start.X, start.Y + 4)).ToList());
                    OuterPoints.AddRange(Calculate.PointsAlongStraightLine(new Coord(stop.X, stop.Y - 4), new Coord(stop.X, stop.Y + 4)).ToList());
                    break;
            }
        }

        internal void AddIntersection(RoadNames name, Road road)
        {
            List<Coord> overlap = Overlap(road).ToList();
            RoadIntersection i = new RoadIntersection(road.StreetNumber, name, overlap);
            Intersections.Add(i);

        }
        internal void AddIntersection(RoadNumbers number, Road road)
        {
            List<Coord> overlap = Overlap(road).ToList();
            RoadIntersection i = new RoadIntersection(number, road.StreetName, overlap);
            Intersections.Add(i);
        }
        internal void AddIntersection(RoadIntersection ri)
        {
            Intersections.Add(ri);
        }
    }

    internal class RoadIntersection : Area
    {
        public RoadNumbers HorizontalStreet { get; }
        public RoadNames VerticalStreet { get; }
        internal RoadIntersection(RoadNumbers horizontalCrossStreet, RoadNames verticalCrossStreet, List<Coord> points) :
            base(
                horizontalCrossStreet.ToString() + "-" + verticalCrossStreet + " Intersection",
                points.OrderBy(x => x.Y).ToList().Last(),
                points.OrderBy(x => x.Y).ToList().Last(),
                points.OrderBy(x => x.Y).ToList().Last(),
                points.OrderBy(x => x.Y).ToList().Last()
                )
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
