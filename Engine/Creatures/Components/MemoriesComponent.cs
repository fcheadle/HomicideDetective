using Engine.Components;
using SadConsole;

namespace Engine.Creatures.Components
{
    public class MemoriesComponent : Component
    {
        //List<Memory> Memories;
        public MemoriesComponent(BasicEntity parent) : base(isUpdate: true, isKeyboard: false, isDraw: true, isMouse: false)
        {
            Parent = parent;
            Name = "Memories";
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
