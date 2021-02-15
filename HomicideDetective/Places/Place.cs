using GoRogue.MapGeneration;
using HomicideDetective.Mysteries;
using SadRogue.Primitives;

namespace HomicideDetective.Places
{
    public class Place : Region, ISubstantive
    {
        public Substantive? Substantive { get; }
        
        public Place(Substantive details, Point northWest, Point northEast, Point southEast, Point southWest) 
            : base(details.Name ?? string.Empty, northWest, northEast, southEast, southWest)
        {
            Substantive = details;
            Name = details.Name!;
        }

        public Place(Substantive details, Point northWest, Point southEast) : this(details, northWest,
            (southEast.X, northWest.Y), southEast, (northWest.X, southEast.Y))
        {
        }
    }
}