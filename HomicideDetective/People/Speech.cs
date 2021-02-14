using System;
using GoRogue.GameFramework;
using GoRogue.GameFramework.Components;
using HomicideDetective.Mysteries;
using SadRogue.Primitives;
using TheSadRogue.Integration.Components;

namespace HomicideDetective.People
{
    public class Speech : IGameObjectComponent, IDetailed
    {
        public IGameObject? Parent { get; set; }
        public string Name { get; }
        public string Description { get; }
        public Speech()
        {
            Name = $"{Parent}'s Voice";
            Description = GenerateVoiceDescription();
        }

        public string[] GetDetails() => new[]
            {GenerateSpokenText(), GenerateToneOfVoice()};
        
        private string GenerateVoiceDescription()
        {
            int chance = new Random().Next(0, 101);
            string voice = "Their voice is ";
            voice +=
                chance % 15 == 0 ? "a deep bass" :
                chance % 15 == 1 ? "a rumbling bass" :
                chance % 15 == 2 ? "a raspy bass" :
                chance % 15 == 3 ? "a soft bass" :
                chance % 15 == 4 ? "a deep baritone" :
                chance % 15 == 5 ? "a rumbling baritone" :
                chance % 15 == 6 ? "a soft baritone" :
                chance % 15 == 7 ? "a clear baritone" :
                chance % 15 == 8 ? "a pleasant alto" :
                chance % 15 == 9 ? "a gruff alto" :
                chance % 15 == 10 ? "a clear alto" :
                chance % 15 == 11 ? "a nasally alto" :
                chance % 15 == 12 ? "a fabulous soprano" :
                chance % 15 == 13 ? "a nasally soprano" : "a crystal-clear tenor";
            voice += ".";
            return voice;
        }
        
        private string GenerateBodyLanguage()
        {
            int chance = new Random().Next(0, 100);
            string bodyLang = "Posture-wise, they ";
            bodyLang += 
                chance % 15 == 0 ? "are breathing heavily, arms somewhat out to the sides" :
                chance % 15 == 1 ? "are very reserved, hands crossed in front of them" :
                chance % 15 == 2 ? "scratch their neck, then put a hand in a pocket" :
                chance % 15 == 3 ? "hang their head ever so slightly lower" :
                chance % 15 == 4 ? "stand with their legs wide and hands up" :
                chance % 15 == 5 ? "speak emphatically with their hands" :
                chance % 15 == 6 ? "lean slightly back on one leg and cross their arms" : 
                chance % 15 == 7 ? "crack their neck in a subtle motion" :
                chance % 15 == 8 ? "put their hands in their pockets" :
                chance % 15 == 9 ? "scratch their neck feverishly" :
                chance % 15 == 10 ? "wipes something from their eye" :
                chance % 15 == 11 ? "picks their ear" :
                chance % 15 == 12 ? "speaks louder with their hands" :
                chance % 15 == 13 ? "lean against a wall" : "stand tall, with head held high";
            bodyLang += ". ";
            return bodyLang;
        }

        private string GenerateFacialExpression()
        {
            int chance = new Random().Next(0, 100);
            string expression = 
                chance % 15 == 0 ? "with furrowed brow and narrowed eyes" :
                chance % 15 == 1 ? "with furrowed brow" :
                chance % 15 == 2 ? "with one eyebrow raised" :
                chance % 15 == 3 ? "eyebrows conveying a look of concern" :
                chance % 15 == 4 ? "with narrowed eyes" :
                chance % 15 == 5 ? "shifting their eyes about" :
                chance % 15 == 6 ? "looking down" :
                chance % 15 == 7 ? "their eyes flicker up and to the left before answering" :
                chance % 15 == 8 ? "batting their lashes" :
                chance % 15 == 9 ? "through a non-smiling wink" :
                chance % 15 == 10 ? "with a smile" :
                chance % 15 == 11 ? "through a forced smile" :
                chance % 15 == 11 ? "beaming their teeth" :
                chance % 15 == 12 ? "staring fiercely" :
                chance % 15 == 13 ? "with intense eye contact." : "as their pupils narrow.";
            return expression;
        }

        private string GenerateToneOfVoice()
        {
            int chance = new Random().Next(0, 100);
            string tone = "They say in a ";
            tone +=
                chance % 15 == 0 ? "neutral" :
                chance % 15 == 1 ? "slightly higher-pitched" :
                chance % 15 == 2 ? "pointed" :
                chance % 15 == 3 ? "low and commanding":
                chance % 15 == 4 ? "highly distressed" :
                chance % 15 == 5 ? "calm and measured" :
                chance % 15 == 6 ? "frantic" :
                chance % 15 == 7 ? "panicked" :
                chance % 15 == 8 ? "hushed" :
                chance % 15 == 9 ? "cheerful" :
                chance % 15 == 10 ? "playful" :
                chance % 15 == 11 ? "forlorn" :
                chance % 15 == 11 ? "pained" :
                chance % 15 == 12 ? "cracking" :
                chance % 15 == 13 ? "with intense eye contact." : "as their pupils narrow.";
            tone += " tone, ";
            return tone;
        }

        private string GenerateSpokenText()
        {
            int chance = new Random().Next(100);
            string spoken = "\"";
            spoken +=
                chance % 15 == 0 ? "Good day." :
                chance % 15 == 1 ? "Good afternoon, Detective." :
                chance % 15 == 2 ? "Hello, Detective." :
                chance % 15 == 3 ? "Finally off your ass and on the case, eh?":
                chance % 15 == 4 ? "I don't remember anything new since the last time we spoke." :
                chance % 15 == 5 ? "I- I'm sorry. I need a few moments to collect myself." :
                chance % 15 == 6 ? "I have to tell someone what I've seen, the guilt is just too much" :
                chance % 15 == 7 ? "I didn't know the victim." :
                chance % 15 == 8 ? "Oh yeah, I know the victim." :
                chance % 15 == 9 ? "Too bad about your current case. I really liked the victim." :
                chance % 15 == 10 ? "Who?" :
                chance % 15 == 11 ? "... Why does that matter?" :
                chance % 15 == 11 ? "I don't want to talk to you anymore." :
                chance % 15 == 12 ? "I don't anything about the case, and I'm certainly not going to know anything when I go to Lou's Diner after I get off work." :
                chance % 15 == 13 ? "Any new leads?" : "How's the mystery coming along?";
            spoken += "\"";
            return spoken;
        }

        public string[] AllDetails() 
            => new[] {GenerateSpokenText(), GenerateToneOfVoice(), Description, GenerateFacialExpression(), GenerateBodyLanguage()};

        
        public string[] Talk(Direction direction)
        {
            var entities = Parent!.CurrentMap!.Entities.GetItemsAt(Parent!.Position + direction);
            foreach (var entity in entities)
            {
                if (entity is Person person)
                {
                    var speech = person.AllComponents.GetFirst<Speech>();
                    var thoughts = ((Person)Parent).AllComponents.GetFirst<Thoughts>();
                    thoughts.Think(speech.GetDetails());
                    return speech.GetDetails();
                }
            }


            return new[] { "" };
        }
    }
}