using System.Collections.Generic;
using GoRogue;
using GoRogue.Components.ParentAware;
using SadRogue.Integration;

namespace HomicideDetective.People
{
    /// <summary>
    /// The speech component given to RogueLikeEntities who can speak.
    /// </summary>
    /// <remarks>
    /// Requires a RogueLikeEntity Parent who has a Substantive and Thoughts components
    /// </remarks>
    public class Speech : ParentAwareComponentBase<RogueLikeEntity>, IPrintable
    {
        private string _pronoun;
        private string _pronounPossessive;
        public string VoiceDescription { get; }
        public string CurrentSpokenText { get; set; }
        public BoolDependantStringList Posture { get; set; }
        public BoolDependantStringList Stance { get; set; }
        public BoolDependantStringList ArmPositions { get; set; }
        public BoolDependantStringList EyePosition { get; set; }
        public BoolDependantStringList ToneOfVoice { get; set; }
        public BoolDependantStringList SpokenText { get; set; }
        
        public Speech(string pronoun = "they", string pronounPossessive = "their")
        {
            VoiceDescription = GenerateVoiceDescription();
            _pronoun = pronoun;
            _pronounPossessive = pronounPossessive;
            CurrentSpokenText = "Hmm";
            Posture = new BoolDependantStringList();
            Stance= new BoolDependantStringList();
            ArmPositions= new BoolDependantStringList();
            EyePosition= new BoolDependantStringList();
            ToneOfVoice= new BoolDependantStringList();
            SpokenText= new BoolDependantStringList();
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
        public void ApplyPronouns(string pronoun, string possessive)
        {
            _pronoun = pronoun;
            _pronounPossessive = possessive;
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
        
        private string GenerateVoiceDescription()
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
            return $"Their voice is {timbres.RandomItem()} {tones.RandomItem()}.";
        }
    }
}