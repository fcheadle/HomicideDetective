using System;
using System.Collections.Generic;
using System.Linq;
using HomicideDetective.People;
using HomicideDetective.Places.Generation;
using HomicideDetective.Words;
using SadRogue.Integration.Maps;
using SadRogue.Primitives.GridViews;

namespace HomicideDetective.Places
{
    /// <summary>
    /// Controls and manages the behavior of the entire game town as a whole
    /// </summary>
    public class Town
    {
        public ArrayView<CrossRoads> Minimap { get; private set; }
        public ArrayView<RogueLikeMap?> Maps { get; private set; }
        public int Seed { get; }
        private Random _random;
        private readonly int _size;
        private readonly List<string> _families;
        public IEnumerable<ISubstantive> Townsfolk => _townsfolk;
        private readonly List<ISubstantive> _townsfolk;
        public IEnumerable<ISubstantive> Places => _places;
        private readonly List<ISubstantive> _places;

        private readonly int _mapWidth = 100;
        private readonly int _mapHeight = 60;
        private readonly int _viewWidth = Program.Width - 40;
        private readonly int _viewHeight = Program.Height;
        
        public Town(int seed, int size = 4)
        {
            Seed = seed;
            _size = size;
            _random = new (Seed);
            Minimap = GenerateMiniMap();
            _families = GenerateFamilyNames().ToList();
            _townsfolk = GenerateTownsfolk().ToList();
            _places = new();
            Maps = new ArrayView<RogueLikeMap?>(_size, _size);
        }

        public ArrayView<CrossRoads> GenerateMiniMap()
        {
            var required = new List<MapType>
            {
                MapType.Downtown,
                MapType.Park,
                MapType.ServiceDistrict,
                MapType.School,
            };
            
            Minimap = new ArrayView<CrossRoads>(_size, _size);
            int horizontalNameIndex = _random.Next(Enum.GetNames(typeof(RoadNames)).Length - 8);
            int verticalNameIndex = _random.Next(Enum.GetNames(typeof(RoadNumbers)).Length - 8);

            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    var horizontalStreet = horizontalNameIndex + i * 2;
                    var verticalStreet = verticalNameIndex + j * 2;
                    MapType type;
                    if (i == 0 || i == _size - 1 || j == 0 || j == _size - 1)
                    {
                        type = MapType.ResidentialNeighborhood;
                    }
                    else
                    {
                        if (required.Any())
                        {
                            var tile = required[_random.Next(required.Count)];
                            type = tile;
                            required.Remove(tile);
                        }
                        else
                        {
                            type = MapType.Downtown;
                        }
                    }

                    Minimap[i, j] = new CrossRoads(type, (RoadNames)horizontalStreet, (RoadNumbers)verticalStreet);
                }
            }

            return Minimap;
        }

        public void GenerateMap(int x, int y)
        {
            if (Minimap.Contains(x, y))
            {
                var mapBasics = Minimap[x, y];
                RogueLikeMap map;

                switch (mapBasics.Type)
                {
                    // case MapType.Downtown: map = DownTown(); break;
                    case MapType.Park: map = Park(); break;
                    case MapType.ResidentialNeighborhood: map = Neighborhood(); break;
                    default: break;
                }
            }
        }

        // private RogueLikeMap DownTown() 
        //     => MapGen.CreateDownTownMap(_mapWidth, _mapHeight, _viewWidth, _viewHeight);

        private RogueLikeMap Neighborhood()
            => MapGen.CreateNeighborhoodMap(_random.Next(),_mapWidth, _mapHeight, _viewWidth, _viewHeight);

        private RogueLikeMap Park()
            => MapGen.CreateParkMap(_mapWidth, _mapHeight, _viewWidth, _viewHeight);

        public IEnumerable<string> GenerateFamilyNames()
        {
            for (int i = 0; i < 15; i++)
                yield return Constants.FamilyNames[_random.Next(0, Constants.FamilyNames.Length)];
        }

        public IEnumerable<ISubstantive> GenerateTownsfolk()
        {
            foreach(var familyName in _families)
                for (int i = 0; i < _random.Next(1, 7); i++)
                    yield return PersonFactory.GeneratePersonalInfo(_random.Next(), familyName);
        }
    }
}