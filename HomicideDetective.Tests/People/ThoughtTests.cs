using HomicideDetective.People;
using Xunit;

namespace HomicideDetective.Tests.People
{
    public class ThoughtTests
    {
        [Fact]
        public void NewThoughtComponentTest()
        {
            Thoughts component = new Thoughts();
            Assert.Equal("The Thought Process of a creature.", component.Description);
            Assert.Equal("Thoughts", component.Name);
        }

        [Fact]
        public void GetDetailsTest()
        {
            Thoughts component = new Thoughts();
            var answer = component.Details;
            Assert.Empty(answer);
        }

        [Fact]
        public void ThinkSingleThoughtTest()
        {
            Thoughts component = new Thoughts();
            component.Think("Cogito Ergo Sum");
            Assert.Single(component.Details);
            
            component.Think("Cogito Ergo Sum");
            Assert.Single(component.Details);

            component.Think("Non Illegitamae Corporundum");
            Assert.Equal(2, component.Details.Length);

            Assert.Contains("Cogito Ergo Sum", component.Details);
            Assert.Contains("Non Illegitamae Corporundum", component.Details);
        }

        [Fact]
        public void ThinkMultipleThoughtsTest()
        {
            //arrange
            Thoughts component = new Thoughts();
            component.Think("Cogito Ergo Sum");       
            component.Think("Non Illegitamae Corporundum");
            string[] thoughts =
            {
                "I will not fear",
                "fear is the mind-killer.",
                "Fear is the little death that brings total annihilation.",
                "I will face my fear.",
                "I will let it pass through me,",
                "And over me as a storm.",
                "When the storm is gone,",
                "Only I will remain"
            };
            //act
            component.Think(thoughts);
            Assert.Equal(10, component.Details.Length);
            Assert.Contains("I will not fear", component.Details);
            Assert.Contains("Cogito Ergo Sum", component.Details);
            Assert.Contains("Non Illegitamae Corporundum", component.Details);
        }
    }
}