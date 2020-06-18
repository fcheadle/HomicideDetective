using Engine.Items.Markings;
using GoRogue;
using SadConsole;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Components
{
    public class PhysicalComponent : Component
    {
        private List<Marking> _markingsOn = new List<Marking>(); //the markings on this thing
        private List<Marking> _limitedMarkingsLeft = new List<Marking>(); //the (limited) markings left by interaction - ie mud/blood/things that can wash off
        private List<Marking> _unlimitedMarkingsLeft = new List<Marking>(); //the (unlimited) markings left by this thing - ie cuts/hair follicles/footprints/fingerprints

        public int Mass { get; set; }
        public int Volume { get; set; }
        public List<Marking> MarkingsOn => _markingsOn.ToList();
        public List<Marking> MarkingsLeftBy => _limitedMarkingsLeft.Concat(_unlimitedMarkingsLeft).ToList();
        public PhysicalComponent(BasicEntity parent, List<Marking> markingsLeftByUnlimited = null, Dictionary<Marking, int> markingsLeftByLimited = null):base(false, false, false, false)
        {
            var limitedMarkings = markingsLeftByLimited ?? new Dictionary<Marking, int>();
            foreach (KeyValuePair<Marking, int> marking in limitedMarkings)
                for (int i = 0; i < marking.Value; i++)
                    _limitedMarkingsLeft.Add(marking.Key);
            _unlimitedMarkingsLeft = markingsLeftByUnlimited ?? _unlimitedMarkingsLeft;
            Parent = parent;
        }
        public override string[] GetDetails()
        {
            string[] answer = {
                "Mass: " + Mass + "g",
                "Volume: " + Volume + "ml",
            };
            return answer;
        }

        public void OnInteract(BasicEntity interactor)
        {

        }

        public Marking Interact()
        {
            Marking leaving = MarkingsLeftBy.RandomItem();
            if (_limitedMarkingsLeft.Contains(leaving))
                _limitedMarkingsLeft.Remove(leaving);
            return leaving;
        }
        public void AddLimitedMarkings(Marking marking, int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                _limitedMarkingsLeft.Add(marking);
            }
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
