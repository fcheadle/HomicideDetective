using System;
using System.Collections.Generic;
using HomicideDetective.Mysteries;
using HomicideDetective.Things;
using SadConsole;
using SadRogue.Primitives;
using TheSadRogue.Integration.Components;

namespace HomicideDetective.People
{

    public class Thoughts : RogueLikeComponentBase, IDetailed
    {
        public string Name { get; }
        public string Description { get; }
        private readonly List<string> _thoughts;
        public string[] GetDetails() => _thoughts.ToArray();
        public string[] AllDetails() => _thoughts.ToArray(); //for now
            

        public Thoughts() : base(true, false, false, false)
        {
            Name = "Thoughts";
            Description = "The Thought Process of a creature.";
            _thoughts = new List<string>();
        }

        public override void Update(IScreenObject host, TimeSpan delta)
        {
            Think();
            base.Update(host, delta);
        }

        public void Think()
        {
            //todo - flesh out
            //Think(Parent.Position.ToString());
            //Think("Currently thinking about");
        }
        
        public void Think(string[] thoughts)
        {
            _thoughts.Clear();
            foreach (string thought in thoughts) 
                Think(thought);
        }
        public void Think(string thought)
        {
            //todo - more complexity
            if(!_thoughts.Contains(thought))
                _thoughts.Add(thought);
        }

        public string[] Look(Direction d)
        {
            var entities = Parent!.CurrentMap!.Entities.GetItemsAt(Parent!.Position + d);
            foreach (var entity in entities)
            {
                if (entity is Thing thing)
                {
                    Think(thing.GetDetails());
                    return thing.GetDetails();
                }
            }


            return new[] { "" };
            
        }
    }
}