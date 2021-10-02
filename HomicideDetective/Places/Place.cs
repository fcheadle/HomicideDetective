using System.Collections.Generic;
using System.Linq;
using GoRogue.MapGeneration;
using HomicideDetective.Things;
using HomicideDetective.Words;
using SadRogue.Primitives;

namespace HomicideDetective.Places
{
    public class Place : Region, ISubstantive
    {
        // ISubstantive
        public SubstantiveTypes Type => SubstantiveTypes.Place;

        public string Name { get; set; }

        public string Description { get; set; }
        public PhysicalProperties Properties { get; set; }
        public Noun Nouns { get; set; }
        public Pronoun Pronouns { get; set; }
        public Verb? UsageVerb => null;
        public List<string> Details { get; set; }
        public MarkingCollection Markings { get; set; }
        
        // Area / Region stuff
        public IReadOnlyList<Place> SubAreas => _subAreas.AsReadOnly();
        private List<Place> _subAreas;

        public IReadOnlyList<Point> Connections => _connections.AsReadOnly();
        private List<Point> _connections;

        public Place(PolygonArea area, string name, string description, Noun nouns, Pronoun pronouns, PhysicalProperties properties)
            : base(area) 
        { 
            Name = name;
            Description = description;
            Nouns = nouns;
            Pronouns = pronouns;
            Properties = properties;
            _subAreas = new ();
            _connections = new ();
            Markings = new MarkingCollection(); 
        }

        public Place(PolygonArea area, Substantive info)  
            : this(area, info.Name, info.Description, info.Nouns, info.Pronouns, info.Properties)
        { }

        public void AddSubRegion(Place region)
        {
            if(!_subAreas.Contains(region))
                _subAreas.Add(region);
        }

        public void RemoveSubRegion(Place region)
        {
            if(_subAreas.Contains(region)) 
                _subAreas.Remove(region);
        }

        public void AddConnection(Point pos)
        {
            if(!_connections.Contains(pos))
                _connections.Add(pos);
        }
        public IEnumerable<Place> GetPlacesContaining(Point pos)
        {
            foreach (var place in _subAreas.Where(a => a.Area.Contains(pos))) 
                yield return place;
        }

        public string GetPrintableString() => $"I am in {Name}. {Description}";
    }
}