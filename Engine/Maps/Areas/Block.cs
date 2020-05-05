using GoRogue;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Maps
{
    internal class Block : Area
    {
        internal List<Coord> SouthFenceLine { get; private set; }
        internal List<Coord> NorthFenceLine { get; private set; }
        internal List<Coord> WestFenceLine { get; private set; }
        internal List<Coord> EastFenceLine { get; private set; }
        internal List<Coord> Addresses { get; private set; }
        internal List<Coord> FenceLocations { get => SouthFenceLine.Concat(NorthFenceLine).Concat(WestFenceLine).Concat(EastFenceLine).ToList(); }
        internal Block(RoadIntersection nw, RoadIntersection sw, RoadIntersection se, RoadIntersection ne) :
            base(
                Convert.ToInt32(sw.HorizontalStreet + 1).ToString() + "00 Block " + sw.VerticalStreet.ToString(),
                se.SouthEastCorner,
                ne.NorthEastCorner,
                nw.NorthWestCorner,
                sw.SouthWestCorner
                )
        { }

        internal IEnumerable<Coord> GetFenceLocations()
        {
            Coord sw = SouthWestCorner + new Coord(16,-16);
            Coord nw = NorthWestCorner + new Coord(16, 16);
            Coord se = SouthEastCorner + new Coord(-16, -16);
            Coord ne = NorthEastCorner + new Coord(-16, 16);

           List<Coord>answer = new List<Coord>();
            Addresses = new List<Coord>();
            WestFenceLine = Calculate.PointsAlongStraightLine(nw, sw).ToList();
            SouthFenceLine = Calculate.PointsAlongStraightLine(sw, se).ToList();
            NorthFenceLine = Calculate.PointsAlongStraightLine(nw, ne).ToList();
            EastFenceLine = Calculate.PointsAlongStraightLine(ne, se).ToList();
            answer.AddRange(WestFenceLine);
            answer.AddRange(EastFenceLine);
            answer.AddRange(NorthFenceLine);
            answer.AddRange(SouthFenceLine);
            if (Orientation == SadConsole.Orientation.Horizontal)
            {
                answer.AddRange(Calculate.PointsAlongStraightLine(EastFenceLine[EastFenceLine.Count / 2], WestFenceLine[WestFenceLine.Count / 2]));
                for (int i = 0; i < NorthFenceLine.Count - 12; i += 16) //for now
                {
                    Coord start = NorthFenceLine[i];
                    Coord stop = SouthFenceLine[i];
                    answer.AddRange(Calculate.PointsAlongStraightLine(start, stop));
                    Addresses.Add(start/* + new Coord(3, 3)*/);
                    Addresses.Add(stop/* + new Coord(3, -13)*/);
                }
            }
            else
            {
                answer.AddRange(Calculate.PointsAlongStraightLine(NorthFenceLine[NorthFenceLine.Count / 2], SouthFenceLine[SouthFenceLine.Count / 2]));
                for (int i = 0; i < WestFenceLine.Count - 12; i += 16) //for now
                {
                    Coord start = WestFenceLine[i];
                    Coord stop = EastFenceLine[i];
                    answer.AddRange(Calculate.PointsAlongStraightLine(start, stop));
                    Addresses.Add(start + new Coord(-3, 3));
                    Addresses.Add(stop + new Coord(-13, 3));
                }
            }
            return answer;
        }
    }
}