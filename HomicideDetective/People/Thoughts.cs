using System.Collections.Generic;
using GoRogue.GameFramework;
using GoRogue.GameFramework.Components;
using HomicideDetective.Mysteries;

namespace HomicideDetective.People
{
    /// <summary>
    /// Currently a collection of strings used for printing to the message window.
    /// </summary>
    //todo - refactor significantly
    public class Thoughts : IGameObjectComponent, IDetailed
    {
        public IGameObject? Parent { get; set; }
        //public string Name { get; }
        public string Description { get; }
        private readonly List<string> _shortTermMemory;
        private readonly List<string> _longTermMemory;
        public List<string> Details => _shortTermMemory;
        
        public Thoughts() 
        {
            //Name = "Thoughts";
            Description = "The Thought Process of a creature.";
            _shortTermMemory = new List<string>();
            _longTermMemory = new List<string>();
        }
        
        public void Think(string[] thoughts)
        {
            CommitLongTermMemory();
            foreach (string thought in thoughts) 
                Think(thought);
        }

        private void CommitLongTermMemory()
        {
            foreach(var thought in _shortTermMemory)
                if(!_longTermMemory.Contains(thought))
                    _longTermMemory.Add(thought);
            
            _shortTermMemory.Clear();
        }

        public void Think(string thought)
        {
            if(!_shortTermMemory.Contains(thought))
                _shortTermMemory.Add(thought);
        }
    }
}