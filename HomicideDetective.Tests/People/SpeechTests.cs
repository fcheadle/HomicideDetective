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
        }

        [Fact]
        public void SpeakToTest()
        {
            var speech = SetUpComponent();

            for (int i = 0; i < 15; i++)
                Assert.DoesNotContain("lie", speech.SpeakTo(false));
        }

        private Speech SetUpComponent()
        {
            var speech = new Speech();
            speech.AddTruth("true-saying");
            speech.AddLie("lie-saying");
            speech.AddCommonKnowledge("common");
            
            speech.AddTruthFacialExpression("true-face");
            speech.AddLieFacialExpression("lie-face");
            speech.AddCommonFacialExpression("common-face");
            
            speech.AddTruthPosture("true-posture");
            speech.AddLiePosture("lie-posture");
            speech.AddCommonPosture("common-posture");

            speech.AddTruthTone("true-tone");
            speech.AddLieTone("lie-tone");
            speech.AddCommonTone("common-tone");

            return speech;
        }
    }
}