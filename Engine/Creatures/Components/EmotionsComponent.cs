// using SadConsole;
//
// namespace Engine.Creatures.Components
// {
//     public class EmotionsComponent : ComponentBase
//     {
//         #region emotions
//         //here, they are values that go from 0 (feeling as bad as can be) to 100 (feeling barely more extremely than humanly possible)
//         //people start to let their emotions rule their decision making around 20% feeling level (Less than 30 or more than 70)
//         //the coldest, most logical people will let their emotions start leading their decisions around 80% feeling level (less than 10 or more than 90)
//         public int Angry;
//         public int Aroused;
//         public int Bored;
//         public int Confused;
//         public int Envy;
//         public int Fear;
//         public int Grief;
//         public int Guilt;
//         public int Humiliated;
//         public int Hurt;
//         public int Jealous;
//         public int Joy;
//         public int Lonely;
//         public int Love;
//         public int Nervous;
//         public int Pride;
//         public int Panic;
//         public int Relief;
//         public int Sad;
//         public int Shame;
//         public int Sympathy;
//         #endregion
//
//         #region desires
//         //todo: figure out how to implement desires...
//         /*
//          * SpendTimeWithFriend
//          * SpendTimeWithRomanticInterest
//          * FulfillAddiction
//          * 
//          * 
//          * 
//          * 
//          * Collection of Activities thingsToDo?
//          * 
//          * 
//          * 
//          */
//         #endregion
//         public EmotionsComponent(BasicEntity parent) : base(isUpdate: true, isKeyboard: false, isDraw: true, isMouse: false)
//         {
//             Parent = parent;
//             Name = "Emotions";
//             Angry = 50;
//             Aroused = 50;
//             Bored = 50;
//             Confused = 50;
//             Envy = 50;
//             Fear = 50;
//             Grief = 50;
//             Guilt = 50;
//             Humiliated = 50;
//             Hurt = 50;
//             Jealous = 50;
//             Joy = 50;
//             Lonely = 50;
//             Love = 50;
//             Nervous = 50;
//             Pride = 50;
//             Panic = 50;
//             Relief = 50;
//             Sad = 50;
//             Shame = 50;
//             Sympathy = 50;
//         }
//
//         public override string[] GetDetails()
//         {
//             string[] answer =
//             {
//                 "this is a speech component.",
//                 "The entity with this component can speak.",
//                 "\nAnger: " + Angry,
//                 "\nAroused: " + Aroused,
//                 "\nBored: " + Bored,
//                 "\nConfused: " + Confused,
//                 "\nEnvy: " + Envy,
//                 "\nFear: " + Fear,
//                 "\nGrief: " + Grief,
//                 "\nGuilt: " + Guilt,
//                 "\nHumiliated: " + Humiliated,
//                 "\nHurt: " + Hurt,
//                 "\nJealous: " + Jealous,
//                 "\nJoy: " + Joy,
//                 "\nLonely: " + Lonely,
//                 "\nLove: " + Love,
//                 "\nNervous: " + Nervous,
//                 "\nPride: " + Pride,
//                 "\nPanic: " + Panic,
//                 "\nReleif: " + Relief,
//                 "\nSad: " + Sad,
//                 "\nShame: " + Shame,
//                 "\nSympath: " + Sympathy,
//         };
//             return answer;//
//         }
//
//         public override void ProcessTimeUnit()
//         {
//             //todo...
//         }
//     }
// }
