using System.Collections.Generic;
using GoRogue.MapGeneration;
using HomicideDetective.Things;
using HomicideDetective.Words;

namespace HomicideDetective.Places
{
    public class Place : ISubstantive
    {
        public SubstantiveTypes Type => SubstantiveTypes.Place;
        public string Name { get; }
        public string Description { get; }
        public PhysicalProperties Properties { get; }
        public Noun Nouns { get; }
        public Pronoun Pronouns { get; }
        public Verb? UsageVerb => null;
        public List<string> Details { get; }
        public Region Area { get; }
        public MarkingCollection Markings { get; }
        public Place(Region area, string name, string description, PhysicalProperties properties, Noun nouns, Pronoun pronouns)
        {
            Area = area;
            Name = name;
            Description = description;
            Properties = properties;
            Nouns = nouns;
            Pronouns = pronouns;
            Details = new List<string>();
            Markings = new MarkingCollection();
        }

        public Place(Region area, Substantive info) : this(area, info.Name, info.Description, info.Properties,
            info.Nouns, info.Pronouns) { }

        public string GetPrintableString() => $"I am in {Name}. {Description}";
    }
}