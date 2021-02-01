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
    public class Place : RogueLikeMap, IDetailed
    {
        public string Name { get; }
        public string Description { get; }
        public Substantive Substantive { get; }
        public List<Region> Regions;

        public Place(int width, int height, string name, string description) : base(width, height, 16, Distance.Manhattan)
        {
            Description = description;
            Name = name;
            Regions = new List<Region>();
            Substantive = new Substantive(Substantive.Types.Place, name, description, 0, Width * Height * 1000, "", "");
        }

        public Place GenerateHouse()
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
            foreach(var location in generatedMap.Positions().Where(p=> generatedMap[p] != null))
                SetTerrain(generatedMap[location]);

            generatedMap = generator.Context.GetFirst<ISettableGridView<RogueLikeCell>>("house");
            foreach(var location in generatedMap.Positions().Where(p => generatedMap[p] != null))
                SetTerrain(generatedMap[location]);
            
            var cells = new ArrayView<ColoredGlyph>(Width, Height);
            cells.ApplyOverlay(TerrainView);
            
            Regions = generator.Context.GetFirst<List<Region>>("rooms");
            return this;
        }
        public Place GeneratePlains()
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

        public void Populate(IEnumerable<Person> people)
        {
            foreach (Person person in people)
            {
                person.Position = Regions.RandomItem().Points.Last(p => GetTerrainAt(p)!.IsWalkable && !GetEntitiesAt<RogueLikeEntity>(p).Any());
                AddEntity(person);
            }
        }

        public void Populate(IEnumerable<Thing> things)
        {
            foreach (Thing thing in things)
            {
                thing.Position = Regions.RandomItem().Points.Last(p => GetTerrainAt(p)!.IsWalkable && !GetEntitiesAt<RogueLikeEntity>(p).Any());
                AddEntity(thing);
            }        
        }
        
        public string[] GetDetails() => Substantive.Details;
    }
}