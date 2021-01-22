using HomicideDetective.New.People.Components;
using HomicideDetective.New.Places;
using HomicideDetective.New.Things;
using SadRogue.Primitives;
using TheSadRogue.Integration;

namespace HomicideDetective.New.People
{
    public class Person : RogueLikeEntity, IHaveDetails
    {
        public string Description { get; private set; }
        public string GivenName { get; private set; }
        public string FamilyName { get; private set; }
        
        public Person(Point position, string firstname, string lastname, string description = "") : base(position, 1, false, true, 1)
        {
            GivenName = firstname;
            FamilyName = lastname;
            Name = $"{GivenName} {FamilyName}";
            Description = description;
            
            var health = new HealthComponent();
            AllComponents.Add(health);
            
            var thoughts = new ThoughtComponent();
            AllComponents.Add(thoughts);

            var speech = new SpeechComponent();
            AllComponents.Add(speech);
        }
        
        public string[] GetDetails()
        {
            return new[] { Name, Description };
        }

        public void Murder(Person murderer, Item murderWeapon, CrimeScene sceneOfTheCrime)
        {
            Name = $"Corpse of {Name}";
            Description += $" Murdered by {murderer.Name} with a {murderWeapon.Name}, at {sceneOfTheCrime.Name}.";
            
        }
    }
}