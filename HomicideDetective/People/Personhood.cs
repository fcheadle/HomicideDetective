using System;
using System.Collections.Generic;
using System.Linq;
using GoRogue;
using GoRogue.Components.ParentAware;
using HomicideDetective.People.Speech;
using HomicideDetective.Things;
using HomicideDetective.Words;
using SadRogue.Integration;

namespace HomicideDetective.People
{
    /// <summary>
    /// Contains all of the components that are common to all people
    /// </summary>
    public class Personhood : ParentAwareComponentBase<RogueLikeEntity>, ISubstantive
    {
        public string Name
        {
            get => _substantive.Name;
            set => _substantive.Name = value;
        }

        public string Description => _substantive.Description;
        public PhysicalProperties Properties => _substantive.Properties;
        public Noun Nouns => _substantive.Nouns;
        public Pronoun Pronouns => _substantive.Pronouns;
        public Verb? UsageVerb => null;
        public Fingerprint Fingerprint { get; }
        public List<string> Details { get; }
        public SubstantiveTypes Type => SubstantiveTypes.Person;
        public Memories Memories { get; set; }
        public SpeechComponent Speech { get; set; }
        public MarkingCollection Markings { get; }
        public int Age { get; }
        public AgeCategory AgeCategory { get; }
        public Occupations Occupation { get; }
        public bool HasIntroduced { get; private set; }
        public bool HasToldAboutSelf { get; private set; }
        
        private readonly Substantive _substantive;
        
        public Personhood(string name, string description, int age, Occupations occupation, Noun nouns, Pronoun pronouns, PhysicalProperties properties)
            : this(new Substantive(SubstantiveTypes.Person, name, description, nouns, pronouns, properties), age, occupation) { }

        public Personhood(Substantive info, int age, Occupations occupation)
        {
            _substantive = info;
            Age = age;
            AgeCategory = (AgeCategory)age;
            Occupation = occupation;
            Details = new List<string>();
            Memories = new Memories();
            Speech = new SpeechComponent(Pronouns);
            Markings = new MarkingCollection();
            //Fingerprint = new Fingerprint(Properties.Mass + Properties.Volume + age);
        }
        
        public string GetPrintableString()
        {
            var description = Properties.GetPrintableString();
            description = description.Length == 0 ? "a " + Nouns.Singular : $"a {description} {Nouns.Singular}";
            var noun = !HasIntroduced ? description : Name;
            var answer = $"This is {noun}. {Speech.BodyLanguage()}";
            return answer;
        }
        
        #region speech
        public string SpeakTo()
        {
            string answer;
            
            if (!HasIntroduced)
                answer = $"\"{Greet()},\" {Pronouns.Subjective} says, \"{Introduce()}\"";
            else if (!HasToldAboutSelf)
                answer = $"\"{InquireAboutSelf()},\" says {Name}";
            else
            {
                //todo - return CommandContext for engaging in conversation
                var randomMemory = Memories.All.RandomItem();
                Speech.GetNextSpeech(randomMemory.Private);
                if (randomMemory.Private)
                {
                    randomMemory = Memories.FalseNarrative.RandomItem();
                }
                
                Speech.SpeakAbout(randomMemory);
                answer = Speech.GetPrintableString();
            }

            return answer;
        }
        
        public string Greet()
        {
            return "Hello Detective";
        }
        public string Introduce()
        {
            HasIntroduced = true;
            return $"My name is {Name}";
        }
        public string InquireAboutSelf()
        {
            HasToldAboutSelf = true;
            var answer = $"I am a {Enum.GetName(Occupation)}. (info about self?) (info about relationship to victim?)";
            return answer;
        }
        public string InquireWhereabouts(DateTime atTime)
        {
            var happening = Memories.HappeningAtTime(atTime);
            return $"I was at {happening.Where}";
        }
        public string InquireAboutCompany(DateTime atTime)
        {
            var happening = Memories.HappeningAtTime(atTime);
            var answer = "I was with ";
            if (happening.Who.Any())
                foreach (var person in happening.Who)
                    answer += $"{person}, ";
            else
                answer += "no one";
        
            return answer;
        }
        public string InquireAboutMemory(DateTime atTime)
        {
            return Memories.HappeningAtTime(atTime).GetPrintableString();
        }
        public string InquireAboutPerson(string name)
        {
            var memoriesWithPerson = Memories.All.Where(mem => mem.Who.Contains(name));
            var withPerson = memoriesWithPerson as Memory[] ?? memoriesWithPerson.ToArray();
            if (!withPerson.Any())
                return "I've never met that person";
            
            var privateMemoriesWithPerson = withPerson.Where(mem => mem.Private);
            if (privateMemoriesWithPerson.Count() > withPerson.Count())
                return "I don't know them";
            else
                return "Oh yeah, I know them";
        }
        public string InquireAboutPlace(string name)
        {
            var memoriesAtPlace = Memories.All.Where(mem => mem.Where.Contains(name));
            var atPlace = memoriesAtPlace as Memory[] ?? memoriesAtPlace.ToArray();
            if (!atPlace.Any())
                return "I've never been there";
            
            var privateMemoriesAtPlace = memoriesAtPlace.Where(mem => mem.Private);
            if (privateMemoriesAtPlace.Count() > atPlace.Count())
                return "Never heard of it";
            else
                return "I've been there lots of times";
        }
        public string InquireAboutThing(string name)
        {
            var memories = Memories.All.Where(mem => mem.What.Contains(name));
            var memoriesOfItem = memories as Memory[] ?? memories.ToArray();
            if (!memoriesOfItem.Any())
                return "I don't know what you're talking about";
            
            var privateMemoriesAtPlace = memories.Where(mem => mem.Private);
            if (privateMemoriesAtPlace.Count() > memoriesOfItem.Count())
                return "No, I don't believe I know what you mean";
            else
                return "I am aware of that item, yes";
        }
        #endregion
    }
}