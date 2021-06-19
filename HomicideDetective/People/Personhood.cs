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
        public Substantive Info => Parent.AllComponents.GetFirst<Substantive>();
        public Memories Memories { get; set; }
        public Voice Speech { get; set; }
        public BodyLanguage BodyLanguage { get; set; }

        private bool _hasIntroduced = false;
        private bool _hasGreeted = false;
        private bool _hasToldAboutSelf = false;

        public Personhood()
        {
            Memories = new Memories();
            Speech = new Voice();
            BodyLanguage = new BodyLanguage();
        }
        
        public string SpeakTo(bool lying = false)
        {
            if (!_hasGreeted)
                return Greet();
            else if (!_hasIntroduced)
                return Introduce();
            else if (!_hasToldAboutSelf)
                return InquireAboutSelf();
            else if (lying)
                return Memories.FalseNarrative.RandomItem().What;
            else
                return Memories.LongTermMemory.RandomItem().What;
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
            //todo
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