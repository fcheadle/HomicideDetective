using GoRogue.GameFramework;
using GoRogue.GameFramework.Components;

namespace Engine.Components
{
    public class PhysicalComponent : ComponentBase
    {
        public string Description { get; private set; }
        public int Mass { get; set; }
        public int Volume { get; set; }
        public IGameObject Parent { get; set; }
        public PhysicalComponent():base(false, false, false, false)
        {

        }
        public override string[] GetDetails()
        {
            string[] answer = {
                "This entity's mass is " + Mass,
                "This entity's volume is " + Volume
            };
            return answer;
        }

        public override void ProcessGameFrame()
        {
            //do nothing
        }

        public void SetPhysicalDescription(string description)
        {
            Description = description;
        }

        public override string ToString()
        {
            return Description;
        }
    }
}
