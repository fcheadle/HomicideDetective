using System;
using System.Collections.Generic;
using GoRogue;
using GoRogue.GameFramework;
using GoRogue.GameFramework.Components;
using HomicideDetective.Mysteries;

namespace HomicideDetective.People
{
    public class Speech : IGameObjectComponent, IDetailed
    {
        public IGameObject? Parent { get; set; }
        public string Description { get; private set; }
        public List<string> Details => new ()
        {
            SpokenText(), 
            ToneOfVoice(), 
            FacialExpression(), 
            BodyLanguage(),
            Description
        };
        
        public List<string> CommonSayings { get; private set; } = new List<string>();
        public List<string> CommonTones { get; private set; } = new List<string>();
        public List<string> CommonFacialExpressions { get; private set; } = new List<string>();
        public List<string> CommonBodyPosture { get; private set; } = new List<string>();
        public List<string> TruthSayings { get; private set; } = new List<string>();
        public List<string> TruthTones { get; private set; } = new List<string>();
        public List<string> TruthFacialExpressions { get; private set; } = new List<string>();
        public List<string> TruthBodyPosture { get; private set; } = new List<string>();
        public List<string> LieSayings { get; private set; } = new List<string>();
        public List<string> LieTones { get; private set; } = new List<string>();
        public List<string> LieFacialExpressions { get; private set; } = new List<string>();
        public List<string> LieBodyPosture { get; private set; } = new List<string>();
        
        public Speech()
        {
            InitSayings();
            InitTones();
            InitFacialExpressions();
            InitBodyPostures();
            Description = GenerateVoiceDescription();
        }

        private void InitSayings()
        {
            CommonSayings.Add("Common: Good day, Detective.");
            TruthSayings.Add("Truth: I saw the victim the night before she died.");
            LieSayings.Add("Lie: I'm not sure I've ever met the victim.");
        }

        private void InitTones()
        {
            CommonTones.Add("Common: they say in a neutral tone");
            TruthTones.Add("Truth: they say in a low-pitched tone");
            LieTones.Add("Lie: they say in a high-pitched tone");
        }

        private void InitFacialExpressions()
        {
            CommonFacialExpressions.Add("Common: through a flat expression");
            CommonFacialExpressions.Add("Common: with furrowed brow and narrowed eyes");
            CommonFacialExpressions.Add("Common: with furrowed brow");
            CommonFacialExpressions.Add("Common: with one eyebrow raised");
            CommonFacialExpressions.Add("Common: looking down");
            CommonFacialExpressions.Add("Common: with a smile");
            CommonFacialExpressions.Add("Common: staring fiercely");
            TruthFacialExpressions.Add("Truth: while frowning");
            TruthFacialExpressions.Add("Truth: through a forced smile");
            TruthFacialExpressions.Add("Truth: staring fiercely");
            TruthFacialExpressions.Add("Truth: with their brows arched in anger");
            TruthFacialExpressions.Add("Truth: beaming their teeth");
            TruthFacialExpressions.Add("Truth: with intense eye contact");
            LieFacialExpressions.Add("Lie: with their brows arched in anger");
            LieFacialExpressions.Add("Lie: eyebrows conveying a look of concern");
            LieFacialExpressions.Add("Lie: with narrowed eyes");
            LieFacialExpressions.Add("Lie: shifting their eyes about");
            LieFacialExpressions.Add("Lie: their eyes flicker up and to the left before answering");
            LieFacialExpressions.Add("Lie: batting their lashes");
            LieFacialExpressions.Add("Lie: through a non-smiling wink");
            LieFacialExpressions.Add("Lie: with their brows arched in anger");
            LieFacialExpressions.Add("Lie: beaming their teeth");
            LieFacialExpressions.Add("Lie: as their pupils narrow");
        }

        private void InitBodyPostures()
        {
            CommonBodyPosture.Add("Common: their arms hang blankly to their sides");
            CommonBodyPosture.Add("Common: they are breathing heavily, arms somewhat out to the sides");
            CommonBodyPosture.Add("Common: they are very reserved, hands crossed in front of them");
            CommonBodyPosture.Add("Common: their arms hang blankly to their sides");
            CommonBodyPosture.Add("Common: they put their hands in their pockets");
            CommonBodyPosture.Add("Common: they pick their ear");
            CommonBodyPosture.Add("Common: they lean against a wall");
            
            TruthBodyPosture.Add("Truth: they cross their arms in front of their chest");
            TruthBodyPosture.Add("Truth: they speak emphatically with their hands");
            TruthBodyPosture.Add("Truth: they lean slightly back on one leg and cross their arms");
            TruthBodyPosture.Add("Truth: they crack their neck in a subtle motion");
            TruthBodyPosture.Add("Truth: they wipe something from their eye");
            
            LieBodyPosture.Add("Lie: they reach their hand in their pocket and fidget with something");
            LieBodyPosture.Add("Lie: they scratch their neck, then put a hand in a pocket");
            LieBodyPosture.Add("Lie: they hang their head ever so slightly lower");
            LieBodyPosture.Add("Lie: scratch their neck feverishly");
            LieBodyPosture.Add("Lie: they reach their hand in their pocket and fidget with something");
        }

        private string ToneOfVoice(bool lying = false)
        {
            int chance = new Random().Next(100);
            if (lying)
                return chance > 50 ? LieTones.RandomItem() : CommonTones.RandomItem();
            else
                return chance > 50 ? TruthTones.RandomItem() : CommonTones.RandomItem();
        }
        
        private string SpokenText(bool lying = false)
        {
            int chance = new Random().Next(100);
            if (lying)
                return chance > 50 ? LieSayings.RandomItem() : CommonSayings.RandomItem();
            else
                return chance > 50 ? TruthSayings.RandomItem() : CommonSayings.RandomItem();
        }
        
        private string FacialExpression(bool lying = false)
        {
            int chance = new Random().Next(100);
            if (lying)
                return chance > 50 ? LieFacialExpressions.RandomItem() : CommonFacialExpressions.RandomItem();
            else
                return chance > 50 ? TruthFacialExpressions.RandomItem() : CommonFacialExpressions.RandomItem();
        }

        private string BodyLanguage(bool lying = false)
        {
            int chance = new Random().Next(100);
            if (lying)
                return chance > 50 ? LieBodyPosture.RandomItem() : CommonBodyPosture.RandomItem();
            else
                return chance > 50 ? TruthBodyPosture.RandomItem() : CommonBodyPosture.RandomItem();
        }

        private string GenerateVoiceDescription()
        {
            List<string> timbres = new List<string>()
            {
                "rumbly", "raspy", "smooth", "shrill", "soft", "hard", "light", "gruff", "clear", "crystal-clear",
                "grating", "muddy", "aged", "youthly", "mature", "sagely", "rolling", "wisened", "bubbly", "enchanting",
                "bewitching", "lovely", "magnetic", "irritating", "poetic", "artistic", "flavorful", "repulsive", 
                "repugnant",
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