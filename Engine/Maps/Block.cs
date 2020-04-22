using GoRogue;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Maps
{
    internal class Block : Area
    {
        internal List<Coord> Houses { get; private set; }
        internal Block(RoadIntersection nw, RoadIntersection sw, RoadIntersection se, RoadIntersection ne) :
            base(
                Convert.ToInt32(sw.HorizontalStreet).ToString() + "00 Block " + sw.VerticalStreet.ToString(),
                nw.SouthEastCorner,
                sw.NorthEastCorner,
                se.NorthWestCorner,
                ne.SouthWestCorner
                )
        { }

        internal IEnumerable<Coord> GetFenceLocations()
        {
            // left off here - this is returning some bonkers stuff 
            Coord sw = SouthWestCorner + new Coord(-16,-16);
            Coord nw = NorthWestCorner + new Coord(-16, 16);
            Coord se = SouthEastCorner + new Coord(16, -16);
            Coord ne = NorthEastCorner + new Coord(16, 16);
            int centerXTop = (nw.X + ne.X) / 2;
            int centerYLeft = (nw.Y + sw.Y) / 2;
            int centerXBottom = (sw.X + se.X) / 2;
            int centerYRight = (ne.Y + se.Y) / 2;

            List<Coord> answer = new List<Coord>(); 
            answer.AddRange(Calculate.PointsAlongStraightLine(sw, nw));
            answer.AddRange(Calculate.PointsAlongStraightLine(se, sw));
            answer.AddRange(Calculate.PointsAlongStraightLine(ne, nw));
            answer.AddRange(Calculate.PointsAlongStraightLine(ne, se));
            if (Orientation == SadConsole.Orientation.Horizontal)
            {
                answer.AddRange(Calculate.PointsAlongStraightLine(new Coord(centerXTop, (centerYLeft + centerYRight) / 2), new Coord(centerXBottom, (centerYLeft + centerYRight) / 2)).ToList());
                for (int i = 16; i < NorthBoundary.Count - 16; i += 16) //for now
                {
                    Coord start = NorthBoundary[i] + new Coord(0, 24);
                    Coord stop = SouthBoundary[SouthBoundary.Count - i - 1] + new Coord(0, -24);
                    answer.AddRange(Calculate.PointsAlongStraightLine(start, stop));
                }
            }
            else
            {
                answer.AddRange(Calculate.PointsAlongStraightLine(new Coord((centerXTop + centerXBottom) / 2, centerYLeft), new Coord((centerXTop + centerXBottom) / 2, centerYRight)).ToList());
                for (int i = 16; i < EastBoundary.Count - 16; i += 16) //for now
                {
                    Coord start = EastBoundary[i]+ new Coord(16,0);
                    Coord stop = WestBoundary[WestBoundary.Count - i - 1] + new Coord(-16,0);
                    answer.AddRange(Calculate.PointsAlongStraightLine(start, stop));
                }
            }
            return answer;
        }

        internal void Populate()
        {
            throw new NotImplementedException();
        }
    }
}