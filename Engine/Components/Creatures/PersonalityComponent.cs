using SadConsole;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Components.Creature
{
    public class PersonalityComponent : Component
    {
        //personality traits
        //percent values of affecting any given decision
        public int Passion;
        public int Ambition;
        public int Courage;
        public int Creativity;
        public int Empathy;
        public int Adventurousness;
        public int Spirituality;
        public int Laziness;
        public int Jealousness;
        public int Lustfulness;
        public int Greediness;
        public int ProclivityToAnger;
        public int Pridefulness;
        public int Sadism;
        public int NeedToControl;

        //values - these start at 50 and will morph depending on experiences, in the future.
        //so random for now
        public int ImportanceOfFamily;
        public int ImportanceOfFriendship;
        public int ImportanceOfBodilyHealth;
        public int ImportanceOfWealth;
        public int ImportanceOfReligion;
        public int WorkEthic;
        public int AttentionToDetail;

        //reactions to abuse - these start at 0 and increment over time
        public int AbuseReactionFear = 0;        // f
        public int AbuseReactionObligation = 0;  // o
        public int AbuseReactionGuilt = 0;       // g

        //the "bad" one :(
        public int ProbabilityOfAbuseAffectingPersonalityScores = 0;

        public PersonalityComponent(BasicEntity parent)
            : base(isUpdate: false, isKeyboard: false, isDraw: false, isMouse: false)
        {
            Parent = parent;
            Passion = Calculate.Percent();
            Ambition = Calculate.Percent();
            Courage = Calculate.Percent();
            Creativity = Calculate.Percent();
            Empathy = Calculate.Percent();
            Adventurousness = Calculate.Percent();
            Spirituality = Calculate.Percent();
            Laziness = Calculate.Percent();
            Jealousness = Calculate.Percent();
            Lustfulness = Calculate.Percent();
            Greediness = Calculate.Percent();
            ProclivityToAnger = Calculate.Percent();
            Pridefulness = Calculate.Percent();
            Sadism = Calculate.Percent();
            NeedToControl = Calculate.Percent();


            ImportanceOfFamily = Calculate.Percent();
            ImportanceOfFriendship = Calculate.Percent();
            ImportanceOfBodilyHealth = Calculate.Percent();
            ImportanceOfWealth = Calculate.Percent();
            ImportanceOfReligion = Calculate.Percent();
            WorkEthic = Calculate.Percent();
            AttentionToDetail = Calculate.Percent();

        }

        //public override void Update(SadConsole.Console console, TimeSpan delta)
        //{
        //    base.Update(console, delta);
        //}

        public override string[] GetDetails()
        {
            string[] answer = 
            {
                "\nPassion: " + Passion,
                "\nAmbition: " + Ambition,
                "\nCourage: " + Courage,
                "\nCreativity: " + Creativity,
                "\nEmpathy: " + Empathy,
                "\nAdventurousness: " + Adventurousness ,
                "\nSpirituality: " + Spirituality ,
                "\nLaziness: " + Laziness ,
                "\nJealousness: " + Jealousness ,
                "\nLustfulness: " + Lustfulness ,
                "\nGreediness: " + Greediness ,
                "\nProclivityToAnger " + ProclivityToAnger ,
                "\nPridefulness: " + Pridefulness ,
                "\nSadism: " + Sadism ,
                "\nNeedToControl: " + NeedToControl ,
                "\nImportanceOfFamily: " + ImportanceOfFamily,
                "\nImportanceOfFriendship: " + ImportanceOfFriendship,
                "\nImportanceOfBodilyHealth: " + ImportanceOfBodilyHealth,
                "\nImportanceOfWealth: " + ImportanceOfWealth ,
                "\nImportanceOfReligion: " + ImportanceOfReligion,
                "\nWorkEthic: " + WorkEthic ,
                "\nAttentionToDetail: " + AttentionToDetail ,
            };
            return answer;
        }

        public override void ProcessTimeUnit()
        {

        }
    }
}
