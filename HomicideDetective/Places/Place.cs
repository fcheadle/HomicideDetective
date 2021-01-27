using System.Collections.Generic;
using System.Linq;
using GoRogue;
using GoRogue.Components;
using GoRogue.MapGeneration;
using HomicideDetective.People;
using HomicideDetective.Places.Components;
using HomicideDetective.Places.Generation;
using SadConsole;
using SadRogue.Primitives;
using SadRogue.Primitives.GridViews;
using TheSadRogue.Integration;
using TheSadRogue.Integration.Maps;

namespace HomicideDetective.Places
{
    public class Place : RogueLikeMap
    {
        public string Name { get; }
        private readonly int _width;
        private readonly int _height;
        
        public List<Region> Regions;
        public Place(int width, int height, string name) : base(width, height, 16, Distance.Manhattan)
        {
            _width = width;
            _height = height;
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
                person.Position = Regions.RandomItem().Center;
                AddEntity(person);
            }
        }
    }
}