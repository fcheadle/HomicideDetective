using System.Collections.Generic;
using System.Linq;
using GoRogue;
using GoRogue.MapGeneration;
using HomicideDetective.Places;
using SadRogue.Integration;
using SadRogue.Integration.Maps;
using SadRogue.Primitives;
using SadRogue.Primitives.GridViews;

namespace HomicideDetective.Mysteries
{
    public partial class Mystery
    {
        public RogueLikeMap GenerateMap(int mapWidth, int mapHeight, int viewWidth, int viewHeight)
        {
            //use GoRogue generator to combine the steps in Places/Generation and get the resulting three maps
            var generator = new Generator(mapWidth, mapHeight)
                .ConfigAndGenerateSafe(gen =>
                {
                    gen.AddSteps(new Places.Generation.GrassStep());
                    gen.AddSteps(new Places.Generation.StreetStep());
                    gen.AddSteps(new Places.Generation.HouseStep());
                });

            var grassMap = generator.Context.GetFirst<ISettableGridView<RogueLikeCell>>("grass");
            var streetMap = generator.Context.GetFirst<ISettableGridView<RogueLikeCell>>("street");
            var houseMap = generator.Context.GetFirst<ISettableGridView<RogueLikeCell>>("house");
            LocationsOfInterest = generator.Context.GetFirst<IEnumerable<Region>>("houses").ToList();
            SceneOfCrime = LocationsOfInterest.First();
            
            //combine the three smaller maps into one real map
            RogueLikeMap map = new RogueLikeMap(mapWidth, mapHeight, 4, Distance.Euclidean, viewSize: (viewWidth, viewHeight));
            
            foreach(var location in map.Positions())
            {
                if(houseMap[location] != null)
                {
                    houseMap[location].Position = location;
                    map.SetTerrain(houseMap[location]);
                }
                
                else if(streetMap[location] != null)
                {
                    streetMap[location].Position = location;
                    map.SetTerrain(streetMap[location]);
                }
                
                else
                {
                    grassMap[location].Position = location;
                    map.SetTerrain(grassMap[location]);
                }
            }

            //add additional components
            //todo - unsafe?
            Program.Weather = new Weather(map);

            PlacePeopleOnMap(map);
            foreach (var locationOfInterest in LocationsOfInterest)
            {
                foreach (var room in locationOfInterest.SubRegions)
                {
                    var rle = GenerateMiscellaneousItem();
                    rle.Position = RandomFreeSpace(map, room);
                    map.AddEntity(rle);
                }
            }

            return map;
        }
        private Region RandomRoom(RogueLikeMap map) => LocationsOfInterest.RandomItem().SubRegions.RandomItem();
        private void PlacePeopleOnMap(RogueLikeMap map)
        {
            var room = RandomRoom(map);
            //put the murder weapon on the map
            var murderWeapon = MurderWeapon;
            murderWeapon.Position = RandomFreeSpace(map, room);
            map.AddEntity(murderWeapon);

            //put the murderer on the map
            room = RandomRoom(map);
            var murderer = Murderer;
            murderer.Position = RandomFreeSpace(map, room);
            map.AddEntity(murderer);

            //add victim's corpse to the map
            room = RandomRoom(map);
            var victim = Victim;
            victim.Position = RandomFreeSpace(map, room);
            map.AddEntity(victim);

            //populate the map with witnesses
            foreach (var witness in Witnesses)
            {
                room = RandomRoom(map);
                witness.Position = RandomFreeSpace(map, room);
                map.AddEntity(witness);
            }
        }

        public static Point RandomFreeSpace(RogueLikeMap map)
        {
            var point = map.WalkabilityView.RandomPosition();
            while(map.GetEntitiesAt<RogueLikeEntity>(point).Any() || !map.WalkabilityView[point])
                point = map.WalkabilityView.RandomPosition();
            
            return point;
        }

        private static Point RandomFreeSpace(RogueLikeMap map, Region region)
        {
            var point = region.InnerPoints.RandomItem(p => map.Contains(p) && map.WalkabilityView[p]);
            
            while(map.GetEntitiesAt<RogueLikeEntity>(point).Any() || !map.WalkabilityView[point])
                point = region.InnerPoints.RandomItem(p => map.Contains(p) && map.WalkabilityView[p]);
            return point;
        }
    }
}