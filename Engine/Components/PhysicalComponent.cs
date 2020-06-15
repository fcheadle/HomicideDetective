using SadConsole;

namespace Engine.Components
{
    public class PhysicalComponent : Component
    {
        //public string Description { get; private set; }
        public int Mass { get; set; }
        public int Volume { get; set; }
        public PhysicalComponent(BasicEntity parent):base(false, false, false, false)
        {
            Parent = parent;
        }
        public override string[] GetDetails()
        {
            string[] answer = {
                "This entity's mass is " + Mass,
                "This entity's volume is " + Volume
            };
            return answer;
        }

        public void SetPhysicalDescription(string description)
        {
            Description = description;
        }

        public override string ToString()
        {
            return Description;
        }

        public override void ProcessTimeUnit()
        {
            //eventually, we'll want some physics stuff in here (velocity / direction / hardness / so on)
        }
    }
}
