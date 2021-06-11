using System.Collections.Generic;
using GoRogue.MapGeneration;
using HomicideDetective.Places.Weather;
using SadRogue.Integration;
using SadRogue.Integration.Maps;
using SadRogue.Primitives;
using SadRogue.Primitives.GridViews;

namespace HomicideDetective.Places.Generation
{
    /// <summary>
    /// A composite step which has a full RogueLikeMap upon performance
    /// </summary>
    public class NeighborhoodGenerator : PlaceMapGenerator
    {
        public override RogueLikeMap Create(int mapWidth, int mapHeight, int viewWidth, int viewHeight)
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
            var weather = new WeatherController(map);
            map.AllComponents.Add(weather);
            return map;
        }

        protected override Substantive GeneratePlaceInfo(Region region)
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