using System;
using System.Collections.Generic;
using HomicideDetective.Mysteries;
using SadConsole;
using TheSadRogue.Integration.Components;

namespace HomicideDetective.People.Components
{

    public class Thoughts : RogueLikeComponentBase, IDetailed
    {
        public string Name { get; }
        public string Description { get; }
        private readonly List<string> _thoughts;
        public string[] GetDetails() => _thoughts.ToArray();

        public Thoughts() : base(true, false, false, false)
        {
            Name = "Thoughts";
            Description = "The Thought Process of a creature.";
            _thoughts = new List<string>();
        }

        public override void Update(IScreenObject host, TimeSpan delta)
        {
            Think();
            base.Update(host, delta);
        }

        public void Think()
        {
            //todo - flesh out
            //Think(Parent.Position.ToString());
            //Think("Currently thinking about");
        }
        
        public void Think(string[] thoughts)
        {
            _thoughts.Clear();
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