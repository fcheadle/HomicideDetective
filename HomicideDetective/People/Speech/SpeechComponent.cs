using System.Collections.Generic;
using GoRogue;
using GoRogue.Components.ParentAware;
using HomicideDetective.Words;
using SadRogue.Integration;

namespace HomicideDetective.People.Speech
{
    /// <summary>
    /// The speech component given to RogueLikeEntities who can speak.
    /// </summary>
    /// <remarks>
    /// Requires a RogueLikeEntity Parent who has a Substantive and Thoughts components
    /// </remarks>
    public class SpeechComponent : ParentAwareComponentBase<RogueLikeEntity>, IPrintable
    {
        private Pronoun _pronouns;
        public string VoiceDescription { get; }
        public string CurrentSpokenText { get; set; }
        public SpeechStringCollection Posture { get; set; }
        public SpeechStringCollection Stance { get; set; }
        public SpeechStringCollection ArmPositions { get; set; }
        public SpeechStringCollection EyePosition { get; set; }
        public SpeechStringCollection ToneOfVoice { get; set; }
        public SpeechStringCollection SpokenText { get; set; }
        
        public SpeechComponent(Pronoun pronouns)
        {
            VoiceDescription = GenerateVoiceDescription(pronouns.Possessive);
            _pronouns = pronouns;
            CurrentSpokenText = "Hmm";
            Posture = new SpeechStringCollection();
            Stance= new SpeechStringCollection();
            ArmPositions= new SpeechStringCollection();
            EyePosition= new SpeechStringCollection();
            ToneOfVoice= new SpeechStringCollection();
            SpokenText= new SpeechStringCollection();
        }

        public void SpeakText(string text)
        {
            CurrentSpokenText = text;
        }

        public void SpeakText(string text, bool shifty)
        {
            GetNextSpeech(shifty);
            CurrentSpokenText = text;
        }
        public void SpeakAbout(Memory memory)
        {
            CurrentSpokenText = memory.GetPrintableString();
        }

        public string GetPrintableString() =>
            $"\"{CurrentSpokenText},\" {ToneOfVoice.Current}. {VoiceDescription}. {BodyLanguage()}";

        public void GetNextSpeech(bool shifty)
        {
            ToneOfVoice.GetNextString(shifty);
            Stance.GetNextString(shifty);
            Posture.GetNextString(shifty);
            ArmPositions.GetNextString(shifty);
            EyePosition.GetNextString(shifty);
        }

        public string BodyLanguage()
            => $"{Stance.Current}. {Posture.Current}. {ArmPositions.Current}. {EyePosition.Current}.";
        
        private string GenerateVoiceDescription(string possessivePronoun)
        {
            List<string> timbres = new List<string>()
            {
                "a rumbly", "a raspy", "a smooth", "a shrill", "a soft", "a hard", "a light", "a gruff", "a clear", "a crystal-clear",
                "a grating", "a muddy", "an aged", "a youthly", "a mature", "a sagely", "a rolling", "a wisened", "a bubbly", "an enchanting",
                "a bewitching", "a lovely", "a magnetic", "an irritating", "a poetic", "an artistic", "a flavorful", "a repulsive", 
                "a repugnant",
            };
            List<string> tones = new List<string>()
            {
                "ultra-bass", "bass", "baritone", "alto", "tenor", "countertenor", "soprano", "falsetto",
                "coloratura soprano", "mezzo-soprano", "contralto"
            };
            return $"{possessivePronoun} voice is {timbres.RandomItem()} {tones.RandomItem()}.";
        }
    }
}