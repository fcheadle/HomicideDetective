using System.Collections.Generic;
using GoRogue.GameFramework;
using GoRogue.GameFramework.Components;
using HomicideDetective.Places;

namespace HomicideDetective.Mysteries
{
    public class Substantive : IGameObjectComponent
    {
        public enum Types {Person, Place, Thing}
        public IGameObject? Parent { get; set; }
        public Place? Subject { get; set; }
        public Types Type { get; set; } 
        public string Name { get; set; } 
        public string Description { get; set; }
        public int Mass { get; set; }//in grams
        public int Volume { get; set; }//in ml
        public string SizeDescription { get; set; }
        public string WeightDescription { get; set; }
        
        private List<string> _details;
        //arrays play nicer with serialization
        public string[] Details
        {
            get
            {
                var answer = new List<string>();
                answer.Add(Name); 
                answer.Add($"Mass(g): {Mass}"); 
                answer.Add($"Volume(ml): {Volume}"); 
                answer.Add(Description);
                answer.AddRange(_details);
                answer.Add(GenerateDetailedDescription());
                return answer.ToArray();
            }
        }

        public Substantive(){}
        
        public Substantive(Types type, string name, string description, int mass, int volume, string sizeDescription, string weightDescription)
        {
            Type = type;
            Name = name;
            Mass = mass;
            Volume = volume;
            Description = description;
            SizeDescription = sizeDescription;
            WeightDescription = weightDescription;
            _details = new List<string>();
            
        }

        public void AddDetail(string detail) => _details.Add(detail);
        
        public string GenerateDetailedDescription() => $"This is a(n) {Name}. It {SizeDescription}, and it {WeightDescription}.";
    }
}