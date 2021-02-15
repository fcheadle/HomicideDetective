using System;
using System.Collections.Generic;
using System.Linq;
using GoRogue;
using HomicideDetective.People;
using HomicideDetective.Places;
using HomicideDetective.Places.Generation;

namespace HomicideDetective.Mysteries
{
    public class Mystery
    {
        public enum Statuses {Active, Closed, Cold}
        public Statuses Status { get; set; } = Statuses.Active;
        public int Seed { get; set; }
        public int CaseNumber { get; set; }
        public Substantive Victim { get; set; }
        public Substantive Murderer { get; set; }
        public Substantive MurderWeapon { get; set; }
        public Substantive SceneOfCrime { get; set; }
        public Substantive[] Witnesses { get; set; }
        public Substantive[] LocationsOfInterest { get; set; }
        public Substantive[] Evidence { get; set; }
        public Scene CurrentScene { get; set; }
        public Scene[] Scenes { get; set; }
        public Random Random { get; set; }

        string[] _maleGivenNames = { "Nate", "Tom", "Dick", "Harry", "Bob", "Matthew", "Mark", "Luke", "John", "Josh" };

        string[] _femaleGivenNames = { "Alice", "Betty", "Jesse", "Sarah", "Angela", "Christine",  "Mary", "Liz", "Joan", "Jen" };

        string[] _surnames = {"Smith", "Johnson", "Michaels", "Douglas", "Andrews", "MacDonald", "Jenkins", "Peterson"};
        
        public Mystery(){}
        
        public Mystery(int seed, int caseNumber)
        {
            Seed = seed + caseNumber;
            CaseNumber = caseNumber;
            Random = new Random(Seed + CaseNumber);
        }

        private int Chance() => Random.Next(0, 101);
        
        public void CommitMurder()
        {
            if (Random == null)
                Random = new Random(Seed + CaseNumber);

            Victim = GeneratePerson(_surnames.RandomItem());
            Murderer = GeneratePerson(_surnames.RandomItem());
            MurderWeapon = GenerateMurderWeapon();
            var locations = new List<Substantive>();
            var witnesses = new List<Substantive>();
            for (int i = 0; i < 15; i++)
            {
                string surname = _surnames[Random.Next(0, _surnames.Length)];
                
                var address = $"{Chance()} {Enum.GetNames<RoadNames>().RandomItem()}";
                var substantive = new Substantive(Substantive.Types.Place, address, Random.Next(),
                    article: "It", description: $"Location of interest in the Murder of {Victim.Name}");

                
                var placeOfResidence = $"Lives at {address}";
                var owner = GeneratePerson(surname);
                owner.AddDetail(placeOfResidence);
                witnesses.Add(owner);
                
                var occupant = GeneratePerson(surname);
                occupant.AddDetail(placeOfResidence);
                witnesses.Add(occupant);

                var child1 = GeneratePerson(surname);
                child1.AddDetail(placeOfResidence);
                witnesses.Add(child1);

                var child2 = GeneratePerson(surname);
                child2.AddDetail(placeOfResidence);
                witnesses.Add(child2);

                substantive.AddDetail($"{owner.Name}, {occupant.Name}, {child1.Name}, and {child2.Name} live here.");
                locations.Add(substantive);
            }

            LocationsOfInterest = locations.ToArray();
            Witnesses = witnesses.ToArray();
            SceneOfCrime = new Substantive(Substantive.Types.Place, $"{Victim.Name}'s Home", Random.Next(),
                article: "It", description: $"Location of interest in the Murder of {Victim.Name}");
            
            
        }

        public void Open()
        {
            CurrentScene = new Scene(Program.MapWidth, Program.MapHeight, SceneOfCrime);
            CurrentScene.GenerateHouse();
            CurrentScene.Populate(new Substantive[]{Victim, Murderer, MurderWeapon});
            var victim = CurrentScene.Entities.Items.First
                (e => ((ISubstantive) e).Substantive.Name == Victim.Name);
            ((Person)victim).Murder(Murderer, MurderWeapon, SceneOfCrime);
            //CurrentScene.Populate();
        }
        
        public Substantive GeneratePerson(string surname)
        {
            bool isMale = Random.Next(0, 2) == 0;
            bool isTall = Random.Next(0, 2) == 0;
            bool isFat = Random.Next(0, 2) == 0;
            bool isYoung = Random.Next(0, 3) <= 1; 

            string noun = isMale ? "man" : "woman";
            string pronoun = isMale ? "he" : "she";
            string pronounPossessive = isMale ? "his" : "her";
            string pronounPassive = isMale ? "him" : "her";

            string height = isTall ? "tall" : "short";
            string width = isFat ? "full-figured" : "slender";
            string age = isYoung ? "young" : "middle-aged";

            string description = $"{pronoun} is a {height}, {width} {age} {noun}.";
            string givenName = isMale ? _maleGivenNames[ Random.Next(0, _maleGivenNames.Length)] : _femaleGivenNames[ Random.Next(0, _femaleGivenNames.Length)];

            var substantive = new Substantive(Substantive.Types.Person, $"{givenName} {surname}", Random.Next(),
                isMale ? "male" : "female", null, pronoun, pronounPossessive, description, 37500, 24000);  
            
            return substantive;
        }

        public Substantive GenerateMurderWeapon()
        {
            string name;
            string description;
            string detail;
            string size;
            string weight;
            int mass;
            int volume;
            switch (Random.Next(0,10))
            {
                default:
                case 0: 
                    name = "Hammer";
                    description = "A small tool, normally used for carpentry";
                    mass = 710;
                    volume = 490;
                    detail = "It is completely free of dust, and has a slight smell of bleach";
                    size = "is exactly average in size";
                    weight = "weighs a little less than it looks like";
                    break;
                case 1: 
                    name = "Switchblade"; 
                    description = "A small, concealable knife";
                    mass = 125;
                    volume = 325;
                    detail = "There is a small patch of red rust near the hinge";
                    size = "is tiny";
                    weight = "weighs almost nothing";
                    break;
                case 2: 
                    name = "Pistol"; 
                    description = "A small, concealable handgun";
                    mass = 6750;
                    volume = 465;
                    detail = "There are residue patterns on the muzzle";
                    size = "is easily concealable";
                    weight = "weighs more than it looks like";
                    break;
                case 3: 
                    name = "Poison"; 
                    description = "A lethal dose of hydrogen-cyanide";
                    mass = 15;
                    volume = 12;
                    detail = "There is a puncture hole in the cap";
                    size = "is fits within an insulin bottle";
                    weight = "weighs little more than water";
                    break;
                case 4: 
                    name = "Kitchen Knife"; 
                    description = "A small tool used for preparing food";
                    mass = 240;
                    volume = 390;
                    detail = "It is somewhat warped along its flat plane";
                    size = "is long and curved";
                    weight = "weighs very little";
                    break;
                case 5: 
                    name = "Shotgun";
                    description = "A large gun used for scaring off vermin";
                    mass = 13500;
                    volume = 8825;
                    detail = "there is one shell in the chamber";
                    size = "is exactly average in size";
                    weight = "is heavy and has powerful kickback";
                    break;
                case 6: 
                    name = "Rock"; 
                    description = "A stone from off the ground";
                    mass = 10000;
                    volume = 750;
                    detail = "It is covered in blood and haphazardly discarded nearby";
                    size = "is three times the size of your fist";
                    weight = "is very dense and hard";
                    break;
                case 7: 
                    name = "Screwdriver"; 
                    description = "A small tool, used for all manner of handiwork";
                    mass = 120;
                    volume = 200;
                    detail = "This flathead has been recently cleaned";
                    size = "is four and a half inches long";
                    weight = "has a clear plastic handle";
                    break;
                case 8: 
                    name = "Revolver"; 
                    description = "A handgun";
                    mass = 7000;
                    volume = 700;
                    detail = "it has five bullets, and one empty chamber";
                    size = "has finely-etched filligree";
                    weight = "looks like it costs a lot of money";
                    break;
                case 9: 
                    name = "Rifle"; 
                    description = "A large gun, used for hunting animals";
                    mass = 12000;
                    volume = 8200;
                    detail = "It is coated with gunpowder residue.";
                    size = "has scuff marks on the butt";
                    weight = "looks like it has taken a beating";
                    break;
            }

            var substantive = new Substantive(Substantive.Types.Thing, name, Random.Next(), 
                description: description, mass: mass, volume: volume, sizeDescription:size, weightDescription: weight);
            substantive.AddDetail(detail);
            return substantive;
        }
    }
}