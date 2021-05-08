using System.Collections.Generic;
using HomicideDetective.People;
using SadRogue.Integration;

namespace HomicideDetective.Mysteries
{
    public partial class Mystery
    {
        public RogueLikeEntity VictimAsEntity()
        {
            var victim = new RogueLikeEntity((0,0), 2, false);
            victim.AllComponents.Add(Victim);
            
            //todo - decompose
            
            return victim;
        }
        
        public RogueLikeEntity MurdererAsEntity()
        {
            var murderer = new RogueLikeEntity((0,0), 1, false);
            murderer.AllComponents.Add(Murderer);
            murderer.AllComponents.Add(new Thoughts());
            murderer.AllComponents.Add(new Speech());
            
            //todo - memories, sayings, etc
            
            return murderer;
        }
        
        public RogueLikeEntity MurderWeaponAsEntity()
        {            
            var murderWeapon = new RogueLikeEntity((0,0), MurderWeapon.Name![0], true, true, 2);
            murderWeapon.AllComponents.Add(MurderWeapon);
            
            //todo - add markings
            
            return murderWeapon;
        }
        public IEnumerable<RogueLikeEntity> WitnessesAsEntity()
        {
            foreach (var witnessDetails in Witnesses)
            {
                var witness = new RogueLikeEntity((0,0), 1, false);
                witness.AllComponents.Add(witnessDetails);
                witness.AllComponents.Add(new Thoughts());
                witness.AllComponents.Add(new Speech());
                
                //todo - memories, sayings, etc

                yield return witness;
            }
        }
    }
}