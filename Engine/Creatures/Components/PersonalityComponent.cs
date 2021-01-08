// using Engine.Utilities.Mathematics;
// using SadConsole;
//
// namespace Engine.Creatures.Components
// {
//     public class PersonalityComponent : ComponentBase
//     {
//         //personality traits
//         //PercentValue values of affecting any given decision
//         public int Passion;
//         public int Ambition;
//         public int Courage;
//         public int Creativity;
//         public int Empathy;
//         public int Adventurousness;
//         public int Spirituality;
//         public int Laziness;
//         public int Jealousness;
//         public int Lustfulness;
//         public int Greediness;
//         public int ProclivityToAnger;
//         public int Pridefulness;
//         public int Sadism;
//         public int NeedToControl;
//
//         //values - these start at 50 and will morph depending on experiences, in the future.
//         //so random for now
//         public int ImportanceOfFamily;
//         public int ImportanceOfFriendship;
//         public int ImportanceOfBodilyHealth;
//         public int ImportanceOfWealth;
//         public int ImportanceOfReligion;
//         public int WorkEthic;
//         public int AttentionToDetail;
//
//         //reactions to abuse - these start at 0 and increment over time
//         public int AbuseReactionFear = 0;        // f
//         public int AbuseReactionObligation = 0;  // o
//         public int AbuseReactionGuilt = 0;       // g
//
//         //the "bad" one :(
//         public int ProbabilityOfAbuseAffectingPersonalityScores = 0;
//
//         public PersonalityComponent(BasicEntity parent)
//             : base(isUpdate: false, isKeyboard: false, isDraw: false, isMouse: false)
//         {
//             Parent = parent;
//             Name = "Personality";
//             Passion = Calculate.PercentValue();
//             Ambition = Calculate.PercentValue();
//             Courage = Calculate.PercentValue();
//             Creativity = Calculate.PercentValue();
//             Empathy = Calculate.PercentValue();
//             Adventurousness = Calculate.PercentValue();
//             Spirituality = Calculate.PercentValue();
//             Laziness = Calculate.PercentValue();
//             Jealousness = Calculate.PercentValue();
//             Lustfulness = Calculate.PercentValue();
//             Greediness = Calculate.PercentValue();
//             ProclivityToAnger = Calculate.PercentValue();
//             Pridefulness = Calculate.PercentValue();
//             Sadism = Calculate.PercentValue();
//             NeedToControl = Calculate.PercentValue();
//
//
//             ImportanceOfFamily = Calculate.PercentValue();
//             ImportanceOfFriendship = Calculate.PercentValue();
//             ImportanceOfBodilyHealth = Calculate.PercentValue();
//             ImportanceOfWealth = Calculate.PercentValue();
//             ImportanceOfReligion = Calculate.PercentValue();
//             WorkEthic = Calculate.PercentValue();
//             AttentionToDetail = Calculate.PercentValue();
//
//         }
//
//         //public override void Update(SadConsole.Console console, TimeSpan delta)
//         //{
//         //    base.Update(console, delta);
//         //}
//
//         public override string[] GetDetails()
//         {
//             string[] answer =
//             {
//                 "\nPassion: " + Passion,
//                 "\nAmbition: " + Ambition,
//                 "\nCourage: " + Courage,
//                 "\nCreativity: " + Creativity,
//                 "\nEmpathy: " + Empathy,
//                 "\nAdventurousness: " + Adventurousness ,
//                 "\nSpirituality: " + Spirituality ,
//                 "\nLaziness: " + Laziness ,
//                 "\nJealousness: " + Jealousness ,
//                 "\nLustfulness: " + Lustfulness ,
//                 "\nGreediness: " + Greediness ,
//                 "\nProclivityToAnger " + ProclivityToAnger ,
//                 "\nPridefulness: " + Pridefulness ,
//                 "\nSadism: " + Sadism ,
//                 "\nNeedToControl: " + NeedToControl ,
//                 "\nImportanceOfFamily: " + ImportanceOfFamily,
//                 "\nImportanceOfFriendship: " + ImportanceOfFriendship,
//                 "\nImportanceOfBodilyHealth: " + ImportanceOfBodilyHealth,
//                 "\nImportanceOfWealth: " + ImportanceOfWealth ,
//                 "\nImportanceOfReligion: " + ImportanceOfReligion,
//                 "\nWorkEthic: " + WorkEthic ,
//                 "\nAttentionToDetail: " + AttentionToDetail ,
//             };
//             return answer;
//         }
//
//         public override void ProcessTimeUnit()
//         {
//
//         }
//     }
// }
