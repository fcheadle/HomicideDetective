using System;
using System.Collections.Generic;
using System.Linq;
using GoRogue;
using HomicideDetective.People;
using HomicideDetective.Places;
using HomicideDetective.Things;

namespace HomicideDetective.Mysteries
{
    public class Mystery
    {
        public enum Statuses {Active, Closed, Cold}
        private Statuses _status = Statuses.Active;
        public Person Victim => _victim;
        public int Number => _number;
        public Place CurrentScene { get; private set; }
        
        private readonly int _seed;
        private readonly int _number;
        private readonly Random _random;
        
        private Place _sceneOfTheCrime;
        // private Place _sceneWhereTheyFoundTheBody;
        private Thing _murderWeapon;
        private Person _murderer;
        private IEnumerable<Person> _witnesses;
        private IEnumerable<Place> _scenes;
        private Person _victim;

        string[] _maleGivenNames = { "Nate", "Tom", "Dick", "Harry", "Bob", "Matthew", "Mark", "Luke", "John", "Josh" };

        string[] _femaleGivenNames = { "Alice", "Betty", "Jesse", "Sarah", "Angela", "Christine",  "Mary", "Liz", "Joan", "Jen" };

        string[] _surnames = {"Smith", "Johnson", "Michaels", "Douglas", "Andrews", "MacDonald", "Jenkins", "Peterson"};
        
        public Mystery(int seed, int caseNumber)
        {
            _seed = seed;
            _number = caseNumber;
            _random = new Random(_seed + _number);
        }

        public void CommitMurder()
        {
            _scenes = GenerateScenes();
            _sceneOfTheCrime = _scenes.First();
            _murderer = GeneratePerson(_surnames.RandomItem());
            _victim = GeneratePerson(_surnames.RandomItem());
            _murderWeapon = GenerateMurderWeapon();
            _victim.Murder(_murderer, _murderWeapon, _sceneOfTheCrime);
            _sceneOfTheCrime.Populate(new[] {_victim});
            _sceneOfTheCrime.Populate(new[] {_murderWeapon});
            CurrentScene = _sceneOfTheCrime;
        }
        
        private IEnumerable<Place> GenerateScenes()
        {
            _scenes = new List<Place>();
            for (int i = 0; i < 15; i++)
            {
                string surname = _surnames[_random.Next(0, _surnames.Length)];
                var owner = GeneratePerson(surname);
                var occupant = GeneratePerson(surname);
                var child1 = GeneratePerson(surname);
                var child2 = GeneratePerson(surname);
                var scene =  new Place(Program.MapWidth, Program.MapHeight, $"{surname} Residence",$"Case {_number} Location of Interest").GenerateHouse();
                scene.Populate(new[]{owner, occupant, child1, child2});
                yield return scene;
            }
        }

        private Person GeneratePerson(string surname)
        {
            string description = "";

            bool isMale = _random.Next(0, 2) == 0;
            bool isTall = _random.Next(0, 2) == 0;
            bool isFat = _random.Next(0, 2) == 0;
            bool isYoung = _random.Next(0, 3) <= 1; 

            string noun = isMale ? "man" : "woman";
            string pronoun = isMale ? "he" : "she";
            string pronounPossessive = isMale ? "his" : "her";
            string pronounPassive = isMale ? "him" : "her";

            string height = isTall ? "tall" : "short";
            string width = isFat ? "full-figured" : "slender";
            string age = isYoung ? "young" : "middle-aged";

            description = $"{pronoun} is a {height}, {width} {age} {noun}.";
            int i = _random.Next(0, _maleGivenNames.Length);
            string givenName = isMale ? _maleGivenNames[i] : _femaleGivenNames[i];

            Person person = new Person((0, 0), givenName, surname, description, 24, 24, "average", "average");
            return person;
        }

        private Thing GenerateMurderWeapon()
        {
            string name;
            string description;
            
            switch (_random.Next(0,10))
            {
                default:
                case 0: 
                    name = "hammer";
                    description = "a small tool, normally used for carpentry";
                    break;
                case 1: 
                    name = "switchblade"; 
                    description = "a small, concealable knife";
                    break;
                case 2: 
                    name = "pistol"; 
                    description = "a small, concealable handgun";
                    break;
                case 3: 
                    name = "poison"; 
                    description = "a lethal dose of hydrogen-cyanide";
                    break;
                case 4: 
                    name = "kitchen knife"; 
                    description = "a small tool used for preparing food";
                    break;
                case 5: 
                    name = "shotgun";
                    description = "a large gun used for scaring off vermin";
                    break;
                case 6: 
                    name = "rock"; 
                    description = "a stone from off the ground";
                    break;
                case 7: 
                    name = "screwdriver"; 
                    description = "a small tool, used for all manner of handiwork";
                    break;
                case 8: 
                    name = "revolver"; 
                    description = "a handgun";
                    break;
                case 9: 
                    name = "rifle"; 
                    description = "a large gun, used for hunting animals";
                    break;
            }

            var item = new Thing((0, 0), name, description, 240, 240, "is exactly average in size", "weighs about what you expect");
            
            item.Substantive.AddDetail(description);
            return item;
        }
    }
}