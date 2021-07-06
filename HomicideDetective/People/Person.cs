using System;
using System.Collections.Generic;
using System.Linq;
using GoRogue;
using SadRogue.Integration;
using SadRogue.Primitives;

namespace HomicideDetective.People
{
    /// <summary>
    /// Contains all of the components that are common to all people
    /// </summary>
    public class Person : RogueLikeEntity, ISubstantive
    {
        public string Description { get; }
        public string Noun { get; }
        public string Pronoun { get; }
        public string PronounPossessive { get; }
        public List<string> Details { get; }
        public ISubstantive.Types Type => ISubstantive.Types.Person;
        public Memories Memories { get; set; }
        public Speech Speech { get; set; }

        private bool _hasIntroduced;
        private bool _hasGreeted;
        private bool _hasToldAboutSelf;

        
        public Person(string name, string description, string noun, string pronoun, string pronounPossessive) : 
            base(Point.None, Color.LightGray, Color.Black, 1, false)
        {
            Name = name;
            Description = description;
            Noun = noun;
            Pronoun = pronoun;
            PronounPossessive = pronounPossessive;
            Details = new List<string>();
            Memories = new Memories();
            Speech = new Speech();
            Speech.GetNextSpeech(false);
        }
        
        #region speech
        public string SpeakTo()
        {
            Speech.ApplyPronouns(Pronoun, PronounPossessive);
            string answer;
            
            if (!_hasGreeted)
                answer = $"\"{Greet()},\" says {Name}";
            else if (!_hasIntroduced)
                answer = $"\"{Introduce()},\" {Pronoun} says";
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
            _hasGreeted = true;
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
            return $"{Description}";
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
            throw new NotImplementedException();
        }
        public string InquireAboutPlace(string name)
        {
            throw new NotImplementedException();
        }
        public string InquireAboutThing(string name)
        {
            throw new NotImplementedException();
        }
        #endregion

        public string GetPrintableString()
        {
            var answer = "This is ";
            answer += (!_hasIntroduced ? "a " + Noun : Name);
            answer += $". {Speech.CurrentStance}. {Speech.CurrentPosture}. {Speech.CurrentArmPosition}";
            return answer;
        }
    }
}