using Engine.Utilities.Mathematics;
using GoRogue;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Scenes.Areas
{
    public class Block : Area
    {
        public List<Coord> SouthFenceLine { get; private set; }
        public List<Coord> NorthFenceLine { get; private set; }
        public List<Coord> WestFenceLine { get; private set; }
        public List<Coord> EastFenceLine { get; private set; }
        public List<Coord> Addresses { get; private set; }
        public List<Coord> FenceLocations { get => SouthFenceLine.Concat(NorthFenceLine).Concat(WestFenceLine).Concat(EastFenceLine).ToList(); }
        public Block(RoadIntersection nw, RoadIntersection sw, RoadIntersection se, RoadIntersection ne) :
            base(
                Convert.ToInt32(sw.HorizontalStreet + 1).ToString() + "00 Block " + sw.VerticalStreet.ToString(),
                se.NorthWestCorner,
                ne.SouthWestCorner,
                nw.SouthEastCorner,
                sw.NorthEastCorner
                )
        { }

        //this reads like the drunken noodling of a redneck whose had a few too many. fix it later.
        public IEnumerable<Coord> GetFenceLocations()
        {
            List<Coord> answer = new List<Coord>();
            int houseDistance = 24;
            int length = 0;
            switch (Orientation)
            {
                case SadConsole.Orientation.Horizontal:
                    length += (NorthBoundary.Count + SouthBoundary.Count) / 2;
                    break;
                case SadConsole.Orientation.Vertical:
                    length += (WestBoundary.Count + EastBoundary.Count) / 2;
                    break;
            }

            if (length <= 48) //no fence blocks (too small)
                return answer;

            Coord sw = SouthWestCorner + new Coord(16, -16);
            Coord nw = NorthWestCorner + new Coord(16, 16);
            Coord se = SouthEastCorner + new Coord(-16, -16);
            Coord ne = NorthEastCorner + new Coord(-16, 16);

            Addresses = new List<Coord>();
            WestFenceLine = Calculate.PointsAlongStraightLine(nw, sw).ToList();
            SouthFenceLine = Calculate.PointsAlongStraightLine(sw, se).ToList();
            NorthFenceLine = Calculate.PointsAlongStraightLine(nw, ne).ToList();
            EastFenceLine = Calculate.PointsAlongStraightLine(ne, se).ToList();
            answer.AddRange(WestFenceLine);
            answer.AddRange(EastFenceLine);
            answer.AddRange(NorthFenceLine);
            answer.AddRange(SouthFenceLine);

            if (length <= 73) //one fence block
                return answer;

            List<Coord> lowerBoundary = new List<Coord>();
            List<Coord> upperBoundary = new List<Coord>();

            switch (Orientation)
            {
                case SadConsole.Orientation.Horizontal:
                    lowerBoundary = NorthFenceLine;
                    upperBoundary = SouthFenceLine;
                    answer.AddRange(Calculate.PointsAlongStraightLine(EastFenceLine[EastFenceLine.Count / 2], WestFenceLine[WestFenceLine.Count / 2]));
                    break;
                case SadConsole.Orientation.Vertical:
                    lowerBoundary = WestFenceLine;
                    upperBoundary = EastFenceLine;
                    answer.AddRange(Calculate.PointsAlongStraightLine(NorthFenceLine[NorthFenceLine.Count / 2], SouthFenceLine[SouthFenceLine.Count / 2]));
                    break;
            }

            int halfNumOfAddresses = length / houseDistance;
            Coord offset = Orientation == SadConsole.Orientation.Horizontal ? new Coord(0, 12) : new Coord(12, 0);
            for (int i = 0; i < halfNumOfAddresses; i++)
            {
                int targetIndex = lowerBoundary.Count / halfNumOfAddresses;
                targetIndex *= i;
                Coord start = lowerBoundary[targetIndex];
                targetIndex = upperBoundary.Count / halfNumOfAddresses;
                targetIndex *= i;
                Coord stop = upperBoundary[targetIndex];
                Addresses.Add(start - offset);
                Addresses.Add(stop - offset);

                answer.AddRange(Calculate.PointsAlongStraightLine(start, stop));
            }
            return answer;
        }
    }
}