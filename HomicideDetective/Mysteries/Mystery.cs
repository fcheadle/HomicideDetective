using System;
using System.Collections.Generic;
using System.Linq;
using GoRogue;
using SadRogue.Integration;

namespace HomicideDetective.Mysteries
{
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
        public List<RogueLikeEntity> Witnesses { get; set; }
        //public List<RogueLikeEntity> LocationsOfInterest { get; set; }
        // public List<Substantive> Evidence { get; set; }
        
        public Random Random { get; set; }

        string[] _maleGivenNames = { "Nate", "Tom", "Dick", "Harry", "Bob", "Matthew", "Mark", "Luke", "John", "Josh" };

        string[] _femaleGivenNames = { "Alice", "Betty", "Jesse", "Sarah", "Angela", "Christine",  "Mary", "Liz", "Joan", "Jen" };

        string[] _surnames = {"Smith", "Johnson", "Michaels", "Douglas", "Andrews", "MacDonald", "Jenkins", "Peterson"};

        //preferred constructor
        public Mystery(int seed, int caseNumber)
        {
            Seed = seed;
            CaseNumber = caseNumber;
            Random = new Random(Seed + CaseNumber);
        }

        private int Chance() => Random.Next(0, 101);

        
        public void Generate()
        {
            Victim = GenerateVictim();
            Murderer = GenerateMurderer();
            MurderWeapon = GenerateMurderWeapon();
            Witnesses = GenerateWitnesses().ToList();
            SceneOfCrime = GenerateSceneOfMurderInfo();
        }
    }
}