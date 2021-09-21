using System.Collections.Generic;
using GoRogue.MapGeneration;
using HomicideDetective.Things;

namespace HomicideDetective.Places
{
    public class Place : ISubstantive
    {
        public SubstantiveTypes Type => SubstantiveTypes.Place;
        public string Name { get; }
        public string Description { get; }
        public string Noun { get; }
        public string Pronoun { get; }
        public string PronounPossessive { get; }
        public List<string> Details { get; }
        public Region Area { get; }
        public MarkingCollection Markings { get; }
        public Place(Region area, string name, string description, string noun)
        {
            Area = area;
            Name = name;
            Description = description;
            Noun = noun;
            Pronoun = "it";
            PronounPossessive = "it's";
            Details = new List<string>();
            Markings = new MarkingCollection();
        }

        public string GetPrintableString() => $"I am in {Name}. {Description}";
    }
}