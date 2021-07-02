using System.Collections.Generic;
using HomicideDetective.UserInterface;

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
    public class Substantive : IPrintable
    {
        public int Mass { get; set; } //in grams
        public int Volume { get; set; } //in ml
        public enum Types {Person, Place, Thing}
        public Types? Type { get; set; } 
        public string? Name { get; set; } 
        public string? Article { get; set; }
        public string? Pronoun { get; set; }
        public string? PronounPossessive { get; set; }
        public string? Gender { get; set; }
        public string? Description { get; set; }

        private List<string> _details;
        public List<string> Details => _details;

        public Substantive(Types type, string name, string? gender = null, string? article = null, string? pronoun = null, 
            string? pronounPossessive = null, string? description = null, int mass = 0, int volume = 0)
        {
            Type = type;
            Name = name;
            Article = article;
            Pronoun = pronoun;
            PronounPossessive = pronounPossessive;
            Gender = gender;
            Mass = mass;
            Volume = volume;
            Description = description;
            _details = new List<string>();
        }
        
        public void AddDetail(string detail) => _details.Add(detail);
        public string GetPrintableString()
        {
            var description = $"This is {Article} {Name}. ";
            description += $"{Description} ";
            if(Type != Types.Place)
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