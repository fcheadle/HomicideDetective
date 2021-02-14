using System.Collections.Generic;
using HomicideDetective.Mysteries;
using SadRogue.Primitives;
using TheSadRogue.Integration;

namespace HomicideDetective.Things
{
    public class Thing : RogueLikeEntity, IDetailed
    {
        public string Description => Substantive.Description!;
        public Substantive Substantive { get; }
        public string[] GetDetails() => Substantive.Details;
        public string[] AllDetails() => Substantive.AllDetails;

        public Thing(Point position, Substantive substantive) 
            : base(position, Color.LightGray, Color.Transparent, substantive.Name![0], true, true, 2)
        {
            Substantive = substantive;
            Name = substantive.Name;
        }
    }
}