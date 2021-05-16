using HomicideDetective.People;
using Xunit;

namespace HomicideDetective.Tests.People
{
    public class SpeechTests
    {
        [Fact]
        public void NewSpeechComponentTest()
        {
            var speech = new Speech();
            Assert.Contains("Their voice is ", speech.Description);
            Assert.Equal(5, speech.Details.Count);
        }
        
        //todo - theorize tests talking to someone
        //todo - test initialization of sayings
        //todo - test initialization of facial expressions
        //todo - test initialization of body language
        //todo - test initialization of tones of voice
    }
}