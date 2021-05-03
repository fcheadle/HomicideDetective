using System;
using HomicideDetective.Mysteries;
using HomicideDetective.Things;
using HomicideDetective.Things.Marks;
using SadRogue.Primitives;
using SadRogue.Integration;

namespace HomicideDetective.People
{
    public class Person : RogueLikeEntity, ISubstantive
    {
        public bool Alive { get; private set; }
        public Substantive Substantive => AllComponents.GetFirst<Substantive>();
        public Thoughts Thoughts => AllComponents.GetFirst<Thoughts>();
        public Speech Speech => AllComponents.GetFirst<Speech>();
        //public Health Health => AllComponents.GetFirst<Health>();
        public MarkingCollection Markings => AllComponents.GetFirst<MarkingCollection>();
        public Person(Point position, Substantive substantive) : base(position, 1, false)
        {
            AllComponents.Add(substantive);
            //AllComponents.Add(new Health());
            AllComponents.Add(new Thoughts());
            AllComponents.Add(new Speech());

            var markings = new MarkingCollection();
            var print = new Fingerprint(64);
            markings.AddUnlimitedMarkings(print);
            AllComponents.Add(markings);

            Name = substantive.Name;
        }

        public Action InteractUp => () => Interact(Direction.Up);
        public Action InteractDown => () => Interact(Direction.Down);
        public Action InteractLeft => () => Interact(Direction.Left);
        public Action InteractRight => () => Interact(Direction.Right);

        private void Interact(Direction d)
        {
            var entities = CurrentMap!.Entities.GetItemsAt(Position + d);
            foreach (var entity in entities)
            {
                if (entity is Person person)
                    Thoughts.Think(person.Speech.Details.ToArray()); 
                
                else if (entity is Thing thing)
                    Thoughts.Think(thing.Substantive.Details);
            }            
        }

        public void Murder(Substantive murderer, Substantive murderWeapon, Substantive sceneOfTheCrime)
        {
            Alive = false;
            //Health.Murder(murderer, murderWeapon, sceneOfTheCrime);
            Speech.Alive = false;
        }
    }
}