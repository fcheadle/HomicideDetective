using System.Collections.Generic;
using System.Linq;

namespace HomicideDetective.Words
{
    public class Verb
    {
        public class Tense
        {
            public string FirstPersonSingular { get; set; }
            public string FirstPersonPlural { get; set; }
            public string SecondPersonSingular { get; set; }
            public string SecondPersonPlural { get; set; }
            public string ThirdPersonSingular { get; set; }
            public string ThirdPersonPlural { get; set; }
            
            public Tense(string firstPersonSingular, string firstPersonPlural, string secondPersonSingular, string secondPersonPlural, string thirdPersonSingular, string thirdPersonPlural)
            {
                FirstPersonSingular = firstPersonSingular;
                FirstPersonPlural = firstPersonPlural;
                SecondPersonSingular = secondPersonSingular;
                SecondPersonPlural = secondPersonPlural;
                ThirdPersonSingular = thirdPersonSingular;
                ThirdPersonPlural = thirdPersonPlural;
            }

            public Tense(string all)
            {
                FirstPersonSingular = all;
                FirstPersonPlural = all;
                SecondPersonSingular = all;
                SecondPersonPlural = all;
                ThirdPersonSingular = all;
                ThirdPersonPlural = all;
            }
        }

        public string Infinitive { get; set; }
        public IEnumerable<string> Synonyms { get; set; }
        public Tense PastTense { get; set; }
        public Tense PresentTense { get; set; }
        public Tense FutureTense { get; set; }
        
        public Verb(string infinitive, Tense pastTense, Tense presentTense, Tense futureTense, params string[] synonyms)
        {
            Infinitive = infinitive;
            PastTense = pastTense;
            PresentTense = presentTense;
            FutureTense = futureTense;
            Synonyms = synonyms;
        }

        public bool IsSynonym(string with) => Synonyms.Contains(with);
    }
}