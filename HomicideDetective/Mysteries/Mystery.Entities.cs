using System.Collections.Generic;
using GoRogue;
using HomicideDetective.People;
using SadRogue.Integration;

namespace HomicideDetective.Mysteries
{
    public partial class Mystery
    {
        /// <summary>
        /// Generates a RogueLikeEntity that is the victim in a murder investigation.
        /// </summary>
        /// <returns></returns>
        public RogueLikeEntity GenerateVictim()
        {
            var victimInfo = GeneratePersonalInfo(_surnames.RandomItem());
            string descr = $"This is the body of {victimInfo.Name}. {victimInfo.Pronoun} was a";
            victimInfo.Description = victimInfo.Description!.Replace($"{victimInfo.Pronoun} is a", descr);
            victimInfo.SizeDescription = "is bloated from gases building up it's interior";
            victimInfo.WeightDescription = "is discolored from decay";
            
            var victim = new RogueLikeEntity((0,0), 2, false);
            victim.AllComponents.Add(victimInfo);
            
            //todo - decompose
            TimeOfDeath = new Time(Random.Next(0, 23), Random.Next(0, 59));
            return victim;
        }
        
        /// <summary>
        /// Generates a RogueLikeEntity who is the murderer in a murder investigation.
        /// </summary>
        /// <returns></returns>
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
        
        /// <summary>
        /// Generates the item which was used to kill someone
        /// </summary>
        /// <returns></returns>
        public RogueLikeEntity GenerateMurderWeapon()
        {
            var itemInfo = GenerateMurderWeaponInfo();
            var murderWeapon = new RogueLikeEntity((0,0), itemInfo.Name![0], true, true, 2);
            murderWeapon.AllComponents.Add(itemInfo);
            
            //todo - add markings
            
            return murderWeapon;
        }
        
        /// <summary>
        /// Generates everyone related to the case: family members, friends, coworkers, etc.
        /// </summary>
        /// <returns></returns>
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