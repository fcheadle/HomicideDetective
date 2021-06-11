using HomicideDetective.People;
using SadRogue.Integration;

namespace HomicideDetective
{
    public static class RogueLikeEntityExtensions
    {
        public static Substantive? Info(this RogueLikeEntity self) =>
            self.AllComponents.GetFirstOrDefault<Substantive>();
        
        public static Memories? ThoughtProcess(this RogueLikeEntity self) =>
            self.AllComponents.GetFirstOrDefault<Memories>();
    }
}