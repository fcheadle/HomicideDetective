using Engine.Items.Markings;
using SadConsole;
using System.Collections.Generic;

namespace Engine.Components
{
    public class PhysicalComponent : Component
    {
        private List<Marking> _markings = new List<Marking>();

        public int Mass { get; set; }
        public int Volume { get; set; }
        public List<Marking> Markings => _markings;
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

        //public Action<Marking> OnInteract(BasicEntity interactor)
        //{

        //}
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
