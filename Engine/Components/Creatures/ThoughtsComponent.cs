using SadConsole;
using System;

namespace Engine.Components.Creature
{
    public class ThoughtsComponent : Component
    {
        public ThoughtsComponent(BasicEntity parent) : base(true, false, false, false)
        {
            Parent = parent;
        }

        public override string[] GetDetails()
        {
            string[] answer = {
                "This is a thought component.",
                "The entity with this component has a thought process."
            };
            return answer;
        }

        public override void ProcessTimeUnit()
        {
            ThinkAboutSenses();
            ThinkAboutDesires();
            DecideWhatToDo();
        }

        private void ThinkAboutSenses()
        {
            //todo...
        }

        private void ThinkAboutDesires()
        {
            //todo...
        }

        private void DecideWhatToDo()
        {
            //todo...
        }
    }
}
