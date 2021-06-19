using System.Collections.Generic;
using System.Linq;
using GoRogue.MapGeneration;
using HomicideDetective.Places.Weather;
using SadRogue.Integration.FieldOfView.Memory;
using SadRogue.Integration.Maps;
using SadRogue.Primitives;
using SadRogue.Primitives.GridViews;

namespace HomicideDetective.Places.Generation
{
    public static class PlaceMapGenerator
    {
        private static readonly int _layers = 16;
        private static readonly Distance _distance = Distance.Euclidean;
        private static readonly float _dimmingEffect = 0.5f;
        
        #region private-static
        private static WindyPlain GetPlains(GenerationContext context) 
            => context.GetFirst<IEnumerable<WindyPlain>>("plains").First();

        private static BodyOfWater GetPond(GenerationContext context)
            => context.GetFirst<BodyOfWater>("pond");
        private static IEnumerable<Region> GetRegions(GenerationContext context)
            => context.GetFirst<IEnumerable<Region>>("regions");

        private static ISettableGridView<MemoryAwareRogueLikeCell> GetMapSource(GenerationContext context, string name)
            => context.GetFirst<ISettableGridView<MemoryAwareRogueLikeCell>>(name);
        
        private static RogueLikeMap DrawMap(ISettableGridView<MemoryAwareRogueLikeCell> primaryMap,
            ISettableGridView<MemoryAwareRogueLikeCell> secondaryMap,
            ISettableGridView<MemoryAwareRogueLikeCell> backgroundMap,
            int mapWidth, int mapHeight, int viewWidth, int viewHeight)
            => DrawMap(new RogueLikeMap(mapWidth, mapHeight, 4, _distance, viewSize: (viewWidth, viewHeight)),
                primaryMap, secondaryMap, backgroundMap);

        private static RogueLikeMap DrawMap(ISettableGridView<MemoryAwareRogueLikeCell> primaryMap,
            ISettableGridView<MemoryAwareRogueLikeCell> secondaryMap, int mapWidth, int mapHeight, int viewWidth, int viewHeight) 
                => DrawMap(primaryMap, secondaryMap, secondaryMap, mapWidth, mapHeight, viewWidth, viewHeight);

        private static RogueLikeMap DrawMap(RogueLikeMap map, ISettableGridView<MemoryAwareRogueLikeCell> primarySource,
            ISettableGridView<MemoryAwareRogueLikeCell> secondarySource)
            => DrawMap(map, primarySource, secondarySource, secondarySource);

        private static RogueLikeMap DrawMap(RogueLikeMap map, ISettableGridView<MemoryAwareRogueLikeCell> primarySource, 
            ISettableGridView<MemoryAwareRogueLikeCell> secondarySource, ISettableGridView<MemoryAwareRogueLikeCell> tertiarySource)
        {
            map.AllComponents.Add(new DimmingMemoryFieldOfViewHandler(_dimmingEffect));
            map.AllComponents.Add(new WeatherController());
            
            foreach (var location in map.Positions())
            {
                if (primarySource[location] != null)
                {
                    primarySource[location].Position = location;
                    map.SetTerrain(primarySource[location]);
                }

                else if (secondarySource[location] != null)
                {
                    secondarySource[location].Position = location;
                    map.SetTerrain(secondarySource[location]);
                }

                else
                {
                    tertiarySource[location].Position = location;
                    map.SetTerrain(tertiarySource[location]);
                }
            }
            
            return map;
        }

        
        #endregion
        
        #region create-maps
        public static RogueLikeMap CreateDownTownMap(int mapWidth, int mapHeight, int viewWidth, int viewHeight)
        {
            var generator = new Generator(mapWidth, mapHeight)
                .ConfigAndGenerateSafe(gen =>
                {
                    gen.AddSteps(new GrassStep());
                    gen.AddSteps(new StreetStep(true));
                    gen.AddSteps(new DownTownStep());
                });
            var grassMap = GetMapSource(generator.Context, "grass");
            var streetMap = GetMapSource(generator.Context,"street");
            var downTown = GetMapSource(generator.Context,"downtown");
            var map = DrawMap(downTown, streetMap, grassMap, mapWidth, mapHeight, viewWidth, viewHeight);
            map.GoRogueComponents.Add(GetPlains(generator.Context));
            PlaceRegions(map, GetRegions(generator.Context));
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
            var grassMap = GetMapSource(generator.Context, "grass");
            var parkMap = GetMapSource(generator.Context, "park");
            var map = DrawMap(parkMap, grassMap, mapWidth, mapHeight, viewWidth, viewHeight);
            map.GoRogueComponents.Add(GetPond(generator.Context));
            map.GoRogueComponents.Add(GetPlains(generator.Context));
            PlaceRegions(map, GetRegions(generator.Context));
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

            var grassMap = GetMapSource(generator.Context, "grass");
            var streetMap = GetMapSource(generator.Context, "street");
            var houseMap = GetMapSource(generator.Context, "house");

            var map = DrawMap(houseMap, streetMap, grassMap, mapWidth, mapHeight, viewWidth, viewHeight);
            map.GoRogueComponents.Add(GetPlains(generator.Context));
            PlaceRegions(map, GetRegions(generator.Context));
            return map;
        }
        #endregion
        
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