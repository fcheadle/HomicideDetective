using HomicideDetective.Mysteries;
using SadRogue.Primitives;
using TheSadRogue.Integration;

namespace HomicideDetective.Things
{
    public class Thing : RogueLikeEntity, IHaveDetails
    {
        public Substantive AsSubstantive => AllComponents.GetFirst<Substantive>();

        public string Description  => AsSubstantive.Description;

        public Thing(Point position, Substantive substantive) : base(position, Color.LightGray, Color.Transparent, substantive.Name[0], true, true, 2)
        {
            Name = substantive.Name;
            AllComponents.Add(substantive);
        }
        public string[] GetDetails()
        {
            return new[] {Name, Description};
        }
    }
}