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
            => map.GoRogueComponents.GetFirst<PlaceCollection>().RandomItem(pc => pc.Area.Points.Any(map.Contains));

        public static Point RandomFreeSpace(this RogueLikeMap map)
            => RandomFreeSpace(map, RandomRoom(map).Area);
        
        public static Point RandomFreeSpace(this RogueLikeMap map, Region region)
            => region.InnerPoints.RandomItem(p => map.Contains(p) && map.WalkabilityView[p]);

        public static ISubstantive Info(this RogueLikeEntity rle)
            => rle.AllComponents.GetFirst<ISubstantive>(Constants.SubstantiveTag);
    }
}