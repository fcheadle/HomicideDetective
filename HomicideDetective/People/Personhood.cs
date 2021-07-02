using System;
using System.Linq;
using GoRogue;
using GoRogue.Components.ParentAware;
using SadRogue.Integration;

namespace HomicideDetective.People
{
    /// <summary>
    /// Contains all of the components that are common to all people
    /// </summary>
    public class Personhood : ParentAwareComponentBase<RogueLikeEntity>
    {
        public Substantive Info => Parent!.AllComponents.GetFirst<Substantive>();
        public Memories Memories { get; set; }
        public Voice Voice { get; set; }
        public BodyLanguage BodyLanguage { get; set; }

        private bool _hasIntroduced = false;
        private bool _hasGreeted = false;
        private bool _hasToldAboutSelf = false;

        public Personhood()
        {
            Memories = new Memories();
            Voice = new Voice();
            BodyLanguage = new BodyLanguage();
        }
        
        public string SpeakTo()
        {
            BodyLanguage.ApplyPronouns(Info.Pronoun, Info.PronounPossessive);
            string answer = "";
            
            if (!_hasGreeted)
                answer = $"\"{Greet()},\" says {Info.Name}";
            else if (!_hasIntroduced)
                answer = $"\"{Introduce()},\" {Info.Pronoun} says";
            else if (!_hasToldAboutSelf)
                answer = $"\"{InquireAboutSelf()},\" says {Info.Name}";
            else
            {
                var randomMemory = Memories.All.RandomItem();
                BodyLanguage.NextBodyLanguage(randomMemory.Private);
                
                if (randomMemory.Private)
                {
                    randomMemory = Memories.FalseNarrative.RandomItem();
                }
                
                Voice.TalkAbout(randomMemory);
                answer = $"\"{Voice.CurrentSpokenText},\" {Info.Pronoun} says. {Voice.CurrentToneOfVoice}. ";
                answer += BodyLanguage.GetPrintableString();
            }

            return answer;
        }
        
        public string Greet()
        {
            _hasGreeted = true;
            return "Hello Detective.";
        }
        public string Introduce()
        {
            _hasIntroduced = true;
            return $"My name is {Info.Name}.";
        }
        public string InquireAboutSelf()
        {
            _hasToldAboutSelf = true;
            return $"{Info.Description}.";
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
        public string InquireAboutHappening(DateTime atTime)
        {
            return Memories.HappeningAtTime(atTime).ToString();
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
    }
}