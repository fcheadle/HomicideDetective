using System;
using System.Collections.Generic;
using GoRogue;
using HomicideDetective.People;
using HomicideDetective.Things;
using SadRogue.Integration;

namespace HomicideDetective.Mysteries
{
    /// <remarks>
    /// Generates the RogueLikeEntities and all of their components
    /// </remarks>
    public partial class Mystery
    {
        /// <summary>
        /// Generates a RogueLikeEntity that is the victim in a murder investigation.
        /// </summary>
        /// <returns></returns>
        public RogueLikeEntity GenerateVictimEntity()
        {
            var victimInfo = GeneratePersonalInfo(_surnames.RandomItem());
            string descr = $"This is the body of {victimInfo.Name}. {victimInfo.Pronoun} was a";
            victimInfo.Description = victimInfo.Description!.Replace($"{victimInfo.Pronoun} is a", descr);
            victimInfo.AddDetail("It is bloated from gases building up it's interior.");
            victimInfo.AddDetail("It is discolored from decay.");
            
            var victim = new RogueLikeEntity((0,0), 2, false);
            victim.AllComponents.Add(victimInfo);
            
            //todo - decompose
            TimeOfDeath = new DateTime(1970, 7,4, Random.Next(0,24), Random.Next(0,60), 0);
            return victim;
        }
        
        /// <summary>
        /// Generates a RogueLikeEntity who is the murderer in a murder investigation.
        /// </summary>
        /// <returns></returns>
        public RogueLikeEntity GenerateMurdererEntity()
        {
            var murdererInfo = GeneratePersonalInfo(_surnames.RandomItem());
            var murderer = new RogueLikeEntity((0,0), 1, false);
            murderer.AllComponents.Add(murdererInfo);
            murderer.AllComponents.Add(new Memories());
            murderer.AllComponents.Add(new Speech());
            
            //todo - memories, sayings, etc
            
            return murderer;
        }
        
        /// <summary>
        /// Generates everyone related to the case: family members, friends, coworkers, etc.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<RogueLikeEntity> GenerateWitnessEntities()
        {
            for(int i = 0; i < 10; i++)
            {
                var witnessInfo = GeneratePersonalInfo(_surnames.RandomItem());
                var witness = new RogueLikeEntity((0,0), 1, false);
                witness.AllComponents.Add(witnessInfo);
                witness.AllComponents.Add(new Memories());
                witness.AllComponents.Add(new Speech());
                witness.AllComponents.Add(GenerateMarkings());
                //todo - memories, sayings, etc

                yield return witness;
            }
        }
        
        /// <summary>
        /// Generate the descriptive info for any random person.
        /// </summary>
        /// <param name="surname">The Surname of the individual to generate.</param>
        /// <returns></returns>
        public Substantive GeneratePersonalInfo(string surname)
        {
            bool isMale = Random.Next(0, 2) == 0;
            bool isTall = Random.Next(0, 2) == 0;
            bool isFat = Random.Next(0, 2) == 0;
            bool isYoung = Random.Next(0, 3) <= 1; 

            string noun = isMale ? "man" : "woman";
            string pronoun = isMale ? "he" : "she";
            string pronounPossessive = isMale ? "his" : "her";
            string article = "";

            string height = isTall ? "tall" : "short";
            string heightDescription = isTall ? "slightly taller than average" : "rather short";
            string width = isFat ? "full-figured" : "slender";
            string widthDescription = isFat ? "moderately over-weight" : "rather slender";
            string age = isYoung ? "young" : "middle-aged";

            string description = $"{pronoun} is a {heightDescription}, {widthDescription} {age} {noun}. ";
            string givenName = isMale ? _maleGivenNames[ Random.Next(0, _maleGivenNames.Length)] : _femaleGivenNames[ Random.Next(0, _femaleGivenNames.Length)];

            var substantive = new Substantive(Substantive.Types.Person, $"{givenName} {surname}",
                gender: isMale ? "male" : "female", article, pronoun, pronounPossessive, description,
                37500, 24000);  
            
            return substantive;
        }

        public MarkingCollection GenerateMarkings()
        {
            MarkingCollection collection = new MarkingCollection();
            string name, adjective, description, color;

            name = "hair";
            int chance = Random.Next(1, 101);
            switch (chance % 7)
            {
                default:
                case 0: adjective = "curly"; break;
                case 1: adjective = "wavy"; break;
                case 2: adjective = "frizzy"; break;
                case 3: adjective = "flat"; break;
                case 4: adjective = "straight"; break;
                case 5: adjective = "springy"; break;
                case 6: adjective = "poofy"; break;
            }
            chance = Random.Next(1, 101);
            switch (chance % 12)
            {
                default:
                case 0: color = "light brown"; break;
                case 1: color = "brown"; break;
                case 2: color = "dark brown"; break;
                case 3: color = "black"; break;
                case 4: color = "dark grey"; break;
                case 5: color = "grey"; break;
                case 6: color = "light grey"; break;
                case 7: color = "white"; break;
                case 8: color = "blonde"; break;
                case 9: color = "dark blonde"; break;
                case 10: color = "strawberry blonde"; break;
                case 11: color = "dirty blonde"; break;
            }
            chance = Random.Next(1, 101);
            switch (chance % 6)
            {
                default:
                case 0: description = "waxy"; break;
                case 1: description = "dry"; break;
                case 2: description = "damp"; break;
                case 3: description = "moist"; break;
                case 4: description = "wet"; break;
                case 5: description = "greasy"; break;
            }

            var hair = new Marking(name, color, description, adjective);
            hair = new Marking(name, color, description, adjective, Substantive.Types.Person, new []{hair});
            collection.AddUnlimitedMarkings(hair);
            collection.LeaveMarkOn(collection, Substantive.Types.Person);
            return collection;
        }
    }
}