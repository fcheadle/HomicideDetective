using System.Collections.Generic;
using GoRogue.MapGeneration;
using SadRogue.Integration;
using SadRogue.Integration.Maps;
using SadRogue.Primitives;
using SadRogue.Primitives.GridViews;

namespace HomicideDetective.Places.Generation
{
    public abstract class PlaceMapGenerator
    {
        public abstract RogueLikeMap Create(int mapWidth, int mapHeight, int viewWidth, int viewHeight);
        protected RogueLikeMap DrawMap(ISettableGridView<RogueLikeCell> primaryMap,
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

        protected RogueLikeMap DrawMap(ISettableGridView<RogueLikeCell> primaryMap,
            ISettableGridView<RogueLikeCell> secondaryMap, int mapWidth, int mapHeight, int viewWidth, int viewHeight) 
                => DrawMap(primaryMap, secondaryMap, secondaryMap, mapWidth, mapHeight, viewWidth, viewHeight);
        
        protected void PlaceRegions(RogueLikeMap map, IEnumerable<Region> regionMap)
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

        protected abstract Substantive GeneratePlaceInfo(Region region);
    }
}