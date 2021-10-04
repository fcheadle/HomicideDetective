using System.Collections.Generic;
using HomicideDetective.Things;
using HomicideDetective.Words;

namespace HomicideDetective
{
    public interface ISubstantive : IPrintable
    {
        SubstantiveTypes Type { get; }
        
        string Name { get; }
        
        string Description { get; }
        
        PhysicalProperties Properties { get; }
        
        Noun Nouns { get; }
        
        Pronoun Pronouns { get; }
        
        Verb? UsageVerb { get; }
        
        List<string> Details { get; }                                                                                                                                    
        
        MarkingCollection Markings { get; }
    }
}