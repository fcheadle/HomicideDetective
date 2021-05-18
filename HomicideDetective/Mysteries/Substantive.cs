using System.Collections.Generic;
using System.Drawing;
using GoRogue.GameFramework;
using GoRogue.GameFramework.Components;

namespace HomicideDetective.Mysteries
{
    /// <summary>
    /// A substantive is a person, place, or thing.
    /// </summary>
    public class Substantive : IGameObjectComponent
    {
        public int Mass { get; set; } //in grams
        public int Volume { get; set; } //in ml
        public enum Types {Person, Place, Thing}
        public Types? Type { get; set; } 
        public IGameObject? Parent { get; set; }
        public string? Name { get; set; } 
        public string? Article { get; set; }
        public string? Pronoun { get; set; }
        public string? PronounPossessive { get; set; }
        public string? Gender { get; set; }
        public string? Description { get; set; }
        public string? SizeDescription { get; set; }
        public string? WeightDescription { get; set; }

        private List<string> _details;
        public List<string> Details => _details;

        public Substantive(Types type, string name, string? gender = null, string? article = null, string? pronoun = null, 
            string? pronounPossessive = null, string? description = null, int mass = 0, int volume = 0,
            string? sizeDescription = null, string? weightDescription = null)
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
            SizeDescription = sizeDescription;
            WeightDescription = weightDescription;
            _details = new List<string>();
        }
        
        public void AddDetail(string detail) => _details.Add(detail);
        public string GenerateDetailedDescription()
        {
            var description = $"This is {Article} {Name}. ";
            description += $"{Description}. ";
            description += $"{Pronoun} weighs {Mass}g, {WeightDescription}. ";
            description += $"{Pronoun} is {Volume}ml in size, {SizeDescription}. ";

            foreach (var detail in _details)
            {
                description += $"{detail}. ";
            }

            return description;
        }
    }
}