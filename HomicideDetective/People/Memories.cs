using System;
using System.Collections.Generic;
using System.Linq;

namespace HomicideDetective.People
{
    /// <summary>
    /// A collection of memories, including long-term, mid-term, and
    /// short-term memories, and the memory currently being formed.
    /// </summary>
    public class Memories
    {
        private Memory _currentThought;
        private readonly List<Memory> _shortTermMemory;
        private readonly List<Memory> _midTermMemory; 
        private readonly List<Memory> _longTermMemory;
        private readonly List<Memory> _falseNarrative;
        public Memory CurrentThought => _currentThought;
        public List<Memory> ShortTermMemory => _shortTermMemory;
        public List<Memory> MidTermMemory => _midTermMemory;
        public List<Memory> LongTermMemory => _longTermMemory;
        public List<Memory> FalseNarrative => _falseNarrative;
        public List<Memory> All => _shortTermMemory.Concat(_midTermMemory).Concat(_longTermMemory).ToList();
        
        public Memories()
        {
            _currentThought = new Memory(new DateTime(1970, 07, 04, 0, 0, 0), 
                "I fell asleep.", "home");
            _shortTermMemory = new ();
            _midTermMemory = new ();
            _longTermMemory = new ();
            _falseNarrative = new ();
        }

        
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
                return HappeningAtTime(_shortTermMemory, time);
            
            if (time - Program.CurrentGame.CurrentTime < TimeSpan.FromHours(24))
                return HappeningAtTime(_midTermMemory, time);

            return HappeningAtTime(_longTermMemory, time);
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
        
        public static Memory HappeningAtTime(List<Memory> self, DateTime time)
        {
            if (self.Any(h => h.When == time))
                return self.First(h => h.When == time);
            
            return self.Where(x => x.When < time).OrderBy(x => x.When).Last();
        }
    }
}