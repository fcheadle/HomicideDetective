using System;
using System.Collections.Generic;
using System.Linq;
using GoRogue;
using HomicideDetective.People;
using HomicideDetective.Places;
using HomicideDetective.Places.Generation;
using HomicideDetective.Words;
using SadRogue.Integration.Maps;
using SadRogue.Primitives.GridViews;

namespace HomicideDetective.Mysteries
{
    public partial class Mystery
    {
        public ArrayView<CrossRoads> Minimap { get; private set; }
        public ArrayView<RogueLikeMap?> LocationsOfInterest { get; private set; }
        public bool GenerationComplete { get; private set; } = false;
        public DateTime CurrentDate { get; private set; } = new DateTime(1970, 05, 05);       
        
        public IEnumerable<ISubstantive> Townsfolk => _townsfolk;
        private List<ISubstantive> _townsfolk = new();
        
        public IEnumerable<ISubstantive> Places => _places;
        private List<ISubstantive> _places = new();

        private readonly int _size = 4;
        private List<string> _familyNames = new();
        private List<Family> _families = new();
        private readonly int _desiredFamilyCount = 64;
        private readonly int _minimumFamilySize = 1;
        private readonly int _maximumFamilySize = 6;
        private int _currentHorizontalIndex = 0;
        private int _currentVerticalIndex = 0;
        private bool _createdMinimap = false;
        private bool _beganCurrentMap = false;
        private bool _finishedGeneratingMaps = false;
        private bool _finishedGeneratingFamilySurnames = false;
        private bool _finishedGeneratingFamilies = false;
        private bool _beganGeneratingFamilies = false;
        private bool _beganSurnames = false;
        private bool _placedPeopleOnMaps = false;
        private bool _beganElapsingTime = false;
        private bool _finishedElapsingTime = false;
        private bool _beganStalkingVictim = false;
        private bool _finishedStalkingVictim = false;
        private bool _committedMurder = false;
        private bool _chosenMotive = false;
        private bool _finalized = false;
        private int _timesShiftedUp = 0;
        
        private Personhood _victim;
        private Personhood _murderer;

        public void GenerateToCompletion()
        {
            while (!GenerationComplete)
            {
                DoGenerationStep();
            }
        }
        public string DoGenerationStep()
        {
            string answer = string.Empty;

            if (!_createdMinimap)
                return GenerateMiniMap();
            
            if (!_finishedGeneratingMaps)
                return GenerateMapStep();

            if(!_finishedGeneratingFamilySurnames)
                return GenerateSurnamesStep();
            
            if (!_finishedGeneratingFamilies)
                return GenerateFamilyStep();

            if (!_placedPeopleOnMaps)
                return PlacePeopleOnMapsStep();
            
            if (!_finishedElapsingTime)
                return ElapseTimeStep();
            
            if (!_beganStalkingVictim)
                return SelectVictimStep();
            
            if (!_chosenMotive)
                return DetermineMotiveStep();
            
            if (!_finishedStalkingVictim)
                return StalkVictimStep();
            
            if (!_committedMurder)
                return CommitMurderStep();

            if (!_finalized)
                return FinalizeStep();

            GenerationComplete = true;
            return "Generation Complete! Good luck, Detective.";
        }

        private string PlacePeopleOnMapsStep()
        {
            Witnesses = GenerateWitnessEntities().ToList();
            foreach (var witness in Witnesses)
            {
                var map = LocationsOfInterest.RandomItem();
                var position = map.RandomFreeSpace();
                witness.Position = position;
                map.AddEntity(witness);
            }

            _placedPeopleOnMaps = true;
            return "People added to maps.";
        }

        private string FinalizeStep()
        {
            Victim = GenerateVictimEntity();
            Murderer = GenerateMurdererEntity();
            MurderWeapon = GenerateMurderWeapon();
            // Witnesses = GenerateWitnessEntities().ToList();
            SceneOfCrimeInfo = GenerateSceneOfMurderInfo();
            _finalized = true;
            return "Dotting 'i's, crossing 't's...";
        }
        
        private string SelectVictimStep()
        {
            _beganStalkingVictim = true;
            _victim = RandomAdult();
            _murderer = RandomAdult();
            return $"This is the murder of {_victim.Name}. The Murderer has also been chosen.";
        }

        private string DetermineMotiveStep()
        {
            bool victimFemale = _victim.Pronouns == Constants.FemalePronouns;
            bool victimMale = _victim.Pronouns == Constants.MalePronouns;
            bool murdererFemale = _murderer.Pronouns == Constants.FemalePronouns;
            bool murdererMale = _murderer.Pronouns == Constants.MalePronouns;
            
            //spousal murder
            var marriedFamily = _families
                .Where(f => f.Adults.Contains(_victim) && f.Adults.Contains(_murderer));
            
            var sameFamily = _families
                .Where(f => f.Members.Contains(_victim) && f.Members.Contains(_murderer));
            
            if (marriedFamily.Any())
            {
                //husband murderers his wife
                if (victimFemale && murdererMale)
                {
                    //todo - elaborate
                    Motive = Motives.DomesticAssault;
                }
                
                //wife murders her husband
                else if (victimMale && murdererFemale)
                {
                    //todo - elaborate
                    Motive = Motives.SelfDefense;
                }
            }
            else if (sameFamily.Any())
            {
                //todo - elaborate
                Motive = Motives.Greed;
            }
            else
            {
                Motive = RandomMotive();
            }

            _chosenMotive = true;
            return "Chosen Motive.";
        }

        private Motives RandomMotive()
        {
            var motives = Enum.GetValues<Motives>();
            return motives[Random.Next(motives.Length)];
        }

        private Personhood RandomAdult()
        {
            var adult = _families.RandomItem().AdultMembers.ToList().RandomItem(); //ugly
            
            while (adult is null)
                adult = _families.RandomItem().AdultMembers.ToList().RandomItem();
            return adult;
        }

        private string CommitMurderStep()
        {
            //todo: pick means
            //todo: pick opportunity
            //todo: elapse time until date of Murder
            //todo: victim's last day
            //throw new NotImplementedException();
            _committedMurder = true;
            return "Committed Murder (Placeholder).";
        }

        private string StalkVictimStep()
        {
            // todo - switch depends on murderer
            _finishedStalkingVictim = true;
            return "Stalked Victim (Placeholder)";
            //throw new NotImplementedException();
        }

        private string ElapseTimeStep()
        {
            CurrentDate += TimeSpan.FromDays(1);
            
            if (CurrentDate >= Program.CurrentGame.CurrentTime - TimeSpan.FromDays(14))
                _finishedElapsingTime = true;
            
            //todo - elapse time?
            
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
                    answer = $"Generated {family.Surname} family.";
                    _families.Add(family);
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
            if (LocationsOfInterest[_currentHorizontalIndex, _currentVerticalIndex] == null)
            {
                if (_beganCurrentMap)
                {
                    LocationsOfInterest[_currentHorizontalIndex, _currentVerticalIndex] =
                        GenerateMap(_currentHorizontalIndex, _currentVerticalIndex);

                    _beganCurrentMap = false;
                    return
                        $"Finished generating {Minimap[_currentHorizontalIndex, _currentVerticalIndex].Type} at {Minimap[_currentHorizontalIndex, _currentVerticalIndex].Name}";
                }

                else
                {
                    _beganCurrentMap = true;
                    return
                        $"Beginning to Generate {Minimap[_currentHorizontalIndex, _currentVerticalIndex].Type}... ";
                }
            }
            else
            {
                //Map at the current place is already generated
                if (_currentHorizontalIndex < _size - 1)
                {
                    _currentHorizontalIndex++;
                    return GenerateMapStep();
                }
                else
                {
                    _currentHorizontalIndex = 0;

                    if (_currentVerticalIndex < _size - 1)
                    {
                        _currentVerticalIndex++;
                        return GenerateMapStep();
                    }
                    else
                    {
                        _finishedGeneratingMaps = true;
                        return "Done generating maps.";
                    }
                }
            }
        }

        private string GenerateMiniMap()
        {
            Minimap = new ArrayView<CrossRoads>(_size, _size);
            
            var required = new List<MapType>
            {
                MapType.Downtown,
                MapType.Park,
                MapType.ServiceDistrict,
                MapType.School,
            };
            
            int horizontalNameIndex = Random.Next(Enum.GetNames(typeof(RoadNames)).Length - 8);
            int verticalNameIndex = Random.Next(Enum.GetNames(typeof(RoadNumbers)).Length - 8);

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
                            var tile = required[Random.Next(required.Count)];
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

            _createdMinimap = true;
            return "Created Minimap";
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
            => MapGen.CreateDownTownMap(Random.Next(), _mapWidth, _mapHeight, _viewWidth, _viewHeight);

        private RogueLikeMap Neighborhood()
            => MapGen.CreateNeighborhoodMap(Random.Next(),_mapWidth, _mapHeight, _viewWidth, _viewHeight);

        private RogueLikeMap Park()
            => MapGen.CreateParkMap(_mapWidth, _mapHeight, _viewWidth, _viewHeight);
        
        private IEnumerable<string> GenerateFamilyNames()
        {
            for (int i = 0; i < _desiredFamilyCount; i++)
                yield return Constants.FamilyNames[Random.Next(0, Constants.FamilyNames.Length)];
        }

        private Family GenerateFamily(string surname)
        {
            int size = Random.Next(_minimumFamilySize, _maximumFamilySize);
            var family = new Family(surname);
            for (int i = 0; i < size; i++)
            {
                var familyMember = PersonFactory.GeneratePersonalInfo(Random.Next(), surname);
                
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