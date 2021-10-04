using System.Collections.Generic;
using System.Text;
using HomicideDetective.Things;
using HomicideDetective.Words;

namespace HomicideDetective
{
    /// <summary>
    /// A substantive is a person, place, or thing.
    /// </summary>
    /// <remarks>
    /// All other components may count on a RogueLikeEntity having a substantive component.
    /// Substantive should not have any knowledge of any other components.
    /// It exists purely to be a collection of strings to describe something.
    /// </remarks>
    public class Substantive : ISubstantive
    {
        public SubstantiveTypes Type { get; set; } 
        public PhysicalProperties Properties { get; set; }
        public string Name { get; set; } 
        public string? Article { get; set; }
        public Noun Nouns { get; set; }
        public Pronoun Pronouns { get; set; }
        public Verb? UsageVerb { get; }
        public string Description { get; set; }
        public MarkingCollection Markings { get; set; }

        private List<string> _details;
        public List<string> Details => _details;

        public Substantive(SubstantiveTypes type, string name, string description, Noun nouns, Pronoun pronouns, PhysicalProperties properties, Verb? usageVerbs = null, string? article = null)
        {
            Type = type;
            Name = name;
            Article = article;
            Pronouns = pronouns;
            Properties = properties;
            Nouns = nouns;
            Description = description;
            UsageVerb = usageVerbs;
            _details = new List<string>();
            Markings = new MarkingCollection();
        }
        
        public void AddDetail(string detail) => _details.Add(detail);
        public string GetPrintableString()
        {
            var description = new StringBuilder($"This is {Article} {Properties.GetPrintableString()} {Name}. ");
            description.Append($"{Description} ");
            
            foreach (var detail in _details)
            {
                description.Append($"{detail} ");
            }

            return description.ToString();
        }
    }
}