using System;
using System.Collections.Generic;
using System.Linq;
using GoRogue;
using GoRogue.Components.ParentAware;
using HomicideDetective.UserInterface;
using SadRogue.Integration;

namespace HomicideDetective.People
{
    /// <summary>
    /// The speech component given to RogueLikeEntities who can speak.
    /// </summary>
    /// <remarks>
    /// Requires a RogueLikeEntity Parent who has a Substantive and Thoughts components
    /// </remarks>
    public class Voice : ParentAwareComponentBase<RogueLikeEntity>, IPrintable
    {
        public string Description { get; private set; }
        public string CurrentToneOfVoice { get; private set; } = "";
        public string CurrentSpokenText { get; set; }
        
        public Voice()
        {
            Description = GenerateVoiceDescription();
        }

        public static List<string> CommonTones { get; } = new List<string>()
        {
            "Their voice remains a steady volume throughout.",
            "They pause slightly before speaking.",
            "They say, after a slight pause.",
            "Their words come out with no hesitation.",
            "They speak in a hushed tone.",
            "They say in a flat monotone",
            "They say with little vocal inflection",
            "They blurt out, somewhat loudly"
        };
        public static List<string> LieTones { get; } = new List<string>()
        {
            "Their voice pitches slightly higher.",
            "They say as their voice cracks.",
            "They say after a considerable pause.",
            "They say in a measured monotone.",
            "They say, voice carrying a tinge of sincerity."
        };
        public static List<string> TruthTones { get; } = new List<string>()
        {
            "They raise their voice and their tone becomes pointed.",
            "They gasp the words out hurriedly.",
            "They speak in a very low tone.",
            "They sound annoyed.",
            "Their voice carries a tinge of sincerity."
        };

        public string GetPrintableString() => Description;
        private string GenerateToneOfVoice(bool lying = false)
        {
            int chance = new Random().Next(100);
            if (lying)
                return chance > 50 ? LieTones.RandomItem() : CommonTones.RandomItem();
            else
                return chance > 50 ? TruthTones.RandomItem() : CommonTones.RandomItem();
        }

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

        public string TalkAbout(Memory randomMemory)
        {
            CurrentSpokenText = randomMemory.GetPrintableString();
            CurrentToneOfVoice = GenerateToneOfVoice(randomMemory.Private);
            return CurrentSpokenText;
        }
    }
}