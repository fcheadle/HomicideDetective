using System;
using System.Linq;
using GoRogue.Components;
using HomicideDetective.People.Speech;
using HomicideDetective.Words;

namespace HomicideDetective.People
{
    public class PersonFactory
    {
        /// <summary>
        /// Generate the descriptive info for any random person.
        /// </summary>
        /// <param name="seed">The RNG seed to use for generating this info</param>
        /// <param name="surname">The Surname of the individual to generate.</param>
        /// <returns></returns>
        public static Personhood GeneratePersonalInfo(int seed, string surname)
        {
            var random = new Random(seed);
            bool isMale = random.Next(0, 2) == 0;

            //todo - intersex, non binary
            var nouns = isMale ? Constants.MaleNouns : Constants.FemaleNouns; 
            var pronouns = isMale ? Constants.MalePronouns : Constants.FemalePronouns;
            
            int age = random.Next(0, 99);
            var ageCategory = CategoryFromAge(age);
            Occupations occupation;
            if (ageCategory < AgeCategory.PreTeen)
                occupation = Occupations.Child;
            else if (ageCategory < AgeCategory.YoungAdult)
                occupation = Occupations.Student;
            else
                Enum.TryParse(RandomItem(seed, Enum.GetNames<Occupations>()), out occupation);
            
            var mass = DetermineMass(seed, ageCategory);
            var volume = DetermineVolume(seed, ageCategory);
            
            string heightDescription = RandomHeightDescription(seed);
            string widthDescription = RandomWidthDescription(seed);
            var properties = new PhysicalProperties(mass, volume, heightDescription, widthDescription, ageCategory.ToString());
            var massText = $"{mass / 1000}";
            var volumeText = $"{volume / 1000}";
            string description = $"{pronouns.Subjective} weighs {massText}kg and is {volumeText}l in volume.";
            string givenName = isMale ? RandomItem(seed, Constants.MaleGivenNames): RandomItem(seed, Constants.FemaleGivenNames);
            var person = new Personhood($"{givenName} {surname}", description, age, occupation, nouns, pronouns, properties);
            InitSpeech(person);
            return person;
        }

        private static string RandomWidthDescription(int seed)
        {
            var descriptors = new[]
            {
                "slender",
                "skinny",
                "thin",
                "trim",
                "fit",
                "lean",
                "thick",
                "jacked",
                "slightly overweight",
                "somewhat overweight",
                "portly",
                "shapely",
                "rotund",
                "overweight",
            };

            return RandomItem(seed, descriptors);
        }

        private static string RandomHeightDescription(int seed)
        {
            var descriptors = new[]
            {
                "short",
                "somewhat short",
                "average-height",
                "sort of tall",
                "tall",
                "very short",
                "very tall",
            };

            return RandomItem(seed, descriptors);
        }

        private static IComponentCollection PersonhoodComponents(int seed, ISubstantive substantive)
        {
            return new ComponentCollection
            {
                { new Memories(), Constants.MemoryComponentTag },
                { new Fingerprint(seed), Constants.FingerprintComponentTag },
                { new SpeechComponent(substantive.Pronouns), Constants.SpeechComponentTag },
            };
        }

        private static string RandomItem(int seed, string[] possible)
        {
            var random = new Random(seed);
            return possible[random.Next(0, possible.Length)];
        }

        private static AgeCategory CategoryFromAge(int age)
        {
            var val = Enum.GetValues<AgeCategory>().Where(ac => ac <= (AgeCategory)age);
            return val.OrderByDescending(ac => ac).First();
        }

        private static int DetermineMass(int seed, AgeCategory age)
        {
            var random = new Random(seed);
            
            switch (age)
            {
                case AgeCategory.Baby: return random.Next(2200, 4500);
                case AgeCategory.Infant: return random.Next(8000, 13000);
                case AgeCategory.Toddler: return random.Next(11000, 18000);
                case AgeCategory.Child: return random.Next(17000, 25000);
                case AgeCategory.PreTeen: return random.Next(25000, 55000);
                case AgeCategory.Teenager: return random.Next(38000, 80000);
                case AgeCategory.HighSchoolFreshmen: return random.Next(40000, 90000);
                case AgeCategory.HighSchoolSophomore: return random.Next(50000, 100000);
                case AgeCategory.HighSchoolJunior: return random.Next(60000, 110000);
                default: return random.Next(70000, 120000);
            }
        }
        
        //currently, just heights cubed
        private static int DetermineVolume(int seed, AgeCategory age)
        {
            var random = new Random(seed);
            switch (age)
            {
                case AgeCategory.Baby: return random.Next(91125, 166375);
                case AgeCategory.Infant: return random.Next(125000, 216000);
                case AgeCategory.Toddler: return random.Next(166375, 274625);
                case AgeCategory.Child: return random.Next(216000, 343000);
                case AgeCategory.PreTeen: return random.Next(274625, 421875);
                case AgeCategory.Teenager: return random.Next(343000, 512000);
                case AgeCategory.HighSchoolFreshmen: return random.Next(421875, 614125);
                case AgeCategory.HighSchoolSophomore: return random.Next(512000, 729000);
                case AgeCategory.HighSchoolJunior: return random.Next(614125, 857375);
                default: return random.Next(729000, 1000000);
            }
        }

        private static void InitSpeech(Personhood person)
        {
            person.Speech.Posture = GeneratePostures(person.Pronouns.Subjective, person.Pronouns.Possessive);
            person.Speech.ToneOfVoice = GenerateTonesOfVoice(person.Pronouns.Subjective, person.Pronouns.Possessive);
            person.Speech.Stance = GenerateStances(person.Pronouns.Subjective, person.Pronouns.Possessive);
            person.Speech.ArmPositions = GenerateArmPositions(person.Pronouns.Subjective, person.Pronouns.Possessive);
            person.Speech.EyePosition = GenerateEyeMovements(person.Pronouns.Subjective, person.Pronouns.Possessive);
        }
        #region conversational strings
        public static SpeechStringCollection GenerateTonesOfVoice(string pronoun, string pronounPossessive)
        {
            var tones = new SpeechStringCollection();
            tones.Common.AddRange(new[]
            {
                $"{pronounPossessive} voice remains a steady volume throughout.",
                $"{pronoun} pause slightly before speaking.",
                $"{pronoun} says, after a slight pause.",
                $"{pronounPossessive} words come out with no hesitation.",
                $"{pronoun} speaks in a hushed tone.",
                $"{pronoun} says in a flat monotone",
                $"{pronoun} says with little vocal inflection",
                $"{pronoun} blurts out, somewhat loudly"
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
        public static SpeechStringCollection GenerateArmPositions(string pronoun, string pronounPossessive)
        {
            var positions = new SpeechStringCollection();
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
        public static SpeechStringCollection GenerateEyeMovements(string pronoun, string pronounPossessive)
        {
            var answer = new SpeechStringCollection();
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
        public static SpeechStringCollection GenerateStances(string pronoun, string pronounPossessive)
        {
            var answer = new SpeechStringCollection();
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
        public static SpeechStringCollection GeneratePostures(string pronoun, string pronounPossessive)
        {
            var answer = new SpeechStringCollection();
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
    }
}