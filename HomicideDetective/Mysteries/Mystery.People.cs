using System;
using System.Collections.Generic;
using GoRogue;
using HomicideDetective.People;
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
            murderer.AllComponents.Add(new Thoughts());
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
                witness.AllComponents.Add(new Thoughts());
                witness.AllComponents.Add(new Speech());
                
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
    }
}