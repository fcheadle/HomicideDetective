using System.Collections.Generic;
using System.Linq;
using GoRogue;
using GoRogue.GameFramework;
using GoRogue.GameFramework.Components;

namespace HomicideDetective.Things
{
    /// <summary>
    /// A collection markings, used for both receiving and leaving markings.
    /// </summary>
    public class MarkingCollection : IGameObjectComponent
    {
        public IGameObject? Parent { get; set; }
        
        //the markings on the parent
        public List<Marking> MarkingsOn => _markingsOn.ToList();
        private List<Marking> _markingsOn; 
        
        //markings left by the parent
        public List<Marking> MarkingsLeftBy => _limitedMarkingsLeft.Concat(_unlimitedMarkingsLeft).ToList();
        private List<Marking> _limitedMarkingsLeft;
        private List<Marking> _unlimitedMarkingsLeft;
        
        public MarkingCollection()
        {
            _markingsOn = new List<Marking>();
            _limitedMarkingsLeft = new List<Marking>();
            _unlimitedMarkingsLeft = new List<Marking>();
        }

        public Marking LeaveMark()
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
        
        public void AddUnlimitedMarkings(Marking marking)
            => _unlimitedMarkingsLeft.Add(marking);
        

        public void Mark(Marking marking, int amount = 1)
        {
            _markingsOn.Add(marking);
            for (int i = 0; i < amount; i++)
            {
                _limitedMarkingsLeft.Add(marking);
            }
        }
    }
}