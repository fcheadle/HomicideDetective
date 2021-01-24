using System;
using SadRogue.Primitives;
using TheSadRogue.Integration.Components;

namespace HomicideDetective.People.Components
{
    public class SpeechComponent : RogueLikeComponentBase, IHaveDetails
    {
        public string Name { get; }
        public string Description { get; }
        public SpeechComponent() : base(false, false, false, false, 7)
        {
            Name = "Speech Component";
            Description = "I handle Speech.";
        }

        public string[] GetDetails()
        {
            int chance = new Random().Next(0, 100);

            string spoken = "\"";
            spoken +=
                chance % 5 == 0 ? "Good Afternoon, Detective." :
                chance % 5 == 1 ? "It's about time you showed up." : 
                chance % 5 == 2 ? "No, I don't remember anything new." : 
                chance % 5 == 3 ? "G-good afternoon..." : "Oh! Hello, detective.";
            spoken += "\"";
            
            string tone = "They say in a";
            tone +=
                chance % 6 == 0 ? "stark and to the point" :
                chance % 6 == 1 ? "higher pitched than usual" :
                chance % 6 == 2 ? "slow drawl" :
                chance % 6 == 3 ? "drunken slur" :
                chance % 6 == 4 ? "measured and even" : "steady, neutral";
            tone += "tone, ";
            
            string expression = 
                chance % 7 == 0 ? "with furrowed brow and narrowed eyes." :
                chance % 7 == 1 ? "with one eyebrow raised." :
                chance % 7 == 2 ? "as their eyes shift about." :
                chance % 7 == 3 ? "looking up and to the left before answering." :
                chance % 7 == 4 ? "briefly flashing open their eyes, then returning to neutral position." :
                chance % 7 == 5 ? "with a fierce intensity of eye contact." : "as their pupils narrow.";

            string bodyLang = "Posture-wise, they";
            bodyLang += 
                chance % 8 == 0 ? "are breathing heavily, arms somewhat out to the sides" :
                chance % 8 == 1 ? "are very reserved, hands crossed in front of them" :
                chance % 8 == 2 ? "scratch their neck, then put a hand in a pocket" :
                chance % 8 == 3 ? "hang their head ever so slightly lower" :
                chance % 8 == 4 ? "stand with their legs wide and hands up" :
                chance % 8 == 5 ? "speak emphatically with their hands" :
                chance % 8 == 6 ? "lean slightly back on one leg and cross their arms" : "stand  tall, with head held high";
            bodyLang += ".";
            
            if (Parent is Person parent)
                return new[] { parent.Name, parent.Description, spoken, tone, expression, bodyLang };
            
            else
                return new[] { spoken, tone, expression, bodyLang };
        }

        public string[] Talk(Direction direction)
        {
            var entities = Parent!.CurrentMap!.Entities.GetItemsAt(Parent!.Position + direction);
            foreach (var entity in entities)
            {
                if (entity is Person person)
                {
                    var speech = person.AllComponents.GetFirst<SpeechComponent>();
                    var thoughts = ((Person)Parent).AllComponents.GetFirst<ThoughtComponent>();
                    thoughts.Think(speech.GetDetails());
                    return speech.GetDetails();
                }
            }


            return new[] { "" };
        }

        public Action TalkLeft() => () => Talk(Direction.Left);
        public Action TalkRight() => () => Talk(Direction.Right);
        public Action TalkUp() => () => Talk(Direction.Up);
        public Action TalkDown() => () => Talk(Direction.Down);
    }
}