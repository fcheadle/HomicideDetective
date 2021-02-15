using System;
using System.Collections.Generic;
using GoRogue.GameFramework;
using GoRogue.GameFramework.Components;

namespace HomicideDetective.Mysteries
{
    public class Substantive : IGameObjectComponent
    {
        public int Seed { get; set; }
        public int Mass { get; set; } = 0;//in grams
        public int Volume { get; set; } = 0;//in ml
        public enum Types {Person, Place, Thing}
        public IGameObject? Parent { get; set; }
        public ISubstantive? Subject { get; set; }
        public Types? Type { get; set; } 
        public string? Name { get; set; } 
        public string? Article { get; set; }
        public string? Pronoun { get; set; }
        public string? PronounPossessive { get; set; }
        public string? Gender { get; set; }
        public string? Description { get; set; }
        public string? SizeDescription { get; set; }
        public string? WeightDescription { get; set; }
        public Random? Random { get; set; }

        private List<string> _details = new List<string>();

        public string[] Details => _details.ToArray();

        public Substantive()
        {
            var temp = new Random();
            Seed = temp.Next();
            Random = new Random(Seed);
        }

        public Substantive(Types type, int seed)
        {
            Type = type;
            Seed = seed;
            Random = new Random(Seed);
        }

        public Substantive(Types type, string name, int seed, string? gender = null, string? article = null, string? pronoun = null, 
            string? pronounPossessive = null, string? description = null, int mass = 0, int volume = 0,
            string? sizeDescription = null, string? weightDescription = null)
        {
            Type = type;
            Name = name;
            Seed = seed;
            Article = article;
            Pronoun = pronoun;
            PronounPossessive = pronounPossessive;
            Gender = gender;
            Mass = mass;
            Volume = volume;
            Description = description;
            SizeDescription = sizeDescription;
            WeightDescription = weightDescription;
            _details = new();
            _details.Add(Name); 
            _details.Add($"Mass(g): {Mass}"); 
            _details.Add($"Volume(ml): {Volume}"); 
            _details.Add(Description!);
            _details.Add(GenerateDetailedDescription());
        }

        public Substantive Generate()
        {
            if (Type == null) 
                throw new NullReferenceException("Tried to Generate a Substantive that doesn't have a Type.");

            if (Type == Types.Person)
            {
                throw new NotImplementedException();
            }
            else if (Type == Types.Place)
            {
                throw new NotImplementedException();
            }
            else if (Type == Types.Thing)
            {
                throw new NotImplementedException();
            }

            return this;
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

        public static Substantive Person(int seed) => new Substantive(Types.Person, seed).Generate();
        public static Substantive Place(int seed) => new Substantive(Types.Place, seed).Generate();
        public static Substantive Thing(int seed) => new Substantive(Types.Thing, seed).Generate();
    }
}