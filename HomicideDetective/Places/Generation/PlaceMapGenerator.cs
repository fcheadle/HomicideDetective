using System.Collections.Generic;
using System.Linq;
using GoRogue.MapGeneration;
using HomicideDetective.Places.Weather;
using SadRogue.Integration;
using SadRogue.Integration.Maps;
using SadRogue.Primitives;
using SadRogue.Primitives.GridViews;

namespace HomicideDetective.Places.Generation
{
    public static class PlaceMapGenerator
    {
        public static RogueLikeMap CreateDownTownMap(int mapWidth, int mapHeight, int viewWidth, int viewHeight)
        {
            var generator = new Generator(mapWidth, mapHeight)
                .ConfigAndGenerateSafe(gen =>
                {
                    gen.AddSteps(new GrassStep());
                    gen.AddSteps(new StreetStep(true));
                    gen.AddSteps(new DownTownStep());
                });
            var grassMap = generator.Context.GetFirst<ISettableGridView<RogueLikeCell>>("grass");
            var streetMap = generator.Context.GetFirst<ISettableGridView<RogueLikeCell>>("street");
            var downTown = generator.Context.GetFirst<ISettableGridView<RogueLikeCell>>("downtown");
            var map = DrawMap(downTown, streetMap, grassMap, mapWidth, mapHeight, viewWidth, viewHeight);
            
            var weather = new WeatherController();
            map.AllComponents.Add(weather);
            var plainMap = generator.Context.GetFirst<IEnumerable<WindyPlain>>("plains").First();
            map.GoRogueComponents.Add(plainMap);
            var regionMap = generator.Context.GetFirst<IEnumerable<Region>>("regions");
            PlaceRegions(map, regionMap);
            return map;
        }
        
        public static RogueLikeMap CreateParkMap(int mapWidth, int mapHeight, int viewWidth, int viewHeight)
        {
            
            var generator = new Generator(mapWidth, mapHeight)
                .ConfigAndGenerateSafe(gen =>
                {
                    gen.AddSteps(new GrassStep());
                    gen.AddSteps(new ParkFeaturesStep());
                });
            var grassMap = generator.Context.GetFirst<ISettableGridView<RogueLikeCell>>("grass");
            var parkMap = generator.Context.GetFirst<ISettableGridView<RogueLikeCell>>("park");
            var pondMap = generator.Context.GetFirst<IEnumerable<BodyOfWater>>("pond").First();
            var plainMap = generator.Context.GetFirst<IEnumerable<WindyPlain>>("plains").First();
            var map = DrawMap(parkMap, grassMap, mapWidth, mapHeight, viewWidth, viewHeight);
            
            var weather = new WeatherController();
            map.AllComponents.Add(weather);
            map.GoRogueComponents.Add(pondMap);
            map.GoRogueComponents.Add(plainMap);
            
            
            var regionMap = generator.Context.GetFirst<IEnumerable<Region>>("regions");
            PlaceRegions(map, regionMap);
            return map;
        }
        
        public static RogueLikeMap CreateNeighborhoodMap(int mapWidth, int mapHeight, int viewWidth, int viewHeight)
        {
            var generator = new Generator(mapWidth, mapHeight)
                .ConfigAndGenerateSafe(gen =>
                {
                    gen.AddSteps(new GrassStep());
                    gen.AddSteps(new StreetStep());
                    gen.AddSteps(new HouseStep());
                });

            var grassMap = generator.Context.GetFirst<ISettableGridView<RogueLikeCell>>("grass");
            var streetMap = generator.Context.GetFirst<ISettableGridView<RogueLikeCell>>("street");
            var houseMap = generator.Context.GetFirst<ISettableGridView<RogueLikeCell>>("house");
            var regions = generator.Context.GetFirst<List<Region>>("regions");

            var map = new RogueLikeMap(mapWidth, mapHeight, 4, Distance.Euclidean, viewSize:(viewWidth, viewHeight));
            map = DrawMap(houseMap, streetMap, grassMap, mapWidth, mapHeight, viewWidth, viewHeight);
            PlaceRegions(map, regions);
            var weather = new WeatherController();
            map.AllComponents.Add(weather);
            var plainMap = generator.Context.GetFirst<IEnumerable<WindyPlain>>("plains").First();
            map.GoRogueComponents.Add(plainMap);
            return map;
        }
        
        private static RogueLikeMap DrawMap(ISettableGridView<RogueLikeCell> primaryMap,
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

        private static RogueLikeMap DrawMap(ISettableGridView<RogueLikeCell> primaryMap,
            ISettableGridView<RogueLikeCell> secondaryMap, int mapWidth, int mapHeight, int viewWidth, int viewHeight) 
                => DrawMap(primaryMap, secondaryMap, secondaryMap, mapWidth, mapHeight, viewWidth, viewHeight);
        
        private static void PlaceRegions(RogueLikeMap map, IEnumerable<Region> regionMap)
        {
            var places = new PlaceCollection();
            foreach (var region in regionMap)
            {
                places.Add(new Place(region, GeneratePlaceInfo(region)));
                foreach (var subRegion in region.SubRegions)
                    places.Add(new Place(subRegion, GeneratePlaceInfo(subRegion)));
            }

            map.AllComponents.Add(places);
        }


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