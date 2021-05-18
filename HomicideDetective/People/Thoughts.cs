using System.Collections.Generic;
using GoRogue;
using GoRogue.GameFramework;
using GoRogue.GameFramework.Components;
using HomicideDetective.Mysteries;

namespace HomicideDetective.People
{
    /// <summary>
    /// Currently a collection of strings used for printing to the message window.
    /// </summary>
    public class Thoughts : IGameObjectComponent//, IDetailed
    {
        public IGameObject? Parent { get; set; }
        private Happening _shortTermMemory; //current thoughts only
        private Timeline _midTermMemory; //today's thoughts
        private List<Timeline> _longTermMemory; //1 timeline == 1 day
        public string SurfaceThought => _shortTermMemory.Occurrence;
        
        public Thoughts()
        {
            _shortTermMemory = new Happening(new Time(0, 0), "I fell asleep.");
            _midTermMemory = new Timeline();
            _longTermMemory = new List<Timeline>();
        }

        public void Think(IEnumerable<string> thoughts)
        {
            CommitMidTermMemory();

            string observation = "";

            foreach (string thought in thoughts)
                observation += $" {thought}";

            Think(observation);
        }

        public void Think(string thought)
        {
            CommitMidTermMemory();
            _shortTermMemory = new Happening(Program.CurrentTime, thought);
        }

        public void CommitMidTermMemory()
        {
            //todo - if midnight rolls around, start a new day's memories
            if (!_midTermMemory.Contains(_shortTermMemory))
                _midTermMemory.Add(_shortTermMemory);
        }

        private void CommitLongTermMemory()
        {
            _longTermMemory.Add(_midTermMemory);
            _midTermMemory = new Timeline();
        }

        public void Think(Timeline thoughts)
        {
            _midTermMemory.AddRange(thoughts);
            _shortTermMemory = thoughts.MostRecent();
        }
    }
}