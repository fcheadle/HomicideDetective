using System;
using System.Collections.Generic;
using System.Linq;
using GoRogue.Components.ParentAware;
using HomicideDetective.Happenings;
using SadRogue.Integration;

namespace HomicideDetective.People
{
    public class Memories : ParentAwareComponentBase<RogueLikeEntity>
    {
        public RogueLikeEntity Parent { get; set; }
        private Memory _currentThought;
        private readonly Timeline _shortTermMemory;
        private readonly Timeline _midTermMemory; 
        private readonly Timeline _longTermMemory;
        private readonly Timeline _falseNarrative;
        public Memory CurrentThought => _currentThought;
        public Timeline ShortTermMemory => _shortTermMemory;
        public Timeline MidTermMemory => _midTermMemory;
        public Timeline LongTermMemory => _longTermMemory;
        public Timeline FalseNarrative => _falseNarrative;
        
        public Memories()
        {
            _currentThought = new Memory(new DateTime(1970, 07, 04, 0, 0, 0), "I fell asleep.", "home", false);
            _shortTermMemory = new Timeline();
            _midTermMemory = new Timeline();
            _longTermMemory = new Timeline();
            _falseNarrative = new Timeline();
        }

        public void Think(string thought) => Think(new Memory(Program.CurrentGame.CurrentTime, thought, "", false));

        public void Think(Memory thought)
        {
            CommitToMemory();
            _currentThought = thought;
        }
        public void Think(IEnumerable<Memory> thoughts)
        {
            foreach (var thought in thoughts)
                Think(thought);
        }
        
        public void Think(IEnumerable<string> thoughts)
        {
            foreach (string thought in thoughts)
                Think(thought);
        }

        public void ThinkFalseFact(string fact) => ThinkFalseFact(new Memory(DateTime.Now, fact, "", false));

        public void ThinkFalseFact(Memory fact)
        {
            if(!_falseNarrative.Contains(fact))
                _falseNarrative.Add(fact);
        }
        public void ThinkFalseNarrative(IEnumerable<Memory> narrative)
        {
            foreach (var falseHappening in narrative)
                ThinkFalseFact(falseHappening);
        }

        public Memory HappeningAtTime(DateTime time)
        {
            if (time - Program.CurrentGame.CurrentTime < TimeSpan.FromHours(1))
                return _shortTermMemory.HappeningAtTime(time);
            
            if (time - Program.CurrentGame.CurrentTime < TimeSpan.FromHours(24))
                return _midTermMemory.HappeningAtTime(time);

            return _longTermMemory.HappeningAtTime(time);
        }
        private void CommitToMemory()
        {
            if(!_shortTermMemory.Contains(_currentThought))
                _shortTermMemory.Add(_currentThought);

            var oldMemories = _shortTermMemory.Where(m => _currentThought.When - m.When >= TimeSpan.FromHours(1)).ToList();
            foreach (var memory in oldMemories)
            {
                _shortTermMemory.Remove(memory);
                _midTermMemory.Add(memory);
            }

            var veryOldMemories = _midTermMemory.Where(m => _currentThought.When - m.When >= TimeSpan.FromHours(24)).ToList();
            foreach (var memory in veryOldMemories)
            {
                _midTermMemory.Remove(memory);
                _longTermMemory.Add(memory);
            }
        }
    }
}