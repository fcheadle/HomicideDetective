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
    public static class MapGen
    {
        private static readonly int _layers = 16;
        private static Distance _distance = Distance.Euclidean;
        private static readonly float _dimmingEffect = 0.5f;
        
        #region private-static
        private static WindyPlain GetPlains(GenerationContext context) 
            => context.GetFirst<IEnumerable<WindyPlain>>("plains").First();

        private static BodyOfWater GetPond(GenerationContext context)
            => context.GetFirst<BodyOfWater>("pond");
        private static Region GetRegions(GenerationContext context)
            => context.GetFirst<Region>("regions");

        private static ISettableGridView<MemoryAwareRogueLikeCell> GetMapSource(GenerationContext context, string name)
            => context.GetFirst<ISettableGridView<MemoryAwareRogueLikeCell>>(name);
        
        private static RogueLikeMap DrawMap(ISettableGridView<MemoryAwareRogueLikeCell> source, int viewWidth, int viewHeight)
        {
            var map = new RogueLikeMap(source.Width, source.Height, new DefaultRendererParams((viewWidth, viewHeight)), _layers, _distance);
            
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
        
        #region static methods for steps to use
        public static Region Parallelogram(int left, int bottom, int width, int height)
        {
            Point nw = (left + height, bottom - height);
            Point ne = (left + height + width, bottom - height);
            Point se = (left + width, bottom);
            Point sw = (left, bottom);
            return new Region("parallelogram", nw, ne, se, sw);
        }
        
        public static void ConnectAllSides(Region region)
        {
            region.AddConnection(MiddlePoint(region.WestBoundary));
            region.AddConnection(MiddlePoint(region.EastBoundary));
            region.AddConnection(MiddlePoint(region.NorthBoundary));
            region.AddConnection(MiddlePoint(region.SouthBoundary));
        }
        public static Point MiddlePoint(IReadOnlyArea area) => area[area.Count() / 2];

        
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
        
        
        public static void ConnectOnLeftSide(Region plot)
            => plot.AddConnection(MiddlePoint(plot.WestBoundary));
        

        public static void ConnectOnRightSide(Region plot)
            => plot.AddConnection(MiddlePoint(plot.EastBoundary));

        public static void ConnectOnTopSide(Region plot)
            => plot.AddConnection(MiddlePoint(plot.NorthBoundary));

        public static void ConnectOnBottomSide(Region plot)
            => plot.AddConnection(MiddlePoint(plot.SouthBoundary));

        public static Region BaseRegion(string name, int width, int height)
            => new Region(name, (0, 0), (width, 0), (width, height), (0, height));

        #endregion


    }
}