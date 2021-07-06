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
        private static Distance _distance = Distance.Euclidean;
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
        
        private static RogueLikeMap DrawMap(ISettableGridView<MemoryAwareRogueLikeCell> source, int viewWidth, int viewHeight)
        {
            var map = new RogueLikeMap(source.Width, source.Height, new DefaultRendererParams((viewWidth, viewHeight)),
                _layers, _distance);
            
            map.AllComponents.Add(new DimmingMemoryFieldOfViewHandler(_dimmingEffect));
            map.AllComponents.Add(new WeatherController());
            
            foreach (var location in map.Positions())
            {
                source[location].Position = location;
                map.SetTerrain(source[location]);
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
            
            var backingMap = GetMapSource(generator.Context, "WallFloor");
            var map = DrawMap(backingMap, viewWidth, viewHeight);
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
            
            var backingMap = GetMapSource(generator.Context, "WallFloor");
            var map = DrawMap(backingMap, viewWidth, viewHeight);
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
            
            var backingMap = GetMapSource(generator.Context, "WallFloor");
            var map = DrawMap(backingMap, viewWidth, viewHeight);
            map.GoRogueComponents.Add(GetPlains(generator.Context));
            map.GoRogueComponents.Add(GetRegions(generator.Context));
            return map;
        }
        #endregion
    }
}