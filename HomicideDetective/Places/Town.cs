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
        public bool GenerationComplete { get; private set; } = false;
        public DateTime CurrentDate { get; private set; } = new(1960, 1, 1, 0, 0, 0);        
        
        public IEnumerable<ISubstantive> Townsfolk => _townsfolk;
        private List<ISubstantive> _townsfolk = new();
        
        public IEnumerable<ISubstantive> Places => _places;
        private List<ISubstantive> _places = new();
        
        private Random _random;
        private readonly int _size;
        private List<string> _familyNames = new();
        private List<Family> _families = new();
        private readonly int _mapWidth = 100;
        private readonly int _mapHeight = 60;
        private readonly int _viewWidth = Program.Width - 40;
        private readonly int _viewHeight = Program.Height;
        private readonly int _desiredFamilyCount;
        private readonly int _minimumFamilySize;
        private readonly int _maximumFamilySize;
        private int _currentHorizontalIndex = 0;
        private int _currentVerticalIndex = 0;
        private bool _beganCurrentMap = false;
        private bool _finishedGeneratingMaps = false;
        private bool _finishedGeneratingFamilySurnames = false;
        private bool _finishedGeneratingFamilies = false;
        private bool _beganGeneratingFamilies = false;
        private bool _beganSurnames = false;
        private bool _beganElapsingTime = false;
        private bool _finishedElapsingTime = false;
        private bool _beganStalkingVictim = false;
        private bool _finishedStalkingVictim = false;
        private bool _committedMurder = false;
        
        public Town(int seed, int size = 4, int desiredFamilyCount = 64, int minimumFamilySize = 1, int maximumFamilySize = 6)
        {
            Seed = seed;
            _maximumFamilySize = maximumFamilySize;
            _minimumFamilySize = minimumFamilySize;
            _size = size;
            _desiredFamilyCount = desiredFamilyCount;
            _random = new (Seed);
            Minimap = GenerateMiniMap();
            Maps = new ArrayView<RogueLikeMap?>(_size, _size);
        }

        public string DoGenerationStep()
        {
            string answer = string.Empty;

            if (!_finishedGeneratingMaps)
            {
                answer = GenerateMapStep();
            }
            
            else if(!_finishedGeneratingFamilySurnames)
            {
                answer = GenerateSurnamesStep();
            }
            
            else if (!_finishedGeneratingFamilies)
            {
                answer = GenerateFamilyStep();
            }
            
            else if (!_finishedElapsingTime)
            {
                answer = ElapseTimeStep();
            }
            
            else if (!_finishedStalkingVictim)
            {
                answer = StalkVictimStep();
            }
            
            else if (!_committedMurder)
            {
                answer = CommitMurderStep();
            }
            
            return answer;
        }

        private string CommitMurderStep()
        {
            //todo: pick means
            //todo: pick opportunity
            //todo: elapse time until date of Murder
            //todo: victim's last day
            throw new NotImplementedException();
        }

        private string StalkVictimStep()
        {
            //todo: pick victim
            //todo: pick murderer
            //todo: pick motive
            throw new NotImplementedException();
        }

        private string ElapseTimeStep()
        {
            //todo
            CurrentDate += TimeSpan.FromDays(1);
            
            if (CurrentDate >= new DateTime(1970, 1, 1, 0, 0, 0))
                _finishedElapsingTime = true;
            
            return $"Living through {CurrentDate.Date}";
        }

        private string GenerateFamilyStep()
        {
            string answer;
            if (_beganGeneratingFamilies)
            {
                if (_families.Count < _desiredFamilyCount)
                {
                    var family = GenerateFamily(_familyNames[_families.Count]);
                    answer = $"Generated {family.Surname} family: ";
                    foreach (var member in family.Elderly)
                        answer += $"{member.Name}, ";
                    foreach (var member in family.Adults)
                        answer += $"{member.Name}, ";
                    foreach (var member in family.Children)
                        answer += $"{member.Name}, ";
                }
                else
                {
                    answer = "Finished Generating Families. ";
                    _finishedGeneratingFamilies = true;
                }
            }
            else
            {
                answer = "Generating Families... ";
                _beganGeneratingFamilies = true;
            }

            return answer;
        }

        private string GenerateSurnamesStep()
        {
            if (_beganSurnames)
            {
                _familyNames = GenerateFamilyNames().ToList();
                _finishedGeneratingFamilySurnames = true;
                return "Done. ";
            }
            else
            {
                _beganSurnames = true;
                return "Generating Family Surnames... ";
            }
        }

        private string GenerateMapStep()
        {
            if (Maps[_currentHorizontalIndex, _currentVerticalIndex] == null)
            {
                if (_beganCurrentMap)
                {
                    Maps[_currentHorizontalIndex, _currentVerticalIndex] =
                        GenerateMap(_currentHorizontalIndex, _currentVerticalIndex);

                    _beganCurrentMap = false;
                    return "Done. ";
                }

                else
                {
                    _beganCurrentMap = true;
                    return $"Beginning to Generate {Minimap[_currentHorizontalIndex, _currentVerticalIndex].Type}... ";
                }
            }
            else
            {
                //Map at the current place is already generated
                if (_currentHorizontalIndex < _size)
                {
                    _currentHorizontalIndex++;
                    return GenerateMapStep();
                }
                else
                {
                    _currentHorizontalIndex = 0;

                    if (_currentVerticalIndex < _size)
                    {
                        _currentVerticalIndex++;
                        return GenerateMapStep();
                    }
                    else
                    {
                        _finishedGeneratingMaps = true;
                        return "Done. ";
                    }
                }
            }        
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


        public RogueLikeMap? GenerateMap(int x, int y)
        {
            RogueLikeMap map;

            var mapBasics = Minimap[x, y];
            switch (mapBasics.Type)
            {
                default:
                case MapType.Park: map = Park(); break;
                case MapType.Downtown: map = DownTown(); break;
                case MapType.ResidentialNeighborhood: map = Neighborhood(); break;
                //todo: case MapType.ServiceDistrict: map = ServiceDistrict(); break;
                //todo: case MapType.School: map = School(); break;
            }
            
            return map;
        }

        private RogueLikeMap DownTown() 
            => MapGen.CreateDownTownMap(_random.Next(), _mapWidth, _mapHeight, _viewWidth, _viewHeight);

        private RogueLikeMap Neighborhood()
            => MapGen.CreateNeighborhoodMap(_random.Next(),_mapWidth, _mapHeight, _viewWidth, _viewHeight);

        private RogueLikeMap Park()
            => MapGen.CreateParkMap(_mapWidth, _mapHeight, _viewWidth, _viewHeight);
        
        private IEnumerable<string> GenerateFamilyNames()
        {
            for (int i = 0; i < _desiredFamilyCount; i++)
                yield return Constants.FamilyNames[_random.Next(0, Constants.FamilyNames.Length)];
        }

        private Family GenerateFamily(string surname)
        {
            int size = _random.Next(_minimumFamilySize, _maximumFamilySize);
            var family = new Family(surname);
            for (int i = 0; i < size; i++)
            {
                var familyMember = PersonFactory.GeneratePersonalInfo(_random.Next(), surname);
                
                if (familyMember.AgeCategory >= AgeCategory.Elderly)
                    family.Elderly.Add(familyMember);
                else if (familyMember.AgeCategory >= AgeCategory.YoungAdult)
                    family.Adults.Add(familyMember);
                else 
                    family.Children.Add(familyMember);
            }

            return family;
        }
    }
}