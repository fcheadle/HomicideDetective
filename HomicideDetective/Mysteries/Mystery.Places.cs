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
        /// <summary>
        /// Creates the map for the crime scene
        /// </summary>
        /// <param name="mapWidth"></param>
        /// <param name="mapHeight"></param>
        /// <param name="viewWidth"></param>
        /// <param name="viewHeight"></param>
        /// <returns></returns>
        public RogueLikeMap GenerateMap(int mapWidth, int mapHeight, int viewWidth, int viewHeight)
        {
            //use GoRogue generator to combine the steps in Places/Generation and get the resulting three maps
            var generator = new Generator(mapWidth, mapHeight)
                .ConfigAndGenerateSafe(gen =>
                {
                    gen.AddSteps(new Places.Generation.GrassStep());
                    gen.AddSteps(new Places.Generation.StreetStep());
                    gen.AddSteps(new Places.Generation.HouseStep());
                });

            var grassMap = generator.Context.GetFirst<ISettableGridView<RogueLikeCell>>("grass");
            var streetMap = generator.Context.GetFirst<ISettableGridView<RogueLikeCell>>("street");
            var houseMap = generator.Context.GetFirst<ISettableGridView<RogueLikeCell>>("house");
            var regionMap = generator.Context.GetFirst<IEnumerable<Region>>("houses");
            
            //combine the three smaller maps into one real map
            RogueLikeMap map = DrawMap(houseMap, streetMap, grassMap, mapWidth, mapHeight, viewWidth, viewHeight);
            
            //add additional components
            //todo - unsafe?
            Program.Weather = new Weather(map);
            var places = new PlaceCollection();
            foreach (var region in regionMap)
            {
                places.Add(new Place(region, GeneratePlaceInfo(region)));
                foreach(var subRegion in region.SubRegions)
                    places.Add(new Place(subRegion, GeneratePlaceInfo(subRegion)));
            }
            
            map.AllComponents.Add(places);

            LocationsOfInterest.Add(map);
            SceneOfCrime = RandomRoom(map);
            PlacePeopleOnMap(map);

            return map;
        }

        private Substantive GeneratePlaceInfo(Region region)
        {
            string name, description, detail, article;
            if (region.Name.Contains("hall"))
            {
                name = "hall";
                description = "A central corridor connecting many rooms.";
                detail = "It is immaculately clean.";
                article = "a";
            }
            else if(region.Name.Contains("kitchen"))
            {
                name = "kitchen";
                description = "A room used to prepare food.";
                detail = "It has a patterned floor and granite countertops.";
                article = "a";
            }
            else if(region.Name.Contains("dining"))
            {
                name = "dining";
                description = "A room for eating meals.";
                detail = "It is dominated by a large table and chairs in the center.";
                article = "a";
            }
            else if(region.Name.Contains("bedroom"))
            {
                name = "bedroom";
                description = "A room in which to sleep.";
                detail = "The floor is covered in clutter.";
                article = "a";
            }
            else if(region.Name.Contains("house"))
            {
                return GenerateSceneOfMurderInfo();
            }
            else 
            {
                name = "unknown room type";
                description = "God only knows what the purpose of this room is.";
                detail = "Neither floor nor wall appear to be made of anything at all...";
                article = "a";
            }

            var substantive = new Substantive(Substantive.Types.Place, name, gender: "", article: article, pronoun: "it",
                pronounPossessive: "its", description: description, mass: 0, volume: 0);
            
            substantive.AddDetail(detail);
            return substantive;
        }

        private RogueLikeMap DrawMap(ISettableGridView<RogueLikeCell> primaryMap,
            ISettableGridView<RogueLikeCell> secondaryMap, ISettableGridView<RogueLikeCell> backgroundMap, 
            int mapWidth, int mapHeight, int viewWidth, int viewHeight)
        {
            RogueLikeMap map = new RogueLikeMap(mapWidth, mapHeight, 4, Distance.Euclidean,
                viewSize: (viewWidth, viewHeight));

            foreach (var location in map.Positions())
            {
                if (primaryMap[location] != null)
                {
                    primaryMap[location].Position = location;
                    map.SetTerrain(primaryMap[location]);
                }

                else if (secondaryMap[location] != null)
                {
                    secondaryMap[location].Position = location;
                    map.SetTerrain(secondaryMap[location]);
                }

                else
                {
                    backgroundMap[location].Position = location;
                    map.SetTerrain(backgroundMap[location]);
                }
            }
            
            return map;
        }

        private Place RandomRoom(RogueLikeMap map) => map.GoRogueComponents.GetFirst<PlaceCollection>().RandomItem();
        
        private void PlacePeopleOnMap(RogueLikeMap map)
        {
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

            //populate the map with witnesses
            foreach (var witness in Witnesses)
            {
                room = RandomRoom(map);
                witness.Position = RandomFreeSpace(map, room.Area);
                map.AddEntity(witness);
            }
        }

        public static Point RandomFreeSpace(RogueLikeMap map)
        {
            var point = map.WalkabilityView.RandomPosition();
            while(map.GetEntitiesAt<RogueLikeEntity>(point).Any() || !map.WalkabilityView[point])
                point = map.WalkabilityView.RandomPosition();
            
            return point;
        }

        private static Point RandomFreeSpace(RogueLikeMap map, Region region)
        {
            var point = region.InnerPoints.RandomItem(p => map.Contains(p) && map.WalkabilityView[p]);
            
            while(map.GetEntitiesAt<RogueLikeEntity>(point).Any() || !map.WalkabilityView[point])
                point = region.InnerPoints.RandomItem(p => map.Contains(p) && map.WalkabilityView[p]);
            return point;
        }
        
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
            foreach (var region in LocationsOfInterest.First().GoRogueComponents.GetFirst<PlaceCollection>().GetPlacesContaining(position))
            {
                answer += "\r\n";
                answer += region.Info.GenerateDetailedDescription();
            }

            return answer;
        }
    }
}