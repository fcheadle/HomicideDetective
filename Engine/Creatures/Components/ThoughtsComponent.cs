using Engine.Components;
using SadConsole;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Creatures.Components
{
    public class ThoughtsComponent : Component
    {
        private List<string> _thoughts = new List<string>();

        public ThoughtsComponent(BasicEntity parent) : base(true, false, false, false)
        {
            Parent = parent;
            Name = "Thoughts";
        }

        public override string[] GetDetails() => _thoughts.ToArray();
        public override void ProcessTimeUnit()
        {
            ThinkAboutSenses();
            ThinkAboutDesires();
            DecideWhatToDo();
        }
        public void Think(string[] thoughts)
        {
            //foreach (string thought in thoughts) 
            //    Think(thought);

            _thoughts = thoughts.ToList();
        }
        public void Think(string thought)
        {
            //needs to be more complex
            if(!_thoughts.Contains(thought))
                _thoughts.Add(thought);
            else
            {
                
            }
        }

        private void ThinkAboutSenses()
        {
            //todo...
        }

        private void ThinkAboutDesires()
        {
            //todo...
        }

        public void DecideWhatToDo()
        {
            //todo...
        }
    }
}
