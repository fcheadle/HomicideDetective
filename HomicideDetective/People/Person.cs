using System.Collections.Generic;
using HomicideDetective.Mysteries;
using HomicideDetective.People.Components;
using HomicideDetective.Places;
using HomicideDetective.Things;
using SadRogue.Primitives;
using TheSadRogue.Integration;

namespace HomicideDetective.People
{
    public class Person : RogueLikeEntity, ISubstantive
    {
        public ISubstantive.Types? Type { get; set; }
        public string Description { get; set;}
        public int Mass { get; set; }
        public int Volume { get; set; }
        public string SizeDescription { get; set; }
        public string WeightDescription { get; set; }
        public string FirstName { get; }
        public string LastName { get; }
        public string Name => $"{FirstName} {LastName}";
        public ThoughtComponent Thoughts => AllComponents.GetFirst<ThoughtComponent>();
        public SpeechComponent Speech => AllComponents.GetFirst<SpeechComponent>();
        public HealthComponent Health => AllComponents.GetFirst<HealthComponent>();
        
        private readonly List<string> _details;
        public string[] Details
        {
            get
            {
                var answer = new List<string>();
                answer.Add(Name); 
                answer.Add($"Weight(grams): {Mass}"); 
                answer.Add($"$Volume(cubic cm): {Volume}"); 
                answer.Add(Description);
                answer.AddRange(Details);
                answer.AddRange(_details);
                answer.Add(GetDetailedDescription());
                return answer.ToArray();
            }
        }

        public Person(Point position, string firstname, string lastname, int size, int weight, string sizeDescription, 
            string weightDescription) : base(position, 1, false)
        {
            FirstName = firstname;
            LastName = lastname;
            Mass = weight;
            Volume = size;
            SizeDescription = sizeDescription;
            WeightDescription = weightDescription;
            
            var health = new HealthComponent();
            AllComponents.Add(health);
            
            var thoughts = new ThoughtComponent();
            AllComponents.Add(thoughts);

            var speech = new SpeechComponent();
            AllComponents.Add(speech);

            _details = new List<string>();
        }

        public string GetDetailedDescription() 
            => $"This is {Name}. They {SizeDescription}, and they {WeightDescription}.";

        public void AddDetail(string detail) => _details.Add(detail);

        public void Murder(Person murderer, Thing murderWeapon, Place sceneOfTheCrime)
        {
            AddDetail($" Murdered by {murderer.Name} with a {murderWeapon.Name}, at {sceneOfTheCrime.Name}.");
            Health.Murder();
        }
    }
}