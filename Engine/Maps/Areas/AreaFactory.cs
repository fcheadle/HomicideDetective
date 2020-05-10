using GoRogue;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Maps.Areas
{
    public static class AreaFactory
    {
        public static Area Room(string name, Rectangle area)
        {
            return new Area(
                name,
                new Coord(area.MaxExtentX + 1, area.MaxExtentY + 1),
                new Coord(area.MaxExtentX + 1, area.MinExtentY),
                new Coord(area.MinExtentX, area.MinExtentY),
                new Coord(area.MinExtentX, area.MaxExtentY + 1)
            );
        }
    }
}
