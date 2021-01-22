using SadRogue.Primitives;
using TheSadRogue.Integration;

namespace HomicideDetective.New.People
{
    public class Person : RogueLikeEntity, IHaveDetails
    {
        public string Name => $"{GivenName} {FamilyName}";
        
        public string Description { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        
        public Person(Point position, string firstname = "", string lastname = "", string description = "") : base(position, 1, false, true, 1)
        {
            GivenName = firstname;
            FamilyName = lastname;
            Description = description;
            
            var health = new HealthComponent();
            AllComponents.Add(health);
            
            var thoughts = new ThoughtComponent();
            AllComponents.Add(thoughts);
        }
        
        public string[] GetDetails()
        {
            return new[] { Description };
        }
    }
}