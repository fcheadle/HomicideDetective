using System.Collections.Generic;
using System.Linq;
using GoRogue;
using GoRogue.MapGeneration;
using HomicideDetective.Places;
using SadRogue.Integration;
using SadRogue.Integration.Maps;
using SadRogue.Primitives;
using SadRogue.Primitives.GridViews;

namespace HomicideDetective.Mysteries
{
    public partial class Mystery
    {
        public void NextMap()
        {
            if (_currentMapIndex >= LocationsOfInterest.Count - 1)
                _currentMapIndex = 0;
            else
                _currentMapIndex++;
        }

        private static Place RandomRoom(RogueLikeMap map) => map.GoRogueComponents.GetFirst<PlaceCollection>()
            .RandomItem(pc => pc.Area.Points.Any(p => map.Contains(p)));
        
        private void PlacePeopleOnMaps()
        {
            var map = LocationsOfInterest[0];
            SceneOfCrime = RandomRoom(map);
            var room = RandomRoom(map);
            //put the murder weapon on the map
            var murderWeapon = MurderWeapon;
            murderWeapon.Position = RandomFreeSpace(map, room.Area);
            map.AddEntity(murderWeapon);

            //put the murderer on the map
            room = RandomRoom(map);
            var murderer = Murderer;
            murderer.Position = RandomFreeSpace(map, room.Area);
            map.AddEntity(murderer);

            //add victim's corpse to the map
            room = SceneOfCrime;
            var victim = Victim;
            victim.Position = RandomFreeSpace(map, room.Area);
            map.AddEntity(victim);
            
            foreach (var witness in Witnesses)
            {
                map = LocationsOfInterest.RandomItem();
                room = RandomRoom(map);
                witness.Position = RandomFreeSpace(map, room.Area);
                
                map.AddEntity(witness);
            }
        }

        public static Point RandomFreeSpace(RogueLikeMap map)
            => RandomFreeSpace(map, RandomRoom(map).Area);


        private static Point RandomFreeSpace(RogueLikeMap map, Region region)
            => region.InnerPoints.RandomItem(p => map.Contains(p) && map.WalkabilityView[p]);
        
        /// <summary>
        /// Generates the descriptive information about the scene of the crime.
        /// </summary>
        /// <returns></returns>
        public Substantive GenerateSceneOfMurderInfo()
        {
            string name, noun, pronoun, height, width;
            
            name = $"{Victim.Info().Name}'s home";
            pronoun = "it";
            string pronounPossessive = "its";
            string pronounPassive = "the";
            switch (Random.Next(1, 101) % 8)
            {
                default:
                case 0:
                    height = "two-story";
                    width = "slender";
                    noun = "brownstone";
                    break;
                case 1: 
                    height = "single-floor";
                    width = "slender";
                    noun = "flat";
                    break;
                case 2: 
                    height = "single-floor";
                    width = "wide";
                    noun = "row home";
                    break;
                case 3: 
                    height = "single-floor";
                    width = "wide";
                    noun = "brick home";
                    break;
                case 4: 
                    height = "two-story";
                    width = "ornate";
                    noun = "victorian";
                    break;
                case 5: 
                    height = "two-story";
                    width = "'L'-shaped";
                    noun = "brick home";
                    break;
                case 6: 
                    height = "single-floor";
                    width = "modest";
                    noun = "prarie home";
                    break;
                case 7:
                    height = "two-story";
                    width = "decadent";
                    noun = "tutor";
                    break;
            }

            string description = $"{pronoun} is a {height}, {width} {noun}.";

            var substantive = new Substantive(Substantive.Types.Place, name, gender: "", article: pronounPassive, pronoun: pronoun,
                pronounPossessive: pronounPossessive, description: description, mass: 0, volume: 0);

            return substantive;
        }
        
        public string CurrentPlaceInfo(Point position)
        {
            var answer = "";
            foreach (var region in CurrentLocation.GoRogueComponents.GetFirst<PlaceCollection>().GetPlacesContaining(position))
            {
                answer += "\r\n";
                answer += region.Info.GenerateDetailedDescription();
            }

            return answer;
        }
    }
}