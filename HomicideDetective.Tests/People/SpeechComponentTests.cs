using HomicideDetective.People;
using HomicideDetective.People.Components;
using Xunit;

namespace HomicideDetective.Tests.People
{
    public class SpeechComponentTests
    {
        [Fact]
        public void NewSpeechComponentTest()
        {
            Speech component = new Speech();
            Assert.Equal("I handle Speech for ", component.Description);
            Assert.Equal("Speech", component.Name);
        }

        [Fact]
        public void GetDetailsTest()
        {
            Speech component = new Speech();
            var answer = component.GetDetails();
            Assert.Equal(4, answer.Length);
        }
    }
}