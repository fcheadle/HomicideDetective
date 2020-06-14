using Engine.Mathematics;
using GoRogue;
using SadConsole;

namespace Engine.Components.Creature
{
    public class SpeechComponent : Component
    {
        internal Console Dialogue;
        internal readonly Font Voice;
        public string Saying;
        public string BodyLanguage;
        //abusive tendencies - aka ways to gaslight
        public int TendencyToMinimize;    // m
        public int TendencyToInvalidate;  // i
        public int TendencyToDeny;        // d
        public int TendencyToJustify;     // j
        public int TendencyToArgue;       // a
        public int TendencyToDefend;      // d
        public int TendencyToExplain;     // e

        public SpeechComponent(BasicEntity parent) : base(isUpdate: true, isKeyboard: false, isDraw: true, isMouse: false)
        {
            Parent = parent;
            Name = "Speech";
            TendencyToMinimize = Calculate.PercentValue();    // m
            TendencyToInvalidate = Calculate.PercentValue();  // i
            TendencyToDeny = Calculate.PercentValue();        // d
            TendencyToJustify = Calculate.PercentValue();     // j
            TendencyToArgue = Calculate.PercentValue();       // a
            TendencyToDefend = Calculate.PercentValue();      // d
            TendencyToExplain = Calculate.PercentValue();     // e
        }

        //for now, just print something random to the screen
        //PrintDialogue("Hello there!");
        private void PrintDialogue(string message)
        {
            Dialogue = new Console(message.Length, 2/*, Voice*/);//(Voice, message, );
            Dialogue.Position = new Coord(Parent.Position.X - message.Length / 2, Parent.Position.Y - 1);
            Dialogue.Print(0, 0, message);
        }

        public override string[] GetDetails()
        {
            string[] answer =
            {
                "this is a speech component.",
                "The entity with this component can speak.",
                "\nTendencyToMinimize: " + TendencyToMinimize ,
                "\nTendencyToInvalidate: " + TendencyToInvalidate ,
                "\nTendencyToDeny: " + TendencyToDeny,
                "\nTendencyToJustify: " + TendencyToJustify,
                "\nTendencyToArgue: " + TendencyToArgue,
                "\nTendencyToDefend: " + TendencyToDefend,
                "\nTendencyToExplain: " + TendencyToExplain,
                "\n", Saying, "\n",
                BodyLanguage,
            };
            return answer;//
        }

        public override void ProcessTimeUnit()
        {
            //todo: fade in or out
        }


        public void Talk()
        {
            switch (Calculate.PercentValue())
            {
                case 0:  Saying = "Ah, it is you."; break;
                case 1:  Saying = "Hello again, detective."; break;
                case 2:  Saying = "Hello, how are you?"; break;
                case 3:  Saying = "Hiya"; break;
                case 4:  Saying = "Ah, it is you."; break;
                case 5:  Saying = "Hello again, detective."; break;
                case 6:  Saying = "Good to see you again."; break;
                case 7:  Saying = "Have you found anything in the case yet?"; break;
                case 8:  Saying = "Any leads in your case yet, detective?"; break;
                case 9:  Saying = "I already told you everything I know"; break;
                case 10: Saying = "Detective, I sure would like to see you again some time."; break;
                case 11: Saying = "Let's go get a drink, Detective."; break;
                case 12: Saying = "I just want my daughter back, Detective."; break;
                case 13: Saying = "Good day, detective."; break;
                case 14: Saying = "Well, well."; break;
                case 15: Saying = "If it isn't you."; break;
                case 16: Saying = "Ah, Detective. How is the case going?"; break;
                case 17: Saying = "Do you have any leads regarding my sister?"; break;
                case 18: Saying = "What leads have you gathered?"; break;
                case 19: Saying = "What are you even doing?"; break;
                case 20: Saying = "Detective, please get away from me."; break;
                case 21: Saying = "What have you been up to lately?"; break;
                case 22: Saying = "You shouldn't be here"; break;
                case 23: Saying = "I was just thinking of you, detective."; break;
                case 24: Saying = "I saw something the other day that made me think of you."; break;
                case 25: Saying = "I will never forgive you for falsely acusing my cousin."; break;
                case 26: Saying = "You falsely arrested my friend! I hate you!"; break;
                case 27: Saying = "My sister's killer is alive, behind bars... We wanted him dead."; break;
                case 28: Saying = "I'll never forget how you brought my sister's killer to justice, detective."; break;
                case 29: Saying = "Uh, hi there!"; break;
                case 30: Saying = "I've already told you everything I know."; break;
                case 31: Saying = "Beat it, I ain't talking to you."; break;
                case 32: Saying = "Why?! Why would someone do this to my daughter?"; break;
                case 33: Saying = "If you ever find, and that's a mighty big 'if', she'll be dead."; break;
                case 34: Saying = "I don't think you're ever going to find her body."; break;
                case 35: Saying = "Hey. You didn't hear this from me, but I'm about to tell you something."; break;
                case 36: Saying = "I heard about the case you're working."; break;
                case 37: Saying = "The postman knows everyone in town."; break;
                case 38: Saying = "Oh, it's you. Hi"; break;
                case 39: Saying = "I used to do what you do."; break;
                case 40: Saying = "Er, hello!"; break;
                case 41: Saying = "Yes, yes, yes."; break;
                case 42: Saying = "*hiccup* hello!"; break;
                case 43: Saying = "N-No..."; break;
                case 44: Saying = "It was dark. I hit my head. That's all I remember."; break;
                case 45: Saying = "Er, no, I don't think so."; break;
                case 46: Saying = "Haha... What?"; break;
                case 47: Saying = "You brought closure to these old bones, detective."; break;
                case 48: Saying = "I don't think you're ever going to find her body, detective."; break;
                case 49: Saying = "Again? But I've already talked to you!"; break;
                case 50: Saying = "I think we've spoken enough."; break;
                case 51: Saying = "Get out of my face!"; break;
                case 52: Saying = "Who are you to question me, in my own house?!"; break;
                case 53: Saying = "How did you get here?"; break;
                case 54: Saying = "What's new with you?"; break;
                case 55: Saying = "Hi, I don't believe we've met before."; break;
                case 56: Saying = "Never forget this feeling, detective."; break;
                case 57: Saying = "I don't have to take this from you."; break;
                case 58: Saying = "Just, please leave."; break;
                case 59: Saying = "Ah come in, come in."; break;
                case 60: Saying = "Do I know you?"; break;
                case 61: Saying = "And how do you know me?"; break;
                case 62: Saying = "Thank you, detective."; break;
                case 63: Saying = "So this is what justice feels like."; break;
                case 64: Saying = "I find revenge quite to my liking."; break;
                case 65: Saying = "Catch me already, I cannot stop myself."; break;
                case 66: Saying = "Well, yes I was there when the police were searching."; break;
                case 67: Saying = "A lot of us were on lawn chairs watching the whole thing."; break;
                case 68: Saying = "I grow weary of you."; break;
                case 69: Saying = "You should keep your nose out of other peoples business."; break;
                case 70: Saying = "Stand back, I need to mourn."; break;
                case 71: Saying = "I shouldn't blame you. You're just the messenger."; break;
                case 72: Saying = "Don't forget: Midnight. Behind the Burger Barn."; break;
                case 73: Saying = "Hey, I think you dropped something."; break;
                case 74: Saying = "Sh! Meet me round back. Then, we'll talk."; break;
                case 75: Saying = "But I thought I cleaned everything up."; break;
                case 76: Saying = "Hang justice, I want revenge!"; break;
                case 77: Saying = "I thought I would get to hug her again."; break;
                case 78: Saying = "Forgive me, I just need to be alone."; break;
                case 79: Saying = "Holy hell. I didn't see that coming."; break;
                case 80: Saying = "Did you find her journal?"; break;
                case 81: Saying = "May I see the evidence against me?"; break;
                case 82: Saying = "I always hated the cold."; break;
                case 83: Saying = "I despise hot, dry weather."; break;
                case 84: Saying = "I can still see her smile when I close my eyes."; break;
                case 85: Saying = "Have you heard about the neighbor girl?"; break;
                case 86: Saying = "I don't get it. How could anyone do that?"; break;
                case 87: Saying = "*sob* My baby girl!"; break;
                case 88: Saying = "I'm not surprised. Mark my words, her boyfriend, is a drunk."; break;
                case 89: Saying = "I'm not gonna miss the nightly yelling match."; break;
                case 90: Saying = "I heard that guy has an insurance policy on the kids."; break;
                case 91: Saying = "I would just love to get my hands on the killer's neck..."; break;
                case 92: Saying = "I hope that she rests in peace, wherever she is..."; break;
                case 93: Saying = "Did you know her personally, detective?"; break;
                case 94: Saying = "If you find her, tell her I love her."; break;
                case 95: Saying = "Let's see.. on that night... (what was I doing?)"; break;
                case 96: Saying = "Sorry, I was at my niece's piano recital then."; break;
                case 97: Saying = "I was at a basketball game that night."; break;
                case 98: Saying = "I was with my friends... they went away together."; break;
                case 99: Saying = "*sob* I admit it! I did it!"; break;
            }

            switch (Calculate.PercentValue())
            {
                case 0:  BodyLanguage = "He stares blankly."; break;
                case 1:  BodyLanguage = "He takes on a blank expression."; break;
                case 2:  BodyLanguage = "His face turned red as he said that."; break;
                case 3:  BodyLanguage = "He says as he winks."; break;
                case 4:  BodyLanguage = "His eyes flutter."; break;
                case 5:  BodyLanguage = "He raises his eyebrows."; break;
                case 6:  BodyLanguage = "He raises an eyebrow."; break;
                case 7:  BodyLanguage = "His ear twitches."; break;
                case 8:  BodyLanguage = "He strokes his chin."; break;
                case 9:  BodyLanguage = "His pupils dilate."; break;
                case 10: BodyLanguage = "He hides a smirk."; break;
                case 11: BodyLanguage = "He sweats along his brow."; break;
                case 12: BodyLanguage = "His eyes begin to water."; break;
                case 13: BodyLanguage = "His lip quivers."; break;
                case 14: BodyLanguage = "His pupils are incredibly narrow."; break;
                case 15: BodyLanguage = "He furrows his brow, as in deep anger."; break;
                case 16: BodyLanguage = "There is a fire in his eyes."; break;
                case 17: BodyLanguage = "He believes what he's saying."; break;
                case 18: BodyLanguage = "He stands up straight."; break;
                case 19: BodyLanguage = "Maintains a rigid posture."; break;
                case 20: BodyLanguage = "His form cowers under the weight of the questions."; break;
                case 21: BodyLanguage = "He is shaking like an earthquake."; break;
                case 22: BodyLanguage = "His breathing becomes quicker."; break;
                case 23: BodyLanguage = "His breathing becomes slower."; break;
                case 24: BodyLanguage = "He takes a deep breath."; break;
                case 25: BodyLanguage = "He says while cringing."; break;
                case 26: BodyLanguage = "He bites his tongue, as though wanting to say more."; break;
                case 27: BodyLanguage = "He looks at you like he hates you."; break;
                case 28: BodyLanguage = "He says, through a look that shows both sorrow and relief."; break;
                case 29: BodyLanguage = "He looks as though he might sneeze."; break;
                case 30: BodyLanguage = "He looks like he has more to say."; break;
                case 31: BodyLanguage = "He bites his bottom lips when he quiets."; break;
                case 32: BodyLanguage = "He's eyeing you very closely."; break;
                case 33: BodyLanguage = "You notice a blood stain on his undershirt"; break;
                case 34: BodyLanguage = "He just made himself laugh pretty hard."; break;
                case 35: BodyLanguage = "You think he expected laughter when he said that."; break;
                case 36: BodyLanguage = "He looks at you like he thinks you're an idiot."; break;
                case 37: BodyLanguage = "You could feel the pain in his voice when he said that."; break;
                case 38: BodyLanguage = "His face contorts into a fake smile."; break;
                case 39: BodyLanguage = "His eyes betray a hidden pain as he talks."; break;
                case 40: BodyLanguage = "He is trying to remain stoic."; break;
                case 41: BodyLanguage = "He appears apathetic."; break;
                case 42: BodyLanguage = "He says, unenthusiastically."; break;
                case 43: BodyLanguage = "The way he said that, you'd think he hated you."; break;
                case 44: BodyLanguage = "Pain causes him to wince when he talks."; break;
                case 45: BodyLanguage = "He fidgets with his cuff as he speaks."; break;
                case 46: BodyLanguage = "He is chewing the end of a pencil as he talks."; break;
                case 47: BodyLanguage = "He seems sincere when he says this."; break;
                case 48: BodyLanguage = "He raises his eyebrows to emphasize the last sentence."; break;
                case 49: BodyLanguage = "He nudges you with his elbow."; break;
                case 50: BodyLanguage = "He makes a subtle gesture with his hand."; break;
                case 51: BodyLanguage = "He sets his watch."; break;
                case 52: BodyLanguage = "He is chewing on something."; break;
                case 53: BodyLanguage = "He reaches his hand into his jacket pocket."; break;
                case 54: BodyLanguage = "He starts to fish for something in his left pants pocket."; break;
                case 55: BodyLanguage = "He appears genuinely confused."; break;
                case 56: BodyLanguage = "He seems to be trying hard to remain firm."; break;
                case 57: BodyLanguage = "He appears to be lost to grief."; break;
                case 58: BodyLanguage = "He is on the verge of tears."; break;
                case 59: BodyLanguage = "He snaps his fingers to some beat only he can hear."; break;
                case 60: BodyLanguage = "He keeps bringing his hands to his face."; break;
                case 61: BodyLanguage = "He shifts his weight from one leg to the other."; break;
                case 62: BodyLanguage = "He leans against the nearby wall."; break;
                case 63: BodyLanguage = "He looks down."; break;
                case 64: BodyLanguage = "He seems to have a hard time meeting your gaze."; break;
                case 65: BodyLanguage = "Eyecontact is not this guy's forte."; break;
                case 66: BodyLanguage = "He moves the hair from in front of his eyes."; break;
                case 67: BodyLanguage = "There is a fierce determining in his eyes."; break;
                case 68: BodyLanguage = "He sounds almost cheerful now."; break;
                case 69: BodyLanguage = "Clearly in anguish, he pretends to be fine."; break;
                case 70: BodyLanguage = "His body posture moved to a very closed-state."; break;
                case 71: BodyLanguage = "His body language becomes more open."; break;
                case 72: BodyLanguage = "His body stiffens as he says that."; break;
                case 73: BodyLanguage = "He seems to feel some relief when you say that."; break;
                case 74: BodyLanguage = "He becomes more limber."; break;
                case 75: BodyLanguage = "He glances upwards and to the right."; break;
                case 76: BodyLanguage = "He glances upwards and to the left."; break;
                case 77: BodyLanguage = "He glances downward and to the left."; break;
                case 78: BodyLanguage = "He glances downward and to the right."; break;
                case 79: BodyLanguage = "It's like he's looking through you."; break;
                case 80: BodyLanguage = "He brings his head to his hands."; break;
                case 81: BodyLanguage = "He swings his arms wildly."; break;
                case 82: BodyLanguage = "He flails about, out of control."; break;
                case 83: BodyLanguage = "You can feel the anger coming off him like waves of heat."; break;
                case 84: BodyLanguage = "You've never seen the look he is giving you before."; break;
                case 85: BodyLanguage = "He says, almost giddily"; break;
                case 86: BodyLanguage = "He blushes and looks away."; break;
                case 87: BodyLanguage = "He appears to have found his resolve again."; break;
                case 88: BodyLanguage = "His breath smells slightly of alcohol"; break;
                case 89: BodyLanguage = "He shouts at the top of his lungs."; break;
                case 90: BodyLanguage = "He finally looks like he regrets what he did."; break;
                case 91: BodyLanguage = "He brings his hand up to the back of his neck."; break;
                case 92: BodyLanguage = "He scratches his belly."; break;
                case 93: BodyLanguage = "He fiddles with his suspenders."; break;
                case 94: BodyLanguage = "He cleans his fingernails."; break;
                case 95: BodyLanguage = "He looks incredibly bored."; break;
                case 96: BodyLanguage = "He notices something across the room."; break;
                case 97: BodyLanguage = "He yawns as you speak."; break;
                case 98: BodyLanguage = "He furrows his brow as though worried."; break;
                case 99: BodyLanguage = "At least his tears seem convincing."; break;
            }

            PrintDialogue(Saying);
        }
    }
}
