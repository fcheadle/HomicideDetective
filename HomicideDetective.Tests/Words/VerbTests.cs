using HomicideDetective.Words;
using Xunit;

namespace HomicideDetective.Tests.Words
{
    public class VerbTests
    {        
        private const string InfinitiveOne = "to at";
        private const string InfinitiveTwo = "to aweyf";
        private const string InfinitiveThree = "to po";
        private static Verb.Tense PastTense => new Verb.Tense("at", "ate", "atr", "atg", "ath", "aty");
        private static Verb.Tense PresentTense => new Verb.Tense("gt", "gte", "gtr", "gtg", "gth", "gty");
        private static Verb.Tense FutureTense => new Verb.Tense("ft", "fte", "ftr", "ftg", "fth", "fty");

        [Fact]
        public void TestVerbs()
        {
            var verb = new Verb(InfinitiveOne, PastTense, PresentTense, FutureTense);
            Assert.Equal(InfinitiveOne, verb.Infinitive);
            Assert.Equal(PastTense.FirstPersonSingular, verb.PastTense.FirstPersonSingular);
            Assert.Equal(PresentTense.SecondPersonPlural, verb.PresentTense.SecondPersonPlural);
            Assert.Equal(FutureTense.FirstPersonPlural, verb.FutureTense.FirstPersonPlural);
        }

        [Fact]
        public void TenseTest()
        {
            Assert.Equal("at", PastTense.FirstPersonSingular);
            Assert.Equal("ate", PastTense.FirstPersonPlural);
            Assert.Equal("atr", PastTense.SecondPersonSingular);
            Assert.Equal("atg", PastTense.SecondPersonPlural);
            Assert.Equal("ath", PastTense.ThirdPersonSingular);
            Assert.Equal("aty", PastTense.ThirdPersonPlural);
        }

        [Fact]
        public void SynonymsTest()
        {
            var verbOne = new Verb(InfinitiveOne, PastTense, PresentTense, FutureTense, InfinitiveTwo, InfinitiveThree);
            var verbTwo = new Verb(InfinitiveTwo, PastTense, PresentTense, FutureTense, InfinitiveOne, InfinitiveThree);
            var verbThree = new Verb(InfinitiveThree, PastTense, PresentTense, FutureTense, InfinitiveTwo, InfinitiveOne);
            
            Assert.True(verbOne.IsSynonym(verbTwo.Infinitive));
            Assert.True(verbOne.IsSynonym(verbThree.Infinitive));
            Assert.True(verbTwo.IsSynonym(verbThree.Infinitive));
        }
    }
}