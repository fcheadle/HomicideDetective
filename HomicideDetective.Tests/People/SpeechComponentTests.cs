using HomicideDetective.People.Components;
using Xunit;

namespace HomicideDetective.Tests
{
    public class SpeechComponentTests
    {
        [Fact]
        public void NewSpeechComponentTest()
        {
            SpeechComponent component = new SpeechComponent();
            Assert.Equal("I handle Speech.", component.Description);
            Assert.Equal("Speech Component", component.Name);
        }

        [Fact]
        public void GetDetailsTest()
        {
            SpeechComponent component = new SpeechComponent();
            var answer = component.GetDetails();
            Assert.Equal(4, answer.Length);
        }
    }
}