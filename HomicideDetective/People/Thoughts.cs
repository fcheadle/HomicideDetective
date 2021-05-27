using System;
using System.Collections.Generic;
using System.Linq;
using GoRogue.GameFramework;
using GoRogue.GameFramework.Components;
using HomicideDetective.Happenings;

namespace HomicideDetective.People
{
    /// <summary>
    /// Currently a collection of strings used for printing to the message window.
    /// </summary>
    /// <remarks>
    /// Cannot access other components.
    /// Can be accessed by other components.
    /// </remarks>
    public class Thoughts : IGameObjectComponent
    {
        public IGameObject? Parent { get; set; }
        private Happening _currentThought;
        private readonly Timeline _shortTermMemory;
        private readonly Timeline _midTermMemory; 
        private readonly Timeline _longTermMemory;
        private readonly Timeline _falseNarrative;
        public Happening CurrentThought => _currentThought;
        public Timeline ShortTermMemory => _shortTermMemory;
        public Timeline MidTermMemory => _midTermMemory;
        public Timeline LongTermMemory => _longTermMemory;
        public Timeline FalseNarrative => _falseNarrative;
        
        public Thoughts()
        {
            _currentThought = new Happening(new DateTime(1970, 07, 04, 0, 0, 0), "I fell asleep.", "home", false);
            _shortTermMemory = new Timeline();
            _midTermMemory = new Timeline();
            _longTermMemory = new Timeline();
            _falseNarrative = new Timeline();
        }

        public void Think(string thought) => Think(new Happening(Program.CurrentTime, thought, "", false));

        public void Think(Happening thought)
        {
            CommitToMemory();
            _currentThought = thought;
        }
        public void Think(IEnumerable<Happening> thoughts)
        {
            foreach (var thought in thoughts)
                Think(thought);
        }
        
        public void Think(IEnumerable<string> thoughts)
        {
            foreach (string thought in thoughts)
                Think(thought);
        }

        public void ThinkFalseFact(string fact) => ThinkFalseFact(new Happening(Program.CurrentTime, fact, "", false));

        public void ThinkFalseFact(Happening fact)
        {
            if(!_falseNarrative.Contains(fact))
                _falseNarrative.Add(fact);
        }
        public void ThinkFalseNarrative(IEnumerable<Happening> narrative)
        {
            foreach (var falseHappening in narrative)
                ThinkFalseFact(falseHappening);
        }

        public Happening HappeningAtTime(DateTime time)
        {
            if (time - Program.CurrentTime < TimeSpan.FromHours(1))
                return _shortTermMemory.HappeningAtTime(time);
            
            if (time - Program.CurrentTime < TimeSpan.FromHours(24))
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