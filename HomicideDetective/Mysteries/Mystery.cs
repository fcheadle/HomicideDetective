using System;
using System.Collections.Generic;
using System.Linq;
using SadRogue.Integration;

namespace HomicideDetective.Mysteries
{
    /// <summary>
    /// Generates and Contains a Murderer, Victim, Time & Cause of Death, Witnesses, Clues, etc.
    /// </summary>
    public partial class Mystery
    {
        public enum Statuses {Active, Closed, Cold}
        public Statuses Status { get; set; } = Statuses.Active;
        public int Seed { get; set; }
        public int CaseNumber { get; set; }
        public RogueLikeEntity Victim { get; set; }
        public RogueLikeEntity Murderer { get; set; }
        public RogueLikeEntity MurderWeapon { get; set; }
        public Substantive SceneOfCrime { get; set; }
        public DateTime TimeOfDeath { get; set; }
        public List<RogueLikeEntity> Witnesses { get; set; }
        
        public Random Random { get; set; }

        //todo - json names
        string[] _maleGivenNames = { "Nate", "Tom", "Dick", "Harry", "Bob", "Matthew", "Mark", "Luke", "John", "Josh" };
        string[] _femaleGivenNames = { "Alice", "Betty", "Jesse", "Sarah", "Angela", "Christine",  "Mary", "Liz", "Joan", "Jen" };
        string[] _surnames = {"Smith", "Johnson", "Michaels", "Douglas", "Andrews", "MacDonald", "Jenkins", "Peterson"};

        public Mystery(int seed, int caseNumber)
        {
            Seed = seed;
            CaseNumber = caseNumber;
            Random = new Random(Seed + CaseNumber);
        }
        
        public void Generate()
        {
            Victim = GenerateVictim();
            Murderer = GenerateMurderer();
            MurderWeapon = GenerateMurderWeapon();
            Witnesses = GenerateWitnesses().ToList();
            SceneOfCrime = GenerateSceneOfMurderInfo();
            GenerateTimeline();
        }
    }
}