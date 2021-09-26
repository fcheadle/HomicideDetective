using System.Collections.Generic;
using HomicideDetective.People;
using HomicideDetective.People.Speech;
using HomicideDetective.Words;
using Xunit;

namespace HomicideDetective.Tests.People
{
    public class SpeechTests
    {
        private string _name = "Neil Armstrong";
        private string _description = "An out-of-this-world trumpet player and bicyclist";
        private Noun _noun = Constants.MaleNouns;
        private Pronoun _pronoun = Constants.FemalePronouns;
        private PhysicalProperties _properties = new(18, 24);

        private Personhood CreateTestEntity() => new (_name, _description, 26, Occupations.Professor, _properties, _noun, _pronoun);
        
        [Fact]
        public void NewSpeechComponentTest()
        {
            var speech = new SpeechComponent(_pronoun);
            Assert.Contains("her voice is ", speech.VoiceDescription);
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

        private void AssertBodyLanguageHasPronouns(SpeechComponent bl, string pronoun, string pronounPossessive)
        {
            Assert.True(bl.Posture.Current.Contains(pronoun) || bl.Posture.Current.Contains(pronounPossessive));
            Assert.True(bl.Stance.Current.Contains(pronoun) || bl.Stance.Current.Contains(pronounPossessive));
            Assert.True(bl.ArmPositions.Current.Contains(pronoun) || bl.ArmPositions.Current.Contains(pronounPossessive));
            Assert.True(bl.EyePosition.Current.Contains(pronoun) || bl.EyePosition.Current.Contains(pronounPossessive));
        }
        

        private void AssertBodyLanguageDoesNotHavePronouns(SpeechComponent bl, string pronoun, string pronounPossessive)
        {
            Assert.False(bl.Posture.Current.Contains(pronoun) || bl.Posture.Current.Contains(pronounPossessive));
            Assert.False(bl.Stance.Current.Contains(pronoun) || bl.Stance.Current.Contains(pronounPossessive));
            Assert.False(bl.ArmPositions.Current.Contains(pronoun) || bl.ArmPositions.Current.Contains(pronounPossessive));
            Assert.False(bl.EyePosition.Current.Contains(pronoun) || bl.EyePosition.Current.Contains(pronounPossessive));
        }
        
        [Fact]
        public void DefaultBodyLanguageTest()
        {
            var bl = CreateTestEntity();
            Assert.Equal("", bl.Speech.Posture.Current);
            Assert.Equal("", bl.Speech.Stance.Current);
            Assert.Equal("", bl.Speech.ArmPositions.Current);
            Assert.Equal("", bl.Speech.EyePosition.Current);
        }
    }
}