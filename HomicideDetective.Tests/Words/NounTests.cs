using HomicideDetective.Words;
using Xunit;

namespace HomicideDetective.Tests.Words
{
    public class NounTests
    {
        [Fact]
        public void TestNouns()
        {
            var singular = "mommy";
            var plural = "mommies";
            var noun = new Noun(singular, plural);
            Assert.Equal(singular, noun.Singular);
            Assert.Equal(plural, noun.Plural);
        }
    }
}