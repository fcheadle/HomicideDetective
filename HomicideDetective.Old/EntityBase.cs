using System.Collections.Generic;
using System.Linq;
using GoRogue;
using HomicideDetective.Old.Items.Markings;
using Microsoft.Xna.Framework;
using SadConsole;

namespace HomicideDetective.Old
{
    //going away in favor of RogueLikeEntity
    //and markings on the IsPhysical component
    public class EntityBase : BasicEntity
    {
        //public string Name { get; set; }
        public int Mass { get; set; }
        public int Volume { get; set; }
        public int Glyph { get; set; }
        public string Description
        {
            get => _descr;
            set
            {
                _descr = value;
                foreach (Marking marking in _markingsOn)
                {
                    _descr += " On this entity is a " + marking.Name;
                    _descr += " (" + marking.Description + ").";
                }
            }
        }

        public EntityBase(Color foreground, Color background, int glyph, Coord position, int layer, bool isWalkable, bool isTransparent, List<Marking> markingsLeftByUnlimited = null, Dictionary<Marking, int> markingsLeftByLimited = null) : base(foreground, background, glyph, position, layer, isWalkable, isTransparent)
        {
            Glyph = glyph;
            _unlimitedMarkingsLeft = markingsLeftByUnlimited ?? _unlimitedMarkingsLeft;
            var limitedMarkings = markingsLeftByLimited ?? new Dictionary<Marking, int>();
            foreach (KeyValuePair<Marking, int> marking in limitedMarkings)
                for (int i = 0; i < marking.Value; i++)
                    _limitedMarkingsLeft.Add(marking.Key);
        }

        //public BasicTerrain ToBasicTerrain() =>
        //    new BasicTerrain(DefaultForeground, DefaultBackground, Position, IsWalkable, IsTransparent);
        
        
        #region Markings
        private List<Marking> _markingsOn = new List<Marking>(); //the markings on this thing
        private List<Marking> _limitedMarkingsLeft = new List<Marking>(); //the (limited) markings left by interaction - ie mud/blood/things that can wash off
        private List<Marking> _unlimitedMarkingsLeft = new List<Marking>(); //the (unlimited) markings left by this thing - ie cuts/hair follicles/footprints/fingerprints
        private string _descr;

        public List<Marking> MarkingsOn => _markingsOn.ToList();
        public List<Marking> MarkingsLeftBy => _limitedMarkingsLeft.Concat(_unlimitedMarkingsLeft).ToList();
        


        public void InteractWith(EntityBase interactor)
        {
            int priorCount = _limitedMarkingsLeft.Count;
            Marking mark = LeaveMark();
            interactor.Mark(mark, priorCount - _limitedMarkingsLeft.Count);
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
        {
            _unlimitedMarkingsLeft.Add(marking);
        }

        public void Mark(Marking marking, int amount = 0)
        {
            _markingsOn.Add(marking);
            for (int i = 0; i < amount; i++)
            {
                _limitedMarkingsLeft.Add(marking);
            }
        }
        #endregion
        
        public override string ToString() => Name + ", " + Description;
        
        public void ProcessTimeUnit()
        {
            //eventually, we'll want some physics stuff in here (velocity / direction / hardness / so on)
        }
    }
}