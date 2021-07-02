using System.Collections.Generic;
using GoRogue;
using HomicideDetective.UserInterface;

namespace HomicideDetective.People
{
    public class BodyLanguage : IPrintable
    {
        public string CurrentStance { get; private set; } = "";
        public string CurrentPosture { get; private set; } = "";
        public string CurrentArmPosition { get; private set; } = "";
        public string CurrentEyeMovements { get; private set; } = "";

        private string _pronoun { get; set; }
        private string _pronounPossessive { get; set; }
        public BodyLanguage(string pronoun = "they", string pronounPossessive = "their")
        {
            _pronoun = pronoun;
            _pronounPossessive = pronounPossessive;
        }

        public void ApplyPronouns(string pronoun, string possessive)
        {
            _pronoun = pronoun;
            _pronounPossessive = possessive;
        }
        public string GetPrintableString()
            => $"{CurrentStance}. {CurrentPosture}. {CurrentArmPosition}. {CurrentEyeMovements}. ";
        
        public void NextBodyLanguage(bool shifty)
        {
            GetNewStance(shifty);
            GetNewPosture(shifty);
            GetNewArmPosition(shifty);
            GetNewEyeMovements(shifty);
        }

        private void GetNewArmPosition(bool shifty)
        {
            var positions = new List<string>();
            positions.Add($"{_pronounPossessive} arms cross over {_pronounPossessive} chest");
            positions.Add($"{_pronoun} put {_pronounPossessive} hands in {_pronounPossessive} pockets");
            positions.Add($"{_pronoun} put {_pronounPossessive} hands on {_pronounPossessive} hips");
            positions.Add($"{_pronoun} put {_pronounPossessive} hands in {_pronounPossessive} pockets");
            positions.Add($"{_pronoun} scratch {_pronounPossessive} neck.");
            
            if (shifty)
            {
                positions.Add($"{_pronounPossessive} hands fidget in {_pronounPossessive} pockets");    
                positions.Add($"{_pronounPossessive} hands tremble slightly");    
                positions.Add($"A vein in {_pronounPossessive} forearm pulses and bulges");    
            }
            else
            {    
                positions.Add($"{_pronounPossessive} hands stop moving entirely");
                positions.Add($"{_pronounPossessive} hands shake uncontrollably");
                positions.Add($"{_pronoun} make exaggerated motions with {_pronounPossessive} hands");
            }

            CurrentArmPosition = positions.RandomItem();
        }

        private void GetNewEyeMovements(bool shifty)
        {
            var movements = new List<string>();
            movements.Add($"{_pronounPossessive} eyes have a faraway look");
            movements.Add($"{_pronounPossessive} eyes focus squarely on me");
            movements.Add($"{_pronounPossessive} eyes scan {_pronounPossessive} surroundings");
            movements.Add($"{_pronounPossessive} eyes are focused on something behind me");
            movements.Add($"{_pronoun} makes direct eye contact");
            movements.Add($"{_pronoun} focuses her eyes on the bridge of my nose");
            movements.Add($"{_pronoun} focuses her eyes directly on mine");
            
            if (shifty)
            {
                movements.Add($"{_pronounPossessive} eyes dart about quickly");                
                movements.Add($"{_pronoun} avoids meeting your gaze");                
                movements.Add($"{_pronoun} averts {_pronounPossessive} eyes");                
            }
            else
            {
                movements.Add($"{_pronounPossessive} eyes flicker");                
                movements.Add($"{_pronoun} maintains fierce eye contact");                
                movements.Add($"{_pronoun} keeps my gaze intensely");
            }

            CurrentEyeMovements = movements.RandomItem();
        }


        public void GetNewStance(bool shifty)
        {
            var stances = new List<string>();
            stances.Add($"{_pronoun} stands with {_pronounPossessive} feet shoulder width apart");
            stances.Add($"{_pronoun} stands with {_pronounPossessive} feet squared to their shoulders");
            stances.Add($"{_pronoun} carries {_pronounPossessive} weight on {_pronounPossessive} left leg with {_pronounPossessive} right leg cocked out");
            stances.Add($"{_pronoun} carries {_pronounPossessive} weight on {_pronounPossessive} left leg with {_pronounPossessive} right leg at an angle");
            stances.Add($"{_pronoun} carries {_pronounPossessive} weight on {_pronounPossessive} right leg with {_pronounPossessive} left leg cocked out");
            stances.Add($"{_pronoun} carries {_pronounPossessive} weight on {_pronounPossessive} right leg with {_pronounPossessive} left leg at an angle");
            stances.Add($"{_pronoun} leans on {_pronounPossessive} left leg");
            stances.Add($"{_pronoun} leans on {_pronounPossessive} right leg");

            if (shifty)
            {
                stances.Add($"{_pronoun} hyperextends {_pronounPossessive} knees");
                stances.Add($"{_pronoun} shifts weight from {_pronounPossessive} left to {_pronounPossessive} right leg");
                stances.Add($"{_pronoun} shifts weight from {_pronounPossessive} right to {_pronounPossessive} left leg");
                stances.Add($"{_pronoun} shifts weight from one leg to another");
                stances.Add($"{_pronoun} shifts weight onto {_pronounPossessive} heels");
            }
            else
            {
                stances.Add($"{_pronoun} is tapping {_pronounPossessive} foot");
                stances.Add($"{_pronoun} squares {_pronounPossessive} feet towards me");
                stances.Add($"{_pronoun} rocks {_pronounPossessive} weight onto the balls of {_pronounPossessive} feet");
            }

            CurrentStance = stances.RandomItem();
        }
        public void GetNewPosture(bool shifty)
        {
            var stances = new List<string>();
            stances.Add($"{_pronoun} hunches {_pronounPossessive} shoulders slightly");
            stances.Add($"{_pronoun} has the slightest hunch");
            stances.Add($"{_pronoun} slouches {_pronounPossessive} their head");
            stances.Add($"{_pronoun} raises {_pronounPossessive} head as {_pronoun} speaks");
            stances.Add($"{_pronoun} lowers {_pronounPossessive} head as {_pronoun} speaks");
            stances.Add($"{_pronoun} untilts {_pronounPossessive} head");
            stances.Add($"{_pronoun} faces straight ahead");

            if (shifty)
            {
                stances.Add($"{_pronoun} turns {_pronounPossessive} head away");
                stances.Add($"{_pronoun} tilts {_pronounPossessive} head down");
                stances.Add($"{_pronoun} hunches {_pronounPossessive} greatly");
                stances.Add($"{_pronoun} cocks {_pronounPossessive} head ever so slightly");
            }
            else
            {
                stances.Add($"{_pronoun} holds {_pronounPossessive} up high");
                stances.Add($"{_pronoun} turns {_pronounPossessive} head to face me");
                stances.Add($"{_pronoun} straightens the slouch out of {_pronounPossessive} neck");
                stances.Add($"{_pronoun} straightens {_pronounPossessive} neck");
            }

            CurrentPosture = stances.RandomItem();
        }
    }
}