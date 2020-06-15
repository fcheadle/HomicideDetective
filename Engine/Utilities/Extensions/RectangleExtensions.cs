using Engine.Utilities.Mathematics;
using GoRogue;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Utilities.Extensions
{
    public static class RectangleExtensions
    {
        public static IEnumerable<Rectangle> RecursiveBisect(this Rectangle parent, int minimumDimension)
        {
            List<Rectangle> ogChildren = parent.Bisect().ToList();
            List<Rectangle> children = new List<Rectangle>(); //so that we can modify children during the loop
            foreach (Rectangle child in ogChildren)
            {
                if (child.Width <= minimumDimension * 2 && child.Height <= minimumDimension * 2)
                {
                    children.Add(child);
                }
                else
                {
                    children.AddRange(child.RecursiveBisect(minimumDimension));
                }
            }

            return children;
        }

        public static IEnumerable<Rectangle> Bisect(this Rectangle parent)
        {
            if (parent.Width > parent.Height)
                return parent.BisectVertically();

            else if (parent.Width < parent.Height)
                return parent.BisectHorizontally();

            else
                return Calculate.PercentValue() % 2 == 1 ? parent.BisectHorizontally() : parent.BisectVertically();
        }

        //puts a horizontal line through the rectangle
        public static IEnumerable<Rectangle> BisectHorizontally(this Rectangle rectangle)
        {
            int startX = rectangle.MinExtentX;
            int stopY = rectangle.MaxExtentY;
            int stopX = rectangle.MaxExtentX;
            int startY = rectangle.MinExtentY;
            int bisection = 0;
            while (bisection < rectangle.MinExtentY + rectangle.Height / 4 || bisection > rectangle.MaxExtentY - rectangle.Height / 4)
                bisection = rectangle.RandomPosition().Y;


            yield return new Rectangle(new Coord(startX, startY), new Coord(stopX, bisection));
            yield return new Rectangle(new Coord(startX, bisection + 1), new Coord(stopX, stopY));
        }
        public static IEnumerable<Rectangle> BisectVertically(this Rectangle rectangle)
        {
            int startY = rectangle.MinExtentY;
            int stopY = rectangle.MaxExtentY;
            int startX = rectangle.MinExtentX;
            int stopX = rectangle.MaxExtentX;
            int bisection = 0;
            while (bisection < rectangle.MinExtentX + rectangle.Width / 4 || bisection > rectangle.MaxExtentX - rectangle.Width / 4)
                bisection = rectangle.RandomPosition().X;

            yield return new Rectangle(new Coord(startX, startY), new Coord(bisection, stopY));
            yield return new Rectangle(new Coord(bisection + 1, startY), new Coord(stopX, stopY));
        }
    }
}
