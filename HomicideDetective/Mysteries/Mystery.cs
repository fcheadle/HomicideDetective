using System;
using System.Collections.Generic;
using System.Linq;
using HomicideDetective.People;
using HomicideDetective.Places;
using HomicideDetective.Things;
using SadRogue.Primitives;

namespace HomicideDetective.Mysteries
{
    public class Mystery
    {
        public enum Statuses {Active, Closed, Cold}
        public Person Victim => _victim;
        public int Number => _number;
        public Place CurrentScene { get; }
        
        private readonly int _seed;
        private readonly int _number;
        private Statuses _status = Statuses.Active;
        private readonly Random _random;
        
        private Place _sceneOfTheCrime;
        private Place _sceneWhereTheyFoundTheBody;
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
            _random = new Random(seed);
            _victim = GeneratePerson();
            _murderer = GeneratePerson();
            _murderWeapon = GenerateMurderWeapon();
            _sceneOfTheCrime = GenerateScene();
            _witnesses = GenerateWitnesses();
            _scenes = GenerateScenes();
            
            CommitMurder();
            // CurrentScene = _sceneWhereTheyFoundTheBody;
            CurrentScene = new Place(Program.MapWidth, Program.MapHeight, $"plains").GeneratePlains();
        }

        private void CommitMurder()
        {
            _victim.Murder(_murderer, _murderWeapon, _sceneOfTheCrime);
            _sceneWhereTheyFoundTheBody = _sceneOfTheCrime; //for now
            _sceneWhereTheyFoundTheBody.AddEntity(_victim);
        }

        private IEnumerable<Person> GenerateWitnesses()
        {
            for (int i = 0; i < 50; i++)
            {
                yield return GeneratePerson();
            }
        }

        private IEnumerable<Place> GenerateScenes()
        {
            for (int i = 0; i < 10; i++)
            {
                int j = i * 5;
                var scene =  GenerateScene();
                scene.Populate(_witnesses.ToList().GetRange(j, 5));
                yield return scene;
            }        
        }

        private Place GenerateScene() 
            => new Place(Program.MapWidth, Program.MapHeight, $"Case Number {_number} location of interest").GenerateHouse();

        private Person GeneratePerson()
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
            string surname = _surnames[_random.Next(0, _surnames.Length)];
            
            return new Person((0, 0), givenName, surname);
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
                    name = "poison"; 
                    description = "a small tool, normally used for carpentry";
                    break;
                case 8: 
                    name = "poison"; 
                    description = "a small tool, normally used for carpentry";
                    break;
                case 9: 
                    name = "poison"; 
                    description = "a small tool, normally used for carpentry";
                    break;
            }

            var item = new Substantive(Substantive.Types.Thing, name, 240, 18,
                "is almost exactly average in size", "weighs about as much as you would expect");
            return new Thing((0, 0), item);
        }
    }
}