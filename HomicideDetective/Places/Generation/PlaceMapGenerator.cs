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
            map.GoRogueComponents.Add(GetRegions(generator.Context));
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
            map.GoRogueComponents.Add(GetRegions(generator.Context));
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
            map.GoRogueComponents.Add(GetRegions(generator.Context));
            return map;
        }
        #endregion
    }
}