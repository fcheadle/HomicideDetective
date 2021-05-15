using System.Collections.Generic;
using GoRogue;
using HomicideDetective.People;
using SadRogue.Integration;

namespace HomicideDetective.Mysteries
{
    public partial class Mystery
    {
        public RogueLikeEntity GenerateVictim()
        {
            var victimInfo = GeneratePersonalInfo(_surnames.RandomItem());
            string descr = $"This is the body of {victimInfo.Name}. {victimInfo.Pronoun} was a";
            victimInfo.Description = victimInfo.Description.Replace($"{victimInfo.Pronoun} is a", descr);
            victimInfo.SizeDescription = "is bloated from gases building up it's interior";
            victimInfo.WeightDescription = "is discolored from decay";
            
            var victim = new RogueLikeEntity((0,0), 2, false);
            victim.AllComponents.Add(victimInfo);
            
            //todo - decompose
            
            return victim;
        }
        
        public RogueLikeEntity GenerateMurderer()
        {
            var murdererInfo = GeneratePersonalInfo(_surnames.RandomItem());
            var murderer = new RogueLikeEntity((0,0), 1, false);
            murderer.AllComponents.Add(murdererInfo);
            murderer.AllComponents.Add(new Thoughts());
            murderer.AllComponents.Add(new Speech());
            
            //todo - memories, sayings, etc
            
            return murderer;
        }
        
        public RogueLikeEntity GenerateMurderWeapon()
        {
            var itemInfo = GenerateMurderWeaponInfo();
            var murderWeapon = new RogueLikeEntity((0,0), itemInfo.Name![0], true, true, 2);
            murderWeapon.AllComponents.Add(itemInfo);
            
            //todo - add markings
            
            return murderWeapon;
        }
        
        public IEnumerable<RogueLikeEntity> GenerateWitnesses()
        {
            for(int i = 0; i < 10; i++)
            {
                var witnessInfo = GeneratePersonalInfo(_surnames.RandomItem());
                var witness = new RogueLikeEntity((0,0), 1, false);
                witness.AllComponents.Add(witnessInfo);
                witness.AllComponents.Add(new Thoughts());
                witness.AllComponents.Add(new Speech());
                
                //todo - memories, sayings, etc

                yield return witness;
            }
        }
    }
}