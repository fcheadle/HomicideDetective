using HomicideDetective.People;
using Xunit;

namespace HomicideDetective.Tests.People
{
    public class ThoughtTests
    {
        [Fact]
        public void GetDetailsTest()
        {
            Thoughts component = new Thoughts();
            var answer = component.Details;
            Assert.Single(answer);
        }
    }
}