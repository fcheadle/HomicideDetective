using System.Collections.Generic;
using HomicideDetective.Mysteries;
using SadRogue.Primitives;
using TheSadRogue.Integration;

namespace HomicideDetective.Things
{
    public class Thing : RogueLikeEntity, IDetailed
    {
        public string Description { get; }
        public Substantive Substantive { get; }
        public string[] GetDetails() => Substantive.Details;

        public Thing(Point position, string name, string description, int mass, int volume, string sizeDescript, string weightDescript) 
            : base(position, Color.LightGray, Color.Transparent, name[0], true, true, 2)
        {
            Name = name;
            Description = description;
            Substantive = new Substantive(Substantive.Types.Person, name, description, mass, volume, sizeDescript, weightDescript);
        }
    }
}