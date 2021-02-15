using HomicideDetective.People;
using Xunit;

namespace HomicideDetective.Tests.People
{
    public class SpeechTests
    {
        [Fact]
        public void NewSpeechComponentTest()
        {
            Speech component = new Speech();
            Assert.Contains("Their voice is ", component.Description);
            Assert.Equal("'s Voice", component.Name);//because it has no owner
        }

        [Fact]
        public void GetDetailsTest()
        {
            Speech component = new Speech();
            var answer = component.Details;
            Assert.Equal(5, answer.Length);
        }
    }
}