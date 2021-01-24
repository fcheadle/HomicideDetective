using HomicideDetective.Mysteries;
using HomicideDetective.People.Components;
using HomicideDetective.Places;
using HomicideDetective.Things;
using SadRogue.Primitives;
using TheSadRogue.Integration;

namespace HomicideDetective.People
{
    public class Person : RogueLikeEntity, IHaveDetails
    {
        public string Description => Substantive.Description!;
        public ThoughtComponent Thoughts 
            => AllComponents.GetFirst<ThoughtComponent>();
        public SpeechComponent Speech 
            => AllComponents.GetFirst<SpeechComponent>();
        public HealthComponent Health 
            => AllComponents.GetFirst<HealthComponent>();
        public Substantive Substantive 
            => AllComponents.GetFirst<Substantive>();

        public Person(Point position, Substantive substantive) : base(position, 1, false)
        {
            Name = substantive.Name;
            AllComponents.Add(substantive);
            InitRequiredComponents();
        }
        
        public Person(Point position, string firstname, string lastname, int size = 24, int weight = 240, 
            string sizeDescription = "average in size", string weightDescription = "weighs about what you'd expect") 
            : base(position, 1, false)
        {
            Name = $"{firstname} {lastname}";

            var physical = new Substantive(Substantive.Types.Person, Name, weight, size,
                sizeDescription, weightDescription);
            
            AllComponents.Add(physical);
            InitRequiredComponents();
        }

        private void InitRequiredComponents()
        {
            var health = new HealthComponent();
            AllComponents.Add(health);
            
            var thoughts = new ThoughtComponent();
            AllComponents.Add(thoughts);

            var speech = new SpeechComponent();
            AllComponents.Add(speech);        
        }

        public string[] GetDetails()
            => Substantive.GetDetails();

        public void Murder(Person murderer, Thing murderWeapon, CrimeScene sceneOfTheCrime)
        {
            //Name = $"Corpse of {Name}";
            Substantive.AddDetail($" Murdered by {murderer.Name} with a {murderWeapon.Name}, at {sceneOfTheCrime.Name}.");
            Health.Murder();
        }
    }
}