using System.Collections.Generic;
using System.Linq;
using GoRogue;
using GoRogue.Components;
using GoRogue.MapGeneration;
using HomicideDetective.Mysteries;
using HomicideDetective.People;
using HomicideDetective.Places.Components;
using HomicideDetective.Places.Generation;
using HomicideDetective.Things;
using SadConsole;
using SadRogue.Primitives;
using SadRogue.Primitives.GridViews;
using TheSadRogue.Integration;
using TheSadRogue.Integration.Maps;

namespace HomicideDetective.Places
{
    public class Place : RogueLikeMap, ISubstantive
    {
        public ISubstantive.Types? Type => ISubstantive.Types.Place;
        public string Name { get; }
        public string Description { get; }
        public int Mass => 0; //mass of place doesn't make sense
        public int Volume { get; }
        public string SizeDescription => "";
        public string WeightDescription => "";
        public string[] Details { get; }

        public string GetDetailedDescription()
            => $"This is {Name}. It is {Volume / 1000} cubic meters.";

        public void AddDetail(string detail) => _details.Add(detail);

        private readonly int _width;
        private readonly int _height;
        
        public List<Region> Regions;
        private List<string> _details;

        public Place(int width, int height, string name, string description) : base(width, height, 16, Distance.Manhattan)
        {
            _width = width;
            _height = height;
            Volume = width * height * 1000;
            Description = description;
            Name = name;
            Regions = new List<Region>();
        }

        public Place GenerateHouse()
        {
            var generator = new Generator(_width, _height)
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
            
            var cells = new ArrayView<ColoredGlyph>(_width, _height);
            cells.ApplyOverlay(TerrainView);
            
            Regions = generator.Context.GetFirst<List<Region>>("rooms");
            return this;
        }
        public Place GeneratePlains()
        {
            var generator = new Generator(_width, _height)
                .AddStep(new GrassStep())
                .Generate();
            
            var generatedMap = generator.Context.GetFirst<ISettableGridView<RogueLikeCell>>("grass");
            foreach(var location in generatedMap.Positions())
                SetTerrain(generatedMap[location]);

            var weather = new WeatherComponent(this);
            GoRogueComponents!.Add(weather);
            var cells = new ArrayView<ColoredGlyph>(_width, _height);
            cells.ApplyOverlay(TerrainView);
            
            return this;
        }

        public void Populate(IEnumerable<Person> people)
        {
            foreach (Person person in people)
            {
                person.Position = Regions.RandomItem().Points.Last(p => GetTerrainAt(p).IsWalkable && !GetEntitiesAt<RogueLikeEntity>(p).Any());
                AddEntity(person);
            }
        }

        public void Populate(IEnumerable<Thing> things)
        {
            foreach (Thing thing in things)
            {
                thing.Position = Regions.RandomItem().Points.Last(p => GetTerrainAt(p).IsWalkable && !GetEntitiesAt<RogueLikeEntity>(p).Any());
                AddEntity(thing);
            }        
        }
    }
}