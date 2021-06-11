using System.Collections.Generic;
using System.Linq;
using GoRogue.MapGeneration;
using HomicideDetective.Places.Weather;
using SadRogue.Integration;
using SadRogue.Integration.Maps;
using SadRogue.Primitives.GridViews;

namespace HomicideDetective.Places.Generation
{
    public class ParkGenerator : PlaceMapGenerator
    {
        public override RogueLikeMap Create(int mapWidth, int mapHeight, int viewWidth, int viewHeight)
        {
            
            var generator = new Generator(mapWidth, mapHeight)
                .ConfigAndGenerateSafe(gen =>
                {
                    gen.AddSteps(new Places.Generation.GrassStep());
                    gen.AddSteps(new Places.Generation.ParkFeaturesStep());
                });
            var grassMap = generator.Context.GetFirst<ISettableGridView<RogueLikeCell>>("grass");
            var parkMap = generator.Context.GetFirst<ISettableGridView<RogueLikeCell>>("park");
            var pondMap = generator.Context.GetFirst<IEnumerable<BodyOfWater>>("pond").First();
            var map = DrawMap(parkMap, grassMap, mapWidth, mapHeight, viewWidth, viewHeight);
            
            var weather = new WeatherController(map);
            map.AllComponents.Add(weather);
            map.GoRogueComponents.Add(pondMap);
            
            
            var regionMap = generator.Context.GetFirst<IEnumerable<Region>>("regions");
            PlaceRegions(map, regionMap);
            return map;
        }

        protected override Substantive GeneratePlaceInfo(Region region)
        {
            string name = region.Name, description, detail, article = "";
            if(region.Name.Contains("Park"))
            {
                description = "This is the park near the victim's home.";
                detail = "The victim was often spotted here, including on the day they were murdered.";
            }
            else if(region.Name.Contains("Pond"))
            {
                description = "This pond is a popular spot for locals to gather and watch the waves go by.";
                detail = "The victim was often spotted here, including on the day they were murdered.";
            }        
            else
            {
                description = "???";
                detail = "???";
            }

            var substantive = new Substantive(Substantive.Types.Place, name, gender: "", article: article,
                pronoun: "it", pronounPossessive: "its", description: description, mass: 0, volume: 0);

            substantive.AddDetail(detail);
            return substantive;
        }
    }
}