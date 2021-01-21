using System;
using System.Collections.Generic;
using SadConsole;
using TheSadRogue.Integration.Components;

namespace HomicideDetective.New.People
{

    public class ThoughtComponent : RogueLikeComponentBase, IHaveDetails
    {
        public string Name { get; }
        public string Description { get; }
        private List<string> _thoughts = new List<string>();
        public string[] GetDetails() => _thoughts.ToArray();

        public ThoughtComponent() : base(true, false, false, false)
        {
            Name = "Thoughts";
            Description = "The Thought Process of a creature.";
        }

        public override void Update(IScreenObject host, TimeSpan delta)
        {
            Think();
            base.Update(host, delta);
        }

        public void Think()
        {
            //todo - flesh out
            Think(Parent.Position.ToString());
            Think("Currently thinking about...");
        }
        
        public void Think(string[] thoughts)
        {
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