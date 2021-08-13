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
            $"\"{CurrentSpokenText},\" {_pronoun} says in a {ToneOfVoice.Current}. {VoiceDescription}. {BodyLanguage()}";

        public void GetNextSpeech(bool shifty)
        {
            ToneOfVoice.GetNextString(shifty);
            Stance.GetNextString(shifty);
            Posture.GetNextString(shifty);
            ArmPositions.GetNextString(shifty);
            EyePosition.GetNextString(shifty);
        }
        
        // private void GetNewToneOfVoice(bool shifty)
        // {
        //     var tones = new List<string>()
        //     {
        //         $"{_pronounPossessive} voice remains a steady volume throughout.",
        //         $"{_pronoun} pause slightly before speaking.",
        //         $"{_pronoun} say, after a slight pause.",
        //         $"{_pronounPossessive} words come out with no hesitation.",
        //         $"{_pronoun} speak in a hushed tone.",
        //         $"{_pronoun} say in a flat monotone",
        //         $"{_pronoun} say with little vocal inflection",
        //         $"{_pronoun} blurt out, somewhat loudly"
        //     };
        //
        //     if (shifty)
        //     {
        //         tones.Add($"{_pronounPossessive} voice pitches slightly higher.");
        //         tones.Add($"{_pronoun} say as {_pronounPossessive} voice cracks.");
        //         tones.Add($"{_pronoun} say after a considerable pause.");
        //         tones.Add($"{_pronoun} say in a measured monotone.");
        //         tones.Add($"{_pronoun} say, voice carrying a tinge of sincerity.");
        //     }
        //     else
        //     {
        //         tones.Add($"{_pronoun} raise {_pronounPossessive} voice and {_pronounPossessive} tone becomes pointed.");
        //         tones.Add($"{_pronoun} gasps the words out hurriedly.");
        //         tones.Add($"{_pronoun} speaks in a very low tone.");
        //         tones.Add($"{_pronoun} sound annoyed.");
        //         tones.Add($"{_pronounPossessive} voice carries a tinge of sincerity.");
        //     }
        //
        //     CurrentToneOfVoice = tones.RandomItem();
        // }
        //
        // private void GetNewArmPosition(bool shifty)
        // {
        //     var positions = new List<string>();
        //     positions.Add($"{_pronounPossessive} arms cross over {_pronounPossessive} chest");
        //     positions.Add($"{_pronoun} put {_pronounPossessive} hands in {_pronounPossessive} pockets");
        //     positions.Add($"{_pronoun} put {_pronounPossessive} hands on {_pronounPossessive} hips");
        //     positions.Add($"{_pronoun} put {_pronounPossessive} hands in {_pronounPossessive} pockets");
        //     positions.Add($"{_pronoun} scratch {_pronounPossessive} neck.");
        //     
        //     if (shifty)
        //     {
        //         positions.Add($"{_pronounPossessive} hands fidget in {_pronounPossessive} pockets");    
        //         positions.Add($"{_pronounPossessive} hands tremble slightly");    
        //         positions.Add($"A vein in {_pronounPossessive} forearm pulses and bulges");    
        //     }
        //     else
        //     {    
        //         positions.Add($"{_pronounPossessive} hands stop moving entirely");
        //         positions.Add($"{_pronounPossessive} hands shake uncontrollably");
        //         positions.Add($"{_pronoun} make exaggerated motions with {_pronounPossessive} hands");
        //     }
        //
        //     CurrentArmPosition = positions.RandomItem();
        // }
        //
        // private void GetNewEyeMovements(bool shifty)
        // {
        //     var movements = new List<string>();
        //     movements.Add($"{_pronounPossessive} eyes have a faraway look");
        //     movements.Add($"{_pronounPossessive} eyes focus squarely on me");
        //     movements.Add($"{_pronounPossessive} eyes scan {_pronounPossessive} surroundings");
        //     movements.Add($"{_pronounPossessive} eyes are focused on something behind me");
        //     movements.Add($"{_pronoun} makes direct eye contact");
        //     movements.Add($"{_pronoun} focuses her eyes on the bridge of my nose");
        //     movements.Add($"{_pronoun} focuses her eyes directly on mine");
        //     
        //     if (shifty)
        //     {
        //         movements.Add($"{_pronounPossessive} eyes dart about quickly");                
        //         movements.Add($"{_pronoun} avoids meeting your gaze");                
        //         movements.Add($"{_pronoun} averts {_pronounPossessive} eyes");                
        //     }
        //     else
        //     {
        //         movements.Add($"{_pronounPossessive} eyes flicker");                
        //         movements.Add($"{_pronoun} maintains fierce eye contact");                
        //         movements.Add($"{_pronoun} keeps my gaze intensely");
        //     }
        //
        //     CurrentEyeMovements = movements.RandomItem();
        // }
        //
        //
        // public void GetNewStance(bool shifty)
        // {
        //     var stances = new List<string>();
        //     stances.Add($"{_pronoun} stands with {_pronounPossessive} feet shoulder width apart");
        //     stances.Add($"{_pronoun} stands with {_pronounPossessive} feet squared to {_pronounPossessive} shoulders");
        //     stances.Add($"{_pronoun} carries {_pronounPossessive} weight on {_pronounPossessive} left leg with {_pronounPossessive} right leg cocked out");
        //     stances.Add($"{_pronoun} carries {_pronounPossessive} weight on {_pronounPossessive} left leg with {_pronounPossessive} right leg at an angle");
        //     stances.Add($"{_pronoun} carries {_pronounPossessive} weight on {_pronounPossessive} right leg with {_pronounPossessive} left leg cocked out");
        //     stances.Add($"{_pronoun} carries {_pronounPossessive} weight on {_pronounPossessive} right leg with {_pronounPossessive} left leg at an angle");
        //     stances.Add($"{_pronoun} leans on {_pronounPossessive} left leg");
        //     stances.Add($"{_pronoun} leans on {_pronounPossessive} right leg");
        //
        //     if (shifty)
        //     {
        //         stances.Add($"{_pronoun} hyper-extends {_pronounPossessive} knees");
        //         stances.Add($"{_pronoun} shifts weight from {_pronounPossessive} left to {_pronounPossessive} right leg");
        //         stances.Add($"{_pronoun} shifts weight from {_pronounPossessive} right to {_pronounPossessive} left leg");
        //         stances.Add($"{_pronoun} shifts weight from one leg to another");
        //         stances.Add($"{_pronoun} shifts weight onto {_pronounPossessive} heels");
        //     }
        //     else
        //     {
        //         stances.Add($"{_pronoun} is tapping {_pronounPossessive} foot");
        //         stances.Add($"{_pronoun} squares {_pronounPossessive} feet towards me");
        //         stances.Add($"{_pronoun} rocks {_pronounPossessive} weight onto the balls of {_pronounPossessive} feet");
        //     }
        //
        //     CurrentStance = stances.RandomItem();
        // }
        // public void GetNewPosture(bool shifty)
        // {
        //     var stances = new List<string>();
        //     stances.Add($"{_pronoun} hunches {_pronounPossessive} shoulders slightly");
        //     stances.Add($"{_pronoun} has the slightest hunch");
        //     stances.Add($"{_pronoun} slouches {_pronounPossessive} head");
        //     stances.Add($"{_pronoun} raises {_pronounPossessive} head as {_pronoun} speaks");
        //     stances.Add($"{_pronoun} lowers {_pronounPossessive} head as {_pronoun} speaks");
        //     stances.Add($"{_pronoun} untilts {_pronounPossessive} head");
        //     stances.Add($"{_pronoun} faces straight ahead");
        //
        //     if (shifty)
        //     {
        //         stances.Add($"{_pronoun} turns {_pronounPossessive} head away");
        //         stances.Add($"{_pronoun} tilts {_pronounPossessive} head down");
        //         stances.Add($"{_pronoun} hunches {_pronounPossessive} greatly");
        //         stances.Add($"{_pronoun} cocks {_pronounPossessive} head ever so slightly");
        //     }
        //     else
        //     {
        //         stances.Add($"{_pronoun} holds {_pronounPossessive} up high");
        //         stances.Add($"{_pronoun} turns {_pronounPossessive} head to face me");
        //         stances.Add($"{_pronoun} straightens the slouch out of {_pronounPossessive} neck");
        //         stances.Add($"{_pronoun} straightens {_pronounPossessive} neck");
        //     }
        //
        //     CurrentPosture = stances.RandomItem();
        // }

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