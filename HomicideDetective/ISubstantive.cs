using System.Collections.Generic;
using HomicideDetective.Things;

namespace HomicideDetective
{
    public interface ISubstantive : IPrintable
    {
        public enum Types {Person, Place, Thing}
        public Types Type { get; }
        
        string Name { get; }
        string Description { get; }
        string Noun { get; }
        string Pronoun { get; }
        string PronounPossessive { get; }
        List<string> Details { get; }                                                                                                                                    
        MarkingCollection Markings { get; }
    }
}