using System;
using System.Collections.Generic;
using HomicideDetective.New.People;
using HomicideDetective.New.Places;
using HomicideDetective.New.Things;
using SadRogue.Primitives;

namespace HomicideDetective.New
{
    public enum CaseStatus
    {
        Active,
        Closed,
        Cold
    }
    public class Mystery
    {
        public Person Victim => _victim;
        public int Number => _number;
        
        private readonly int _seed;
        private readonly int _number;
        private CaseStatus _status = CaseStatus.Active;
        private readonly Random _random;

        private CrimeScene _sceneOfTheCrime;
        private Item _murderWeapon;
        private Person _murderer;
        private List<Person> _witnesses;
        private List<CrimeScene> _scenes;
        private Person _victim;

        string[] _maleGivenNames = { "Nate", "Tom", "Dick", "Harry", "Bob", "Matthew", "Mark", "Luke", "John", "Josh" };

        string[] _femaleGivenNames = { "Alice", "Betty", "Jesse", "Sarah", "Angela", "Christine",  "Mary", "Liz", "Joan", "Jen" };

        string[] _surnames = {"Smith", "Johnson", "Michaels", "Douglas", "Andrews", "MacDonald", "Jenkins", "Peterson"};
        
        public Mystery(int seed, int caseNumber)
        {
            _seed = seed;
            _number = caseNumber;
            _random = new Random(seed);
            _victim = GenerateVictim();
            _murderer = GenerateMurderer();
            _murderWeapon = GenerateMurderWeapon();
            _sceneOfTheCrime = GenerateSceneOfMurder();
            _scenes = GenerateCrimeScenes();
            _witnesses = GenerateWitnesses();

            CommitMurder();
        }

        private void CommitMurder()
        {
            throw new NotImplementedException();
        }

        private List<Person> GenerateWitnesses()
        {
            throw new NotImplementedException();
        }

        private List<CrimeScene> GenerateCrimeScenes()
        {
            throw new NotImplementedException();
        }

        private CrimeScene GenerateSceneOfMurder()
        {
            throw new NotImplementedException();
        }

        private Person GenerateVictim()
        {
            string description = "";

            bool isMale = _random.Next(0, 6) <= 4; //women are more likely to be victims of murder
            bool isTall = _random.Next(0, 2) == 0;
            bool isFat = _random.Next(0, 2) == 0;
            bool isYoung = _random.Next(0, 3) <= 1; //young people more likely to be murdered than old people

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
            
            return new Person((0, 0), givenName, surname, description);
        }

        private Person GenerateMurderer()
        {
            string description = "";

            bool isMale = _random.Next(0, 5) <= 3; //men are more likely to murder
            bool isTall = _random.Next(0, 2) == 0;
            bool isFat = _random.Next(0, 2) == 0;
            bool isYoung = _random.Next(0, 2) == 2; //young people more likely to murder

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
            
            return new Person((0, 0), givenName, surname, description);        
        }

        private Item GenerateMurderWeapon()
        {
            string name = "hammer";
            string desctription = "a small tool, normally used for carpentry";
            return new Item((0, 0), Color.Gray, 'T', name, desctription);
        }
    }
}