using System.Collections.Generic;
using System.Linq;
using GoRogue.MapGeneration;
using HomicideDetective.New.People;
using HomicideDetective.New.Places.Generation;
using SadConsole;
using SadRogue.Primitives;
using SadRogue.Primitives.GridViews;
using TheSadRogue.Integration;
using TheSadRogue.Integration.Maps;

namespace HomicideDetective.New.Places
{
    public class CrimeScene : RogueLikeMap
    {
        private int _width;
        private int _height;
        
        public List<Region> Regions;
        public CrimeScene(int width, int height) : base(width, height, 16, Distance.Manhattan)
        {
            _width = width;
            _height = height;
            Regions = new List<Region>();
        }

        public void Generate()
        {
            var generator = new Generator(_width, _height)
                .AddStep(new GrassStep())
                .AddStep(new StreetStep())
                .AddStep(new HouseStep())
                .AddStep(new WitnessStep())
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
                        
            var peopleMap = generator.Context.GetFirst<IEnumerable<Person>>("people");
            foreach(var person in peopleMap)
                if(Terrain[person.Position]!.IsWalkable)
                    if(!GetEntitiesAt<Person>(person.Position).Any())
                        AddEntity(person);
            
            var cells = new ArrayView<ColoredGlyph>(_width, _height);
            cells.ApplyOverlay(TerrainView);
        }
    }
}