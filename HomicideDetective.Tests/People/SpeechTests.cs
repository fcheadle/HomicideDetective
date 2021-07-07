using System.Collections.Generic;
using HomicideDetective.People;
using Xunit;

namespace HomicideDetective.Tests.People
{
    public class SpeechTests
    {
        private string _name = "Neil Armstrong";
        private string _description = "An out-of-this-world trumpet player and bicyclist";
        private string _noun = "man";
        private string _pronoun = "he";
        private string _pronounPossessive = "his";
        
        private Person CreateTestEntity() => new Person(_name, _description, _noun, _pronoun, _pronounPossessive);
        
        [Fact]
        public void NewSpeechComponentTest()
        {
            var speech = new Speech();
            Assert.Contains("Their voice is ", speech.VoiceDescription);
        }

        [Fact]
        public void NewPersonHasSpeechComponentTest()
        {
            var e = CreateTestEntity();
            Assert.NotNull(e.Speech);
        }
        public static readonly IEnumerable<object[]> Pronouns = new List<object[]>()
        {
            new object[] {"he", "his"},
            new object[] {"she", "hers"},
            new object[] {"zhim", "zhey"},
            new object[] {"kwan", "kwiz"},
            new object[] {"zorn", "zort"},
            new object[] {"quall", "quill"},
        };

        private void AssertBodyLanguageHasPronouns(Speech bl, string pronoun, string pronounPossessive)
        {
            Assert.True(bl.CurrentPosture.Contains(pronoun) || bl.CurrentPosture.Contains(pronounPossessive));
            Assert.True(bl.CurrentStance.Contains(pronoun) || bl.CurrentStance.Contains(pronounPossessive));
            Assert.True(bl.CurrentArmPosition.Contains(pronoun) || bl.CurrentArmPosition.Contains(pronounPossessive));
            Assert.True(bl.CurrentEyeMovements.Contains(pronoun) || bl.CurrentEyeMovements.Contains(pronounPossessive));
        }
        

        private void AssertBodyLanguageDoesNotHavePronouns(Speech bl, string pronoun, string pronounPossessive)
        {
            Assert.False(bl.CurrentPosture.Contains(pronoun) || bl.CurrentPosture.Contains(pronounPossessive));
            Assert.False(bl.CurrentStance.Contains(pronoun) || bl.CurrentStance.Contains(pronounPossessive));
            Assert.False(bl.CurrentArmPosition.Contains(pronoun) || bl.CurrentArmPosition.Contains(pronounPossessive));
            Assert.False(bl.CurrentEyeMovements.Contains(pronoun) || bl.CurrentEyeMovements.Contains(pronounPossessive));
        }
        
        [Fact]
        public void DefaultBodyLanguageTest()
        {
            var bl = new Speech();
            Assert.Equal("", bl.CurrentPosture);
            Assert.Equal("", bl.CurrentStance);
            Assert.Equal("", bl.CurrentArmPosition);
            Assert.Equal("", bl.CurrentEyeMovements);
            
            bl.GetNextSpeech(true);
            AssertBodyLanguageHasPronouns(bl, "they", "their");
            
            bl.GetNextSpeech(false);
            AssertBodyLanguageHasPronouns(bl, "they", "their");
        }

        [Theory]
        [MemberData(nameof(Pronouns))]
        public void ApplyPronounsTest(string pronoun, string pronounPossessive)
        {
            var bl = new Speech();
            bl.ApplyPronouns(pronoun, pronounPossessive);
            
            bl.GetNextSpeech(true);
            AssertBodyLanguageDoesNotHavePronouns(bl, "they", "their");
            AssertBodyLanguageHasPronouns(bl, pronoun, pronounPossessive);
            
            bl.GetNextSpeech(false);
            AssertBodyLanguageDoesNotHavePronouns(bl, "they", "their");
            AssertBodyLanguageHasPronouns(bl, pronoun, pronounPossessive);
        }

        [Theory]
        [MemberData(nameof(Pronouns))]
        public void WithPronounsTest(string pronoun, string pronounPossessive)
        {
            var bl = new Speech(pronoun, pronounPossessive);
            
            bl.GetNextSpeech(true);
            AssertBodyLanguageDoesNotHavePronouns(bl, "they", "their");
            AssertBodyLanguageHasPronouns(bl, pronoun, pronounPossessive);
            
            bl.GetNextSpeech(false);
            AssertBodyLanguageDoesNotHavePronouns(bl, "they", "their");
            AssertBodyLanguageHasPronouns(bl, pronoun, pronounPossessive);
        }
    }
}