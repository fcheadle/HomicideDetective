using System;
using System.Linq;
using HomicideDetective.People;
using SadRogue.Integration;

namespace HomicideDetective.Mysteries
{
    public partial class Mystery
    {
        private void GenerateTimeline()
        {
            if (Victim == null || Murderer == null || !Witnesses.Any())
                throw new Exception("Attempted to generate a timeline before we have a murderer, victim, or witnesses.");

            foreach (var witness in Witnesses)
            {
                switch (Random.Next(1, 6))
                {
                    case 1: GenerateSchoolDay(witness); break;
                    case 2: GenerateOfficeWorkDay(witness); break;
                    case 3: GenerateRetailWorkDay(witness); break;
                    case 4: GenerateActiveDayOff(witness); break;
                    case 5: GenerateLazyDayOff(witness); break;
                }
            }
        }

        private void GenerateSchoolDay(RogueLikeEntity rogueLikeEntity)
        {
            var memories = rogueLikeEntity.AllComponents.GetFirst<Thoughts>();
            var speech = rogueLikeEntity.AllComponents.GetFirst<Speech>();
            throw new System.NotImplementedException();
        }

        private void GenerateOfficeWorkDay(RogueLikeEntity rogueLikeEntity)
        {
            var memories = rogueLikeEntity.AllComponents.GetFirst<Thoughts>();
            var speech = rogueLikeEntity.AllComponents.GetFirst<Speech>();
            throw new System.NotImplementedException();
        }

        private void GenerateRetailWorkDay(RogueLikeEntity rogueLikeEntity)
        {
            var memories = rogueLikeEntity.AllComponents.GetFirst<Thoughts>();
            var speech = rogueLikeEntity.AllComponents.GetFirst<Speech>();
            throw new System.NotImplementedException();
        }

        private void GenerateActiveDayOff(RogueLikeEntity rogueLikeEntity)
        {
            var memories = rogueLikeEntity.AllComponents.GetFirst<Thoughts>();
            var speech = rogueLikeEntity.AllComponents.GetFirst<Speech>();
            throw new System.NotImplementedException();
        }
        
        private void GenerateLazyDayOff(RogueLikeEntity rogueLikeEntity)
        {
            var memories = rogueLikeEntity.AllComponents.GetFirst<Thoughts>();
            var speech = rogueLikeEntity.AllComponents.GetFirst<Speech>();
            throw new System.NotImplementedException();
        }
    }
}