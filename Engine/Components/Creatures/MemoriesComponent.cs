using Engine.States;
using GoRogue;
using SadConsole;
using System.Collections.Generic;

namespace Engine.Components.Creature
{
    public class MemoriesComponent : Component
    {
        //List<Memory> Memories;
        public MemoriesComponent(BasicEntity parent) : base(isUpdate: true, isKeyboard: false, isDraw: true, isMouse: false)
        {
            Parent = parent;
            Name = "Memories Component";
        }

        public override string[] GetDetails()
        {
            string[] answer =
            {
                "this is a memory component.",
                "The entity with this component has memories and can remember.",
        };
            return answer;//
        }

        public override void ProcessTimeUnit()
        {
            //todo...
        }
    }
}
