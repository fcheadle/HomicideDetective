using System.Collections.Generic;
using GoRogue.MapGeneration;
using HomicideDetective.Places.Weather;
using SadRogue.Integration;
using SadRogue.Integration.Maps;
using SadRogue.Primitives.GridViews;

namespace HomicideDetective.Places.Generation
{
    public class DownTownGenerator : PlaceMapGenerator
    {
        public override RogueLikeMap Create(int mapWidth, int mapHeight, int viewWidth, int viewHeight)
        {
            
            var generator = new Generator(mapWidth, mapHeight)
                .ConfigAndGenerateSafe(gen =>
                {
                    gen.AddSteps(new GrassStep());
                    gen.AddSteps(new StreetStep());
                    gen.AddSteps(new DownTownStep());
                });
            var grassMap = generator.Context.GetFirst<ISettableGridView<RogueLikeCell>>("grass");
            var streetMap = generator.Context.GetFirst<ISettableGridView<RogueLikeCell>>("street");
            var downTown = generator.Context.GetFirst<ISettableGridView<RogueLikeCell>>("downtown");
            var map = DrawMap(downTown, streetMap, grassMap, mapWidth, mapHeight, viewWidth, viewHeight);
            
            var weather = new WeatherController(map);
            map.AllComponents.Add(weather);
            var regionMap = generator.Context.GetFirst<IEnumerable<Region>>("regions");
            PlaceRegions(map, regionMap);
            return map;
        }

        protected override Substantive GeneratePlaceInfo(Region region)
        {

            string name = region.Name, description, detail, article = "a";
            if (region.Name.Contains("clothing"))
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

            var substantive = new Substantive(Substantive.Types.Place, name, gender: "", article: article, pronoun: "it",
                pronounPossessive: "its", description: description, mass: 0, volume: 0);
            
            substantive.AddDetail(detail);
            return substantive;        
        }
    }
}