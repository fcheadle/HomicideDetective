using System.Collections.Generic;
using System.Linq;
using GoRogue;
using GoRogue.GameFramework;
using HomicideDetective.Places;
using SadRogue.Integration;

namespace HomicideDetective.Things
{
    /// <summary>
    /// A collection markings, used for both receiving and leaving markings.
    /// </summary>
    public class MarkingCollection
    {
        
        //the markings on the parent
        public List<Marking> MarkingsOn => _markingsOn;
        private readonly List<Marking> _markingsOn; 
        
        //markings left by the parent
        public List<Marking> MarkingsLeftBy => _limitedMarkingsLeft.Concat(_unlimitedMarkingsLeft).ToList();
        private readonly List<Marking> _limitedMarkingsLeft;
        private readonly List<Marking> _unlimitedMarkingsLeft;
        
        public MarkingCollection()
        {
            _markingsOn = new List<Marking>();
            _limitedMarkingsLeft = new List<Marking>();
            _unlimitedMarkingsLeft = new List<Marking>();
        }

        public Marking? LeaveMark()
        {
            if (MarkingsLeftBy.Any())
            {
                Marking leaving = MarkingsLeftBy.RandomItem();

                if (_limitedMarkingsLeft.Contains(leaving))
                    _limitedMarkingsLeft.Remove(leaving);

                return leaving;
            }
            else return null;
        }

        public void LeaveMarkOn(Place place) => LeaveMarkOn(place.Markings, ISubstantive.Types.Place);
        public void LeaveMarkOn(RogueLikeEntity entity) =>
            LeaveMarkOn(entity.AllComponents.GetFirst<MarkingCollection>(), entity.AllComponents.GetFirst<Substantive>().Type);

        public void LeaveMarkOn(MarkingCollection markings, ISubstantive.Types? type)
        {
            if (MarkingsLeftBy.Any(m => m.LeftOn == type))
            {
                Marking leaving = MarkingsLeftBy.RandomItem(m => m.LeftOn == type);

                if (_limitedMarkingsLeft.Contains(leaving))
                    _limitedMarkingsLeft.Remove(leaving);
                
                markings._markingsOn.Add(leaving);
            }
        }

        public void AddLimitedMarkings(Marking marking, int amount)
        {
            for (int i = 0; i < amount; i++)
                _limitedMarkingsLeft.Add(marking);
        }
        
        public void AddUnlimitedMarkings(Marking marking) => _unlimitedMarkingsLeft.Add(marking);
    }
}