using HomicideDetective.Mysteries;
using SadRogue.Primitives;
using TheSadRogue.Integration;

namespace HomicideDetective.Things
{
    public class Thing : RogueLikeEntity, ISubstantive
    {
        public string? Description => Substantive.Description!;
        public Substantive Substantive => AllComponents.GetFirst<Substantive>();

        public Thing(Point position, Substantive substantive) 
            : base(position, Color.LightGray, Color.Transparent, substantive.Name![0], true, true, 2)
        {
            AllComponents.Add(substantive);
            Name = substantive.Name;
        }
    }
}