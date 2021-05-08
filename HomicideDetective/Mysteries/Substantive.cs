using System;
using System.Collections.Generic;
using GoRogue.GameFramework;
using GoRogue.GameFramework.Components;

namespace HomicideDetective.Mysteries
{
    public class Substantive : IGameObjectComponent
    {
        public int Mass { get; set; } = 0;//in grams
        public int Volume { get; set; } = 0;//in ml
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
        public Random? Random { get; set; }

        private List<string> _details => new List<string>()
        {
            Name,
            $"Mass: {Mass}g",
            $"Volume: {Volume}ml",
            Description!,
            GenerateDetailedDescription()
        };

        public string[] Details => _details.ToArray();

        public Substantive(Types type)
        {
            Type = type;
        }

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
        }
        
        public void AddDetail(string detail) => _details.Add(detail);
        public string GenerateDetailedDescription()
        {
            var description = "This is ";
            if (Type == Types.Person || Type == Types.Place)
                description += $"{Name}. ";
            else
                description += $"a {Name}. ";
           
            description += $"It {SizeDescription}, and it {WeightDescription}.";

            return description;
        }
    }
}