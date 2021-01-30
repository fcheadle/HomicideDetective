using System.Collections.Generic;
using HomicideDetective.Mysteries;
using SadRogue.Primitives;
using TheSadRogue.Integration;

namespace HomicideDetective.Things
{
    public class Thing : RogueLikeEntity, ISubstantive
    {
        public ISubstantive.Types? Type => ISubstantive.Types.Thing;
        public string Description { get; }
        public int Mass { get; }
        public int Volume { get; }
        public string SizeDescription { get; }
        public string WeightDescription { get; }

        private List<string> _details;
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
        public string GetDetailedDescription()
            => $"This is {Name}. They {SizeDescription}, and they {WeightDescription}.";

        public void AddDetail(string detail)=> _details.Add(detail);

        public Thing(Point position, string name, string description, int mass, int volume, string sizeDescript, string weightDescript) 
            : base(position, Color.LightGray, Color.Transparent, 't', true, true, 2)
        {
            Name = name;
            Description = description;
            Mass = mass;
            Volume = volume;
            SizeDescription = sizeDescript;
            WeightDescription = weightDescript;
            _details = new List<string>();
            // AllComponents.Add(substantive);
        }
    }
}