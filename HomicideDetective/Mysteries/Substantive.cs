using System.Collections.Generic;
using System.Linq;

namespace HomicideDetective.Mysteries
{
    public class Substantive : IHaveDetails
    {
        public enum Types { Person,Place,Thing }
        public Types? Type { get; set; } 
        public string Name { get; set; } 
        public string Description => GenerateDetailedDescription();
        public int Mass { get; set; }//in grams
        public int Volume { get; set; }//in cm^3
        public string SizeDescription { get; set; }
        public string WeightDescription { get; set; }
        //arrays play nicer with serialization
        public string[] Details { get; set; }

        public Substantive(Types type, string name, int mass, int volume, string sizeDescription, string weightDescription)
        {
            Type = type;
            Name = name;
            Mass = mass;
            Volume = volume;
            SizeDescription = sizeDescription;
            WeightDescription = weightDescription;
            Details = new[] { "" };
        }

        public string[] GetDetails()
        {
            var answer = new List<string>();
            answer.Add(Name); 
            answer.Add($"Weight(grams): {Mass}"); 
            answer.Add($"$Volume(cubic cm): {Volume}"); 
            answer.Add(Description);
            answer.AddRange(Details);
            return answer.ToArray();
        }

        public void AddDetail(string detail)
        {
            var details = Details.ToList();
            details.Add(detail);
            Details = details.ToArray();
        }
        private string GenerateDetailedDescription()
        {
            string description = "";
            description += $"This is a(n) {Name}. It {SizeDescription}, and it {WeightDescription}.";
            return description;
        }
    }
}