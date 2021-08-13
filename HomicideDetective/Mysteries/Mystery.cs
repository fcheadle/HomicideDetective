using System;
using System.Collections.Generic;
using System.Linq;
using HomicideDetective.People;
using HomicideDetective.Places;
using SadRogue.Integration;
using SadRogue.Integration.Maps;
#pragma warning disable 8618

namespace HomicideDetective.Mysteries
{
    /// <summary>
    /// Generates and Contains a Murderer, Victim, Time & Cause of Death, Witnesses, Clues, etc.
    /// </summary>
    /// <remarks>
    /// The Mystery class should be the only thing which instantiates most of the classes present.
    /// This is to keep the generation of everything in one place, and to guarantee that all instantiation
    /// and management are done by a singular class that is knowledgeable of everything.
    /// </remarks>
    public partial class Mystery
    {
        public enum Statuses {Active, Closed, Cold}
        public Statuses Status { get; set; } = Statuses.Active;
        public int Seed { get; set; }
        public int CaseNumber { get; set; }
        public RogueLikeEntity Victim { get; set; }
        public RogueLikeEntity Murderer { get; set; }
        public RogueLikeEntity MurderWeapon { get; set; }
        public Substantive SceneOfCrimeInfo { get; set; }
        public Place SceneOfCrime { get; set; }
        public RogueLikeMap CurrentLocation => LocationsOfInterest[_currentMapIndex];
        public List<RogueLikeMap> LocationsOfInterest { get; set; }
        public DateTime TimeOfDeath { get; set; }
        public List<RogueLikeEntity> Witnesses { get; set; }
        private int _currentMapIndex;
        public Random Random { get; set; }

        private int _mapWidth;
        private int _mapHeight;
        private int _viewWidth;
        private int _viewHeight;

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
        
        public void Generate(int mapWidth, int mapHeight, int viewWidth, int viewHeight)
        {
            SetDimensions(mapWidth, mapHeight, viewWidth, viewHeight);
            LocationsOfInterest = new();
            Victim = GenerateVictimEntity();
            Murderer = GenerateMurdererEntity();
            MurderWeapon = GenerateMurderWeapon();
            Witnesses = GenerateWitnessEntities().ToList();
            SceneOfCrimeInfo = GenerateSceneOfMurderInfo();
            GenerateTimeline();
            GenerateLocationsOfInterest();
            PlacePeopleOnMaps();
        }

        private void SetDimensions(int mapWidth, int mapHeight, int viewWidth, int viewHeight)
        {
            _mapWidth = mapWidth;
            _mapHeight = mapHeight;
            _viewWidth = viewWidth;
            _viewHeight = viewHeight;
        }
    }
}