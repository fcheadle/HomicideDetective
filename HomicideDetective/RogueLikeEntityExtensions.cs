using HomicideDetective.People;
using SadRogue.Integration;

namespace HomicideDetective
{
    public static class RogueLikeEntityExtensions
    {
        public static Substantive? Info(this RogueLikeEntity self) =>
            self.AllComponents.GetFirstOrDefault<Substantive>();
        
        public static Thoughts? ThoughtProcess(this RogueLikeEntity self) =>
            self.AllComponents.GetFirstOrDefault<Thoughts>();
    }
}