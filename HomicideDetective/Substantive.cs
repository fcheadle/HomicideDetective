using System.Collections.Generic;
using HomicideDetective.Things;
#pragma warning disable 8618

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
        public int Mass { get; set; } //in grams
        public int Volume { get; set; } //in ml
        public ISubstantive.Types Type { get; set; } 
        public string Name { get; set; } 
        public string Article { get; set; }
        public string Noun { get; set; }
        public string Pronoun { get; set; }
        public string PronounPossessive { get; set; }
        public string? Gender { get; set; }
        public string Description { get; set; }
        
        public MarkingCollection Markings { get; set; }

        private List<string> _details;
        public List<string> Details => _details;

        public Substantive(ISubstantive.Types type, string name, string? gender = null, string? article = null, string? pronoun = null, 
            string? pronounPossessive = null, string? description = null, int mass = 0, int volume = 0, string? noun = null)
        {
            Type = type;
            Name = name;
            Article = article;
            Pronoun = pronoun;
            PronounPossessive = pronounPossessive;
            Gender = gender;
            Mass = mass;
            Volume = volume;
            Noun = noun;
            Description = description;
            _details = new List<string>();
            Markings = new MarkingCollection();
        }
        
        public void AddDetail(string detail) => _details.Add(detail);
        public string GetPrintableString()
        {
            var description = $"This is {Article} {Name}. ";
            description += $"{Description} ";
            if(Type != ISubstantive.Types.Place)
            {
                description += $"{Pronoun} weighs {Mass}g. ";
                description += $"{Pronoun} is {Volume}ml in size. ";
            }
            foreach (var detail in _details)
            {
                description += $"{detail} ";
            }

            return description;
        }
    }
}