using System.Collections.Generic;
using System.Linq;
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
            SceneOfCrime = generator.Context.GetFirst<IEnumerable<Region>>("houses").First();
            
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

            PlaceEntitiesOnMap(map);
            
            return map;
        }

        private void PlaceEntitiesOnMap(RogueLikeMap map)
        {
            //put the murder weapon on the map
            var murderWeapon = MurderWeapon;
            murderWeapon.Position = RandomFreeSpace(map);
            map.AddEntity(murderWeapon);

            //put the murderer on the map
            var murderer = Murderer;
            murderer.Position = RandomFreeSpace(map);
            map.AddEntity(murderer);

            //add victim's corpse to the map
            var victim = Victim;
            victim.Position = RandomFreeSpace(map);
            map.AddEntity(victim);

            //populate the map with witnesses
            foreach (var witness in Witnesses)
            {
                witness.Position = RandomFreeSpace(map);
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
    }
}