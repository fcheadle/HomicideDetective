using Engine.Components;
using SadConsole;
using System.Collections.Generic;

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
            if (thoughts.Length == 1)
            {
                Think(thoughts[0]);
                return;
            }

            string thought = "";
            for (int i = 0; i < thoughts.Length - 1; i++)
                thought += thoughts[i] + ", ";

            thought += "and " + thoughts[thoughts.Length - 1];
            _thoughts.Add(thought);
        }
        public void Think(string thought) => _thoughts.Add(thought);

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
