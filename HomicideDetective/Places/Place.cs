using System.Collections.Generic;
using System.Linq;
using GoRogue;
using GoRogue.MapGeneration;
using HomicideDetective.People;
using HomicideDetective.Places.Generation;
using SadConsole;
using SadRogue.Primitives;
using SadRogue.Primitives.GridViews;
using TheSadRogue.Integration;
using TheSadRogue.Integration.Maps;

namespace HomicideDetective.Places
{
    public class CrimeScene : RogueLikeMap
    {
        public string Name { get; }
        private readonly int _width;
        private readonly int _height;
        
        public List<Region> Regions;
        public CrimeScene(int width, int height, string name) : base(width, height, 16, Distance.Manhattan)
        {
            _width = width;
            _height = height;
            Name = name;
            Regions = new List<Region>();
        }

        public CrimeScene GenerateHouse()
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