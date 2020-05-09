using GoRogue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Extensions
{
    public static class ListOfCoordExtensions
    {
        public static int LeftAt(this List<Coord> self, int y) => self.Where(c => c.Y == y).OrderBy(c => c.X).First().X;
        public static int RightAt(this List<Coord> self, int y) => self.Where(c => c.Y == y).OrderBy(c => c.X).Last().X;
        public static int TopAt(this List<Coord> self, int x) => self.Where(c => c.X == x).OrderBy(c => c.Y).First().Y;
        public static int BottomAt(this List<Coord> self, int x) => self.Where(c => c.X == x).OrderBy(c => c.Y).Last().Y;
    }
}
