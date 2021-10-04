using HomicideDetective.Words;
using Xunit;

namespace HomicideDetective.Tests.Words
{
    public class PronounTests
    {
        [Fact]
        public void TestPronouns()
        {
            var subjective = "le";
            var objective = "la";
            var possessive = "quo";
            var reflexive = "umbulf";
            var pronoun = new Pronoun(subjective, objective, possessive, reflexive);
            Assert.Equal(subjective, pronoun.Subjective);
            Assert.Equal(objective, pronoun.Objective);
            Assert.Equal(possessive, pronoun.Possessive);
            Assert.Equal(reflexive, pronoun.Reflexive);
        }
    }
}