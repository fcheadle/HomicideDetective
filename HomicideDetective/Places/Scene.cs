using System.Collections.Generic;
using System.Linq;
using GoRogue;
using GoRogue.MapGeneration;
using HomicideDetective.Mysteries;
using HomicideDetective.People;
using HomicideDetective.Places.Generation;
using HomicideDetective.Things;
using SadConsole;
using SadRogue.Primitives;
using SadRogue.Primitives.GridViews;
using TheSadRogue.Integration;
using TheSadRogue.Integration.Maps;

namespace HomicideDetective.Places
{
    public class Scene : RogueLikeMap
    {
        public List<Place> Locations;
        private Substantive withScene;
        public Scene(int width, int height) : base(width, height, 16, Distance.Manhattan)
        {
            Locations = new List<Place>();
        }

        public Scene(int width, int height, Substantive place) : this(width, height)
        {
            withScene = place;
        }
        
        public Scene GenerateHouse()
        {
            var generator = new Generator(Width, Height)
                .AddStep(new GrassStep())
                .AddStep(new StreetStep())
                .AddStep(new HouseStep())
                .Generate();
            
            var generatedMap = generator.Context.GetFirst<ISettableGridView<RogueLikeCell>>("grass");
            foreach(var location in generatedMap.Positions())
                SetTerrain(generatedMap[location]);

            generatedMap = generator.Context.GetFirst<ISettableGridView<RogueLikeCell>>("block");
            foreach(var location in generatedMap.Positions().Where(p => generatedMap[p] != null))
                SetTerrain(generatedMap[location]);

            generatedMap = generator.Context.GetFirst<ISettableGridView<RogueLikeCell>>("house");
            foreach(var location in generatedMap.Positions().Where(p => generatedMap[p] != null))
                SetTerrain(generatedMap[location]);
            
            var cells = new ArrayView<ColoredGlyph>(Width, Height);
            cells.ApplyOverlay(TerrainView);
            
            Locations = generator.Context.GetFirst<List<Place>>("rooms");
            return this;
        }
        public Scene GeneratePlains()
        {
            var generator = new Generator(Width, Height)
                .AddStep(new GrassStep())
                .Generate();
            
            var generatedMap = generator.Context.GetFirst<ISettableGridView<RogueLikeCell>>("grass");
            foreach(var location in generatedMap.Positions())
                SetTerrain(generatedMap[location]);

            var weather = new Weather(this);
            GoRogueComponents!.Add(weather);
            var cells = new ArrayView<ColoredGlyph>(Width, Height);
            cells.ApplyOverlay(TerrainView);
            
            return this;
        }

        public void Populate(IEnumerable<Substantive> entities)
        {
            var people = new List<Person>();
            foreach(var entity in entities.Where(e => e.Type == Substantive.Types.Person))
                people.Add(new Person((0,0), entity));

            var things = new List<Thing>();
            foreach(var entity in entities.Where(e => e.Type == Substantive.Types.Thing))
                things.Add(new Thing(Point.None, entity));
            
            Populate(people);
            Populate(things);
        }
        
        public void Populate(IEnumerable<Person> people)
        {
            foreach (Person person in people)
            {
                person.Position = Locations.RandomItem().Points.Last(p => GetTerrainAt(p)!.IsWalkable && !GetEntitiesAt<RogueLikeEntity>(p).Any());
                AddEntity(person);
            }
        }

        public void Populate(IEnumerable<Thing> things)
        {
            foreach (Thing thing in things)
            {
                thing.Position = Locations.RandomItem().Points.Last(p => GetTerrainAt(p)!.IsWalkable && !GetEntitiesAt<RogueLikeEntity>(p).Any());
                AddEntity(thing);
            }        
        }
    }
}