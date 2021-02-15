using System;
using System.Collections.Generic;
using GoRogue.GameFramework;
using GoRogue.GameFramework.Components;
using HomicideDetective.Mysteries;
using HomicideDetective.Things;
using SadConsole;
using SadRogue.Primitives;
using TheSadRogue.Integration.Components;

namespace HomicideDetective.People
{

    public class Thoughts : IGameObjectComponent, IDetailed
    {
        public IGameObject? Parent { get; set; }
        public string Name { get; }
        public string Description { get; }
        private readonly List<string> _thoughts;
        public string[] Details => _thoughts.ToArray();
        
        public Thoughts() 
        {
            Name = "Thoughts";
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