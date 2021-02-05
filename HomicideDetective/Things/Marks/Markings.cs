using System.Collections.Generic;
using System.Linq;
using GoRogue;
using GoRogue.GameFramework;
using GoRogue.GameFramework.Components;

namespace HomicideDetective.Things.Marks
{
    public class Markings : IGameObjectComponent
    {
        public IGameObject? Parent { get; set; }
        
        private List<Mark> _markingsOn; //the markings on this thing
        private List<Mark> _limitedMarkingsLeft; //the (limited) markings left by interaction - ie mud/blood/things that can wash off
        private List<Mark> _unlimitedMarkingsLeft; //the (unlimited) markings left by this thing - ie cuts/hair follicles/footprints/fingerprints
        
        public List<Mark> MarkingsOn => _markingsOn.ToList();
        public List<Mark> MarkingsLeftBy => _limitedMarkingsLeft.Concat(_unlimitedMarkingsLeft).ToList();
        
        public Markings()
        {
            _markingsOn = new List<Mark>();
            _limitedMarkingsLeft = new List<Mark>();
            _unlimitedMarkingsLeft = new List<Mark>();
        }

        public Mark LeaveMark()
        {
            Mark leaving = MarkingsLeftBy.RandomItem();
            if (_limitedMarkingsLeft.Contains(leaving))
                _limitedMarkingsLeft.Remove(leaving);
            return leaving;
        }
        public void AddLimitedMarkings(Mark marking, int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                _limitedMarkingsLeft.Add(marking);
            }
        }
        public void AddUnlimitedMarkings(Mark marking)
            => _unlimitedMarkingsLeft.Add(marking);
        

        public void Mark(Mark marking, int amount = 1)
        {
            _markingsOn.Add(marking);
            for (int i = 0; i < amount; i++)
            {
                _limitedMarkingsLeft.Add(marking);
            }
        }
    }
}