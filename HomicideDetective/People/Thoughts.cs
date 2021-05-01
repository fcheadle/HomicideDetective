using System.Collections.Generic;
using GoRogue.GameFramework;
using GoRogue.GameFramework.Components;
using HomicideDetective.Mysteries;

namespace HomicideDetective.People
{
    //currently how speech and senses communicate with the player.
    public class Thoughts : IGameObjectComponent, IDetailed
    {
        public IGameObject? Parent { get; set; }
        //public string Name { get; }
        public string Description { get; }
        private readonly List<string> _thoughts;
        public List<string> Details => _thoughts;
        
        public Thoughts() 
        {
            //Name = "Thoughts";
            Description = "The Thought Process of a creature.";
            _thoughts = new List<string>();
        }
        
        public void Think(string[] thoughts)
        {
            //_thoughts.Clear();
            foreach (string thought in thoughts) 
                Think(thought);
        }
        public void Think(string thought)
        {
            //todo - more complexity
            if(!_thoughts.Contains(thought))
                _thoughts.Add(thought);
        }
    }
}