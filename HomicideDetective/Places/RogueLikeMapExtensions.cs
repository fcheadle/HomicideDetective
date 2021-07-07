using System.Linq;
using SadRogue.Integration;
using SadRogue.Integration.Maps;
using SadRogue.Primitives;
using SadRogue.Primitives.GridViews;

namespace HomicideDetective.Places
{
    public static class RogueLikeMapExtensions
    {
        public static Point RandomFreeSpace(this RogueLikeMap self)
        {
            var point = self.RandomPosition();
            while (!self.WalkabilityView[point] || self.GetEntitiesAt<RogueLikeEntity>(point).Any())
                point = self.RandomPosition();
            return point;
        }
    }
}