using GoRogue;
using System.Collections.Generic;
using System.Linq;
using Region = SadConsole.Maps.Region;

namespace Engine.Maps
{
    internal static class RegionExtensions
    {
        internal static int Left(this Region r) => r.OuterPoints.OrderBy(c => c.X).ToList().First().X; 
        internal static int Right(this Region r) => r.OuterPoints.OrderBy(c => c.X).ToList().Last().X; 
        internal static int Top(this Region r) => r.OuterPoints.OrderBy(c => c.Y).ToList().First().Y; 
        internal static int Bottom(this Region r) => r.OuterPoints.OrderBy(c => c.Y).ToList().Last().Y;
        
        internal static int LeftAt(this Region r, int y) => r.OuterPoints.Where(c => c.Y == y).OrderBy(c => c.X).First().X;
        internal static int RightAt(this Region r, int y) => r.OuterPoints.Where(c => c.Y == y).OrderBy(c => c.X).Last().X;
        internal static int TopAt(this Region r, int x) => r.OuterPoints.Where(c => c.X == x).OrderBy(c => c.Y).First().X;
        internal static int BottomAt(this Region r, int x) => r.OuterPoints.Where(c => c.X == x).OrderBy(c => c.Y).Last().X;

        internal static Coord SouthEastCorner(this Region r) => r.OuterPoints.Where(c => c.Y == r.Bottom()).OrderBy(c => c.X).ToList().Last();
        internal static Coord SouthWestCorner(this Region r) => r.OuterPoints.Where(c => c.Y == r.Bottom()).OrderBy(c => c.X).ToList().First();
        internal static Coord NorthWestCorner(this Region r) => r.OuterPoints.Where(c => c.Y == r.Top()).OrderBy(c => c.X).ToList().Last();
        internal static Coord NorthEastCorner(this Region r) => r.OuterPoints.Where(c => c.Y == r.Top()).OrderBy(c => c.X).ToList().First();

        internal static IEnumerable<Coord> OverlappingPoints(this Region r, Region other)
        {
            foreach(Coord c in r.InnerPoints)
            {
                if(other.InnerPoints.Contains(c))
                    yield return c;
            }
        }
    }
}
