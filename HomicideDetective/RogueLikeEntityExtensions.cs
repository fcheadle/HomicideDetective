using HomicideDetective.Mysteries;
using HomicideDetective.People;
using SadRogue.Integration;

namespace HomicideDetective
{
    public static class RogueLikeEntityExtensions
    {
        public static Substantive Info(this RogueLikeEntity self) => self.AllComponents.GetFirst<Substantive>();
        public static Speech Speech(this RogueLikeEntity self) => self.AllComponents.GetFirst<Speech>();
        public static Thoughts Thoughts(this RogueLikeEntity self) => self.AllComponents.GetFirst<Thoughts>();
    }
}