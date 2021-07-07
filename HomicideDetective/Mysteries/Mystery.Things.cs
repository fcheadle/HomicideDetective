using SadRogue.Integration;

namespace HomicideDetective.Mysteries
{
    public partial class Mystery
    {

        /// <summary>
        /// Generate the descriptive info for the murder weapon.
        /// </summary>
        /// <returns></returns>
        public Substantive GenerateMurderWeaponInfo()
        {
            string name, description, detail, article;
            int mass, volume;
            switch (Random.Next(0,10))
            {
                default: 
                    name = "hammer";
                    description = "A small tool, normally used for carpentry";
                    article = "a";
                    mass = 710;
                    volume = 490;
                    detail = "It is completely free of dust, and has a slight smell of bleach";
                    break;
                case 1: 
                    name = "switchblade"; 
                    description = "A small, concealable knife";
                    article = "a";
                    mass = 125;
                    volume = 325;
                    detail = "There is a small patch of red rust near the hinge";
                    break;
                case 2: 
                    name = "pistol"; 
                    description = "A small, concealable handgun";
                    article = "a";
                    mass = 6750;
                    volume = 465;
                    detail = "There are residue patterns on the muzzle";
                    break;
                case 3: 
                    name = "poison"; 
                    description = "A lethal dose of hydrogen-cyanide";
                    article = "a";
                    mass = 15;
                    volume = 12;
                    detail = "There is a puncture hole in the cap";
                    break;
                case 4: 
                    name = "kitchen Knife"; 
                    description = "A small tool used for preparing food";
                    article = "a";
                    mass = 240;
                    volume = 390;
                    detail = "It is somewhat warped along its flat plane";
                    break;
                case 5: 
                    name = "shotgun";
                    description = "A large gun used for scaring off vermin";
                    article = "a";
                    mass = 13500;
                    volume = 8825;
                    detail = "there is one shell in the chamber";
                    break;
                case 6: 
                    name = "rock"; 
                    description = "A stone from off the ground";
                    article = "a";
                    mass = 10000;
                    volume = 750;
                    detail = "It is covered in blood and haphazardly discarded nearby";
                    break;
                case 7: 
                    name = "screwdriver"; 
                    description = "A small tool, used for all manner of handiwork";
                    article = "a";
                    mass = 120;
                    volume = 200;
                    detail = "This flathead has been recently cleaned";
                    break;
                case 8: 
                    name = "revolver"; 
                    description = "A handgun";
                    article = "a";
                    mass = 7000;
                    volume = 700;
                    detail = "it has five bullets, and one empty chamber";
                    break;
                case 9: 
                    name = "rifle"; 
                    description = "A large gun, used for hunting animals";
                    article = "a";
                    mass = 12000;
                    volume = 8200;
                    detail = "It is coated with gunpowder residue.";
                    break;
            }

            var substantive = new Substantive(ISubstantive.Types.Thing, name, article: article, pronoun: "it",
                description: description, mass: mass, volume: volume);
            substantive.AddDetail(detail);
            return substantive;
        }

        /// <summary>
        /// Generates info for a random item.
        /// </summary>
        /// <returns>A substantive for a generic item that could exist anywhere.</returns>
        public Substantive GenerateMiscellaneousItemInfo()
        {
            string name, description, detail;
            int mass, volume;
            switch (Random.Next(1, 101) % 10)
            {
                default:
                    name = "coffee mug";
                    description = "a small ceramic cup used for hot liquids";
                    detail = "it has brown stains on the interior.";
                    mass = 101;
                    volume = 64;
                    break;
                case 1:
                    name = "typewriter";
                    description = "device used to write quickly and legibly";
                    detail = "it is worn from years of use.";
                    mass = 64000;
                    volume = 320;
                    break;
                case 2:
                    name = "photo album";
                    description = "a booklet for holding photographs";
                    detail = "there is a page in the middle with no photos.";
                    mass = 2050;
                    volume = 180;
                    break;
                case 3:
                    name = "handsaw";
                    description = "a small tool, normally used for carpentry";
                    detail = "it is rusted all over.";
                    mass = 881;
                    volume = 200;
                    break;
                case 4:
                    name = "record player";
                    description = "a device which plays music from records";
                    detail = "there is no record in it right now.";
                    mass = 1675;
                    volume = 850;
                    break;
                case 5:
                    name = "schoolbook";
                    description = "a book containing basic information about math";
                    detail = "the inside is covered in crayon drawings";
                    mass = 1750;
                    volume = 600;
                    break;
                case 6:
                    name = "toaster";
                    description = "a device used for toasting bread";
                    detail = "it is filled with crumbs.";
                    mass = 1200;
                    volume = 1200;
                    break;
                case 7:
                    name = "shirt";
                    description = "a shirt that has been tossed on the ground";
                    detail = "it is filthy beyond belief.";
                    mass = 101;
                    volume = 64;
                    break;
                case 8:
                    name = "vinyl record";
                    description = "a record that contains music and can be played by a record player";
                    detail = "it is a hard rock band with violent album art.";
                    mass = 0;
                    volume = 0;
                    break;
                case 9:
                    name = "hairbrush";
                    description = "a tool used for grooming one's hair";
                    detail = "it has red, brown, and grey hairs in its teeth.";
                    mass = 0;
                    volume = 0;
                    break;
            }   
            
            var substantive = new Substantive(ISubstantive.Types.Thing, name, article: "a", pronoun: "it",
                description: description, mass: mass, volume: volume);
            substantive.AddDetail(detail);
            return substantive;
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
        /// Creates a generic rle item
        /// </summary>
        /// <returns></returns>
        public RogueLikeEntity GenerateMiscellaneousItem()
        {
            var item = GenerateMiscellaneousItemInfo();
            var rle = new RogueLikeEntity((0,0), item.Name![0]);
            rle.AllComponents.Add(item);
            return rle;
        }
    }
}