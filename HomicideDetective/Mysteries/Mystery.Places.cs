using System.Collections.Generic;
using System.Linq;
using GoRogue;
using GoRogue.MapGeneration;
using HomicideDetective.Places;
using HomicideDetective.Places.Generation;
using HomicideDetective.UserInterface;
using SadRogue.Integration.Maps;
using SadRogue.Primitives;
using SadRogue.Primitives.GridViews;

namespace HomicideDetective.Mysteries
{
    public partial class Mystery
    {
        private void GenerateLocationsOfInterest()
        { 
            LocationsOfInterest.Add(NeighborhoodMap());
            LocationsOfInterest.Add(ParkMap());
            LocationsOfInterest.Add(DownTownMap());
        }

        private static RogueLikeMap NeighborhoodMap()
        {
            var map = PlaceMapGenerator.CreateNeighborhoodMap(GameContainer.MapWidth, GameContainer.MapHeight,
                Program.Width * 2 / 3 + 1, Program.Height);
            return PlaceRegions(map);
        }
        
        private RogueLikeMap ParkMap()
        {
            var map = PlaceMapGenerator.CreateParkMap(GameContainer.MapWidth, GameContainer.MapHeight, Program.Width * 2 / 3 + 1,
                Program.Height);
            return PlaceRegions(map);
        }

        private RogueLikeMap DownTownMap()
        {
            var map = PlaceMapGenerator.CreateDownTownMap(GameContainer.MapWidth, GameContainer.MapHeight,
                Program.Width * 2 / 3 + 1, Program.Height);
            return PlaceRegions(map);
        }
        
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
        
        private static RogueLikeMap PlaceRegions(RogueLikeMap map)
        {
            var regionMap = map.AllComponents.GetFirst<IEnumerable<Region>>();
            var places = new PlaceCollection();
            foreach (var region in regionMap)
            {
                places.Add(new Place(region, GeneratePlaceInfo(region)));
                foreach (var subRegion in region.SubRegions)
                    places.Add(new Place(subRegion, GeneratePlaceInfo(subRegion)));
            }

            map.AllComponents.Add(places);
            return map;
        }

        //todo - delete (move to mystery)
        private static Substantive GeneratePlaceInfo(Region region)
        {
            string name = region.Name, description, detail, article = "a";
            if (region.Name.Contains("hall"))
            {
                description = "This central corridor connects many rooms.";
                detail = "It is immaculately clean.";
            }
            else if (region.Name.Contains("kitchen"))
            {
                description = "This room is used to prepare food.";
                detail = "It has a patterned floor and granite countertops.";
            }
            else if (region.Name.Contains("dining"))
            {
                name = "dining room";
                description = "This room is for eating meals.";
                detail = "It is dominated by a large table and chairs in the center.";
            }
            else if (region.Name.Contains("bedroom"))
            {
                description = "This is a room in which to sleep.";
                detail = "The floor is covered in clutter.";
            }
            else if (region.Name.Contains("house"))
            {
                description = "This is someone's home.";
                detail = "It is quaint, with sparse decorations.";
                article = "";
            }
            else if (region.Name.Contains("street"))
            {
                description = "This is a street in a neighborhood.";
                detail = "It is a quiet day on this street.";
                article = "";
            }
            else if (region.Name.Contains("block"))
            {
                description = "This is the block on which the victim lived.";
                detail = "A moderate breeze blows through the grass.";
                article = "";
            }
            else if(region.Name.Contains("Park"))
            {
                description = "This is the park near the victim's home.";
                detail = "According to witnesses, the victim walked through the park on the day they died.";
                article = "";
            }
            else if(region.Name.Contains("Pond"))
            {
                description = "This pond is a popular spot for locals to gather and watch the waves go by.";
                detail = "The victim was often spotted here, including on the day they were murdered.";
                article = "";
            }        
            else if (region.Name.Contains("clothing"))
            {
                description = "This is a shop for buying clothing.";
                detail = "It has a welcoming aura and pleasant odor.";
            }
            else if(region.Name.Contains("diner"))
            {
                description = "This is a diner.";
                detail = "It claims to have world-famous coffee and hash browns.";
            }
            else if(region.Name.Contains("antique"))
            {
                description = "This is an antique shop.";
                detail = "Many of the items here are older than I am.";
            }
            else 
            {
                description = "(description unclear)";
                detail = "(???)";
            }

            var substantive = new Substantive(Substantive.Types.Place, name, gender: "", article: article,
                pronoun: "it", pronounPossessive: "its", description: description, mass: 0, volume: 0);

            substantive.AddDetail(detail);
            return substantive;
        }
    }
}