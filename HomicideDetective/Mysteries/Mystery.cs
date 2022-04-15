using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using GoRogue;
using HomicideDetective.People;
using HomicideDetective.Places;
using HomicideDetective.Places.Generation;
using HomicideDetective.Things;
using HomicideDetective.Words;
using SadRogue.Integration;
using SadRogue.Integration.Maps;
using SadRogue.Primitives;
using SadRogue.Primitives.GridViews;

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
        public RogueLikeEntity? Victim { get; set; }
        public RogueLikeEntity? Murderer { get; set; }
        public RogueLikeEntity? MurderWeapon { get; set; }
        public Motives? Motive { get; set; }
        public Substantive? SceneOfCrimeInfo { get; set; }
        public Place? SceneOfCrime { get; set; }
        public RogueLikeMap? CurrentLocation => LocationsOfInterest![_currentMapIndex];
        //public List<RogueLikeMap>? LocationsOfInterest { get; set; }
        public DateTime? TimeOfDeath { get; set; }
        public List<RogueLikeEntity>? Witnesses { get; set; }
        private int _currentMapIndex;
        public Random Random { get; set; }

        private int _mapWidth;
        private int _mapHeight;
        private int _viewWidth;
        private int _viewHeight;

        public Mystery(int seed, int caseNumber)
        {
            Seed = seed;
            CaseNumber = caseNumber;
            Random = new Random(Seed + CaseNumber);
            LocationsOfInterest = new(_size, _size);
        }
        
        // public void Generate(int mapWidth, int mapHeight, int viewWidth, int viewHeight)
        // {
        //     SetDimensions(mapWidth, mapHeight, viewWidth, viewHeight);
        //     LocationsOfInterest = new();
        //     Victim = GenerateVictimEntity();
        //     Murderer = GenerateMurdererEntity();
        //     MurderWeapon = GenerateMurderWeapon();
        //     Witnesses = GenerateWitnessEntities().ToList();
        //     GenerateTimeline();
        //     GenerateLocationsOfInterest();
        //     SceneOfCrimeInfo = GenerateSceneOfMurderInfo();
        //     PlacePeopleOnMaps();
        // }

        public void SetDimensions(int mapWidth, int mapHeight, int viewWidth, int viewHeight)
        {
            _mapWidth = mapWidth;
            _mapHeight = mapHeight;
            _viewWidth = viewWidth;
            _viewHeight = viewHeight;
        }
        
        #region people
        /// <summary>
        /// Generates a RogueLikeEntity that is the victim in a murder investigation.
        /// </summary>
        /// <returns></returns>
        public RogueLikeEntity GenerateVictimEntity()
        {
            var victimInfo = _victim;//PersonFactory.GeneratePersonalInfo(Random.Next(), RandomItem(Constants.FamilyNames));
            var descr = new StringBuilder($"This is the body of {victimInfo.Name}. {victimInfo.Pronouns.Subjective} was a");
            descr.Append($"{victimInfo.Properties.GetPrintableString()} {victimInfo.Nouns.Singular}. ");
            descr.Append($"{victimInfo.Pronouns.Subjective} is bloated from gases building up in {victimInfo.Pronouns.Possessive} interior, ");
            descr.Append($"and discolored from decomposition.");
            
            var substantive = new Substantive(SubstantiveTypes.Person, victimInfo.Name, descr.ToString(), victimInfo.Nouns, victimInfo.Pronouns, victimInfo.Properties, null, "the");
            
            var victim = new RogueLikeEntity((0,0), 2, false);
            victim.AllComponents.Add(substantive, Constants.SubstantiveTag);
            
            //todo - decompose
            TimeOfDeath = new DateTime(1970, 7,4, Random.Next(0,24), Random.Next(0,60), 0);
            return victim;
        }

        /// <summary>
        /// Generates a RogueLikeEntity who is the murderer in a murder investigation.
        /// </summary>
        /// <returns></returns>
        public RogueLikeEntity GenerateMurdererEntity() => _murderer.Parent;
        
        /// <summary>
        /// Generates everyone related to the case: family members, friends, coworkers, etc.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<RogueLikeEntity> GenerateWitnessEntities()
        {
            foreach (var family in _families)
            {
                foreach (var person in family.Members)
                {
                    var entity = new RogueLikeEntity(default, 1, false);
                    entity.AllComponents.Add(person);
                    yield return entity;
                }
            }
        }
        #endregion
        #region places

        public void NextMap()
        {
            if (_currentMapIndex >= LocationsOfInterest.Count - 1)
                _currentMapIndex = 0;
            else
                _currentMapIndex++;
        }

        private void PlacePeopleOnMaps()
        {
            var map = LocationsOfInterest![0];
            SceneOfCrime = map.GoRogueComponents.GetFirst<Place>().SubAreas
                .First(sa => sa.Name == SceneOfCrimeInfo.Name);
            //SceneOfCrime = map.RandomRoom();
            var room = map.RandomRoom();
            
            //put the murder weapon on the map
            var murderWeapon = MurderWeapon;
            murderWeapon!.Position = map.RandomFreeSpace(room);
            map.AddEntity(murderWeapon);

            //add victim's corpse to the map
            var victim = Victim;
            victim!.Position = map.RandomFreeSpace(SceneOfCrime);
            map.AddEntity(victim);
            
            foreach (var witness in Witnesses!)
            {
                map = LocationsOfInterest.RandomItem();
                room = map.RandomRoom();
                witness.Position = map.RandomFreeSpace(room);
                
                map.AddEntity(witness);
            }
        }

        /// <summary>
        /// Generates the descriptive information about the scene of the crime.
        /// </summary>
        /// <returns></returns>
        private Substantive GenerateSceneOfMurderInfo(string name = "the victim's home")
        {
            //pick a random map
            var map = LocationsOfInterest![0];
            var places = map.AllComponents.GetFirst<Place>(Constants.RegionCollectionTag);
            var place = places.SubAreas.First(p => Regex.IsMatch(p.Name, "\\d+ \\w+ street"));
            Noun nouns = MapGen.HouseNouns(Random.Next());
            Pronoun pronouns = Constants.ItemPronouns;
            PhysicalProperties properties = MapGen.HousePhysicalProperties(Random.Next());
            
            string description = $"This {properties.GetPrintableString()} {nouns.Singular} is located at {place.Name}";

            var substantive = new Substantive(SubstantiveTypes.Place, place.Name, description, nouns, pronouns, properties);

            return substantive;
        }

        public IEnumerable<Place> CurrentPlaceInfo(Point position)
            => CurrentLocation!.GoRogueComponents.GetFirst<Place>().GetPlacesContaining(position);
        
        #endregion
        #region things 
        
        /// <summary>
        /// Generates the item which was used to kill someone
        /// </summary>
        /// <returns></returns>
        public RogueLikeEntity GenerateMurderWeapon()
        {
            var itemInfo = ThingFactory.Weapon(Random.Next());
            var murderWeapon = new RogueLikeEntity((0,0), itemInfo.Name[0], true, true, 2);
            murderWeapon.AllComponents.Add(itemInfo, Constants.SubstantiveTag);
            
            //todo - add markings
            
            return murderWeapon;
        }

        /// <summary>
        /// Creates a generic rle item
        /// </summary>
        /// <returns></returns>
        public RogueLikeEntity GenerateMiscellaneousItem()
        {
            var item = ThingFactory.MiscellaneousItem(Random.Next());
            var rle = new RogueLikeEntity((0,0), item.Name[0]);
            rle.AllComponents.Add(item, Constants.SubstantiveTag);
            return rle;
        }
        #endregion
    }
}