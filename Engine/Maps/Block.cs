using GoRogue;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Maps
{
    internal class Block : SadConsole.Maps.Region
    {
        public string HundredBlock;
        internal List<Coord> Houses { get; private set; }
        internal Block(RoadIntersection nw, RoadIntersection sw, RoadIntersection se, RoadIntersection ne)
        {
            HundredBlock = Convert.ToInt32(sw.HorizontalStreet).ToString() + "00 Block " + sw.VerticalStreet.ToString();

            Coord start = new Coord(nw.Right(), nw.Bottom());
            Coord stop = new Coord(sw.Right(), sw.Top());
            OuterPoints.AddRange(Calculate.PointsAlongStraightLine(start, stop));

            start = new Coord(sw.Right(), sw.Top());
            stop = new Coord(se.Left(), se.Top());
            OuterPoints.AddRange(Calculate.PointsAlongStraightLine(start, stop));

            start = new Coord(se.Left(), se.Top());
            stop = new Coord(ne.Left(), ne.Bottom());
            OuterPoints.AddRange(Calculate.PointsAlongStraightLine(start, stop));

            start = new Coord(ne.Left(), ne.Bottom());
            stop = new Coord(nw.Right(), se.Top());
            OuterPoints.AddRange(Calculate.PointsAlongStraightLine(start, stop));

            InnerPoints = Calculate.InnerFromOuterPoints(OuterPoints).ToList();
        }

        internal IEnumerable<Coord> GetFenceLocations()
        {
            Coord sw = this.SouthWestCorner();
            Coord nw = this.NorthWestCorner();
            Coord se = this.SouthEastCorner();
            Coord ne = this.NorthEastCorner();
            sw = new Coord(sw.X + 24, sw.Y - 24);
            nw = new Coord(nw.X + 24, nw.Y + 24);
            se = new Coord(se.X - 24, se.Y - 24);
            ne = new Coord(ne.X - 24, ne.Y + 24);
            int centerXTop = (nw.X + ne.X) / 2;
            int centerYLeft = (nw.Y + sw.Y) / 2;
            int centerXBottom = (sw.X + se.X) / 2;
            int centerYRight = (ne.Y + se.Y) / 2;
            List<Coord> west =  Calculate.PointsAlongStraightLine(nw, sw).ToList();
            List<Coord> north = Calculate.PointsAlongStraightLine(nw, ne).ToList();
            List<Coord> east =  Calculate.PointsAlongStraightLine(ne, se).ToList();
            List<Coord> south = Calculate.PointsAlongStraightLine(sw, se).ToList();

            bool horizontal = west.Count + east.Count > north.Count + south.Count;

            List<Coord> answer;
            if (horizontal)
            {
                answer = Calculate.PointsAlongStraightLine(new Coord((nw.X + sw.X) / 2, centerYLeft), new Coord((ne.X + se.X) / 2, centerYRight)).ToList();
                for (int i = nw.X; i < ne.X - 24; i += (north.Count / 24)) //for now
                {
                    Coord start = north[i];
                    Coord stop = south[i];
                    answer.AddRange(Calculate.PointsAlongStraightLine(start, stop));
                }
            }
            else
            {
                answer = Calculate.PointsAlongStraightLine(new Coord(centerXTop, (ne.Y + nw.Y) / 2), new Coord(centerXBottom, (ne.X + se.X) / 2)).ToList();
                for (int i = nw.Y; i < sw.Y - 24; i += (north.Count / 24)) //for now
                {
                    Coord start = north[i];
                    Coord stop = south[i];
                    answer.AddRange(Calculate.PointsAlongStraightLine(start, stop));
                }
            }
            answer.AddRange(west);
            answer.AddRange(south);
            answer.AddRange(north);
            answer.AddRange(east);
            return answer;
        }
    }
}