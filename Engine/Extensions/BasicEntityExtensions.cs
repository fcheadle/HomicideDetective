using Engine.Maps;
using SadConsole;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Extensions
{
    public static class BasicEntityExtensions
    {
        public static IEnumerable<Area> GetCurrentRegions(this BasicEntity self)
        {
            foreach (Area area in Program.MapScreen.TownMap.Regions)
            {
                if (area.InnerPoints.Contains(self.Position))
                    yield return area;
            }
        }
    }
}
