using GoRogue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Extensions
{
    public static class RectangleExtensions
    {
        public static IEnumerable<Rectangle> RecursiveBisect(this Rectangle parent, int minimumDimension)
        {
            List<Rectangle> children = Bisect(parent).ToList();
            foreach(Rectangle child in children)
                if(child.Width > minimumDimension && child.Height > minimumDimension)
                {
                    children.AddRange(RecursiveBisect(child, minimumDimension));
                }
            return children;
        }

        public static IEnumerable<Rectangle> Bisect(Rectangle parent)
        {
            if (parent.Width > parent.Height)
                return BisectHorizontally(parent);

            else if (parent.Width < parent.Height)
                return BisectVertically(parent);

            else
                return Calculate.Percent() % 2 == 1 ? BisectHorizontally(parent) : BisectVertically(parent);
        }

        public static IEnumerable<Rectangle> BisectHorizontally(this Rectangle rectangle)
        {
            int startY = rectangle.MinExtentY;
            int stopY = rectangle.MaxExtentY;
            int startX = 0;
            while (startX < rectangle.MinExtentX - rectangle.Width / 5 || startX > rectangle.MaxExtentX - rectangle.Width / 5)
                startX = rectangle.RandomPosition().X;

            int stopX = startX;
            yield return new Rectangle(new Coord(rectangle.MinExtentX, startY), new Coord(stopX, stopY));
            yield return new Rectangle(new Coord(startX, startY), new Coord(rectangle.MaxExtentX, stopY));
        }
        public static IEnumerable<Rectangle> BisectVertically(this Rectangle rectangle)
        {
            int startX = rectangle.MinExtentX;
            int stopX = rectangle.MaxExtentX;
            int startY = 0;
            while (startY < rectangle.MinExtentX - rectangle.Width / 5 || startY > rectangle.MaxExtentX - rectangle.Width / 5)
                startY = rectangle.RandomPosition().X;

            int stopY = startY;
            yield return new Rectangle(new Coord(startX, rectangle.MinExtentY), new Coord(stopX, startY));
            yield return new Rectangle(new Coord(startX, startY), new Coord(stopX, rectangle.MaxExtentY));
        }
    }
}
