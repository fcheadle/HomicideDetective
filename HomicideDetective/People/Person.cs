using HomicideDetective.Mysteries;
using HomicideDetective.People.Components;
using HomicideDetective.Places;
using HomicideDetective.Things;
using HomicideDetective.Things.Marks;
using SadRogue.Primitives;
using TheSadRogue.Integration;

namespace HomicideDetective.People
{
    public class Person : RogueLikeEntity, IDetailed
    {
        public string FirstName { get; }
        public string LastName { get; }
        public string Description { get; }
        public Substantive Substantive => AllComponents.GetFirst<Substantive>();
        public Thoughts Thoughts => AllComponents.GetFirst<Thoughts>();
        public Speech Speech => AllComponents.GetFirst<Speech>();
        public Health Health => AllComponents.GetFirst<Health>();
        public Markings Markings => AllComponents.GetFirst<Markings>();
        

        public Person(Point position, string firstname, string lastname, string description, int mass, int volume, 
            string sizeDescription, string weightDescription) : base(position, 1, false)
        {
            FirstName = firstname;
            LastName = lastname;
            Name = $"{FirstName} {LastName}";
            Description = description;
            
            var physical = new Substantive(Substantive.Types.Person, Name, description, mass, volume, sizeDescription, weightDescription);
            AllComponents.Add(physical);
            
            var health = new Health();
            AllComponents.Add(health);
            
            var thoughts = new Thoughts();
            AllComponents.Add(thoughts);

            var speech = new Speech();
            AllComponents.Add(speech);

            var markings = new Markings();
            var print = new Fingerprint(64);
            markings.AddUnlimitedMarkings(print);
            AllComponents.Add(markings);
            
        }
        
        public string[] GetDetails() => Substantive.Details;

        public void Murder(Person murderer, Thing murderWeapon, Place sceneOfTheCrime)
        {
            Substantive.AddDetail($" Murdered by {murderer.Name} with a {murderWeapon.Name}, at {sceneOfTheCrime.Name}.");
            Health.Murder();
        }
    }
}