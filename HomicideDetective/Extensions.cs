using System.Linq;
using GoRogue;
using GoRogue.MapGeneration;
using HomicideDetective.Places;
using HomicideDetective.Words;
using SadRogue.Integration;
using SadRogue.Integration.Maps;
using SadRogue.Primitives;
using SadRogue.Primitives.GridViews;

namespace HomicideDetective
{
    public static class Extensions
    {
        public static Place RandomRoom(this RogueLikeMap map) 
            => map.GoRogueComponents.GetFirst<Place>().SubAreas.RandomItem(pc => pc.Area.InnerPoints.Any(map.Contains));

        public static Point RandomFreeSpace(this RogueLikeMap map)
            => RandomFreeSpace(map, RandomRoom(map));
        
        public static Point RandomFreeSpace(this RogueLikeMap map, Region region)
            => region.Area.InnerPoints.RandomItem(p => map.Contains(p) && map.WalkabilityView[p]);

        public static ISubstantive Info(this RogueLikeEntity rle)
            => rle.AllComponents.GetFirst<ISubstantive>(Constants.SubstantiveTag);
    }
}