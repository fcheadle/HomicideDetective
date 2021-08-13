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
            var substantive = new Substantive(ISubstantive.Types.Person, victimInfo.Name, pronoun: victimInfo.Pronoun,
                pronounPossessive: victimInfo.PronounPossessive);
            string descr = $"This is the body of {victimInfo.Name}. {victimInfo.Pronoun} was a";
            substantive.Description = victimInfo.Description!.Replace($"{victimInfo.Pronoun} is a", descr);
            substantive.AddDetail("It is bloated from gases building up it's interior.");
            substantive.AddDetail("It is discolored from decay.");
            
            var victim = new RogueLikeEntity((0,0), 2, false);
            victim.AllComponents.Add(substantive);
            
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
            var entity = new RogueLikeEntity(default, 1, false);
            entity.AllComponents.Add(new Personhood(murdererInfo.Name, murdererInfo.Description, murdererInfo.Noun,
                murdererInfo.Pronoun, murdererInfo.PronounPossessive));
            return entity;
        }
        
        /// <summary>
        /// Generates everyone related to the case: family members, friends, coworkers, etc.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<RogueLikeEntity> GenerateWitnessEntities()
        {
            for(int i = 0; i < 10; i++)
            {
                var entity = new RogueLikeEntity(default, 1, false);
                entity.AllComponents.Add(GeneratePersonalInfo(_surnames.RandomItem()));
                yield return entity;
            }
        }
        
        /// <summary>
        /// Generate the descriptive info for any random person.
        /// </summary>
        /// <param name="surname">The Surname of the individual to generate.</param>
        /// <returns></returns>
        public Personhood GeneratePersonalInfo(string surname)
        {
            bool isMale = Random.Next(0, 2) == 0;
            bool isTall = Random.Next(0, 2) == 0;
            bool isFat = Random.Next(0, 2) == 0;
            bool isYoung = Random.Next(0, 3) <= 1; 

            string noun = isMale ? "man" : "woman";
            string pronoun = isMale ? "he" : "she";
            string pronounPossessive = isMale ? "his" : "her";
            string article = "";

            string heightDescription = isTall ? "slightly taller than average" : "rather short";
            string widthDescription = isFat ? "moderately over-weight" : "rather slender";
            string age = isYoung ? "young" : "middle-aged";

            string description = $"{pronoun} is a {heightDescription}, {widthDescription} {age} {noun}. ";
            string givenName = isMale ? _maleGivenNames[ Random.Next(0, _maleGivenNames.Length)] : _femaleGivenNames[ Random.Next(0, _femaleGivenNames.Length)];

            var person = new Personhood($"{givenName} {surname}", description, noun, pronoun, pronounPossessive);
            InitSpeech(person);
            return person;
        }

        private void InitSpeech(Personhood person)
        {
            person.Speech.Posture = GeneratePostures(person.Pronoun, person.PronounPossessive);
            person.Speech.ToneOfVoice = GenerateTonesOfVoice(person.Pronoun, person.PronounPossessive);
            person.Speech.Stance = GenerateStances(person.Pronoun, person.PronounPossessive);
            person.Speech.ArmPositions = GenerateArmPositions(person.Pronoun, person.PronounPossessive);
            person.Speech.EyePosition = GenerateEyeMovements(person.Pronoun, person.PronounPossessive);
        }


        #region conversational strings
        public BoolDependantStringList GenerateTonesOfVoice(string pronoun, string pronounPossessive)
        {
            var tones = new BoolDependantStringList();
            tones.Common.AddRange(new[]
            {
                $"{pronounPossessive} voice remains a steady volume throughout.",
                $"{pronoun} pause slightly before speaking.",
                $"{pronoun} say, after a slight pause.",
                $"{pronounPossessive} words come out with no hesitation.",
                $"{pronoun} speak in a hushed tone.",
                $"{pronoun} say in a flat monotone",
                $"{pronoun} say with little vocal inflection",
                $"{pronoun} blurt out, somewhat loudly"
            });


            tones.Lies.AddRange(new[]
            {
                $"{pronounPossessive} voice pitches slightly higher.",
                $"{pronoun} say as {pronounPossessive} voice cracks.",
                $"{pronoun} say after a considerable pause.",
                $"{pronoun} say in a measured monotone.",
                $"{pronoun} say, voice carrying a tinge of sincerity.",
            });

            tones.Truths.AddRange(new[]
            {
                $"{pronoun} raise {pronounPossessive} voice and {pronounPossessive} tone becomes pointed.",
                $"{pronoun} gasps the words out hurriedly.",
                $"{pronoun} speaks in a very low tone.",
                $"{pronoun} sound annoyed.",
                $"{pronounPossessive} voice carries a tinge of sincerity.",
            });

            return tones;

        }
        public BoolDependantStringList GenerateArmPositions(string pronoun, string pronounPossessive)
        {
            var positions = new BoolDependantStringList();
            positions.Common.AddRange(new[]
            {
                $"{pronounPossessive} arms cross over {pronounPossessive} chest",
                $"{pronoun} put {pronounPossessive} hands in {pronounPossessive} pockets",
                $"{pronoun} put {pronounPossessive} hands on {pronounPossessive} hips",
                $"{pronoun} put {pronounPossessive} hands in {pronounPossessive} pockets",
                $"{pronoun} scratch {pronounPossessive} neck.",
            });

            positions.Lies.AddRange(new[]
            {
                $"{pronounPossessive} hands fidget in {pronounPossessive} pockets",
                $"{pronounPossessive} hands tremble slightly",
                $"A vein in {pronounPossessive} forearm pulses and bulges",
            });

            positions.Truths.AddRange(new[]
            {
                $"{pronounPossessive} hands stop moving entirely",
                $"{pronounPossessive} hands shake uncontrollably",
                $"{pronoun} make exaggerated motions with {pronounPossessive} hands",
            });
            return positions;
        }
        public BoolDependantStringList GenerateEyeMovements(string pronoun, string pronounPossessive)
        {
            var answer = new BoolDependantStringList();
            answer.Common.AddRange(new[]
            {
                $"{pronounPossessive} eyes have a faraway look",
                $"{pronounPossessive} eyes focus squarely on me",
                $"{pronounPossessive} eyes scan {pronounPossessive} surroundings",
                $"{pronounPossessive} eyes are focused on something behind me",
                $"{pronoun} makes direct eye contact",
                $"{pronoun} focuses her eyes on the bridge of my nose",
                $"{pronoun} focuses her eyes directly on mine",
            });

            answer.Lies.AddRange(new[]
            {
                $"{pronounPossessive} eyes dart about quickly",
                $"{pronoun} avoids meeting your gaze",
                $"{pronoun} averts {pronounPossessive} eyes",
            });

            answer.Truths.AddRange(new[]
            {
                $"{pronounPossessive} eyes flicker",
                $"{pronoun} maintains fierce eye contact",
                $"{pronoun} keeps my gaze intensely",
            });

            return answer;
        }
        public BoolDependantStringList GenerateStances(string pronoun, string pronounPossessive)
        {
            var answer = new BoolDependantStringList();
            answer.Common.AddRange(new[]
            {
                $"{pronoun} stands with {pronounPossessive} feet shoulder width apart",
                $"{pronoun} stands with {pronounPossessive} feet squared to {pronounPossessive} shoulders",
                $"{pronoun} carries {pronounPossessive} weight on {pronounPossessive} left leg with {pronounPossessive} right leg cocked out",
                $"{pronoun} carries {pronounPossessive} weight on {pronounPossessive} left leg with {pronounPossessive} right leg at an angle",
                $"{pronoun} carries {pronounPossessive} weight on {pronounPossessive} right leg with {pronounPossessive} left leg cocked out",
                $"{pronoun} carries {pronounPossessive} weight on {pronounPossessive} right leg with {pronounPossessive} left leg at an angle",
                $"{pronoun} leans on {pronounPossessive} left leg",
                $"{pronoun} leans on {pronounPossessive} right leg",
            });
            
            answer.Truths.AddRange(new[]
            {
                $"{pronoun} is tapping {pronounPossessive} foot",
                $"{pronoun} squares {pronounPossessive} feet towards me",
                $"{pronoun} rocks {pronounPossessive} weight onto the balls of {pronounPossessive} feet",
            });
            
            answer.Lies.AddRange(new[]
            {
                $"{pronoun} hyper-extends {pronounPossessive} knees",
                $"{pronoun} shifts weight from {pronounPossessive} left to {pronounPossessive} right leg",
                $"{pronoun} shifts weight from {pronounPossessive} right to {pronounPossessive} left leg",
                $"{pronoun} shifts weight from one leg to another",
                $"{pronoun} shifts weight onto {pronounPossessive} heels",
            });

            return answer;
        }
        public BoolDependantStringList GeneratePostures(string pronoun, string pronounPossessive)
        {
            var answer = new BoolDependantStringList();
            answer.Common.AddRange(new[]
            {
                $"{pronoun} hunches {pronounPossessive} shoulders slightly",
                $"{pronoun} has the slightest hunch",
                $"{pronoun} slouches {pronounPossessive} head",
                $"{pronoun} raises {pronounPossessive} head as {pronoun} speaks",
                $"{pronoun} lowers {pronounPossessive} head as {pronoun} speaks",
                $"{pronoun} untilts {pronounPossessive} head",
                $"{pronoun} faces straight ahead",
            });
            
            answer.Lies.AddRange(new[]
            {
                $"{pronoun} turns {pronounPossessive} head away",
                $"{pronoun} tilts {pronounPossessive} head down",
                $"{pronoun} hunches {pronounPossessive} greatly",
                $"{pronoun} cocks {pronounPossessive} head ever so slightly",
            });
            answer.Truths.AddRange(new[]
            {
                $"{pronoun} holds {pronounPossessive} up high",
                $"{pronoun} turns {pronounPossessive} head to face me",
                $"{pronoun} straightens the slouch out of {pronounPossessive} neck",
                $"{pronoun} straightens {pronounPossessive} neck",
            });

            return answer;
        }
        #endregion
        
        public MarkingCollection GenerateMarkings()
        {
            MarkingCollection collection = new MarkingCollection();
            string name, adjective, description, color;

            name = "hair";
            int chance = Random.Next(1, 101);
            switch (chance % 7)
            {
                default: adjective = "curly"; break;
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
                default: color = "light brown"; break;
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
                default: description = "waxy"; break;
                case 1: description = "dry"; break;
                case 2: description = "damp"; break;
                case 3: description = "moist"; break;
                case 4: description = "wet"; break;
                case 5: description = "greasy"; break;
            }

            var hair = new Marking(name, color, description, adjective);
            hair = new Marking(name, color, description, adjective, ISubstantive.Types.Person, new []{hair});
            collection.AddUnlimitedMarkings(hair);
            collection.LeaveMarkOn(collection, ISubstantive.Types.Person);
            return collection;
        }
    }
}