using System;
using System.Collections.Generic;
using System.Linq;
using GoRogue.MapGeneration;
using HomicideDetective.Places.Weather;
using HomicideDetective.Words;
using SadRogue.Integration.FieldOfView.Memory;
using SadRogue.Integration.Maps;
using SadRogue.Primitives;
using SadRogue.Primitives.GridViews;

namespace HomicideDetective.Places.Generation
{
    public static class MapGen
    {
        private static readonly int _layers = 16;
        private static Distance _distance = Distance.Euclidean;
        private static readonly float _dimmingEffect = 0.5f;
        
        #region private-static
        private static WindyPlain GetPlains(GenerationContext context) 
            => context.GetFirst<IEnumerable<WindyPlain>>(Constants.WindyPlainsTag).First();

        private static BodyOfWater GetPond(GenerationContext context)
            => context.GetFirst<BodyOfWater>(Constants.BodyOfWaterTag);
        private static Region GetRegions(GenerationContext context)
            => context.GetFirst<Region>(Constants.RegionCollectionTag);

        private static ISettableGridView<MemoryAwareRogueLikeCell> GetMapSource(GenerationContext context, string name)
            => context.GetFirst<ISettableGridView<MemoryAwareRogueLikeCell>>(name);
        
        private static RogueLikeMap DrawMap(ISettableGridView<MemoryAwareRogueLikeCell> source, int viewWidth, int viewHeight)
        {
            var map = new RogueLikeMap(source.Width, source.Height, new DefaultRendererParams((viewWidth, viewHeight)), _layers, _distance);
            
            map.AllComponents.Add(new DimmingMemoryFieldOfViewHandler(_dimmingEffect));
            map.AllComponents.Add(new WeatherController(), Constants.WeatherTag);
            
            foreach (var location in map.Positions())
            {
                source[location].Position = location;
                map.SetTerrain(source[location]);
            }

            return map;
        }
        
        public static Noun HouseNouns(int seed)
        {
            var type = RandomHouseType(seed);
            return new Noun(type, type + "s");//for now
        }
        private static string RandomHouseType(int seed)
        {
            switch (new Random(seed).Next(1, 20))
            {
                default: return "brownstone";
                case 1: return "tutor";
                case 2: return "prairie home";
                case 3: return "cottage";
                case 4: return "brick home";
                case 5: return "ranch house";
                case 6: return "gothic manor";
                case 7: return "victorian";
                case 8: return "edwardian";
                case 9: return "mobile home";
                case 10: return "trailer";
                case 11: return "townhome";
                case 12: return "rowhome";
                case 13: return "bungalow";
                case 14: return "shotgun house";
                case 15: return "farmhouse";
                case 16: return "flat";
                case 17: return "walkup";
                case 18: return "apartment";
                case 19: return "cabin";
            }
        }
        private static string RandomHouseWidth(int seed)
        {
            switch (new Random(seed).Next(1, 20))
            {
                default: return "slender";
                case 1: return "skinny";
                case 2: return "thin";
                case 3: return "thinly-walled";
                case 4: return "squished";
                case 5: return "very thin";
                case 6: return "extremely thin";
                case 7: return "somewhat thin";
                case 8: return "thick";
                case 9: return "slightly thin";
                case 10: return "slightly thick";
                case 11: return "somewhat thick";
                case 12: return "thick-walled";
                case 13: return "very thick";
                case 14: return "extremely thick";
                case 15: return "really thick";
                case 16: return "thick";
                case 17: return "average-width";
                case 18: return "moderate width";
            }
        }
        private static string RandomHouseHeight(int seed)
        {
            switch (new Random(seed).Next(1, 20))
            {
                default: return "short, flat";
                case 1: return "single-story";
                case 2: return "one-floor";
                case 3: return "short";
                case 4: return "split-level";
                case 5: return "multiple split-level";
                case 6: return "flat";
                case 7: return "steep";
                case 8: return "two-story";
                case 9: return "two-level";
                case 10: return "two-story-split-level";
                case 11: return "double story";
                case 12: return "three-story";
                case 13: return "three-level";
                case 14: return "two-level with an attic";
                case 15: return "two-story with an attic";
                case 16: return "one-story with an attic";
                case 17: return "one-level with an attic";
                case 18: return "three-level with an attic";
                case 19: return "three-story with an attic";
            }
        }
        #endregion
        
        #region create-maps
        public static RogueLikeMap CreateDownTownMap(int seed, int mapWidth, int mapHeight, int viewWidth, int viewHeight)
        {
            var random = new Random(seed);
            var horiz = random.Next(Enum.GetValues<RoadNumbers>().Length - 1);
            var vertic = random.Next(Enum.GetValues<RoadNames>().Length - 1);
            
            var generator = new Generator(mapWidth, mapHeight)
                .ConfigAndGenerateSafe(gen =>
                {
                    gen.AddSteps(new GrassStep());
                    gen.AddSteps(new StreetStep(true, horiz, vertic));
                    gen.AddSteps(new DownTownStep(horiz, vertic));
                });
            
            var backingMap = GetMapSource(generator.Context, Constants.GridViewTag);
            var map = DrawMap(backingMap, viewWidth, viewHeight);
            map.GoRogueComponents.Add(GetPlains(generator.Context), Constants.WindyPlainsTag);
            map.GoRogueComponents.Add(GetRegions(generator.Context), Constants.RegionCollectionTag);
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
            
            var backingMap = GetMapSource(generator.Context, Constants.GridViewTag);
            var map = DrawMap(backingMap, viewWidth, viewHeight);
            map.GoRogueComponents.Add(GetPond(generator.Context), Constants.BodyOfWaterTag);
            map.GoRogueComponents.Add(GetPlains(generator.Context), Constants.WindyPlainsTag);
            map.GoRogueComponents.Add(GetRegions(generator.Context), Constants.RegionCollectionTag);
            return map;
        }
        
        public static RogueLikeMap CreateNeighborhoodMap(int seed, int mapWidth, int mapHeight, int viewWidth, int viewHeight)
        {
            var random = new Random(seed);
            var horiz = random.Next(Enum.GetValues<RoadNumbers>().Length);
            var vertic = random.Next(Enum.GetValues<RoadNames>().Length);
            var generator = new Generator(mapWidth, mapHeight)
                .ConfigAndGenerateSafe(gen =>
                {
                    gen.AddSteps(new GrassStep());
                    gen.AddSteps(new StreetStep(false, horiz, vertic));
                    gen.AddSteps(new HouseStep(horiz, vertic));
                });
            
            var backingMap = GetMapSource(generator.Context, Constants.GridViewTag);
            var map = DrawMap(backingMap, viewWidth, viewHeight);
            map.GoRogueComponents.Add(GetPlains(generator.Context), Constants.WindyPlainsTag);
            map.GoRogueComponents.Add(GetRegions(generator.Context), Constants.RegionCollectionTag);
            return map;
            // return PlaceRegions(map);
        }
        #endregion
        
        #region static methods for steps to use
        public static PhysicalProperties HousePhysicalProperties(int seed)
        {
            var rand = new Random(seed);
            //minimum 16m, maximum 32m
            var width = rand.Next(16, 32);
            var height = rand.Next(16, 32);
            return new PhysicalProperties(-1, width * height * 1000, RandomHouseWidth(rand.Next()), RandomHouseHeight(rand.Next()));
        }
        internal static Point MiddlePoint(IReadOnlyArea area) => area[area.Count() / 2];
        
        public static void Finalize(ISettableGridView<MemoryAwareRogueLikeCell> map)
        {
            foreach (var point in map.Positions())
            {
                var here = map[point];
                if (map.Contains(point + Direction.Right) && here?.IsTransparent == true && here?.IsWalkable == true)
                {
                    var right = map[point + Direction.Right];
                    if (right?.IsTransparent == false)
                    {
                        map[point] = new MemoryAwareRogueLikeCell(point, right.Appearance.Foreground,
                            right.Appearance.Background, right.Appearance.Glyph, right.Layer, right.IsWalkable, false);
                    }

                }
            }
        }
        
        #endregion
    }
}