using System.Collections.Generic;
using System.Runtime.Serialization;
using HomicideDetective.Words;
using Xunit;

namespace HomicideDetective.Tests
{
    public class SubstantiveTests
    {
        [DataMember] 
        public static IEnumerable<object[]> Types = new List<object[]>
        {
            new object[] {SubstantiveTypes.Person},
            new object[] {SubstantiveTypes.Place},
            new object[] {SubstantiveTypes.Thing},
        };
        
        [DataMember] 
        public static IEnumerable<object[]> NounData = new List<object[]>
        {
            new object[] { Constants.FemaleNouns },
            new object[] { Constants.MaleNouns },
            new object[] { Constants.GenderNeutralNouns },
            new object[] { Constants.ChildNouns },
            new object[] { new Noun("space critter", "space critters") },
        };
        
        [DataMember] 
        public static IEnumerable<object[]> PronounData = new List<object[]>
        {
            new object[] { Constants.FemalePronouns },
            new object[] { Constants.MalePronouns },
            new object[] { Constants.GenderNeutralPronouns },
            new object[] { Constants.ItemPronouns },
            new object[] { new Pronoun("space critter", "space critter", "space critter's", "space critter's self") },
        };

        private const string InfinitiveOne = "to at";
        private const string InfinitiveTwo = "to aweyf";
        private const string InfinitiveThree = "to po";
        private static Verb.Tense PastTense => new Verb.Tense("at", "ate", "atr", "atg", "ath", "aty");
        private static Verb.Tense PresentTense => new Verb.Tense("gt", "gte", "gtr", "gtg", "gth", "gty");
        private static Verb.Tense FutureTense => new Verb.Tense("ft", "fte", "ftr", "ftg", "fth", "fty");

        [DataMember] public static IEnumerable<object[]> VerbData = new List<object[]>
        {
            new object[] { new Verb(InfinitiveOne, PastTense, PresentTense, FutureTense) },
            new object[] { new Verb(InfinitiveTwo, PastTense, PresentTense, FutureTense) },
            new object[] { new Verb(InfinitiveThree, PastTense, PresentTense, FutureTense) },
        };
        private static Substantive MakeSubstantive(SubstantiveTypes type, Noun? nouns = null, Pronoun? pronouns = null, Verb? verbs = null)
        {
            pronouns ??= new Pronoun("he", "him", "his", "himself");
            nouns ??= new Noun("testperson", "testpeople");
            if (verbs == null)
            {
                var pastTense = new Verb.Tense("tested");
                var presentTense = new Verb.Tense("is testing", "are testing", "are testing", "are testing", "is testing", "are testing");
                var futureTense = new Verb.Tense("tested");
                verbs = new Verb("to test", pastTense, presentTense, futureTense);
            }
            
            var properties = new PhysicalProperties(36, 64);
            var name = "Test";
            var description = "a test substantive";
            return new Substantive(type, name, description, nouns, pronouns, properties, verbs);
        }

        [Theory]
        [MemberData(nameof(VerbData))]
        public void VerbTest(Verb verb)
        {
            var subs = MakeSubstantive(SubstantiveTypes.Person, verbs: verb);
            Assert.Equal(verb, subs.UsageVerb);
            
        }
        [Theory]
        [MemberData(nameof(NounData))]
        public void NounsTest(Noun nouns)
        {
            var subs = MakeSubstantive(SubstantiveTypes.Person, nouns: nouns);
            Assert.Equal(nouns, subs.Nouns);
        }

        [Theory]
        [MemberData(nameof(PronounData))]
        public void PronounsTest(Pronoun pronouns)
        {
            var subs = MakeSubstantive(SubstantiveTypes.Person, pronouns: pronouns);
            Assert.Equal(pronouns, subs.Pronouns);
        }
        
        [Fact]
        public void NameTest()
        {
            var substantive = MakeSubstantive(SubstantiveTypes.Person);
            Assert.Equal("Test", substantive.Name);
        }
        
        [Theory]
        [MemberData(nameof(Types))]
        public void TypeTest(SubstantiveTypes type)
        {
            var substantive = MakeSubstantive(type);
            Assert.Equal(type, substantive.Type);    
        }

        [Fact]
        public void DetailsTest()
        {
            var substantive = MakeSubstantive(SubstantiveTypes.Person);
            Assert.Empty(substantive.Details);
            substantive.AddDetail("arbitrary detail");
            Assert.Single(substantive.Details);
            Assert.Contains("arbitrary detail", substantive.Details);
        }
        
        [Theory]
        [MemberData(nameof(Types))]
        public void PrintableStringTest(SubstantiveTypes type)
        {
            var substantive = MakeSubstantive(type);
            var answer = substantive.GetPrintableString();
            Assert.Contains("a test substantive", answer);
        }
    }
}