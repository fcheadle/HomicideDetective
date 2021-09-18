using System;
using System.Collections.Generic;
using System.Linq;
using GoRogue;
using GoRogue.Components.ParentAware;
using HomicideDetective.Things;
using SadRogue.Integration;
using SadRogue.Primitives;

namespace HomicideDetective.People
{
    /// <summary>
    /// Contains all of the components that are common to all people
    /// </summary>
    public class Personhood : ParentAwareComponentBase<RogueLikeEntity>, ISubstantive
    {
        public string Name { get; set; }
        public string Description { get; }
        public string Noun { get; }
        public string Pronoun { get; }
        public string PronounPossessive { get; }
        public Fingerprint Fingerprint { get; }
        public List<string> Details { get; }
        public ISubstantive.Types Type => ISubstantive.Types.Person;
        public Memories Memories { get; set; }
        public Speech Speech { get; set; }
        public MarkingCollection Markings { get; }
        public int Age { get; }
        public AgeCategory AgeCategory { get; }
        public Occupations Occupation { get; }
        private bool _hasIntroduced;
        private bool _hasToldAboutSelf;
        
        public Personhood(string name, string description, string noun, string pronoun, string pronounPossessive, int age, Occupations occupation, int mass, int volume)
        {
            Age = age;
            AgeCategory = (AgeCategory) Age;
            Name = name;
            Description = description;
            Noun = noun;
            Pronoun = pronoun;
            PronounPossessive = pronounPossessive;
            Occupation = occupation;
            Details = new List<string>();
            Memories = new Memories();
            Speech = new Speech(pronoun, pronounPossessive);
            Speech.ApplyPronouns(Pronoun, PronounPossessive);
            Markings = new MarkingCollection();
            Fingerprint = new Fingerprint(mass + volume);
        }
        
        #region speech
        public string SpeakTo()
        {
            Speech.ApplyPronouns(Pronoun, PronounPossessive);
            string answer;
            
            if (!_hasIntroduced)
                answer = $"\"{Greet()},\" {Pronoun} says, \"{Introduce()}\"";
            else if (!_hasToldAboutSelf)
                answer = $"\"{InquireAboutSelf()},\" says {Name}";
            else
            {
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
            _hasIntroduced = true;
            return "Hello Detective";
        }
        public string Introduce()
        {
            _hasIntroduced = true;
            return $"My name is {Name}";
        }
        public string InquireAboutSelf()
        {
            _hasToldAboutSelf = true;
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

        public string GetPrintableString()
        {
            var noun = !_hasIntroduced ? "a " + Noun : Name;
            var answer = $"This is {noun}. {Speech.BodyLanguage()}";
            return answer;
        }
    }
}