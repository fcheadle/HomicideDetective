using System;
using System.Collections.Generic;
using GoRogue;
using GoRogue.GameFramework;
using GoRogue.GameFramework.Components;
using HomicideDetective.Mysteries;
using SadRogue.Integration;

namespace HomicideDetective.People
{
    /// <summary>
    /// The speech component given to RogueLikeEntities who can speak.
    /// </summary>
    public class Speech : IGameObjectComponent//, IDetailed
    {
        public IGameObject? Parent { get; set; }
        public string Description { get; private set; }

        public Speech()
        {
            InitLies();
            InitTruth();
            InitCommon();
            Description = GenerateVoiceDescription();
        }

        private void InitLies()
        {
            LieSayings.Add("I was busy");
            LieTones.Add(", voice slightly cracking");
            LieFacialExpressions.Add("eyes flutter upwards and to the left");
            LieBodyPosture.Add("slouches");
        }

        private void InitTruth()
        {
            TruthSayings.Add("I'm telling you, I don't know anything");
            TruthTones.Add("through an angry gasp");
            TruthFacialExpressions.Add("determination shines through their eyes");
            TruthBodyPosture.Add("holds his head up high");
        }

        private void InitCommon()
        {
            CommonSayings.Add("Hmm... Let me see...");
            CommonTones.Add("in a flat monotone");
            CommonFacialExpressions.Add("expression is blank");
            CommonBodyPosture.Add("stands up straight");
        }

        public string SpeakTo(bool lying = false)
        {
            var info = ((RogueLikeEntity) Parent!).AllComponents.GetFirst<Substantive>();
            var dialogue = $"\"{GenerateSpokenWords(lying)},\" ";
            dialogue += $"{info.Name} says {GenerateToneOfVoice(lying)}, ";
            dialogue += $"{info.PronounPossessive} {GenerateFacialExpression(lying)}. ";
            dialogue += $"{info.Pronoun} {GenerateBodyLanguage(lying)}. ";
            dialogue += Description;
            return dialogue;
        }
        
        //strings to pull details from when either telling the truth or lying
        public List<string> CommonSayings { get; private set; } = new List<string>();
        public List<string> CommonTones { get; private set; } = new List<string>();
        public List<string> CommonFacialExpressions { get; private set; } = new List<string>();
        public List<string> CommonBodyPosture { get; private set; } = new List<string>();
        public void AddCommonKnowledge(string knowledge) => CommonSayings.Add(knowledge);
        public void AddCommonPosture(string posture) => CommonBodyPosture.Add(posture);
        public void AddCommonFacialExpression(string expression) => CommonFacialExpressions.Add(expression);
        public void AddCommonTone(string tone) => CommonTones.Add(tone);
        
        
        //strings to pull details from when people are lying
        public List<string> LieSayings { get; private set; } = new List<string>();
        public List<string> LieTones { get; private set; } = new List<string>();
        public List<string> LieFacialExpressions { get; private set; } = new List<string>();
        public List<string> LieBodyPosture { get; private set; } = new List<string>();
        public void AddLie(string lie) => LieSayings.Add(lie);
        public void AddLiePosture(string lie) => LieBodyPosture.Add(lie);
        public void AddLieFacialExpression(string lie) => LieFacialExpressions.Add(lie);
        public void AddLieTone(string lie) => LieTones.Add(lie);
        
        
        //strings to pull details from when people are telling the truth
        public List<string> TruthSayings { get; private set; } = new List<string>();
        public List<string> TruthTones { get; private set; } = new List<string>();
        public List<string> TruthFacialExpressions { get; private set; } = new List<string>();
        public List<string> TruthBodyPosture { get; private set; } = new List<string>();
        public void AddTruth(string truth) => TruthSayings.Add(truth);
        public void AddTruthPosture(string truth) => TruthBodyPosture.Add(truth);
        public void AddTruthFacialExpression(string truth) => TruthFacialExpressions.Add(truth);
        public void AddTruthTone(string truth) => TruthTones.Add(truth);
        
        
        private string GenerateToneOfVoice(bool lying = false)
        {
            int chance = new Random().Next(100);
            if (lying)
                return chance > 50 ? LieTones.RandomItem() : CommonTones.RandomItem();
            else
                return chance > 50 ? TruthTones.RandomItem() : CommonTones.RandomItem();
        }
        
        private string GenerateSpokenWords(bool lying = false)
        {
            int chance = new Random().Next(100);
            if (lying) 
                return chance > 50 ? LieSayings.RandomItem() : CommonSayings.RandomItem();
            else
                return chance > 50 ? TruthSayings.RandomItem() : CommonSayings.RandomItem();
        }
        
        private string GenerateFacialExpression(bool lying = false)
        {
            int chance = new Random().Next(100);
            if (lying)
                return chance > 50 ? LieFacialExpressions.RandomItem() : CommonFacialExpressions.RandomItem();
            else
                return chance > 50 ? TruthFacialExpressions.RandomItem() : CommonFacialExpressions.RandomItem();
        }

        private string GenerateBodyLanguage(bool lying = false)
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